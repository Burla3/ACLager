using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class ItemTypeViewModel : BaseViewModel {
        public IEnumerable<ItemType> ItemTypes { get; set; }
        public ItemType ItemType { get; set; }
        public IEnumerable<SelectListItem> SelectListItems { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }

        public ItemTypeViewModel() {
            base.SelectColor("ItemType");
        }

        public ItemTypeViewModel(IEnumerable<ItemType> itemTypes, ItemType itemType) : this() {
            ItemTypes = itemTypes;
            ItemType = itemType;
            SelectListItems = new[] {
                new SelectListItem() {Text = "Gram"},
                new SelectListItem() {Text = "Stk."},
                new SelectListItem() {Text = "Liter"}
            };
        }
    }
}