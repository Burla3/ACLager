using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ACLager.CustomClasses.Attributes {
    public class AdminOnly : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies["UserInfo"];

            dynamic cookieData = Json.Decode(cookie.Value);
            bool isAdmin = cookieData["IsAdmin"];

            if (isAdmin) {
                // Do nothing
            } else {
                filterContext.Result = new RedirectResult("/Login/NoPermission");
            }
        }
    }
}