using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(WorkOrderItemMetadata))]
    [DisplayName("Ordrelinje")]
    public partial class WorkOrderItem {
        private class WorkOrderItemMetadata {
            [DisplayName("Ordrelinjenummer")]
            public long UID { get; set; }
            [DisplayName("Mængde")]
            public double Amount { get; set; }
            [DisplayName("Færdiggjort")]
            public double Progress { get; set; }
        }

        public WorkOrderItem ToLoggable() {
            return new WorkOrderItem {
                UID = this.UID,
                Amount = this.Amount,
                Progress = this.Progress,
                WorkOrder = null,
                ItemType = null,
                Item = null
            };
        }
    }
}