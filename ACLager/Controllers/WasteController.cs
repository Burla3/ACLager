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

            IEnumerable<ItemType> theitemtypes;
            List<SelectListItem> SelectItemtypeUIDList = new List<SelectListItem>();

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                wasteReport = db.WasteReportSet.ToList();
                theitemtypes = db.ItemTypeSet.Where(it => it.IsActive);

                foreach(ItemType item in theitemtypes){
                    SelectItemtypeUIDList.Add(new SelectListItem { Text = item.Name, Value = item.UID.ToString() });
                }
            }

            return View(new WasteViewModel(wasteReport, new WasteReport(), SelectItemtypeUIDList));
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

            return View("Create", new WasteViewModel());
        }
    }
}