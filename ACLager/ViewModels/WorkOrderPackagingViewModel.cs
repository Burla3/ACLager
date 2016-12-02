using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.ViewModels {
    public class WorkOrderPackagingViewModel : WorkOrderBaseViewModel {
        public WorkOrderPackagingViewModel() {
            base.SelectSectionSpecials("WorkOrder-Packaging");
        }
    }
}