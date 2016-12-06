using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.CustomClasses;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class WorkOrderPackagingViewModel : WorkOrderBaseViewModel {
        public WorkOrderPackagingViewModel(IEnumerable<WorkOrder> workorders, WorkOrder workorder, IEnumerable<WorkOrderItemGroup> workorderitems, ItemType itemType) : base(workorders, workorder, workorderitems, itemType) {
            base.SelectSectionSpecials("WorkOrder-Packaging");
        }
    }
}