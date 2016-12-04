using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class WorkOrderPackagingViewModel : WorkOrderBaseViewModel {
        public WorkOrderPackagingViewModel(IEnumerable<WorkOrder> workorders, WorkOrder workorder, List<string> workorderitems) : base(workorders, workorder, workorderitems) {
            base.SelectSectionSpecials("WorkOrder-Packaging");
        }
    }
}