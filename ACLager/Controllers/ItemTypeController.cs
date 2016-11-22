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
        public bool AddItemType(ItemType itemType)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                if (!db.ItemTypeSet.Any(it => it.Name == itemType.Name && it.Unit == itemType.Unit))
                {
                    db.ItemTypeSet.Add(itemType);
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Edit the <paramref name="itemType"/> in the database
        /// </summary>
        /// <param name="itemType">The item type to edit in the database.</param>
        /// <returns>true if successful</returns>
        public bool EditItemType(ItemType itemType)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                ItemType dbItemType = db.ItemTypeSet.Find(itemType.UID);

                dbItemType.IsActive = itemType.IsActive;
                dbItemType.Name = itemType.Name;
                dbItemType.Unit = itemType.Unit;
                dbItemType.MinimumAmount = itemType.MinimumAmount;

                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Delete the item type with specified <paramref name="uid"/> from the database
        /// </summary>
        /// <param name="uid">The item type with specified <paramref name="uid"/> to delete from the database.</param>
        /// <returns>true if successful</returns>
        public bool DeleteItemType(long UID)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                db.ItemTypeSet.Remove(db.ItemTypeSet.Find(UID));
                db.SaveChanges();
            }

            return true;
        }
    }
}