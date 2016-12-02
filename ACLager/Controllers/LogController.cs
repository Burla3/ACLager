using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.ViewModels;
using Microsoft.Owin.Security.Provider;

namespace ACLager.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            IEnumerable<LogEntry> logEntries;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                logEntries = db.LogEntrySet.ToList();
            }

            return View(new LogViewModel(logEntries, null));
        }

        [HttpGet]
        public ActionResult Detailed(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            LogViewModel logViewModel = new LogViewModel();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                logViewModel.LogEntry = db.LogEntrySet.Find(Int64.Parse(id));
            }

            return View(logViewModel);
        }

        public ActionResult FillDatabase()
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                db.UserSet.Add(new User() { Name = "Admin", IsActive = true, IsAdmin = true, PIN = 1234 });

                for (int i = 1; i < 5; i++)
                {
                    Location location = new Location() {IsActive = true, Name = "B" + i};
                    Location location2 = new Location() { IsActive = true, Name = "C" + i };
                    db.LocationSet.Add(location);
                    db.LocationSet.Add(location2);

                    ItemType itemType = new ItemType()
                    {
                        Barcode = "BARCODE",
                        Department = "DEPARTMENT",
                        IsActive = true,
                        MinimumAmount = 1234,
                        Name = "ItemType" + i,
                        Unit = "Gram",
                        Procedure = "Some procedure",
                        BatchSize = 765
                    };
                    ItemType itemType2 = new ItemType()
                    {
                        Barcode = "BARCODE",
                        Department = "DEPARTMENT",
                        IsActive = true,
                        MinimumAmount = 1234,
                        Name = "Item2Type" + i,
                        Unit = "Gram",
                        Procedure = "Some procedure",
                        BatchSize = 765
                    };
                    db.ItemTypeSet.Add(itemType);
                    db.ItemTypeSet.Add(itemType2);

                    Item item = new Item()
                    {
                        ItemType = itemType,
                        Amount = i*50,
                        DeliveryDate = DateTime.Now,
                        Location = location,
                        Supplier = "SCRIPT", 
                        Reserved = 0,
                        ExpirationDate = DateTime.MaxValue
                    };
                    Item item2 = new Item()
                    {
                        ItemType = itemType2,
                        Amount = i * 23,
                        DeliveryDate = DateTime.Now,
                        Location = location2,
                        Supplier = "SCRIPT",
                        Reserved = 0,
                        ExpirationDate = DateTime.MaxValue
                    };
                    db.ItemSet.Add(item);
                    db.ItemSet.Add(item2);

                    Ingredient ingredient = new Ingredient() {ItemType = itemType, Amount = i*4, ForItemType = itemType2};
                    db.IngredientSet.Add(ingredient);

                    WorkOrder workOrder = new WorkOrder() {BatchNumber = i*12, IsComplete = false, Type = "Production"};
                    db.WorkOrderSet.Add(workOrder);

                    WorkOrderItem workOrderItem = new WorkOrderItem() {Amount = i*56, Progress = 0, WorkOrder = workOrder, ItemType = itemType};
                    WorkOrderItem workOrderItem2 = new WorkOrderItem() { Amount = i * 31, Progress = 2, WorkOrder = workOrder, ItemType = itemType2 };
                    db.WorkOrderItemSet.Add(workOrderItem);
                    db.WorkOrderItemSet.Add(workOrderItem2);
                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("Index");
        }
    }
}