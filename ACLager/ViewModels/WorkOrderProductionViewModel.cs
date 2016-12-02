using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.ViewModels {
    public class WorkOrderProductionViewModel : WorkOrderBaseViewModel {
        public WorkOrderProductionViewModel() : base() {
            base.SelectSectionSpecials("Wordorder-Production");
        }
    }
}