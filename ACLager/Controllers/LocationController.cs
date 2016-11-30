using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.CustomClasses.Attributes;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    [AdminOnly]
    public class LocationController : Controller {
        // GET: Location
        public ActionResult Index() {
            List<ItemLocationPair> itemLocationPairs = new List<ItemLocationPair>();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                foreach (Location location in db.LocationSet) {
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

        [HttpPost]
        public ActionResult CreateLocation(Location location) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.LocationSet.Add(location);
                db.SaveChanges();
            }

            return RedirectToAction("Detailed", new { id = location.UID});
        }

        [HttpGet]
        public ActionResult EditLocation(string id) {
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
        public ActionResult EditLocation(Location location) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Location dbLocation = db.LocationSet.Find(location.UID);

                dbLocation.IsActive = location.IsActive;
                dbLocation.Name = location.Name;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult DeleteLocation(string id) {
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

                locationViewModel.ItemLocationPair = new ItemLocationPair(dbLocation.Item, dbLocation);
            }

            return View(locationViewModel);
        }

        [HttpPost]
        public ActionResult DeleteLocation(long id) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.LocationSet.Remove(db.LocationSet.Find(id));
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}