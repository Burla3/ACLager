using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.CustomClasses;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class LocationViewModel : BaseViewModel {
        public LocationViewModel() {
            base.SectionColor = "blue";
        }

        public LocationViewModel(IEnumerable<ItemLocationPair> itemLocationPairs, ItemLocationPair itemLocationPair) : this() {
            ItemLocationPairs = itemLocationPairs;
            ItemLocationPair = itemLocationPair;
        }

        public IEnumerable<ItemLocationPair> ItemLocationPairs { get; set; }
        public ItemLocationPair ItemLocationPair { get; set; }
    }
}