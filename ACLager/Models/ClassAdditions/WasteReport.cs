﻿using System;
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
            [Range(1, long.MaxValue)]
            public long Amount { get; set; }
            public long UID { get; set; }
            [DisplayName("Beskrivelse")]
            public string Description { get; set; }
        }
    }
}
