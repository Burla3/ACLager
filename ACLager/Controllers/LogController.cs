using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
                        Department = "Produktion",
                        IsActive = true,
                        MinimumAmount = 1234,
                        Name = i * 43 % 100 + "% Chokolade",
                        Unit = "Gram",
                        Procedure = "Bland lortet.",
                        BatchSize = 765
                    };
                    ItemType itemType2 = new ItemType {
                        Barcode = "BARCODE",
                        Department = "Produktion",
                        IsActive = true,
                        MinimumAmount = 1234,
                        Name = i * 33 % 100 + "% Hvid Chokolade",
                        Unit = "Kg",
                        Procedure = "Bland lortet.",
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

                    WorkOrder workOrder = new WorkOrder {
                        BatchNumber = i*12,
                        IsComplete = false,
                        Type = "Produktion",
                        ItemType = itemType,
                        Amount = 100,
                        Progress = 0,
                        DueDate = DateTime.Now.AddMonths(i*3),
                        OrderNumber = i*3,
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

                    WorkOrder workOrder2 = new WorkOrder {
                        BatchNumber = i * 12,
                        IsComplete = false,
                        Type = "Pakkeri",
                        DueDate = DateTime.Now.AddMonths(i * 3),
                        OrderNumber = i * 3,
                        ShippingInfo = "Send til MATHIAS MED DET SAMME! UDEN KAGE!"
                    };
                    db.WorkOrderSet.Add(workOrder2);

                    WorkOrderItem workOrderItem3 = new WorkOrderItem {
                        Amount = 5,
                        Progress = 0,
                        WorkOrder = workOrder2,
                        ItemType = itemType
                    };
                    WorkOrderItem workOrderItem4 = new WorkOrderItem {
                        Amount = 99,
                        Progress = 0,
                        WorkOrder = workOrder2,
                        ItemType = itemType2
                    };

                    db.WorkOrderItemSet.Add(workOrderItem3);
                    db.WorkOrderItemSet.Add(workOrderItem4);
                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("Index");
        }

        [RequireAuthorization(IsDisabled = true)]
        public ActionResult TestDatabase() {

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                List<User> users = new List<User>();
                users.Add( new User {
                    IsActive = true,
                    IsAdmin = true,
                    Name = "Mads",
                    PIN = 5746,
                });
                users.Add(new User {
                    IsActive = true,
                    IsAdmin = true,
                    Name = "Nicholaine",
                    PIN = 8967,
                });
                users.Add(new User {
                    IsActive = true,
                    IsAdmin = false,
                    Name = "Børge",
                    PIN = 4356,
                });
                users.Add(new User {
                    IsActive = false,
                    IsAdmin = false,
                    Name = "Kurt",
                    PIN = 2134,
                });
                users.Add(new User {
                    IsActive = true,
                    IsAdmin = false,
                    Name = "Egon",
                    PIN = 7634,
                });
                users.Add(new User {
                    IsActive = true,
                    IsAdmin = false,
                    Name = "Mathilde",
                    PIN = 3323,
                });
                users.Add(new User {
                    IsActive = true,
                    IsAdmin = false,
                    Name = "Daniel",
                    PIN = 7862,
                });
                users.Add(new User {
                    IsActive = true,
                    IsAdmin = false,
                    Name = "Line",
                    PIN = 7623,
                });
                users.Add(new User {
                    IsActive = true,
                    IsAdmin = false,
                    Name = "Oskar",
                    PIN = 7345,
                });
                db.UserSet.AddRange(users);

                ItemType kokosbrød = new ItemType {
                    Name = "Kokosbrød",
                    BatchSize = 750,
                    Unit = "stk.",
                    Department = "Produktion",
                    IsActive = true,
                    MinimumAmount = 0,
                    Procedure = "Tag chokoladebrød og drys med kokos."
                };
                ItemType chokolademus = new ItemType {
                    Name = "Chokolademus",
                    BatchSize = 750,
                    Unit = "stk.",
                    Department = "Produktion",
                    IsActive = true,
                    MinimumAmount = 0,
                    Procedure = "Tag marcipanmus og dyp den i opvarmet chokolade."
                };
                ItemType marcipnamus = new ItemType {
                    Name = "Marcipanmus",
                    BatchSize = 750,
                    Unit = "stk.",
                    Department = "Produktion",
                    IsActive = true,
                    MinimumAmount = 1000,
                    Procedure = "Tag marcipanmus og bland med farvestof."
                };
                ItemType chokolade = new ItemType {
                    Name = "Chokolade",
                    Unit = "Kilogram",
                    Department = "Bestilling",
                    IsActive = true,
                    MinimumAmount = 1000
                };
                ItemType hvidChokolade = new ItemType {
                    Name = "Hvid Chokolade",
                    Unit = "Kilogram",
                    Department = "Bestilling",
                    IsActive = true,
                    MinimumAmount = 200
                };
                ItemType marcipan = new ItemType {
                    Name = "Marcipan",
                    Unit = "Kilogram",
                    Department = "Bestilling",
                    IsActive = true,
                    MinimumAmount = 200
                };
                ItemType karamel = new ItemType {
                    Name = "Karamel",
                    Unit = "Kilogram",
                    Department = "Bestilling",
                    IsActive = true,
                    MinimumAmount = 1
                };
                ItemType farvestof = new ItemType {
                    Name = "Farvestof",
                    Unit = "liter",
                    Department = "Bestilling",
                    IsActive = true,
                    MinimumAmount = 0
                };

                db.ItemTypeSet.Add(kokosbrød);
                db.ItemTypeSet.Add(farvestof);
                db.ItemTypeSet.Add(chokolade);
                db.ItemTypeSet.Add(chokolademus);
                db.ItemTypeSet.Add(hvidChokolade);
                db.ItemTypeSet.Add(karamel);
                db.ItemTypeSet.Add(marcipnamus);
                db.ItemTypeSet.Add(marcipan);

                List<Ingredient> ingredientsForChokomus = new List<Ingredient>();
                ingredientsForChokomus.Add(new Ingredient {
                    Amount = 15,
                    ItemType = chokolade,
                    ForItemType = chokolademus
                });
                ingredientsForChokomus.Add(new Ingredient {
                    Amount = 5,
                    ItemType = karamel,
                    ForItemType = chokolademus
                });
                ingredientsForChokomus.Add(new Ingredient {
                    Amount = 1,
                    ItemType = hvidChokolade,
                    ForItemType = chokolademus
                });
                ingredientsForChokomus.Add(new Ingredient {
                    Amount = 750,
                    ItemType = marcipnamus,
                    ForItemType = chokolademus
                });

                List<Ingredient> ingredientsForMarcipanmus = new List<Ingredient>();
                ingredientsForMarcipanmus.Add(new Ingredient {
                    Amount = 75,
                    ItemType = marcipan,
                    ForItemType = marcipnamus
                });
                ingredientsForMarcipanmus.Add(new Ingredient {
                    Amount = 0.1,
                    ItemType = farvestof,
                    ForItemType = marcipnamus
                });

                db.IngredientSet.AddRange(ingredientsForChokomus);
                db.IngredientSet.AddRange(ingredientsForMarcipanmus);

                List<Location> locations = new List<Location>();
                for (int i = 1; i < 11; i++) {
                    locations.Add(new Location {
                        IsActive = true,
                        Name = "A" + i
                    });
                    locations.Add(new Location {
                        IsActive = true,
                        Name = "B" + i
                    });
                }
                db.LocationSet.AddRange(locations);

                List<Item> items = new List<Item>();
                items.Add(new Item {
                    Amount = 2137,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(3),
                    ItemType = marcipnamus,
                    Location = locations.FirstOrDefault(l => l.Name == "A1"),
                    Supplier = "Aalborg Chokolade",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 5,
                    DeliveryDate = DateTime.Now.AddHours(2),
                    ExpirationDate = DateTime.Now.AddMonths(7),
                    ItemType = karamel,
                    Location = locations.FirstOrDefault(l => l.Name == "A2"),
                    Supplier = "Toms",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 2000,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(12),
                    ItemType = chokolade,
                    Location = locations.FirstOrDefault(l => l.Name == "B1"),
                    Supplier = "Odense",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 500,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(4),
                    ItemType = chokolade,
                    Location = locations.FirstOrDefault(l => l.Name == "B2"),
                    Supplier = "Odense",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 10,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    ItemType = chokolade,
                    Location = locations.FirstOrDefault(l => l.Name == "B3"),
                    Supplier = "Odense",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 437,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(5),
                    ItemType = hvidChokolade,
                    Location = locations.FirstOrDefault(l => l.Name == "B7"),
                    Supplier = "Odense",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 0.1,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(3),
                    ItemType = farvestof,
                    Location = locations.FirstOrDefault(l => l.Name == "A4"),
                    Supplier = "Kemikalier",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 500,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(7),
                    ItemType = marcipan,
                    Location = locations.FirstOrDefault(l => l.Name == "A5"),
                    Supplier = "Odense",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 37,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(3),
                    ItemType = marcipan,
                    Location = locations.FirstOrDefault(l => l.Name == "A6"),
                    Supplier = "Odense",
                    Reserved = 0,
                });
                items.Add(new Item {
                    Amount = 5,
                    DeliveryDate = DateTime.Now.AddHours(6),
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    ItemType = chokolademus,
                    Location = locations.FirstOrDefault(l => l.Name == "A10"),
                    Supplier = "Aalborg Chokolade",
                    Reserved = 0,
                });

                db.ItemSet.AddRange(items);

                WorkOrder chokomusWorkOrder = new WorkOrder {
                    Type = chokolademus.Department,
                    Amount = chokolademus.BatchSize,
                    ItemType = chokolademus,
                    OrderNumber = 54,
                    BatchNumber = 33,
                    Progress = 0
                };

                db.WorkOrderSet.Add(chokomusWorkOrder);

                List<WorkOrderItem> workOrderItems = new List<WorkOrderItem>();

                foreach (Ingredient ingredient in chokolademus.IngredientsForRecipe) {
                    workOrderItems.Add(new WorkOrderItem {
                        Amount = ingredient.Amount,
                        ItemType = ingredient.ItemType,
                        WorkOrder = chokomusWorkOrder
                    });
                }

                db.WorkOrderItemSet.AddRange(workOrderItems);

                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [RequireAuthorization(IsDisabled = true)]
        public ActionResult ICLEAN() {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.WasteReportSet.RemoveRange(db.WasteReportSet.ToList());
                db.LogEntrySet.RemoveRange(db.LogEntrySet.ToList());
                db.UserSet.RemoveRange(db.UserSet.ToList());
                db.WorkOrderItemSet.RemoveRange(db.WorkOrderItemSet.ToList());
                db.ItemSet.RemoveRange(db.ItemSet.ToList());
                db.LocationSet.RemoveRange(db.LocationSet.ToList());
                db.IngredientSet.RemoveRange(db.IngredientSet.ToList());
                db.WorkOrderSet.RemoveRange(db.WorkOrderSet.ToList());
                db.ItemTypeSet.RemoveRange(db.ItemTypeSet.ToList());

                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}