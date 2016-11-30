using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;
using System.Web.Mvc;

namespace ACLager.ViewModels
{
    public class WasteViewModel : BaseViewModel
    {
        public WasteViewModel(){
            base.SelectColor("Waste");
        }

        public WasteViewModel(IEnumerable<WasteReport> wasteReports, WasteReport wastereport, IEnumerable<SelectListItem> itemtypename){
            WasteReports = wasteReports;
            WasteReport = wastereport;
            Itemtypename = itemtypename;
        }

        public WasteReport WasteReport { get; set; }
        public IEnumerable<WasteReport> WasteReports { get; set; }
        public IEnumerable<SelectListItem> Itemtypename { get; set; }
    }
}