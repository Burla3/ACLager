using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.Models;

namespace ACLager.ViewModels{
    public class WorkOrderBaseViewModel : BaseViewModel{

        public WorkOrderBaseViewModel() : base() {

        }

        public WorkOrderBaseViewModel(IEnumerable<WorkOrder> workorders, WorkOrder workorder, IEnumerable<WorkOrderItemGroup> workorderitems) : this() {
            Workorders = workorders;
            Workorder = workorder;
            Workorderitems = workorderitems;
        }

       
        public IEnumerable<WorkOrderItemGroup> Workorderitems { get; set; }
        public WorkOrder Workorder { get; set; }
        public IEnumerable<WorkOrder> Workorders { get; set; }

       
    }
}