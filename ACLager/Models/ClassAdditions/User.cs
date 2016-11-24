using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ACLager.CustomClasses.Attributes;

namespace ACLager.Models {
    [MetadataType(typeof(UserMetadata))]
    public partial class User {
        private class UserMetadata
        {
            public long UID { get; private set; }
            [DisplayName("Aktiv")]
            public bool IsActive { get; set; }
            [Required(ErrorMessage = "Brugeren skal have et navn")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Brugerens navn skal være mellem 2 og 50 tegn")]
            [DisplayName("Navn")]
            public string Name { get; set; }
            [DisplayName("Administrator")]
            public bool IsAdmin { get; set; }
            [Required(ErrorMessage = "Medarbejder-PIN skal indtastes")]
            [DisplayName("Indscan eller indtast din medarbejder-PIN")]
            public short PIN { get; set; }
        }
    }
}