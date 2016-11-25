using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.ViewModels
{
    public class WasteViewModel : BaseViewModel
    {
        public WasteViewModel(IEnumerable<WasteReport> wasteReports)
        {
            WasteReports = wasteReports;
        }
        
        public IEnumerable<WasteReport> WasteReports { get; set; }
    }
}