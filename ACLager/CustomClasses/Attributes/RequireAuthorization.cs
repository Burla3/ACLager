using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using ACLager.Controllers;

namespace ACLager.CustomClasses.Attributes {
    public class RequireAuthorization : ActionFilterAttribute {
        public bool IsDisabled { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!IsDisabled) {
                bool redirect = false;
                bool cookieExists = filterContext.HttpContext.Request.Cookies.AllKeys.Contains("UserInfo");

                if (cookieExists) {
                    HttpCookie cookie = filterContext.HttpContext.Request.Cookies["UserInfo"];

                    if (cookie.Value != "LoggedOut" && cookie.Value != String.Empty) {
                        // Do nothing
                    } else {
                        redirect = true;
                    }
                } else {
                    redirect = true;
                }

                if (redirect) {
                    filterContext.Result = new RedirectResult("/Login?nextAction=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.AbsolutePath));
                }
            }
        }
    }
}