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
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                db.ItemSet.Add(item);
                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Removes the item with specified <paramref name="uid"/> by the <paramref name="amount"/>
        /// </summary>
        /// <param name="uid">The item unique id</param>
        /// <param name="amount">The amount to pick</param>
        /// <returns>true if successful</returns>
        public bool PickItem(long UID, long amount)
        {
            Item item;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                item = db.ItemSet.Find(UID);

                item.Amount -= amount;

                db.SaveChanges();
            }

            return true;
        }

        public bool MoveItem()
        {
            throw new NotImplementedException();
        }

        public event ChangedEventHandler Changed;
    }
}