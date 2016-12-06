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
            base.SelectSectionSpecials("Waste-Create");
        }

        public CreateWasteViewModel(WasteReport wasteReport, IEnumerable<SelectListItem> itemTypes, IEnumerable<SelectListItem> workorders, IEnumerable<SelectListItem> locations) : this() {
            WasteReport = wasteReport;
            ItemTypes = itemTypes;
            WorkOrders = workorders;
            Locations = locations;
        }

        public ItemType ItemType { get; set; }
        public WorkOrder WorkOrder { get; set; }
        public Location Location { get; set; }
        public WasteReport WasteReport { get; set; }
        public IEnumerable<SelectListItem> ItemTypes { get; set; }
        public IEnumerable<SelectListItem> WorkOrders { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
    }
}