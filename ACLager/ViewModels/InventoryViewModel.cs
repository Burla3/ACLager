using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.CustomClasses;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class InventoryViewModel : BaseViewModel {
        public InventoryViewModel(IEnumerable<ItemGroup> itemGroups) {
            ItemGroups = itemGroups;
        }

        public IEnumerable<ItemGroup> ItemGroups { get; set; }
    }
}