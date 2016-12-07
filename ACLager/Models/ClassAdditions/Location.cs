using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACLager.Models {
    [MetadataType(typeof(LocationMetadata))]
    [DisplayName("Lokation")]
    public partial class Location {
        private class LocationMetadata {
            [DisplayName("Lokationsnummer")]
            public long UID { get; set; }
            [DisplayName("Lokationsnavn")]
            [Remote("DoesLocationNameExist", "Location", HttpMethod = "POST", ErrorMessage = "Lokation med samme navn eksistere allerede. Indtast venligst et andet navn.", AdditionalFields = "UID")]
            public string Name { get; set; }
            [DisplayName("Status")]
            public bool IsActive { get; set; }
        }

        public Location ToLoggable() {
            return new Location {
                UID = this.UID,
                Name = this.Name,
                IsActive = this.IsActive,
                Item = null
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            Location location = (Location)obj;
            return UID.Equals(location.UID) &&
                   Name.Equals(location.Name) &&
                   IsActive.Equals(location.IsActive);
        }
    }
}