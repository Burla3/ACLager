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
                new HomeMenuBlock("purple", "Lagerstyring", "Tilføj, rediger og pluk lagervarer.", "Inventory", "Index", false),
                new HomeMenuBlock("brown", "Produktion", "Ordrer til produktionen.", "WorkOrder", "Production", false),
                new HomeMenuBlock("orange", "Pakkeri", "Ordrer til pakkeriet.", "WorkOrder", "Packaging", false),
                new HomeMenuBlock("deep-orange", "Spildrapport", "Rapportering af spild.", "Waste", "Create", false),
                new HomeMenuBlock("amber", "Brugere", "Håndtering af brugere.", "User", "Index", true),
                new HomeMenuBlock("indigo", "Varetyper", "Håndtering af varetyper.", "ItemType", "Index", true),
                new HomeMenuBlock("blue", "Lokationer", "Håndtering af lokationer.", "Location", "Index", true),
                new HomeMenuBlock("cyan", "Aktivitetshistorik", "Se aktivitetshistorik.", "Log", "Index", true),
                new HomeMenuBlock("green", "Oversigt over spild", "Se spildrapporter.", "Waste", "Index", true)
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