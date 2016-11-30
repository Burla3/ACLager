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

        public ActionResult Index() {

            IEnumerable<WasteReport> wasteReports;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.Configuration.LazyLoadingEnabled = true;
                wasteReports = db.WasteReportSet.ToList();
            }

            return View(new WasteViewModel(wasteReports));
        }

        [HttpGet]
        public ActionResult CreateWasteReport()
        {

            IEnumerable<Item> items;
            List<SelectListItem> SelectItemList = new List<SelectListItem>();

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                items = db.ItemSet.ToList();

                foreach(Item item in items){
                    SelectItemList.Add(new SelectListItem { Text = item.ItemType.Name, Value = item.UID.ToString() });
                }
            }

            return View(new CreateWasteViewModel(new WasteReport(), SelectItemList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasteReport"></param>
        [HttpPost]
        public ActionResult CreateWasteReport(WasteReport wasteReport)
        {
            HttpCookie cookie = HttpContext.Request.Cookies["UserInfo"];
            dynamic cookieData = System.Web.Helpers.Json.Decode(cookie.Value);
            long userID = cookieData["UID"];

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                wasteReport.WorkOrder = null;
                User dbUser = db.UserSet.Find(userID);
                Item dbItem = db.ItemSet.Find(wasteReport.Item.UID);

                wasteReport.User = dbUser;
                wasteReport.Date = DateTime.Now;
                wasteReport.Item = dbItem;
                db.WasteReportSet.Add(wasteReport);
                db.SaveChanges();
            }

            return View("CreateSucces", new WasteViewModel());
        }
    }
}