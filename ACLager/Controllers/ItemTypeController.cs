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
        private readonly ACLagerDatabaseEntities _db = new ACLagerDatabaseEntities();
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
        public bool AddItemType(ItemType itemType)
        {
            //checks if an itemtype with the same name and unit of measurement exsists. If not create one.
            var exsistingItemType = (from itemT in _db.ItemTypes where itemT.name == itemType.name && itemT.unit == itemType.unit select itemT).FirstOrDefault();
            
            //An itemtype with the same name and unit does not exsist, add it.
            if (exsistingItemType != null)
            {
                if (exsistingItemType.is_active == false)
                {
                    //Hey, it's there it's just inactive
                }

                return false;
            }
            //An item type with this name does not exist if existingItemType == null
            //An item with the same name and unit of measurement already exsists
            else
            {
                _db.ItemTypes.Add(itemType);
                _db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Edit the <paramref name="itemType"/> in the database
        /// </summary>
        /// <param name="itemType">The item type to edit in the database.</param>
        /// <returns>true if successful</returns>
        public bool EditItemType(ItemType itemType)
        {
            var exsistingItemType = _db.ItemTypes.Find(itemType.uid);
            if (exsistingItemType != null)
            {
                exsistingItemType = itemType;
                _db.SaveChanges();
                return true;
            }
            else
            {
                //the item does not exsist, so can't be edited.
                return false;
            }
        }

        /// <summary>
        /// Delete the item type with specified <paramref name="uid"/> from the database
        /// </summary>
        /// <param name="uid">The item type with specified <paramref name="uid"/> to delete from the database.</param>
        /// <returns>true if successful</returns>
        public bool DeleteItemType(long uid)
        {
            //returns null if an itemtype with the uid does not exsist
            var exsistingItemType = _db.ItemTypes.Find(uid);

            //If it doesen't exsist, it can't be deleted
            if (exsistingItemType == null)
            {
                return false;
            }

            //Does exsist, so delete;
            else
            {
                _db.ItemTypes.Remove(exsistingItemType);
                _db.SaveChanges();
                return true;
            }

        }

        /// <summary>
        /// Gets all item types from the database
        /// </summary>
        /// <returns>All item types from the database</returns>
        public IEnumerable<ItemType> GetItemTypes()
        {
            IEnumerable<ItemType> itemTypes = _db.ItemTypes.ToList();
            return itemTypes;
        }
    }
}