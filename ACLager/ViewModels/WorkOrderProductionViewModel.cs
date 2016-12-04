using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class WorkOrderProductionViewModel : WorkOrderBaseViewModel {
        public WorkOrderProductionViewModel(IEnumerable<WorkOrder> workorders, WorkOrder workorder, List<string> workorderitems) : base(workorders, workorder, workorderitems) {
            base.SelectSectionSpecials("Wordorder-Production");
        }
    }
}