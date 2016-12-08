using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.CustomClasses.Attributes;
using ACLager.Interfaces;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    [AdminOnly]
    public class LocationController : Controller, ILoggable {
        public LocationController() {
            new Logger().Subcribe(this);
        }

        // GET: Location
        public ActionResult Index() {
            List<ItemLocationPair> itemLocationPairs = new List<ItemLocationPair>();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                foreach (Location location in db.LocationSet.ToList()) {
                    itemLocationPairs.Add(new ItemLocationPair(location.Item, location));
                }
            }

            return View(new LocationViewModel(itemLocationPairs, new ItemLocationPair(new Item(), new Location())));
        }

        [HttpGet]
        public ActionResult Detailed(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            ItemLocationPair itemLocationPair = new ItemLocationPair();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                itemLocationPair.Location = db.LocationSet.Find(Int64.Parse(id));
                if (itemLocationPair.Location.Item != null) {
                    itemLocationPair.Item = itemLocationPair.Location.Item;
                    itemLocationPair.Item.ItemType = itemLocationPair.Location.Item.ItemType;
                }
            }

            return View(new LocationViewModel(null, itemLocationPair));
        }

        [HttpGet]
        public ActionResult Create (){
            return View(new LocationViewModel(null, new ItemLocationPair(new Item(), new Location())));
        }

        [HttpPost]
        public ActionResult Create(Location location) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.LocationSet.Add(location);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Lokation oprettet",
                        $"Lokationen {location.Name} er blevet oprettet.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Lokation = location.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Detailed", new { id = location.UID});
        }

        [HttpGet]
        public ActionResult Edit(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            LocationViewModel locationViewModel = new LocationViewModel();
            locationViewModel.ItemLocationPair = new ItemLocationPair();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Location dbLocation = db.LocationSet.Find(Int64.Parse(id));

                if (dbLocation == null) {
                    return RedirectToAction("Index");
                }

                locationViewModel.ItemLocationPair.Location = dbLocation;
            }

            return View(locationViewModel);
        }

        [HttpPost]
        public ActionResult Edit(Location location) {
            Location oldLocation;
            Location newLocation;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Location dbLocation = db.LocationSet.Find(location.UID);

                oldLocation = dbLocation.ToLoggable();

                dbLocation.IsActive = location.IsActive;
                dbLocation.Name = location.Name;

                newLocation = dbLocation.ToLoggable();

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Lokation ændret",
                        $"Lokationen {newLocation.Name} er blevet ændret.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Før = oldLocation,
                            Efter = newLocation
                        }
                    )
            );

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            LocationViewModel locationViewModel = new LocationViewModel();
            locationViewModel.ItemLocationPair = new ItemLocationPair();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Location dbLocation = db.LocationSet.Find(Int64.Parse(id));

                if (dbLocation == null) {
                    return RedirectToAction("Index");
                }

                Item item = dbLocation.Item;
                if (item != null) {
                    item.ItemType = dbLocation.Item.ItemType;
                }

                locationViewModel.ItemLocationPair = new ItemLocationPair(item, dbLocation);
            }

            return View(locationViewModel);
        }

        [HttpPost]
        public ActionResult Delete(long id) {
            Location location;
            Item item;
            ItemType itemType = null;
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                location = db.LocationSet.Find(id);
                item = location.Item;
                
                if (item != null) {
                    itemType = item.ItemType;
                    db.ItemSet.Remove(item);
                }
                
                db.LocationSet.Remove(location);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Lokation fjernet",
                        $"Lokationen {location.Name} er blevet slettet.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Lokation = location,
                            Vare = item?.ToLoggable(),
                            Varetype = itemType?.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DoesLocationNameExist(string Name, long UID) {
            Location location;
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                if (UID == 0) {
                    location = db.LocationSet.FirstOrDefault(l => l.Name == Name);
                } else {
                    location = db.LocationSet.FirstOrDefault(l => l.Name == Name && l.UID != UID);
                }
            }
            return Json(location == null);
        }

        public event ChangedEventHandler Changed;
    }
}