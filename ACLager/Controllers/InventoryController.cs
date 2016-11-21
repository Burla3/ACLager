using System;
using System.Collections.Generic;
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
        private readonly ACLagerDatabase db = new ACLagerDatabase();

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

            using (db)
            {
                itemTypes = db.ItemTypeSet;
                items = db.ItemSet;
                locations = db.LocationSet;
            }

            List<ItemGroup> itemGroups = new List<ItemGroup>();

            foreach (ItemType itemType in itemTypes) {
                List<Tuple<Item, Location>> itemAndLocations = new List<Tuple<Item, Location>>();

                foreach (Item item in items.Where(i => itemType.UID == i.ItemTypeUID)) {
                    itemAndLocations.Add(new Tuple<Item, Location>(item, locations.Single(l => l.UID == item.Location.UID)));
                }

                itemGroups.Add(new ItemGroup(itemType, itemAndLocations));
            }

            return View(new InventoryViewModel(itemGroups));
        }

        /// <summary>
        /// Adds the <paramref name="item"/> to the database
        /// </summary>
        /// <param name="item">The item to add to the database.</param>
        /// <returns>true if successful</returns>
        public bool AddItem(Item item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the item with specified <paramref name="uid"/> by the <paramref name="amount"/>
        /// </summary>
        /// <param name="uid">The item unique id</param>
        /// <param name="amount">The amount to pick</param>
        /// <returns>true if successful</returns>
        public bool PickItem(long uid, long amount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the item with specified <paramref name="uid"/> by the <paramref name="amount"/>
        /// to the specified <paramref name="locationId"/>.
        /// </summary>
        /// <param name="uid">The item unique id</param>
        /// <param name="amount">The amount to move</param>
        /// <param name="locationId">The location id</param>
        /// <returns>true if successful</returns>
        public bool MoveItem(long uid, long amount, long locationId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all items from the database
        /// </summary>
        /// <returns>All items from the database</returns>
        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }

        public event ChangedEventHandler Changed;
    }
}