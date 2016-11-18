using System;
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
        public InventoryController()
        {
            new Logger().Subcribe(this);
        }

        // GET: Inventory
        public ActionResult Index()
        {
            List<ItemGroup> itemGroups = new List<ItemGroup>();

            using (ACLagerDatabaseEntities db = new ACLagerDatabaseEntities())
            {
                IEnumerable<Item> items = db.Items.Where(i => i.amount > 0);
                IEnumerable<ItemType> itemTypes = db.ItemTypes.Where(it => it.is_active);
                IEnumerable<Location> locations = db.Locations.Where(l => l.is_active);

                foreach (ItemType itemType in itemTypes)
                {
                    List<ItemLocationPair> itemLocationPairs = new List<ItemLocationPair>();

                    foreach (Item item in items.Where(i => itemType.uid == i.item_type))
                    {
                        itemLocationPairs.Add(new ItemLocationPair(item, locations.Single(l => l.uid == item.location)));
                    }

                    itemGroups.Add(new ItemGroup(itemType, itemLocationPairs));
                }
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
                    long stock = item.amount;
                    item.amount = stock - amount;

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
            ACLagerDatabaseEntities db = new ACLagerDatabaseEntities();

            IEnumerable<Item> items = from item in db.Items
                                      where item.uid == uid
                                      select item;

            foreach (Item item in items)
            {
                item.location = locationId;
                long stock = item.amount;
                item.amount = stock + amount;

                return true;
            }

            return false;
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