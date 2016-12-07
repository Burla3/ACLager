using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class WorkOrderItemTypePair {
        public WorkOrderItemTypePair(ItemType itemType, WorkOrder workOrder) {
            ItemType = itemType;
            WorkOrder = workOrder;
        }

        public ItemType ItemType { get; set; }
        public WorkOrder WorkOrder { get; set; }
    }
}