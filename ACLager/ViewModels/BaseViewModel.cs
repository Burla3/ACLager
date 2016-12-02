using ACLager.Controllers;
using ACLager.CustomClasses;
using ACLager.Models;
using System.Diagnostics;
using System.Web;
using System.Web.Helpers;

namespace ACLager.ViewModels {
    public class BaseViewModel {
        public bool RenderNavBar { get; set; } = true;
        public string SectionColor { get; set; } = "teal";
        public string SectionName { get; set; } = "";
        public bool UserIsAdmin {
            get {
                Debug.WriteLine("noget fra baseviewmodel");
                HttpCookie cookie = HttpContext.Current.Request.Cookies["UserInfo"];
                return Json.Decode(cookie?.Value)["IsAdmin"];
            }
        }

        public BaseViewModel() {
            
        }

        public void SelectSectionSpecials(string controllername) {
            HomeController hc = new HomeController();
            SectionColor = "teal";
            SectionName = "";
            foreach (HomeMenuBlock hmb in hc.homeMenuBlocks) {
                if ((hmb.Controller == controllername) || ((hmb.Controller + "-" + hmb.Action) == controllername)) {
                    SectionColor = hmb.Color;
                    SectionName = hmb.Title;
                }
            }
            
        }
        

    }
}