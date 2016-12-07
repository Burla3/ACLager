using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    public class HomeController : Controller {
        public List<HomeMenuBlock> homeMenuBlocks = new List<HomeMenuBlock> {
                new HomeMenuBlock("purple", "Varer", "Tilføj, rediger og pluk lagervarer.", "Inventory", "Index", false),
                new HomeMenuBlock("brown", "Produktion", "Påbegynd og afslut ordrer til produktion.", "WorkOrder", "Production", false),
                new HomeMenuBlock("orange", "Pakkeri", "Påbegynd og afslut ordrer til pakkeri.", "WorkOrder", "Packaging", false),
                new HomeMenuBlock("indigo", "Varetyper", "Tilføj, rediger og slet varetyper samt opskrifter.", "ItemType", "Index", true),
                new HomeMenuBlock("deep-orange", "Spildrapport", "Rapportér spild.", "Waste", "Create", false),
                new HomeMenuBlock("green", "Spildoversigt", "Se spildrapporter.", "Waste", "Index", true),
                new HomeMenuBlock("amber", "Brugere", "Tilføj, rediger og slet brugere.", "User", "Index", true),
                new HomeMenuBlock("blue", "Lokationer", "Tilføj, rediger og slet lokationer.", "Location", "Index", true),
                new HomeMenuBlock("cyan", "Aktivitetshistorik", "Se aktivitetshistorik.", "Log", "Index", true)
            };

        public ActionResult Index() {
            List<HomeMenuBlock> renderedHomeMenuBlocks = new List<HomeMenuBlock>();

            HttpCookie cookie = HttpContext.Request.Cookies["UserInfo"];

            dynamic cookieData = System.Web.Helpers.Json.Decode(cookie.Value);
            bool isAdmin = cookieData["IsAdmin"];

            foreach (HomeMenuBlock homeMenuBlock in homeMenuBlocks) {
                bool render = false;

                if (!homeMenuBlock.IsAdminOnly) {
                    render = true;
                } else if (homeMenuBlock.IsAdminOnly && isAdmin) {
                    render = true;
                } else {
                    // Do not render
                }

                if (render){
                    renderedHomeMenuBlocks.Add(homeMenuBlock);
                }
            }
            
            return View(new HomeViewModel {HomeMenuBlocks = renderedHomeMenuBlocks});
        }
    }
}