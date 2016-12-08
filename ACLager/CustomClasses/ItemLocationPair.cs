using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class ItemLocationPair {
        public ItemLocationPair() {
            
        }

        /// <summary>
        /// Class to contain an item and the location paired to the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="location"></param>
        public ItemLocationPair(Item item, Location location) {
            Item = item;
            Location = location;
        }

        public Item Item { get; set; }
        public Location Location { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            ItemLocationPair itemLocationPair = (ItemLocationPair)obj;
            return Item.Equals(itemLocationPair.Item) &&
                   Location.Equals(itemLocationPair.Location);
        }
    }
}