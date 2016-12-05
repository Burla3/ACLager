using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class ItemTypeAmountPair {
        public ItemType ItemType { get; set; }
        public double Amount { get; set; }
    }
}