using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            List<WorkOrderItem> workorderitems = new List<WorkOrderItem>();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                workorder = db.WorkOrderSet.Find(Int64.Parse(id));
                foreach (WorkOrderItem workorderitem in workorder.WorkOrderItems) {
                        workorderitems.Add(workorderitem);
                }
            }

            return View(new WorkOrderBaseViewModel(null, workorder, workorderitems));
        }

        /// <summary>
        /// Cancels a workorder and its items.
        /// </summary>
        /// <param name="UID">UID of the workorder.</param>
        /// <returns>Redirects to /WorkOrder.</returns>
        [HttpPost]
        public ActionResult CancelWorkOrder(long UID) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {

                WorkOrder workOrder = db.WorkOrderSet.Find(UID);
                IEnumerable<WorkOrderItem> workOrderItems = workOrder.WorkOrderItems;

                if (workOrder != null) {
                    foreach (WorkOrderItem workOrderItem in workOrderItems) {
                        db.WorkOrderItemSet.Remove(workOrderItem);
                    }

                    db.WorkOrderSet.Remove(workOrder);
                    db.SaveChanges();
                } else {
                    //given uid is not on a known workorder.
                }

            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Cancels a workorder item.
        /// </summary>
        /// <param name="UID">UID of the workorder item.</param>
        /// <returns>Redirects to /WorkOrder</returns>
        [HttpPost]
        public ActionResult CancelWorkOrderItem(long UID) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                if (db.WorkOrderItemSet.Find(UID) != null) {
                    db.WorkOrderItemSet.Remove(db.WorkOrderItemSet.Find(UID));
                    db.SaveChanges();
                } else {
                    //workorder with given UID not found.
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates a workorder.
        /// </summary>
        /// <param name="workOrder">The workorder to be updated.</param>
        /// <returns>Redirects to /WorkOrder.</returns>
        [HttpPost]
        public ActionResult UpdateWorkOrder(WorkOrder workOrder) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                WorkOrder dbWorkOrder = db.WorkOrderSet.Find(workOrder.UID);
                if (dbWorkOrder != null) {
                    dbWorkOrder.DueDate = workOrder.DueDate;
                    db.SaveChanges();
                } else {
                    //Workorder not found. 
                }

            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates a workorder item.
        /// </summary>
        /// <param name="workOrderItem">Workorder item to be updated.</param>
        /// <returns>Redirects to /WorkOrder.</returns>
        [HttpPost]
        public ActionResult UpdateWorkOrderItem(WorkOrderItem workOrderItem) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                WorkOrderItem dbWorkOrderItem = db.WorkOrderItemSet.Find(workOrderItem.UID);

                if (dbWorkOrderItem != null) {
                    dbWorkOrderItem.Amount = workOrderItem.Amount;
                    dbWorkOrderItem.Progress = workOrderItem.Progress;
                    db.SaveChanges();
                } else {
                    //Workorder does not exsist.
                }
            }

            return RedirectToAction("Index");
        }

        /*
        public ActionResult CompleteWorkOrder(WorkOrder workOrder, User completedBy)
        {
            //Tries to find the workorder in the database
            var exsistingWorkOrder = _db.WorkOrders.Find(workOrder);

            if (exsistingWorkOrder != null)
            {
                exsistingWorkOrder.is_complete = true;
                exsistingWorkOrder.completed_by = completedBy.uid;
                _db.SaveChanges();
                return true;
            }
            else
            {
                //workorder not found
                return false;
            }
        }
        */

        /// <summary>
        /// Creates a workorder.
        /// </summary>
        /// <param name="workOrder">Workorder to be created.</param>
        /// <returns>Redirects to /WorkOrder.</returns>
        [HttpPost]
        public ActionResult CreateWorkOrder(WorkOrder workOrder) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                var dbWorkOrder = (db.WorkOrderSet.Where(
                    workorder => workorder.DueDate == workOrder.DueDate && workorder.Type == workOrder.Type));

                if (dbWorkOrder.Any()) {
                    //Similar workorder(s) exsist, make another?
                } else {
                    db.WorkOrderSet.Add(workOrder);
                    db.SaveChanges();
                }

            }

            return RedirectToAction("Index");
        }

        public event ChangedEventHandler Changed;
    }
}