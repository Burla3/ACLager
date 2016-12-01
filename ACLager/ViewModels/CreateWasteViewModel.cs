using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class CreateWasteViewModel : BaseViewModel {
        public CreateWasteViewModel() {
            base.SectionColor = "deep-orange";
        }

        public CreateWasteViewModel(WasteReport wasteReport, IEnumerable<SelectListItem> items, IEnumerable<SelectListItem> workorders) : this() {
            WasteReport = wasteReport;
            Items = items;
            WorkOrders = workorders;
        }
        public Item Item { get; set; }
        public WorkOrder WorkOrder { get; set; }
        public WasteReport WasteReport { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
        public IEnumerable<SelectListItem> WorkOrders { get; set; }
    }
}