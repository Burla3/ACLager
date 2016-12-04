using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(IngredientMetadata))]
    [DisplayName("Ingrediens")]
    public partial class Ingredient {
        private class IngredientMetadata {
            [DisplayName("Ingrediensnummer")]
            public long UID { get; set; }
            [DisplayName("Mængde")]
            public double Amount { get; set; }
        }

        public Ingredient ToLoggable() {
            return new Ingredient {
                UID = this.UID,
                Amount = this.Amount,
                ItemType = null,
                ForItemType = null
            };
        }
    }
}