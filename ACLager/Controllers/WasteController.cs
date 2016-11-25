using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers
{
    public class WasteController : Controller
    {
        // GET: Waste
        public ActionResult Index()
        {
            IEnumerable<WasteReport> wasteReport;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                wasteReport = db.WasteReportSet.ToList();
            }

            return View(new WasteViewModel(wasteReport));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasteReport"></param>

        //[HttpPost]
        //public ActionResult CreateWasteReport(WasteReport wasteReport)
        //{
        //    using (ACLagerDatabase db = new ACLagerDatabase())
        //    {
        //        if (db.WasteReportSet.Find(wasteReport.WorkOrderUID) != null)
        //        
        //            db.WasteReportSet.Add(wasteReport);
        //            db.SaveChanges();
        //        }
        //    }
        //
        //    return RedirectToAction("Index");
        //}
    }
}