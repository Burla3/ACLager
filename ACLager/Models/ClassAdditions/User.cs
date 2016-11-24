using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ACLager.CustomClasses.Attributes;

namespace ACLager.Models {
    [MetadataType(typeof(UserMetadata))]
    public partial class User {
        private class UserMetadata
        {
            [DisplayName("Brugernummer")]
            public long UID { get; set; }
            [Required(ErrorMessage = "Brugeren skal have et navn")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Brugerens navn skal være mellem 2 og 50 tegn")]
            [DisplayName("Navn")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Medarbejder-PIN skal indtastes")]
            [DisplayName("Medarbejder-PIN")]
            public short PIN { get; set; }
            [DisplayName("Administrator")]
            public bool IsAdmin { get; set; }
            [DisplayName("Aktiv")]
            public bool IsActive { get; set; }
            [DisplayName("Slettet")]
            public bool IsDeleted { get; set; }
        }
    }
}