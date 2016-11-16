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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cancels an existing work order.
        /// </summary>
        /// <param name="workOrder"></param>
        /// <returns>Returns true if successful.</returns>
        public bool CancelWorkOrder(WorkOrder workOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cancels an existing work order item.
        /// </summary>
        /// <param name="workOrderItem"></param>
        /// <returns>Returns true if successful.</returns>
        public bool CancelWorkOrderItem(WorkOrderItem workOrderItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the due date for an existing work order.
        /// </summary>
        /// <param name="dueDate"></param>
        /// <returns>Returns true if successful.</returns>
        public bool UpdateWorkOrder(DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the amount and progress of an existing work order item.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="progress"></param>
        /// <returns>Returns true if successful.</returns>
        public bool UpdateWorkOrderItem(long amount, long progress)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marks a work order as completed by setting the is_completed property on the workorder, 
        /// and sets the completed_by property to the name from the user.
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="completedBy"></param>
        /// <returns>Returns true if successful.</returns>
        public bool CompleteWorkOrder(WorkOrder workOrder, User completedBy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new work order and sets its due_date and type properties.
        /// </summary>
        /// <param name="dueDate"></param>
        /// <param name="type"></param>
        /// <returns>Returns true if successful.</returns>
        public bool CreateWorkOrder(DateTime dueDate, string type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new work order item, and sets its work_order, item_type and amount properties.
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="itemType"></param>
        /// <param name="amount"></param>
        /// <returns>Returns true if successful.</returns>
        public bool CreateWorkOrderItem(WorkOrder workOrder, ItemType itemType, long amount)
        {
            throw new NotImplementedException();
        }

        public event ChangedEventHandler Changed;
    }
}