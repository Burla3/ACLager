using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers {
    public class WasteController : Controller {
        // GET: Waste
        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasteReport"></param>
        [HttpPost]
        public void CreateWasteReport(WasteReport wasteReport) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.WasteReportSet.Add(wasteReport);
                db.SaveChanges();
            }
        }
    }
}