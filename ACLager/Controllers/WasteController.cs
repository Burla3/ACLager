using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.CustomClasses.Attributes;
using ACLager.Interfaces;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    [AdminOnly]
    public class WasteController : Controller, ILoggable {
        public WasteController() {
            new Logger().Subcribe(this);
        }

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
            if (id == null) {
                return RedirectToAction("Index");
            }

            WasteViewModel wasteViewModel = new WasteViewModel();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                WasteReport wasteReport = db.WasteReportSet.Find(Int64.Parse(id));

                dynamic objectData = System.Web.Helpers.Json.Decode(wasteReport.ObjectData);
                wasteViewModel.WasteReportGroup = new WasteReportGroup(wasteReport, objectData);
            }

            return View(wasteViewModel);
        }

        [HttpGet]
        public ActionResult Create() {
            List<SelectListItem> itemTypeSelectList = new List<SelectListItem>();
            List<SelectListItem> workorderSelectList = new List<SelectListItem>();
            List<SelectListItem> locationSelectList = new List<SelectListItem>();

            using (ACLagerDatabase db = new ACLagerDatabase()) {

                foreach(ItemType itemType in db.ItemTypeSet) {
                    itemTypeSelectList.Add(new SelectListItem { Text = itemType.Name, Value = itemType.UID.ToString() });
                }

                workorderSelectList.Add(new SelectListItem { Text = "Ingen ordre", Value = "-1" });
                foreach (WorkOrder work in db.WorkOrderSet) {
                    workorderSelectList.Add(new SelectListItem { Text = work.UID.ToString(), Value = work.UID.ToString() });
                }

                locationSelectList.Add(new SelectListItem { Text = "Ingen lokation", Value = "-1" });
                foreach (Location location in db.LocationSet) {
                    locationSelectList.Add(new SelectListItem { Text = location.Name, Value = location.UID.ToString() });
                }

            }

            return View(new CreateWasteViewModel(new WasteReport(), itemTypeSelectList, workorderSelectList, locationSelectList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wasteReportViewModel"></param>
        [HttpPost]
        public ActionResult Create(CreateWasteViewModel wasteReportViewModel) {
            WorkOrder dbWorkOrder;
            ItemType dbItemType;
            Location dbLocation;

            WasteReport wasteReport = wasteReportViewModel.WasteReport;
            wasteReport.Date = DateTime.Now;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbItemType = db.ItemTypeSet.Find(wasteReportViewModel.ItemType.UID).ToLoggable();
                dbWorkOrder = db.WorkOrderSet.Find(wasteReportViewModel.WorkOrder.UID)?.ToLoggable();
                dbLocation = db.LocationSet.Find(wasteReportViewModel.Location.UID)?.ToLoggable();
            }

            object objectData = new {
                    ContextUser = UserController.GetContextUser().ToLoggable(),
                    WorkOrder = dbWorkOrder,
                    Location = dbLocation,
                    ItemType = dbItemType
                };

            wasteReport.ObjectData = System.Web.Helpers.Json.Encode(objectData);

            using (ACLagerDatabase db = new ACLagerDatabase()) { 
                db.WasteReportSet.Add(wasteReport);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Spild rapport oprettet",
                        $"Spild rapport med ID: {wasteReport.UID} oprettet.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            SpildRapport = wasteReport.ToLoggable(),
                            Varetype = dbItemType.ToLoggable(),
                            Lokation = dbLocation?.ToLoggable(),
                            Ordre = dbWorkOrder?.ToLoggable()
                        }
                    )
            );

            return View("CreateSucces", new WasteViewModel());
        }
        public event ChangedEventHandler Changed;
    }   
}