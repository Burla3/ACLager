using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.Interfaces;

namespace ACLager.Controllers
{
    public class WorkOrderController : Controller, ILoggable
    {
        private readonly ACLagerDatabaseEntities _db = new ACLagerDatabaseEntities();
        // GET: WorkOrder
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Gets all work orders and returns them in an IEnumerable.
        /// </summary>
        /// <returns>All users in an IEnumerable</returns>
        public IEnumerable<WorkOrder> GetWorkOrders()
        {
            return _db.WorkOrders;
        }

        /// <summary>
        /// Cancels an existing work order.
        /// </summary>
        /// <param name="workOrder">The workorder to be deleted</param>
        /// <returns>Returns true if successful.</returns>
        public bool CancelWorkOrder(WorkOrder workOrder)
        {
            var exsistingWorkOrder = _db.WorkOrders.Find(workOrder);
            if (exsistingWorkOrder != null)
            {
                _db.WorkOrders.Remove(workOrder);
                _db.SaveChanges();
                return true;
            }
            else
            {
                //workorder does not exsist
                return false;
            }
        }

        /// <summary>
        /// Cancels an existing work order item.
        /// </summary>
        /// <param name="workOrderItem">The workorder item to be removed/cancelled </param>
        /// <returns>Returns true if successful.</returns>
        public bool CancelWorkOrderItem(WorkOrderItem workOrderItem)
        {
            if (_db.WorkOrderItems.Contains(workOrderItem))
            {
                _db.WorkOrderItems.Remove(workOrderItem);
                _db.SaveChanges();
                return true;
            }
            else
            {
                //item not found
                return false;
            }
            
        }

        

        /// <summary>
        /// Updates the due date for an existing work order.
        /// </summary>
        /// <param name="workOrder">The workorder to be updated</param>
        /// <param name="dueDate">The new duedate for the given workorder</param>
        /// <returns>Returns true if successful.</returns>
        public bool UpdateWorkOrder(WorkOrder workOrder, DateTime dueDate) //Hvorfor kun duedate der updates?
        { 
            var exsistingWorkOrder = _db.WorkOrders.Find(workOrder);
            if (exsistingWorkOrder!=null)
            {
                exsistingWorkOrder.due_date = dueDate;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the amount and progress of an existing work order item.
        /// </summary>
        /// <param name="amount"> The new amount </param>
        /// <param name="progress"> The new progress </param>
        /// <param name="workOrderItem"> The workorderitem to be edited </param>
        /// <returns>Returns true if successful.</returns>
        public bool UpdateWorkOrderItem(WorkOrderItem workOrderItem, long amount, long progress)
        {
            var exsistingWorkOrderItem = _db.WorkOrderItems.Find(workOrderItem);
            if (exsistingWorkOrderItem != null)
            {
                exsistingWorkOrderItem.amount = amount;
                exsistingWorkOrderItem.progress = progress;
                _db.SaveChanges();
                return true;
            }
            else
            {
                //The workorderitem does not exsist.
                return false;
            }
        }

        /// <summary>
        /// Marks a work order as completed by setting the is_completed property on the workorder, 
        /// and sets the completed_by property to the name from the user.
        /// </summary>
        /// <param name="workOrder"> The workorder to be set as completed </param>
        /// <param name="completedBy"> The user completing the workorder </param>
        /// <returns>Returns true if successful.</returns>
        public bool CompleteWorkOrder(WorkOrder workOrder, User completedBy)
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

        /// <summary>
        /// Creates a new work order and sets its due_date and type properties.
        /// </summary>
        /// <param name="dueDate"> The duedate for the to be created workorder </param>
        /// <param name="type"> ??? </param>
        /// <returns>Returns true if successful.</returns>
        public bool CreateWorkOrder(DateTime dueDate, string type)
        {
            //Checks if similar workorders already exsists
            var exsistingWorkOrder =
            (from workorder in _db.WorkOrders
                where workorder.due_date == dueDate && workorder.type == type
                select workorder);

            if (exsistingWorkOrder.Any())
            {
                //Similar workorders exsists, prompt to make sure they want to make a new one?
                return false;
            }
            else
            {
                _db.WorkOrders.Add((new WorkOrder() {is_complete = false, type = type, due_date = dueDate}));
                _db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Creates a new work order item, and sets its work_order, item_type and amount properties.
        /// </summary>
        /// <param name="workOrder"> The workorder which the to be created workorderitem is associated to </param>
        /// <param name="itemType"> the itemtype of the workorderitem </param>
        /// <param name="amount"> The amount of the given item </param>
        /// <returns>Returns true if successful.</returns>
        public bool CreateWorkOrderItem(WorkOrder workOrder, ItemType itemType, long amount)
        {
            //checking the database for a dublicate workorder
            var exsistingWorkOrderItem =
            (from workorderitems in _db.WorkOrderItems
                where workorderitems.work_order == workOrder.uid && workorderitems.item_type == itemType.uid && workorderitems.amount == amount
                select workorderitems);

            if (exsistingWorkOrderItem.Any())
            {
                //this itemtype and amount is already associated with the given workorder, generate anyways?
                return false;
            }
            else
            {
                _db.WorkOrderItems.Add( (new WorkOrderItem() {amount = amount, work_order = workOrder.uid, item_type = itemType.uid}));
                _db.SaveChanges();
                return true;
            }
        }

        public event ChangedEventHandler Changed;
    }
}