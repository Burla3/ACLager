﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.Interfaces;
using ACLager.Models;
using ACLager.ViewModels;
using Newtonsoft.Json.Linq;

namespace ACLager.Controllers
{
    public class InventoryController : Controller, ILoggable
    {
        private string _sectionColor = "purple";

        public InventoryController()
        {
            new Logger().Subcribe(this);
        }

        // GET: Inventory
        public ActionResult Index()
        {
            IEnumerable<ItemType> itemTypes;
            IEnumerable<Item> items;
            IEnumerable<Location> locations;
            List<SelectListItem> locationSelectListItems = new List<SelectListItem>();
            List<SelectListItem> itemTypeSelectListItems = new List<SelectListItem>();

            List<ItemGroup> itemGroups = new List<ItemGroup>();

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                items = db.ItemSet.Where(i => i.Amount > 0);
                itemTypes = db.ItemTypeSet.Where(it => it.IsActive);
                locations = db.LocationSet.Where(l => l.IsActive);

                foreach (ItemType itemType in itemTypes) {
                    List<ItemLocationPair> itemLocationPairs = new List<ItemLocationPair>();

                    foreach (Item item in items.Where(i => itemType.UID == i.ItemType.UID)) {
                        itemLocationPairs.Add(new ItemLocationPair(item, locations.Single(l => l.UID == item.Location.UID)));
                    }

                    itemGroups.Add(new ItemGroup(itemType, itemLocationPairs));
                }

                foreach (Location location in locations.Where(l => l.Item == null)) {
                    locationSelectListItems.Add(new SelectListItem { Text = location.Name, Value = location.UID.ToString()});
                }

                foreach (ItemType itemType in itemTypes) {
                    itemTypeSelectListItems.Add(new SelectListItem { Text = itemType.Name, Value = itemType.UID.ToString() });
                }
            }

            

            return View(new InventoryViewModel(itemGroups, new Item(), locationSelectListItems, itemTypeSelectListItems));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detailed(string id) {
            Item item;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                item = db.ItemSet.Find(Int64.Parse(id));
            }

            return View("Detailed", new InventoryViewModel(null, item, null, null));
        }

        /// <summary>
        /// Adds the <paramref name="item"/> to the database
        /// </summary>
        /// <param name="item">The item to add to the database.</param>
        /// <returns>true if successful</returns>
        [HttpPost]
        public ActionResult AddItem(Item item) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                item.ItemType = db.ItemTypeSet.Find(item.ItemType.UID);
                item.Location = db.LocationSet.Find(item.Location.UID);
                item.DeliveryDate = DateTime.Now;
                item.ExpirationDate = DateTime.Now;

                db.ItemSet.Add(item);
                db.SaveChanges();
            }

            return RedirectToAction("Detailed", new {id = item.UID});
        }

        [HttpGet]
        public ActionResult PickItem(string id) {

            if (id == null) {
                return RedirectToAction("Index");
            }

            InventoryViewModel inventoryViewModel = new InventoryViewModel();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Item dbItem = db.ItemSet.Find(Int64.Parse(id));

                if (dbItem == null) {
                    return RedirectToAction("Index");
                }

                inventoryViewModel.Item = dbItem;
            }

            return View(inventoryViewModel);
        }

        [HttpPost]
        public ActionResult PickItem(Item item)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                Item dbItem = db.ItemSet.Find(item.UID);

                dbItem.Amount -= item.Amount;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public bool MoveItem()
        {
            throw new NotImplementedException();
        }

        public event ChangedEventHandler Changed;
    }
}