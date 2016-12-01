using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers
{
    public class WasteController : Controller
    {

        public ActionResult Index() {

            List<WasteReportGroup> wasteReportGroups = new List<WasteReportGroup>();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                foreach (WasteReport wasteReport in db.WasteReportSet) {
                    dynamic objectData = System.Web.Helpers.Json.Decode(wasteReport.ObjectData);
                    wasteReportGroups.Add(new WasteReportGroup(objectData["ItemType"], objectData["WorkOrder"], objectData["User"], wasteReport, objectData["item"]));
                }
            }

            return View(new WasteViewModel(wasteReportGroups, null));
        }

        [HttpGet]
        public ActionResult Detailed(string id) {
            WasteViewModel wasteViewModel = new WasteViewModel();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                WasteReport wasteReport = db.WasteReportSet.Find(Int64.Parse(id));

                dynamic objectData = System.Web.Helpers.Json.Decode(wasteReport.ObjectData);
                wasteViewModel.WasteReportGroup = new WasteReportGroup(objectData["ItemType"], objectData["WorkOrder"], objectData["UserContextUser"], wasteReport, objectData["item"]);
            }

            return View(wasteViewModel);
        }

        [HttpGet]
        public ActionResult CreateWasteReport()
        {

            IEnumerable<Item> items;
            List<SelectListItem> SelectItemList = new List<SelectListItem>();

            IEnumerable<WorkOrder> workorder;
            List<SelectListItem> selectWorkorderList = new List<SelectListItem>();

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                items = db.ItemSet.ToList();
                workorder = db.WorkOrderSet.ToList();

                foreach(Item item in items){
                    SelectItemList.Add(new SelectListItem { Text = item.ItemType.Name, Value = item.UID.ToString() });
                }

                foreach (WorkOrder work in workorder){
                    selectWorkorderList.Add(new SelectListItem { Text = work.UID.ToString(), Value = work.UID.ToString() });
                }

            }

            return View(new CreateWasteViewModel(new WasteReport(), SelectItemList, selectWorkorderList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasteReport"></param>
        [HttpPost]
        public ActionResult CreateWasteReport(CreateWasteViewModel wasteReportViewModel)
        {
            HttpCookie cookie = HttpContext.Request.Cookies["UserInfo"];
            dynamic cookieData = System.Web.Helpers.Json.Decode(cookie.Value);
            long userID = cookieData["UID"];

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                WorkOrder workOrder = db.WorkOrderSet.Find(wasteReportViewModel.WorkOrder.UID);
                User dbUser = db.UserSet.Find(userID);
                Item dbItem = db.ItemSet.Find(wasteReportViewModel.Item.UID);
                ItemType dbItemType = dbItem.ItemType;

                object objectData = new {
                    ContextUser = new {Type = dbUser.GetType().ToString(), Data = dbUser },
                    Item = new {Type = dbItem.GetType().ToString(), Data = dbItem },
                    WorkOrder = new {Type = workOrder.GetType().ToString(), Data = workOrder },
                    ItemType = new {Type = dbItemType.GetType().ToString(), Data = dbItemType }
                };

                WasteReport wasteReport = wasteReportViewModel.WasteReport;

                wasteReport.Date = DateTime.Now;
                wasteReport.ObjectData = System.Web.Helpers.Json.Encode(objectData);

                db.WasteReportSet.Add(wasteReport);
                db.SaveChanges();
            }

            return View("CreateSucces", new WasteViewModel());
        }
    }
}