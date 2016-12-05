using System;
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
                return RedirectToAction("Index");
            }

            WorkOrder workorder;
            List<WorkOrderItemGroup> workOrderItemGroups = new List<WorkOrderItemGroup>();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workorder = db.WorkOrderSet.Find(Int64.Parse(id));
                foreach (WorkOrderItem workorderitem in workorder.WorkOrderItems) {
                    Item item = workorderitem.Item;
                    WorkOrderItemGroup workOrderItemGroup = new WorkOrderItemGroup(item, item.ItemType, item.Location,
                        workorderitem);
                    workOrderItemGroups.Add(workOrderItemGroup);
                }
            }

            return View(new WorkOrderBaseViewModel(null, workorder, workOrderItemGroups));
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
                        "Arbejdsopgave annulleret",
                        $"Arbejdsopgave nr. {dbWorkOrder.OrderNumber} annulleret",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            ArbejdsOpgave = dbWorkOrder.ToLoggable()
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
                        "Opdateret arbejdsopgave punkt",
                        $"Opdateret arbejdsopgave punkt på arbejdsopgave nr. {dbWorkOrder.OrderNumber}",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            ArbejdsOpgave = dbWorkOrder.ToLoggable(),
                            ArbejdsOpgavePunkt = dbWorkOrder.ToLoggable()
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
                        "Arbejdsopgave færdig",
                        $"Arbejdsopgave nr. {dbWorkOrder.OrderNumber} er færdig",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            ArbejdsOpgave = dbWorkOrder.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Index");
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