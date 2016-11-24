using System;
using ACLager.Models;

namespace ACLager.ViewModels {
    public class LoginViewModel : BaseViewModel {
        public User User { get; set; } = new User();
        public bool RenderUserNotFoundWarning { get; set; } = false;
    }
}