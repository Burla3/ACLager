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
using WebGrease.Css.Extensions;

namespace ACLager.Controllers {
    public class InventoryController : Controller, ILoggable, INotifier {
        public InventoryController() {
            new Logger().Subcribe(this);
            new Notifier().Subscribe(this);
        }

        // GET: Inventory
        public ActionResult Index() {
            IEnumerable<ItemGroup> itemGroups;
            IEnumerable<SelectListItem> locationSelectListItems;
            IEnumerable<SelectListItem> itemTypeSelectListItems;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                IEnumerable<Item> items = db.ItemSet.Where(i => i.Amount > 0).ToList();
                IEnumerable<ItemType> itemTypes = db.ItemTypeSet.Where(it => it.IsActive).ToList();
                IEnumerable<Location> locations = db.LocationSet.Where(l => l.IsActive).ToList();

                itemGroups = GenerateItemGroups(items, itemTypes, locations);
                locationSelectListItems = GenerateLocationSelectListItems(locations);
                itemTypeSelectListItems = GenerateItemTypeSelectListItems(itemTypes);
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
            AddItem(item, false);

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
            Location dbLocation;
            ItemType dbItemType;         

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Item dbItem = db.ItemSet.Find(item.UID);
                dbItemType = dbItem.ItemType;
                dbLocation = dbItem.Location;

                if (!PickItem(item)) {
                    return View("Error", new InventoryViewModel());
                }

                if (dbItemType.Items.Sum(i => i.Amount) < dbItemType.MinimumAmount) {
                    Notify?.Invoke(this, new NotificationEventArgs(dbItemType));
                }
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Vare plukket",
                        $"{item.Amount} {dbItemType.Unit} {dbItemType.Name} plukket fra {dbLocation.Name}.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Vare = item.ToLoggable(),
                            Varetype = dbItemType.ToLoggable(),
                            Lokation = dbLocation.ToLoggable()
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
        public ActionResult Move(Item item) {
            double moveAmount = item.Amount;
            Location fromLocation;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Item dbItem = db.ItemSet.Find(item.UID);
                Location newLocation = db.LocationSet.Find(item.Location.UID);
                Item newLocationItem = null;
                if (newLocation.Item != null) {
                    newLocationItem = db.ItemSet.Find(newLocation.Item.UID);
                }

                fromLocation = db.ItemSet.Find(item.UID).Location;

                item = dbItem;
                item.Amount = moveAmount;
                item.ItemType = dbItem.ItemType;
                item.Location = newLocation;
                item.Location.Item = newLocationItem;
            }

            if (!PickItem(item) || !AddItem(item, false)) {
                return RedirectToAction("Move");
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Vare flyttet",
                        $"{moveAmount} {item.ItemType.Unit} {item.ItemType.Name} flyttet fra {fromLocation.Name} til {item.Location.Name}.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Mængde = new { Amount = moveAmount},
                            Varetype = item.ItemType.ToLoggable(),
                            LokationFra = fromLocation.ToLoggable(),
                            LokationTil = item.Location.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        public event ChangedEventHandler Changed;
        public event NotificationEventHandler Notify;

        public bool AddItem(Item item, bool addReserved) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {

                Location location = db.LocationSet.Find(item.Location.UID);
                item.Location = location;
                Item locationItem = null;

                if (location.Item != null) {
                    locationItem = db.ItemSet.Find(location.Item.UID);
                }
                
                if (locationItem == null) {
                    item.ItemType = db.ItemTypeSet.Find(item.ItemType.UID);
                    db.ItemSet.Add(item);
                } else {
                    locationItem.ItemType = locationItem.ItemType;

                    if (item.ExpirationDate == locationItem.ExpirationDate &&
                        item.DeliveryDate == locationItem.DeliveryDate &&
                        item.Supplier == locationItem.Supplier &&
                        item.ItemType.UID == locationItem.ItemType.UID) {

                        locationItem.Amount += item.Amount;

                        if (addReserved) {
                            locationItem.Reserved += item.Reserved;
                        }
                    } else {
                        return false;
                    }
                }

                db.SaveChanges();
            }

            return true;
        }

        public static bool PickItem(Item item, bool pickReserved=false) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                Item dbItem = db.ItemSet.Find(item.UID);

                double compAmount;
                double reservedtmp = dbItem.Reserved;

                // if we pick into reserved we ignore it. If not we must compute the actual amount available
                if (pickReserved) {
                    compAmount = dbItem.Amount;
                } else {
                    compAmount = dbItem.Amount - dbItem.Reserved;
                }

                if (item.Amount < compAmount) {
                    dbItem.Amount -= item.Amount;

                    //if we are picking into reserved we subtract from that field as well
                    if (pickReserved) {
                        reservedtmp -= item.Amount;
                        
                        // but only down to zero
                        if (reservedtmp <= 0) {
                            dbItem.Reserved = 0;
                        } else {
                            dbItem.Reserved = reservedtmp;
                        }
                    }

                // if the amounts exactly match we remove the item and no further logic is required
                } else if (item.Amount == compAmount) {
                    dbItem.Location.Item = null;
                    dbItem.WorkOrderItem.ForEach(woi => woi.Item = null);
                    db.ItemSet.Remove(dbItem);
                } else {
                    // if we reach this it means we could not successfully pick from this item.
                    return false;
                }

                db.SaveChanges();
            }

            return true;
        }

        public IEnumerable<ItemGroup> GenerateItemGroups(
            IEnumerable<Item> items,
            IEnumerable<ItemType> itemTypes,
            IEnumerable<Location> locations)
        {
            List<ItemGroup> itemGroups = new List<ItemGroup>();

            foreach (ItemType itemType in itemTypes) {
                List<ItemLocationPair> itemLocationPairs = new List<ItemLocationPair>();

                foreach (Item item in items.Where(i => itemType.UID == i.ItemType.UID)) {
                    itemLocationPairs.Add(new ItemLocationPair(item, locations.Single(l => l.UID == item.Location.UID)));
                }

                itemGroups.Add(new ItemGroup(itemType, itemLocationPairs));
            }

            return itemGroups;
        }

        public IEnumerable<SelectListItem> GenerateLocationSelectListItems(IEnumerable<Location> locations)
        {
            List<SelectListItem> locationSelectListItems = new List<SelectListItem>();

            foreach (Location location in locations.Where(l => l.Item == null)) {
                locationSelectListItems.Add(new SelectListItem { Text = location.Name, Value = location.UID.ToString() });
            }

            return locationSelectListItems;
        }

        public IEnumerable<SelectListItem> GenerateItemTypeSelectListItems(IEnumerable<ItemType> itemTypes)
        {
            List<SelectListItem> itemTypeSelectListItems = new List<SelectListItem>();

            foreach (ItemType itemType in itemTypes) {
                itemTypeSelectListItems.Add(new SelectListItem { Text = itemType.Name, Value = itemType.UID.ToString() });
            }

            return itemTypeSelectListItems;
        }
    }
}