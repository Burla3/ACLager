﻿using System;
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

                locationViewModel.ItemLocationPair = new ItemLocationPair(dbLocation.Item, dbLocation);
            }

            return View(locationViewModel);
        }

        [HttpPost]
        public ActionResult Delete(long id) {
            Location location;
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                location = db.LocationSet.Find(id);
                db.LocationSet.Remove(location);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Lokation fjernet",
                        $"Lokationen {location.Name} er blevet slettet.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Lokation = location
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        public event ChangedEventHandler Changed;
    }
}