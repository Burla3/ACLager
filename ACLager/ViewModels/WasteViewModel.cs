using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;
using System.Web.Mvc;
using ACLager.CustomClasses;

namespace ACLager.ViewModels
{
    public class WasteViewModel : BaseViewModel
    {
        public WasteViewModel(){
            base.SectionColor = "green";
        }

        public WasteViewModel(IEnumerable<WasteReportGroup> wasteReportGroups, WasteReportGroup wasteReportGroup) : this() {
            WasteReportGroups = wasteReportGroups;
            WasteReportGroup = wasteReportGroup;
        }

        public WasteReportGroup WasteReportGroup { get; set; }
        public IEnumerable<WasteReportGroup> WasteReportGroups { get; set; }
    }
}