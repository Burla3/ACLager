using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class ItemGroup {

        /// <summary>
        /// Class to contain a item type with all associated items and their locations.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="itemLocationPairs"></param>
        public ItemGroup(ItemType itemType, IEnumerable<ItemLocationPair> itemLocationPairs) {
            ItemType = itemType;
            ItemLocationPairs = itemLocationPairs;
        }

        public ItemType ItemType { get; set; }
        public IEnumerable<ItemLocationPair> ItemLocationPairs { get; set; }
    }
}