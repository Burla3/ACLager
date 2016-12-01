//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACLager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkOrder()
        {
            this.IsComplete = false;
            this.WorkOrderItems = new HashSet<WorkOrderItem>();
        }
    
        public long UID { get; set; }
        public string Type { get; set; }
        public long BatchNumber { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public bool IsComplete { get; set; }
        public string ShippingInfo { get; set; }
    
        public virtual User CompletedByUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderItem> WorkOrderItems { get; set; }
    }
}
