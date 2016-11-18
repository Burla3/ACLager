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
        public InventoryController()
        {
            new Logger().Subcribe(this);
        }

        // GET: Inventory
        public ActionResult Index()
        {
            ACLagerDatabaseEntities db = new ACLagerDatabaseEntities();
            //db.Items.Add(new Item("Chokolademus", 100, new DateTime(2020, 12, 17), DateTime.Now, "AC" ));
            db.SaveChanges();

            IEnumerable<ItemType> itemTypes = from itemType in db.ItemTypes
                                                where itemType.is_active
                                                select itemType;

            IEnumerable<Item> items = from item in db.Items
                                        where item.amount > 0
                                        select item;

            IEnumerable<Location> locations = from location in db.Locations
                                                where location.is_active
                                                select location;

            List<ItemGroup> itemGroups = new List<ItemGroup>();

            foreach (ItemType itemType in itemTypes) {
                List<Tuple<Item, Location>> itemAndLocations = new List<Tuple<Item, Location>>();

                foreach (Item item in items.Where(i => itemType.uid == i.item_type)) {
                    itemAndLocations.Add(new Tuple<Item, Location>(item, locations.Single(l => l.uid == item.location)));
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
            ACLagerDatabaseEntities db = new ACLagerDatabaseEntities();

            IEnumerable<Item> items = from item in db.Items
                                      where item.uid == uid
                                      select item;

            foreach (Item item in items)
            {
                if (item.amount < amount)

                    return false;

                if (item.amount > amount)
                {
                    item.amount = item.amount - amount;

                    return true;
                }
            }
            return false;
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