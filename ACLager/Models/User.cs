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
    
    public partial class User
    {
        public long uid { get; set; }
        public bool is_admin { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
    }
}