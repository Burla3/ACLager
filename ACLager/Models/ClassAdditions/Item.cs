using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(ItemMetadata))]
    public partial class Item {
        private class ItemMetadata {
            [DisplayName("Varenummer")]
            public long UID { get; set; }
            [DisplayName("Mængde")]
            [Required(ErrorMessage = "Varen skal have en mængde.")]
            [Range(1, long.MaxValue)]
            public long Amount { get; set; }
            public Nullable<System.DateTime> ExpirationDate { get; set; }
            public System.DateTime DeliveryDate { get; set; }
            [DisplayName("Leverandør")]
            [Required(ErrorMessage = "Varen skal have en leverandør.")]
            public string Supplier { get; set; }
            public long Reserved { get; set; }
            public string IsDeleted { get; set; }
        }
    }
}