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
            [Required(ErrorMessage = "Varetypen skal have et navn")]
            [StringLength(127, MinimumLength = 1, ErrorMessage = "Varetypens navn skal være mellem 1 og 127 tegn")]
            [DisplayName("Varetypenavn")]
            public string Name { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "Mindste mængde skal være positiv")]
            [DisplayName("Mindste beholdning")]
            public decimal MinimumAmount { get; set; }
            [DisplayName("Måleenhed")]
            public string Unit { get; set; }
            [DisplayName("Status")]
            public bool IsActive { get; set; }
            [DisplayName("Fremgangsmåde")]
            public string Procedure { get; set; }
            [DisplayName("Stregkode")]
            public string Barcode { get; set; }
            [Range(1, long.MaxValue, ErrorMessage = "Batch størrelsen skal være højere end 0")]
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
                WorkOrderItems = null,
                WorkOrders = null
            };
        }
    }
}