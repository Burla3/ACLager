using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(ItemMetadata))]
    [DisplayName("Vare")]
    public partial class Item {
        private class ItemMetadata {
            [DisplayName("Varenummer")]
            public long UID { get; set; }
            [DisplayName("Mængde")]
            [Required(ErrorMessage = "Varen skal have en mængde.")]
            [Range(1, long.MaxValue)]
            public long Amount { get; set; }
            [DisplayName("Udløbsdato")]
            public Nullable<System.DateTime> ExpirationDate { get; set; }
            [DisplayName("Leveringsdato")]
            public System.DateTime DeliveryDate { get; set; }
            [DisplayName("Leverandør")]
            [Required(ErrorMessage = "Varen skal have en leverandør.")]
            public string Supplier { get; set; }
            [DisplayName("Reserveret")]
            public long Reserved { get; set; }
        }

        public Item Clone() {
            return new Item {
                Amount = this.Amount,
                DeliveryDate = this.DeliveryDate,
                ExpirationDate = this.ExpirationDate,
                ItemType = null,
                Location = null,
                Reserved = this.Reserved,
                Supplier = this.Supplier,
                UID = this.UID,
                WorkOrderItem = null
            };
        }
    }
}