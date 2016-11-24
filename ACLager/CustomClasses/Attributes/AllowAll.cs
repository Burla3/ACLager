using System.Web.Mvc;

namespace ACLager.CustomClasses.Attributes {
    public class AllowAll : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Do nothing
        }
    }
}