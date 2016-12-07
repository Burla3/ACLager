using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class ItemGroup {
        public ItemGroup()
        {

        }

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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ItemGroup itemGroup = (ItemGroup)obj;
            return ItemType.Equals(itemGroup.ItemType) &&
                   ItemLocationPairs.SequenceEqual(itemGroup.ItemLocationPairs);
        }
    }
}