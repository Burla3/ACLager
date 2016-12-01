using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
            public bool IsDeleted { get; set; }
            [DisplayName("Fremgangsmåde")]
            public string Procedure { get; set; }
            [DisplayName("Stregkode")]
            public string Barcode { get; set; }
            [DisplayName("Batch størrelse")]
            public Nullable<long> BatchSize { get; set; }
            [DisplayName("Afdeling")]
            public string Department { get; set; }
        }
    }
}