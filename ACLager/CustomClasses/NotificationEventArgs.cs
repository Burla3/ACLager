using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class NotificationEventArgs {
        public NotificationEventArgs(ItemType itemType) {
            ItemType = itemType;
        }

        public ItemType ItemType { get; set; }
    }
}