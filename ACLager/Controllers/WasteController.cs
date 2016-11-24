using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers
{
    public class WasteController : Controller
    {
        // GET: Waste
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasteReport"></param>
        [HttpPost]
        public void CreateWasteReport(WasteReport wasteReport)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                var dbWasteReport = (from wastereport in db.WasteReportSet
                                     where wastereport.Amount == wasteReport.Amount && wastereport.WorkOrderUID == wasteReport.WorkOrderUID
                                     && wastereport.ItemUID == wasteReport.ItemUID && wastereport.UserUID == wasteReport.UserUID select wastereport);

                if (dbWasteReport.Any())
                {
                    //similar wastereport(s) exsist, generate anyways?
                }
                else
                {
                    db.WasteReportSet.Add(wasteReport);
                    db.SaveChanges();
                }
                
            }
        }
    }
}