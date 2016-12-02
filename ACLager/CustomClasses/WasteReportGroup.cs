using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class WasteReportGroup {
        public WasteReportGroup(WasteReport wasteReport, dynamic objectData) {
            WasteReport = wasteReport;
            ObjectData = objectData;
        }

        public WasteReport WasteReport { get; set; }
        public dynamic ObjectData { get; set; }
    }
}