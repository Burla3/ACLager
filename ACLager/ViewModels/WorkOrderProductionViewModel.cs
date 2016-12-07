using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.CustomClasses;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class WorkOrderProductionViewModel : WorkOrderBaseViewModel {
        public WorkOrderProductionViewModel(IEnumerable<WorkOrderItemTypePair> workOrderItemTypePairs, WorkOrder workorder, IEnumerable<WorkOrderItemGroup> workorderitems, ItemType itemType) : base(workOrderItemTypePairs, workorder, workorderitems, itemType) {
            base.SelectSectionSpecials("WorkOrder-Production");
        }
    }
}