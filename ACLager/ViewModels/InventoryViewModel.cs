using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class InventoryViewModel : BaseViewModel {
        public InventoryViewModel() {
            base.SelectColor("Inventory");
        }
        public InventoryViewModel(IEnumerable<ItemGroup> itemGroups) : this() {
            ItemGroups = itemGroups;

        }
        public InventoryViewModel(IEnumerable<ItemGroup> itemGroups, Item item, IEnumerable<SelectListItem> locations, IEnumerable<SelectListItem> itemTypes) : this() {
            ItemGroups = itemGroups;
            Item = item;
            Locations = locations;
            ItemTypes = itemTypes;
        }

        public IEnumerable<ItemGroup> ItemGroups { get; set; }
        public Item Item { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<SelectListItem> ItemTypes { get; set; }
    }
}