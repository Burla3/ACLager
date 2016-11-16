using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers
{
    public class WorkOrderController : Controller
    {
        // GET: WorkOrder
        public ActionResult Index()
        {
            return View();
        }
        
        public IEnumerable<WorkOrder> GetWorkOrders()
        {
            throw new NotImplementedException();
        }

        public bool CancelWorkOrder(WorkOrder workOrder)
        {
            throw new NotImplementedException();
        }

        public bool CancelWorkOrderItem(WorkOrderItem workOrderItem)
        {
            throw new NotImplementedException();
        }

        public bool UpdateWorkOrder(DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public bool UpdateWorkOrderItem(long amount, long progress)
        {
            throw new NotImplementedException();
        }

        public bool CompleteWorkOrder(WorkOrder workOrder, User completedBy)
        {
            throw new NotImplementedException();
        }

        public bool CreateWorkOrder(DateTime dueDate, string type)
        {
            throw new NotImplementedException();
        }

        public bool CreateWorkOrderItem(WorkOrder workOrder, ItemType itemType, long amount)
        {
            throw new NotImplementedException();
        }
    }
}