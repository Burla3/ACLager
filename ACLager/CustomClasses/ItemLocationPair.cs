using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses
{
    public class ItemLocationPair
    {
        /// <summary>
        /// Class to contain an item and the location paired to the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="location"></param>
        public ItemLocationPair(Item item, Location location)
        {
            Item = item;
            Location = location;
        }

        public Item Item { get; set; }
        public Location Location { get; set; }
    }
}