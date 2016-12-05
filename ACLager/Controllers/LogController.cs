using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses.Attributes;
using ACLager.Models;
using ACLager.ViewModels;
using Microsoft.Owin.Security.Provider;

namespace ACLager.Controllers {
    [AdminOnly]
    public class LogController : Controller {
        // GET: Log
        public ActionResult Index() {
            IEnumerable<LogEntry> logEntries;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                logEntries = db.LogEntrySet.OrderByDescending(le => le.Date).ToList();
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

        public ActionResult FillDatabase() {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.UserSet.Add(new User {
                    Name = "Admin",
                    IsActive = true,
                    IsAdmin = true,
                    PIN = 1234
                });

                for (int i = 1; i < 10; i++) {
                    Location location = new Location {
                        IsActive = true,
                        Name = "B" + i
                    };
                    Location location2 = new Location {
                        IsActive = true,
                        Name = "C" + i
                    };
                    Location location3 = new Location {
                        IsActive = true,
                        Name = "D" + i
                    };
                    Location location4 = new Location {
                        IsActive = true,
                        Name = "E" + i
                    };
                    db.LocationSet.Add(location);
                    db.LocationSet.Add(location2);
                    db.LocationSet.Add(location3);
                    db.LocationSet.Add(location4);

                    ItemType itemType = new ItemType {
                        Barcode = "BARCODE",
                        Department = "DEPARTMENT",
                        IsActive = true,
                        MinimumAmount = 1234,
                        Name = i * 43 % 100 + "% Chokolade",
                        Unit = "Gram",
                        Procedure = "Some procedure",
                        BatchSize = 765
                    };
                    ItemType itemType2 = new ItemType {
                        Barcode = "BARCODE",
                        Department = "DEPARTMENT",
                        IsActive = true,
                        MinimumAmount = 1234,
                        Name = i * 33 % 100 + "% Hvid Chokolade",
                        Unit = "Gram",
                        Procedure = "Some procedure",
                        BatchSize = 765
                    };
                    db.ItemTypeSet.Add(itemType);
                    db.ItemTypeSet.Add(itemType2);

                    Item item = new Item {
                        ItemType = itemType,
                        Amount = i*567,
                        DeliveryDate = DateTime.Now,
                        Location = location,
                        Supplier = "SCRIPT", 
                        Reserved = 0,
                        ExpirationDate = DateTime.MaxValue
                    };
                    Item item2 = new Item {
                        ItemType = itemType2,
                        Amount = i * 555,
                        DeliveryDate = DateTime.Now,
                        Location = location2,
                        Supplier = "SCRIPT",
                        Reserved = 0,
                        ExpirationDate = DateTime.MaxValue
                    };
                    Item item3 = new Item {
                        ItemType = itemType2,
                        Amount = i * 2,
                        DeliveryDate = DateTime.Now,
                        Location = location3,
                        Supplier = "SCRIPT",
                        Reserved = 0,
                        ExpirationDate = DateTime.Now.AddDays(100),
                    };
                    Item item4 = new Item {
                        ItemType = itemType2,
                        Amount = i * 3,
                        DeliveryDate = DateTime.Now,
                        Location = location4,
                        Supplier = "SCRIPT",
                        Reserved = 0,
                        ExpirationDate = DateTime.Now,
                    };
                    db.ItemSet.Add(item);
                    db.ItemSet.Add(item2);
                    db.ItemSet.Add(item3);
                    db.ItemSet.Add(item4);

                    Ingredient ingredient = new Ingredient {
                        ItemType = itemType,
                        Amount = i*4,
                        ForItemType = itemType2
                    };
                    db.IngredientSet.Add(ingredient);

                    Random rn = new Random();

                    WorkOrder workOrder = new WorkOrder {
                        BatchNumber = i*12,
                        IsComplete = false,
                        Type = rn.Next(Int32.MinValue, Int32.MaxValue) % 2 == 0 ? "Produktion" : "Pakkeri",
                        DueDate = DateTime.Now.AddMonths(i*3),
                        OrderNumber = i*3,
                        ShippingInfo = "Send til MATHIAS MED DET SAMME! UDEN KAGE!"
                    };
                    db.WorkOrderSet.Add(workOrder);

                    WorkOrderItem workOrderItem = new WorkOrderItem {
                        Amount = 5,
                        Progress = 0,
                        WorkOrder = workOrder,
                        ItemType = itemType
                    };
                    WorkOrderItem workOrderItem2 = new WorkOrderItem {
                        Amount = 99,
                        Progress = 0,
                        WorkOrder = workOrder,
                        ItemType = itemType2
                    };
                    db.WorkOrderItemSet.Add(workOrderItem);
                    db.WorkOrderItemSet.Add(workOrderItem2);
                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("Index");
        }
    }
}