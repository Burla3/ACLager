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
            [Range(0, Double.MaxValue)]
            public double Amount { get; set; }
            [DisplayName("Udløbsdato")]
            public Nullable<System.DateTime> ExpirationDate { get; set; }
            [DisplayName("Leveringsdato")]
            public System.DateTime DeliveryDate { get; set; }
            [DisplayName("Leverandør")]
            [Required(ErrorMessage = "Varen skal have en leverandør.")]
            public string Supplier { get; set; }
            [DisplayName("Reserveret")]
            public double Reserved { get; set; }
        }

        public Item ToLoggable() {
            return new Item {
                UID = this.UID,
                Amount = this.UID,
                ExpirationDate = this.ExpirationDate,
                DeliveryDate = this.DeliveryDate,
                Supplier = this.Supplier,
                Reserved = this.Reserved,
                ItemType = null,
                Location = null,
                WorkOrderItem = null
            };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            Item item = (Item)obj;
            return UID.Equals(item.UID) &&
                   Amount.Equals(item.Amount) &&
                   ExpirationDate.Equals(item.ExpirationDate) &&
                   DeliveryDate.Equals(item.DeliveryDate) &&
                   Supplier.Equals(item.Supplier) &&
                   Reserved.Equals(item.Reserved) &&
                   Location.Equals(item.Location) &&
                   ItemType.Equals(item.ItemType);
        }
    }
}