using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(WorkOrderMetadata))]
    [DisplayName("Ordre")]
    public partial class WorkOrder {
        private class WorkOrderMetadata {
            [DisplayName("Ordrenummer")]
            public long UID { get; set; }
            [DisplayName("Ordretype")]
            public string Type { get; set; }
            [DisplayName("Batchnummer")]
            public long BatchNumber { get; set; }
            [DisplayName("Deadline")]
            public System.DateTime DueDate { get; set; }
            [DisplayName("Status")]
            public bool IsComplete { get; set; }
            [DisplayName("Status")]
            public bool IsStarted { get; set; }
            [DisplayName("Leveringsoplysninger")]
            public string ShippingInfo { get; set; }        
        }

        public WorkOrder ToLoggable() {
            return new WorkOrder {
                UID = this.UID,
                Type = this.Type,
                BatchNumber = this.BatchNumber,
                DueDate = this.DueDate,
                IsComplete = this.IsComplete,
                ShippingInfo = this.ShippingInfo,
                OrderNumber = this.OrderNumber,
                CompletedByUser = null,
                WorkOrderItems = null,
                ItemType = null
            };
        }
    }
}