using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(WasteReportMetadata))]
    [DisplayName("Spildrapport")]
    public partial class WasteReport {
        private class WasteReportMetadata {
            [DisplayName("Spildrapportnummer")]
            public long UID { get; set; }
            [DisplayName("Dato")]
            public System.DateTime Date { get; set; }
            [DisplayName("Mængde")]
            [Required(ErrorMessage = "Der skal angives en spildt mængde")]
            [Range(1, Double.MaxValue, ErrorMessage = "Feltet Mængde skal være højere end 0")]
            public double Amount { get; set; }
            [DisplayName("Beskrivelse")]
            public string Description { get; set; }
        }

        public WasteReport ToLoggable() {
            return new WasteReport {
                UID = this.UID,
                Date = this.Date,
                Amount = this.Amount,
                Description = this.Description,
                ObjectData = null
            };
        }
    }
}
