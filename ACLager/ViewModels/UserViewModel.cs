using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class UserViewModel : BaseViewModel {
        public UserViewModel() {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <param name="user"></param>
        public UserViewModel(IEnumerable<User> users, User user) {
            Users = users;
            User = user;
        }

        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}