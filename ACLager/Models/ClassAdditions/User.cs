﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ACLager.CustomClasses.Attributes;

namespace ACLager.Models {
    [MetadataType(typeof(UserMetadata))]
    [DisplayName("Bruger")]
    public partial class User {
        private class UserMetadata {
            [DisplayName("Brugernummer")]
            public long UID { get; set; }
            [Required(ErrorMessage = "Brugeren skal have et navn")]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Brugerens navn skal være mellem 2 og 50 tegn")]
            [DisplayName("Brugernavn")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Medarbejder-PIN skal indtastes")]
            [DisplayName("Medarbejder-PIN")]
            public short PIN { get; set; }
            [DisplayName("Rolle")]
            public bool IsAdmin { get; set; }
            [DisplayName("Status")]
            public bool IsActive { get; set; }
        }

        public User ToLoggable() {
            return new User {
                UID = this.UID,
                Name = this.Name,
                PIN = this.PIN,
                IsAdmin = this.IsAdmin,
                IsActive = this.IsActive,
                WorkOrders = null
            };
        }
    }
}