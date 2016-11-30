using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(LocationMetadata))]
    [DisplayName("Lokation")]
    public partial class Location {
        private class LocationMetadata {
            [DisplayName("Lokationsnummer")]
            public long UID { get; set; }
            [DisplayName("Navn")]
            public string Name { get; set; }
            [DisplayName("Status")]
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}