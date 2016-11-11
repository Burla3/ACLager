using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class InventoryViewModel : BaseViewModel {
        public InventoryViewModel(IEnumerable<Item> items) {
            Items = items;
        }

        public IEnumerable<Item> Items { get; set; }
    }
}