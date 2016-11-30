using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class WasteReportGroup {
        public WasteReportGroup(ItemType itemType, WorkOrder workOrder, User user, WasteReport wasteReport, Item item) {
            ItemType = itemType;
            WorkOrder = workOrder;
            User = user;
            WasteReport = wasteReport;
            Item = item;
        }

        public ItemType ItemType { get; set; }
        public WorkOrder WorkOrder { get; set; }
        public User User { get; set; }
        public WasteReport WasteReport { get; set; }
        public Item Item { get; set; }
    }
}