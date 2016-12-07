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
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Production() {
            List<WorkOrderItemTypePair> workOrderItemTypePairs = new List<WorkOrderItemTypePair>();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                IEnumerable<WorkOrder> workorders = db.WorkOrderSet.Where(wo => wo.Type == "Produktion" && !wo.IsComplete).ToList();
                IEnumerable<ItemType> itemTypes = db.ItemTypeSet.ToList();
                foreach (WorkOrder workOrder in workorders) {
                   workOrderItemTypePairs.Add(new WorkOrderItemTypePair(itemTypes.FirstOrDefault(it => it.UID == workOrder.ItemType.UID), workOrder));
                }
            }
            WorkOrder workorder = new WorkOrder {
                Type = "Produktion"
            };

            return View("Index", new WorkOrderProductionViewModel(workOrderItemTypePairs, workorder, null, null));
        }

        [HttpGet]
        public ActionResult Packaging() {
            List<WorkOrderItemTypePair> workOrderItemTypePairs = new List<WorkOrderItemTypePair>();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                IEnumerable<WorkOrder>  workorders = db.WorkOrderSet.Where(wo => wo.Type == "Pakkeri" && !wo.IsComplete);
                foreach (WorkOrder workOrder in workorders) {
                    workOrderItemTypePairs.Add(new WorkOrderItemTypePair(workOrder.ItemType, workOrder));
                }
            }
            WorkOrder workorder = new WorkOrder {
                Type = "Pakkeri"
            };

            return View("Index", new WorkOrderPackagingViewModel(workOrderItemTypePairs, workorder, null, null));
        }

        [HttpGet]
        public ActionResult Detailed(string id) {
            if (id == null) {
                return RedirectToAction("Index", "Home");
            }

            WorkOrder workorder;
            ItemType itemType = null;
            List<WorkOrderItemGroup> workOrderItemGroups = new List<WorkOrderItemGroup>();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workorder = db.WorkOrderSet.Find(Int64.Parse(id));
                if (workorder.ItemType != null) {
                    itemType = workorder.ItemType;
                }
                
                foreach (WorkOrderItem workorderitem in workorder.WorkOrderItems) {
                    Item item = workorderitem.Item;
                    WorkOrderItemGroup workOrderItemGroup = new WorkOrderItemGroup(item, workorderitem.ItemType, item?.Location, workorderitem);
                    workOrderItemGroups.Add(workOrderItemGroup);
                }
            }

            if (workorder.Type == "Pakkeri") {
                return View(new WorkOrderPackagingViewModel(null, workorder, workOrderItemGroups, itemType));
            } else {
                return View(new WorkOrderProductionViewModel(null, workorder, workOrderItemGroups, itemType));
            }
        }

        [HttpPost]
        public ActionResult Pick(long id) {
            long workOrderUID;
            Item item;
            ItemType dbItemType;
            Location dbLocation;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                WorkOrderItem workOrderItem = db.WorkOrderItemSet.Find(id);
                workOrderUID = workOrderItem.WorkOrder.UID;

                item = new Item {
                    UID = workOrderItem.Item.UID,
                    Amount = workOrderItem.Amount
                };

                Item dbItem = db.ItemSet.Find(item.UID);
                dbItemType = dbItem.ItemType;
                dbLocation = dbItem.Location;

                if (!InventoryController.PickItem(item, true)) {
                    return View("Error", new WorkOrderBaseViewModel());
                }
                
                workOrderItem.Progress += item.Amount;
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Vare plukket",
                        $"{item.Amount} {dbItemType.Unit} {dbItemType.Name} plukket fra {dbLocation.Name}.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Vare = item.ToLoggable(),
                            Varetype = dbItemType.ToLoggable(),
                            Lokation = dbLocation.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Detailed", new { id = workOrderUID });
            
            
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

            return RedirectToAction(dbWorkOrder.Type == "Pakkeri" ? "Packaging" : "Production");
        }

        /// <summary>
        /// Updates a workorder item.
        /// </summary>
        /// <param name="progress">Workorder item to be updated.</param>
        /// <returns>Redirects to /WorkOrder.</returns>
        [HttpPost]
        public ActionResult Update(long id, WorkOrder workOrder) {
            WorkOrder oldWorkOrder;
            WorkOrder dbWorkOrder;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbWorkOrder = db.WorkOrderSet.Find(id);

                if (dbWorkOrder == null) {
                    return RedirectToAction(dbWorkOrder.Type == "Pakkeri" ? "Packaging" : "Production");
                }

                oldWorkOrder = dbWorkOrder.ToLoggable();

                if (workOrder.Progress.HasValue) {
                    dbWorkOrder.Progress = workOrder.Progress.Value;
                }

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Opdateret Ordre",
                        $"Opdateret Ordre nr. {dbWorkOrder.OrderNumber}",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Før = oldWorkOrder,
                            Efter = dbWorkOrder.ToLoggable(),
                        }
                    )
            );

            return RedirectToAction("Detailed", new {id = id});
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
                User user = db.UserSet.Find(UserController.GetContextUser().UID);
                dbWorkOrder.CompletedByUser = user;

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

            return RedirectToAction(dbWorkOrder.Type == "Pakkeri" ? "Packaging" : "Production");
        }

        [HttpPost]
        public ActionResult Start(long id) {
            IEnumerable<WorkOrderItem> loggableWorkOrderItems = new List<WorkOrderItem>();
            WorkOrder workOrder;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workOrder = db.WorkOrderSet.Find(id);
                workOrder.IsStarted = true;

                IEnumerable<WorkOrderItem> workOrderItems = workOrder.WorkOrderItems;
                IEnumerable<WorkOrderItem> newWorkOrderItems = new List<WorkOrderItem>();
                loggableWorkOrderItems = loggableWorkOrderItems.Concat(workOrderItems);

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
                            loggableWorkOrderItems = loggableWorkOrderItems.Concat(newWorkOrderItems);
                        } catch (Exception) {
                            return View("Error", new WorkOrderBaseViewModel(null, workOrder, null, null));
                        }
                        
                    }            
                }
                db.WorkOrderItemSet.AddRange(newWorkOrderItems);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Ordre startet",
                        $"Ordre {workOrder.OrderNumber} startet og vare er blevet reseveret.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Ordre = workOrder.ToLoggable(),
                            OrdrePunkter = loggableWorkOrderItems
                        }
                    )
            );

            return RedirectToAction("Detailed", new {id = id});
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
                db.WorkOrderItemSet.AddRange(workOrderItems);
                db.SaveChanges();
            }
        }

        public event ChangedEventHandler Changed;
    }
}