using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    public class HomeController : Controller {
        public List<HomeMenuBlock> homeMenuBlocks = new List<HomeMenuBlock> {
                new HomeMenuBlock("purple", "Lagerstyring", "Noget om at man kan plukke, søge, indsætte og hvad man nu ellers kan.", "Inventory", "Index"),
                new HomeMenuBlock("brown", "Produktion", "Ordrer til produktionsafdelingen.", "WorkOrder", "Production"),
                new HomeMenuBlock("orange", "Pakkeri", "Ordrer til pakkeriet.", "WorkOrder", "Packaging"),
                new HomeMenuBlock("deep-orange", "Spildrapport", "Rapportering af spild.", "Waste", "Index"),
                new HomeMenuBlock("amber", "Brugere", "Håndtering af brugere", "User", "Index"),
                new HomeMenuBlock("blue", "Lokationer", "Håndtering af lokationer", "Location", "Index"),
                new HomeMenuBlock("cyan", "Aktivitetshistorik", "Se aktivitetshistorik", "Log", "Index"),
                new HomeMenuBlock("green", "Oversigt over spild", "Se spildrapporter", "Waste", "OverView"),
                new HomeMenuBlock("indigo", "Artikel typer", "Håndtere artikel typer.", "ItemType", "Index")
            };
        public ActionResult Index() {

            HomeViewModel viewModel = new HomeViewModel {HomeMenuBlocks = homeMenuBlocks};
            
            return View(viewModel);
        }
    }
}