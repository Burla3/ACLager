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
        public IEnumerable<SelectListItem> UnitSelectListItems { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public Ingredient Ingredient { get; set; }
        public IEnumerable<SelectListItem> DepartmentSelectListItems { get; set; }
        public IEnumerable<SelectListItem> ItemTypeSelectListItems { get; set; }

        public ItemTypeViewModel() {
            base.SelectSectionSpecials("ItemType");
        }

        public ItemTypeViewModel(IEnumerable<ItemType> itemTypes, ItemType itemType) : this() {
            ItemTypes = itemTypes;
            ItemType = itemType;
            UnitSelectListItems = new[] {
                new SelectListItem() {Text = "Gram"},
                new SelectListItem() {Text = "Stk."},
                new SelectListItem() {Text = "Liter"}
            };
            DepartmentSelectListItems = new[] {
                new SelectListItem() {Text = "Produktion", Value = "Production"},
                new SelectListItem() {Text = "Pakkeri", Value = "Packaging"},
                new SelectListItem() {Text = "Bestilling", Value = "Order"}
            };
        }
    }
}