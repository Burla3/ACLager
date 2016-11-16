using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers
{
    public class ItemTypeController : Controller
    {
        // GET: ItemType
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Adds the <paramref name="itemType"/> to the database
        /// </summary>
        /// <param name="itemType">The item type to add to the database.</param>
        /// <returns>true if successful</returns>
        public bool AddItem(ItemType itemType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the <paramref name="itemType"/> in the database
        /// </summary>
        /// <param name="itemType">The item type to edit in the database.</param>
        /// <returns>true if successful</returns>
        public bool EditItem(ItemType itemType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete the item type with specified <paramref name="uid"/> from the database
        /// </summary>
        /// <param name="uid">The item type with specified <paramref name="uid"/> to delete from the database.</param>
        /// <returns>true if successful</returns>
        public bool DeleteItem(long uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all item types from the database
        /// </summary>
        /// <returns>All item types from the database</returns>
        public IEnumerable<ItemType> GetItems()
        {
            throw new NotImplementedException();
        }
    }
}