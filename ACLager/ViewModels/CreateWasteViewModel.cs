using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class CreateWasteViewModel : BaseViewModel {
        public CreateWasteViewModel() {
            base.SectionColor = "deep-orange";
        }

        public CreateWasteViewModel(WasteReport wasteReport, IEnumerable<SelectListItem> items, IEnumerable<SelectListItem> workorder) : this() {
            WasteReport = wasteReport;
            Items = items;
            WorkOrder = workorder;
        }

        public WasteReport WasteReport { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
        public IEnumerable<SelectListItem> WorkOrder { get; set; }
    }
}