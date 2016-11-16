using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.CustomClasses {
    public class ItemGroup {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="itemAndLocations"></param>
        public ItemGroup(ItemType itemType, IEnumerable<Tuple<Item, Location>> itemAndLocations) {
            ItemType = itemType;
            ItemAndLocations = itemAndLocations;
        }

        public ItemType ItemType { get; set; }
        public IEnumerable<Tuple<Item, Location>> ItemAndLocations { get; set; }   
        
    }
}