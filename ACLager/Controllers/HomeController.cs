using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models.ViewModels;

namespace ACLager.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            BaseViewModel viewModel = new BaseViewModel {RenderNavBar = false};
            return View(viewModel);
        }
    }
}