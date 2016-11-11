using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.ViewModels;

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

            IEnumerable<Item> items = from item in db.Items
                        where item.Amount > 0
                        select item;

            return View(new InventoryViewModel(items));
        }
    }
}