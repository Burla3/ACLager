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
                    wasteReportGroups.Add(new WasteReportGroup(wasteReport, objectData));
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
                wasteViewModel.WasteReportGroup = new WasteReportGroup(wasteReport, objectData);
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
        /// <param name="wasteReportViewModel"></param>
        [HttpPost]
        public ActionResult CreateWasteReport(CreateWasteViewModel wasteReportViewModel)
        {
            HttpCookie cookie = HttpContext.Request.Cookies["UserInfo"];
            dynamic cookieData = System.Web.Helpers.Json.Decode(cookie.Value);
            long userID = cookieData["UID"];

            WorkOrder dbWorkOrder;
            User dbUser;
            Item dbItem;
            ItemType dbItemType;

            WasteReport wasteReport = wasteReportViewModel.WasteReport;
            wasteReport.Date = DateTime.Now;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbWorkOrder = db.WorkOrderSet.Find(wasteReportViewModel.WorkOrderUID);
                dbUser = db.UserSet.Find(userID);
                dbItem = db.ItemSet.Find(wasteReportViewModel.Item.UID);
                dbItemType = dbItem.ItemType;
            }
            dbUser = dbUser.Clone();
            dbItem = dbItem.Clone();
            dbItemType = dbItemType.Clone();

            object objectData = new {
                    ContextUser = dbUser,
                    Item = dbItem,
                    WorkOrder = dbWorkOrder,
                    ItemType = dbItemType
                };

            wasteReport.ObjectData = System.Web.Helpers.Json.Encode(objectData);

            using (ACLagerDatabase db = new ACLagerDatabase()) { 
                db.WasteReportSet.Add(wasteReport);
                db.SaveChanges();
            }

            return View("CreateSucces", new WasteViewModel());
        }
    }
}