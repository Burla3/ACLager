using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public class InventoryController : Controller, ILoggable, INotifier
    {
        public InventoryController()
        {
            new Logger().Subcribe(this);
            new Notifier().Subscribe(this);
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

            Item sitem = new Item();
            sitem.DeliveryDate = DateTime.Now;

            return View(new InventoryViewModel(itemGroups, sitem, locationSelectListItems, itemTypeSelectListItems));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detailed(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }
            Item item;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Item dbitem = db.ItemSet.Find(Int64.Parse(id));
                item = dbitem;
                item.ItemType = dbitem.ItemType;
                item.Location = dbitem.Location;
            }

            return View("Detailed", new InventoryViewModel(null, item, null, null));
        }

        /// <summary>
        /// Adds the <paramref name="item"/> to the database
        /// </summary>
        /// <param name="item">The item to add to the database.</param>
        /// <returns>true if successful</returns>
        [HttpPost]
        public ActionResult Add(Item item) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                item.ItemType = db.ItemTypeSet.Find(item.ItemType.UID);
                item.Location = db.LocationSet.Find(item.Location.UID);
   

                db.ItemSet.Add(item);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Vare Tilføjet",
                        $"{item.Amount} {item.ItemType.Unit} {item.ItemType.Name} tilføjet.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Vare = item.ToLoggable(),
                            Varetype = item.ItemType.ToLoggable(),
                            Lokation = item.Location.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Detailed", new {id = item.UID});
        }

        [HttpGet]
        public ActionResult Pick(string id) {

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
                inventoryViewModel.Item.ItemType = dbItem.ItemType;
                inventoryViewModel.Item.Location = dbItem.Location;
            }

            return View(inventoryViewModel);
        }

        [HttpPost]
        public ActionResult Pick(Item item) {
            ItemType itemType;
            Location location;
            Item dbItem;
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                dbItem = db.ItemSet.Find(item.UID);

                itemType = dbItem.ItemType.ToLoggable();
                location = dbItem.Location.ToLoggable();

                dbItem.Amount -= item.Amount;

                db.SaveChanges();

                if (dbItem.ItemType.Items.Sum(i => i.Amount) < dbItem.ItemType.MinimumAmount) {
                    Notify?.Invoke(this, new NotificationEventArgs(dbItem.ItemType));
                }
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Vare plukket",
                        $"{dbItem.Amount} {itemType.Unit} {itemType.Name} plukket fra {location.Name}.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Vare = dbItem.ToLoggable(),
                            Varetype = itemType,
                            Lokation = location
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Move(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            InventoryViewModel inventoryViewModel = new InventoryViewModel();
            IEnumerable<Item> items;
            Dictionary<long, Location> locations;
            List<SelectListItem> locationSelectListItems = new List<SelectListItem>();
            
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Item dbItem = db.ItemSet.Find(long.Parse(id));

                if (dbItem == null) {
                    return RedirectToAction("Index");
                }

                inventoryViewModel.Item = dbItem;
                inventoryViewModel.Item.ItemType = dbItem.ItemType;
                inventoryViewModel.Item.Location = dbItem.Location;

                items = db.ItemSet;
                locations = db.LocationSet.Where(l => l.IsActive).ToDictionary(l => l.UID, l => l);

                foreach (Item item in items)
                {
                    if (item.UID == dbItem.UID)
                    {
                        locations.Remove(item.Location.UID);
                    } else
                    {
                        if (item.ExpirationDate != dbItem.ExpirationDate ||
                            item.DeliveryDate != dbItem.DeliveryDate ||
                            item.Supplier != dbItem.Supplier ||
                            item.ItemType != dbItem.ItemType)
                        {
                            locations.Remove(item.Location.UID);
                        }
                    }
                }

                foreach (Location location in locations.Values) {
                    locationSelectListItems.Add(new SelectListItem { Text = location.Name, Value = location.UID.ToString() });
                }

                inventoryViewModel.Locations = locationSelectListItems;
            }

            return View(inventoryViewModel);
        }

        [HttpPost]
        public ActionResult Move(Item item)
        {
            long moveAmount = item.Amount;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                Item dbItem = db.ItemSet.Find(item.UID);
                Location newLocation = db.LocationSet.Find(item.Location.UID);
                Item newLocationItem = db.ItemSet.Find(newLocation.Item.UID);

                item = dbItem;
                item.Amount = moveAmount;
                item.ItemType = dbItem.ItemType;
                item.Location = newLocation;
                item.Location.Item = newLocationItem;
            }

            if (!PickItem(item) || !AddItem(item, false))
            {
                return RedirectToAction("Move");
            }

            return RedirectToAction("Index");
        }

        public event ChangedEventHandler Changed;
        public event NotificationEventHandler Notify;

        public bool AddItem(Item item, bool addReserved)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                Item locationItem = db.ItemSet.Find(item.Location.Item.UID);

                if (locationItem == null)
                {
                    db.ItemSet.Add(item);
                }
                else
                {
                    locationItem.ItemType = locationItem.ItemType;

                    if (item.ExpirationDate == locationItem.ExpirationDate &&
                        item.DeliveryDate == locationItem.DeliveryDate &&
                        item.Supplier == locationItem.Supplier &&
                        item.ItemType.UID == locationItem.ItemType.UID)
                    {
                        locationItem.Amount += item.Amount;

                        if (addReserved)
                        {
                            locationItem.Reserved += item.Reserved;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                db.SaveChanges();
            }

            return true;
        }

        public bool PickItem(Item item)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                Item dbItem = db.ItemSet.Find(item.UID);

                if (item.Amount < dbItem.Amount)
                {
                    dbItem.Amount -= item.Amount;
                }
                else if (item.Amount == dbItem.Amount)
                {
                    db.ItemSet.Remove(dbItem);
                }
                else
                {
                    return false;
                }

                db.SaveChanges();
            }

            return true;
        }
    }
}