using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.Provider;

namespace ACLager.Models {
    [MetadataType(typeof(ItemTypeMetadata))]
    [DisplayName("Varetype")]
    public partial class ItemType {
        private class ItemTypeMetadata {
            [DisplayName("Varetypenummer")]
            public long UID { get; set; }
            [DisplayName("Varenavn")]
            public string Name { get; set; }
            [DisplayName("Mindste beholdning")]
            public long MinimumAmount { get; set; }
            [DisplayName("Måleenhed")]
            public string Unit { get; set; }
            [DisplayName("Status")]
            public bool IsActive { get; set; }
            [DisplayName("Fremgangsmåde")]
            public string Procedure { get; set; }
            [DisplayName("Stregkode")]
            public string Barcode { get; set; }
            [DisplayName("Batch størrelse")]
            public Nullable<long> BatchSize { get; set; }
            [DisplayName("Afdeling")]
            public string Department { get; set; }
        }

        public ItemType ToLoggable() {
            return new ItemType {
                UID = this.UID,
                Name = this.Name,
                MinimumAmount = this.MinimumAmount,
                Unit = this.Unit,
                IsActive = this.IsActive,
                Procedure = this.Procedure,
                Barcode = this.Barcode,
                BatchSize = this.BatchSize,
                Department = this.Department,
                Items = null,
                IngredientsForRecipe = null,
                IsIngredientFor = null,
                WorkOrderItem = null               
            };
        }
    }
}