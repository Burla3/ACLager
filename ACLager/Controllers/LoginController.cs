using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.CustomClasses.Attributes;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    public class LoginController : Controller {
        [RequireAuthorization(IsDisabled = true)]
        [HttpGet]
        public ActionResult Index() {
            return View(new LoginViewModel { RenderNavBar = false});
        }

        [RequireAuthorization(IsDisabled = true)]
        [HttpPost]
        public ActionResult Index(User user) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                if (db.UserSet.Any(u => u.PIN == user.PIN && u.IsActive)) {
                    User dbUser = db.UserSet.First(u => u.PIN == user.PIN);

                    object cookieData = new {dbUser.UID, dbUser.Name, dbUser.IsAdmin};

                    HttpCookie cookie = new HttpCookie("UserInfo", System.Web.Helpers.Json.Encode(cookieData));
                    Response.Cookies.Add(cookie);

                    string nextAction = HttpUtility.UrlDecode(HttpContext.Request["nextAction"]);

                    if (!string.IsNullOrEmpty(nextAction)) {
                        return Redirect(nextAction);
                    } else {
                        return RedirectToAction("Index", "Home");
                    }
                } else if (db.UserSet.Any(u => u.PIN == user.PIN && !u.IsActive)) {
                    return View(new LoginViewModel { RenderNavBar = false, RenderUserIsDeactivated = true});
                } else {
                    return View(new LoginViewModel { RenderNavBar = false, RenderUserNotFoundWarning = true});
                }
            }
        }

        public ActionResult Logout() {
            Response.Cookies["UserInfo"].Value = "LoggedOut";
            return RedirectToAction("Index");
        }

        public ActionResult NoPermission() {
            HttpContext.Response.AppendHeader("Refresh", "5;url=" + Url.Action("Index", "Home"));
            return View(new BaseViewModel());
        }
    }
}