using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACLager.Models
{
    [MetadataType(typeof(WasteMetadata))]
    public partial class WasteReport
    {
        private class WasteMetadata
        {
            [DisplayName("Mængde")]
            [Required(ErrorMessage = "Der skal angives en misted mængde")]
            [Range(1, long.MaxValue)]
            public long Amount { get; set; }
            public long UID { get; set; }
        }
    }
}