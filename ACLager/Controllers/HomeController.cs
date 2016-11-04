using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {

            List<HomeMenuBlock> homeMenuBlocks = new List<HomeMenuBlock> {
                new HomeMenuBlock("purple", "Lagerstyring", "Noget om at man kan plukke, søge, indsætte og hvad man nu ellers kan."),
                new HomeMenuBlock("brown", "Produktion", "Ordrer til produktionsafdelingen."),
                new HomeMenuBlock("orange", "Pakkeri", "Ordrer til pakkeriet."),
                new HomeMenuBlock("deep-orange", "Spildrapport", "Rapportering af spild."),
                new HomeMenuBlock("amber", "Brugere", "Håndtering af brugere"),
                new HomeMenuBlock("blue", "Lokationer", "Håndtering af lokationer"),
                new HomeMenuBlock("cyan", "Aktivitetshistorik", "Se aktivitetshistorik"),
                new HomeMenuBlock("green", "Oversigt over spild", "Se spildrapporter")
            };

            HomeViewModel viewModel = new HomeViewModel {HomeMenuBlocks = homeMenuBlocks};
            
            return View(viewModel);
        }
    }
}