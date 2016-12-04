using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class WorkOrderItemGroup {
        public WorkOrderItemGroup(Item item, ItemType itemType, Location location, WorkOrderItem workOrderItem) {
            Item = item;
            ItemType = itemType;
            Location = location;
            WorkOrderItem = workOrderItem;
        }

        public Item Item { get; set; }
        public ItemType ItemType { get; set; }
        public Location Location { get; set; }
        public WorkOrderItem WorkOrderItem { get; set; }
    }
}