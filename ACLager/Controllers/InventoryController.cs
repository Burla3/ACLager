using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.Models;
using ACLager.ViewModels;
using Newtonsoft.Json.Linq;

namespace ACLager.Controllers
{
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            ACLagerDatabaseEntities db = new ACLagerDatabaseEntities();
            //db.Items.Add(new Item("Chokolademus", 100, new DateTime(2020, 12, 17), DateTime.Now, "AC" ));
            //db.SaveChanges();

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
    }
}