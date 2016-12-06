using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models {
    [MetadataType(typeof(LogEntryMetadata))]
    [DisplayName("Journallinje")]
    public partial class LogEntry {
        private class LogEntryMetadata {
            [DisplayName("Journallinjenummer")]
            public long UID { get; set; }
            [DisplayName("Type")]
            public string Type { get; set; }
            [DisplayName("Tidspunkt")]
            public System.DateTime Date { get; set; }
            [DisplayName("Beskrivelse")]
            public string LogBody { get; set; }
        }
    }
}