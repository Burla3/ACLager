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
    
    public partial class WasteReport
    {
        public long uid { get; }
        public System.DateTime date { get; set; }
        public long amount { get; set; }
        public Nullable<long> work_order { get; set; }
        public Nullable<long> item { get; set; }
        public long created_by { get; set; }
    }
}
