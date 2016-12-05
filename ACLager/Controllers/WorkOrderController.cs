using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.CustomClasses.Attributes;
using ACLager.Models;
using ACLager.Interfaces;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    public class WorkOrderController : Controller, ILoggable {
        // GET: WorkOrder
        public ActionResult Index() {
            IEnumerable<WorkOrder> workorders;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workorders = db.WorkOrderSet.ToList();
            }

            return View(new WorkOrderBaseViewModel(workorders, new WorkOrder(), null));
        }

        [HttpGet]
        public ActionResult Production() {
            IEnumerable<WorkOrder> workorders;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workorders = db.WorkOrderSet.Where(wo => wo.Type == "Produktion").ToList();
            }

            return View("Index", new WorkOrderProductionViewModel(workorders, new WorkOrder(), null));
        }

        [HttpGet]
        public ActionResult Packaging() {
            IEnumerable<WorkOrder> workorders;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workorders = db.WorkOrderSet.Where(wo => wo.Type == "Pakkeri").ToList();
            }

            return View("Index", new WorkOrderPackagingViewModel(workorders, new WorkOrder(), null));
        }

        [HttpGet]
        public ActionResult Detailed(string id) {
            if (id == null) {
                return RedirectToAction("Index", "Home");
            }

            WorkOrder workorder;
            List<WorkOrderItemGroup> workOrderItemGroups = new List<WorkOrderItemGroup>();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workorder = db.WorkOrderSet.Find(Int64.Parse(id));
                if (workorder.ItemType != null) {
                    workorder.ItemType.UID = workorder.ItemType.UID;
                }
                
                foreach (WorkOrderItem workorderitem in workorder.WorkOrderItems) {
                    Item item = workorderitem.Item;
                    WorkOrderItemGroup workOrderItemGroup = new WorkOrderItemGroup(item, workorderitem.ItemType, item?.Location, workorderitem);
                    workOrderItemGroups.Add(workOrderItemGroup);
                }
            }

            return View(new WorkOrderProductionViewModel(null, workorder, workOrderItemGroups));
        }

        [HttpPost]
        public ActionResult Pick(long id) {
            Item item;
            long woid;
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                WorkOrderItem dbwoi = db.WorkOrderItemSet.Find(id);
                item = dbwoi.Item;
                item.Amount = dbwoi.Amount;
                woid = dbwoi.WorkOrder.UID;
            }

            InventoryController.PickItem(item, true);
            return RedirectToAction("Detailed", new { id = woid });
            
            
        }


        /// <summary>
        /// Cancels a workorder and its items.
        /// </summary>
        /// <param name="id">UID of the workorder.</param>
        /// <returns>Redirects to /WorkOrder.</returns>
        [AdminOnly]
        [HttpPost]
        public ActionResult Cancel(long id) {
            WorkOrder dbWorkOrder;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbWorkOrder = db.WorkOrderSet.Find(id);

                if (dbWorkOrder == null) {
                    return RedirectToAction("Index");
                }
               
                db.WorkOrderItemSet.RemoveRange(dbWorkOrder.WorkOrderItems);

                db.WorkOrderSet.Remove(dbWorkOrder);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Ordre annulleret",
                        $"Ordre nr. {dbWorkOrder.OrderNumber} annulleret",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Ordre = dbWorkOrder.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates a workorder item.
        /// </summary>
        /// <param name="progress">Workorder item to be updated.</param>
        /// <returns>Redirects to /WorkOrder.</returns>
        [HttpPost]
        public ActionResult UpdateWorkOrderItem(long id, long progress) {
            WorkOrderItem dbWorkOrderItem;
            WorkOrder dbWorkOrder;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbWorkOrderItem = db.WorkOrderItemSet.Find(id);

                if (dbWorkOrderItem == null) {
                    return RedirectToAction("Index");
                }

                dbWorkOrder = dbWorkOrderItem.WorkOrder.ToLoggable();

                dbWorkOrderItem.Progress = progress;
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Opdateret Ordre punkt",
                        $"Opdateret Ordre punkt på arbejdsopgave nr. {dbWorkOrder.OrderNumber}",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Ordre = dbWorkOrder.ToLoggable(),
                            OrdrePunkt = dbWorkOrder.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Complete(long id) {
            WorkOrder dbWorkOrder;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbWorkOrder = db.WorkOrderSet.Find(id);

                if (dbWorkOrder == null) {
                    return RedirectToAction("Index");
                }

                dbWorkOrder.IsComplete = true;
                dbWorkOrder.CompletedByUser = UserController.GetContextUser();

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Ordre færdig",
                        $"Ordre nr. {dbWorkOrder.OrderNumber} er færdig",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Ordre = dbWorkOrder.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Start(long id) {

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                WorkOrder workOrder = db.WorkOrderSet.Find(id);
                workOrder.IsStarted = true;

                IEnumerable<WorkOrderItem> workOrderItems = workOrder.WorkOrderItems;
                IEnumerable<WorkOrderItem> newWorkOrderItems = new List<WorkOrderItem>();

                foreach (WorkOrderItem workOrderItem in workOrderItems) {
                    IEnumerable<Item> items =
                        workOrderItem.ItemType.Items.OrderBy(i => i.ExpirationDate ?? DateTime.MaxValue);

                    double amountNeeded = workOrderItem.Amount;
                    Item item = items.First(i => i.Amount > i.Reserved);
                    double amountAvailable = item.Amount - item.Reserved;

                    if (amountAvailable >= amountNeeded) {
                        item.Reserved += amountNeeded;
                        workOrderItem.Item = item;
                    } else {
                        item.Reserved += amountAvailable;
                        amountNeeded -= amountAvailable;
                        workOrderItem.Amount = amountAvailable;
                        workOrderItem.Item = item;

                        try {
                            newWorkOrderItems = CreateNewWorkOrderItems(amountNeeded, workOrderItem, items, workOrder);
                        } catch (Exception) {
                            return View("Error", new WorkOrderBaseViewModel(null, workOrder, null));
                        }
                        
                    }            
                }
                db.WorkOrderItemSet.AddRange(newWorkOrderItems);
                db.SaveChanges();
            }

            return RedirectToAction("Detailed", id);
        }

        private IEnumerable<WorkOrderItem> CreateNewWorkOrderItems(double amountNeeded, WorkOrderItem workOrderItem, IEnumerable<Item> items, WorkOrder workOrder) {
            List<WorkOrderItem> newWorkOrderItems = new List<WorkOrderItem>();
            while (amountNeeded > 0) {
                Item item = items.First(i => i.Amount > i.Reserved);
                double amountAvailable = item.Amount - item.Reserved;

                WorkOrderItem newWorkOrderItem = new WorkOrderItem {
                    ItemType = workOrderItem.ItemType,
                    WorkOrder = workOrder
                };

                if (amountAvailable >= amountNeeded) {
                    newWorkOrderItem.Amount = amountNeeded;
                    item.Reserved += amountNeeded;
                    amountNeeded -= amountNeeded;
                    newWorkOrderItem.Item = item;
                } else {
                    newWorkOrderItem.Amount = amountAvailable;
                    item.Reserved += amountAvailable;
                    amountNeeded -= amountAvailable;
                    newWorkOrderItem.Item = item;
                }

                newWorkOrderItems.Add(newWorkOrderItem);
            }
            return newWorkOrderItems;
        } 

        public void CreateFromC5(IEnumerable<ItemTypeAmountPair> itemTypeAmountPairs, string shippingInfo, DateTime duedate, long orderNumber) {
            WorkOrder workOrder = new WorkOrder {
                DueDate = duedate,
                OrderNumber = orderNumber,
                ShippingInfo = shippingInfo,
                Type = "Pakkeri",
            };

            List<WorkOrderItem> workOrderItems = new List<WorkOrderItem>();

            foreach (ItemTypeAmountPair itemTypeAmountPair in itemTypeAmountPairs) {
                WorkOrderItem workOrderItem = new WorkOrderItem {
                    Amount = itemTypeAmountPair.Amount,
                    ItemType = itemTypeAmountPair.ItemType,
                    WorkOrder = workOrder
                };

                workOrderItems.Add(workOrderItem);
            }

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.WorkOrderSet.Add(workOrder);
                db.SaveChanges();
                db.WorkOrderItemSet.AddRange(workOrderItems);
            }
        }

        public event ChangedEventHandler Changed;
    }
}