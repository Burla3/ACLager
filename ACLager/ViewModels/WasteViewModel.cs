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
            base.SectionColor = "green";
        }

        public WasteViewModel(IEnumerable<WasteReport> wasteReports) : this() {
            WasteReports = wasteReports;
        }


        public IEnumerable<WasteReport> WasteReports { get; set; }
    }
}