using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses.Attributes;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers
{
    [AdminOnly]
    public class ItemTypeController : Controller
    {
        // GET: ItemType
        public ActionResult Index()
        {
            IEnumerable<ItemType> itemTypes;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                itemTypes = db.ItemTypeSet.ToList();
            }

            return View(new ItemTypeViewModel(itemTypes, new ItemType()));
        }

        [HttpPost]
        public ActionResult AddSubmitAction(ItemType itemType, string submitAction) {
            switch (submitAction) {
                case "AddItemType":
                    return AddItemType(itemType);
                case "EditItemType":
                    AddItemTypeToDb(itemType);
                    return RedirectToAction("EditItemType", new { id = itemType.UID.ToString() });
                default:
                    throw new InvalidOperationException("No valid submit action is specified");
            }
        }

        /// <summary>
        /// Adds the <paramref name="itemType"/> to the database
        /// </summary>
        /// <param name="itemType">The item type to add to the database.</param>
        /// <returns>true if successful</returns>
        [HttpPost]
        public ActionResult AddItemType(ItemType itemType) {
            AddItemTypeToDb(itemType);

            return RedirectToAction("Index");
        }

        private void AddItemTypeToDb(ItemType itemType) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                if (!db.ItemTypeSet.Any(it => it.Name == itemType.Name && it.Unit == itemType.Unit))
                {
                    db.ItemTypeSet.Add(itemType);
                    db.SaveChanges();
                }
            }
        }

        [HttpGet]
        public ActionResult EditItemType(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            long uid = long.Parse(id);

            ItemTypeViewModel itemTypeViewModel = new ItemTypeViewModel();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                ItemType dbItemType = db.ItemTypeSet.Find(uid);

                if (dbItemType == null) {
                    return RedirectToAction("Index");
                }

                itemTypeViewModel.ItemType = dbItemType;
                itemTypeViewModel.ItemTypes = db.ItemTypeSet.Where(it => it.IsActive && it.UID != uid).ToList();
                itemTypeViewModel.Ingredients = db.IngredientSet.Where(it => it.ForItemType.UID == uid).ToList();
                itemTypeViewModel.Ingredient = new Ingredient();
                itemTypeViewModel.UnitSelectListItems = new[] {
                    new SelectListItem() {Text = "Gram"},
                    new SelectListItem() {Text = "Stk."},
                    new SelectListItem() {Text = "Liter"}
                };
                itemTypeViewModel.DepartmentSelectListItems = new[] {
                    new SelectListItem() {Text = "Produktion", Value = "Production"},
                    new SelectListItem() {Text = "Pakkeri", Value = "Packaging"}
                };
            }

            return View(itemTypeViewModel);
        }

        [HttpPost]
        public ActionResult EditItemType(ItemType itemType) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                ItemType dbItemType = db.ItemTypeSet.Find(itemType.UID);

                dbItemType.IsActive = itemType.IsActive;
                dbItemType.Name = itemType.Name;
                dbItemType.Unit = itemType.Unit;
                dbItemType.MinimumAmount = itemType.MinimumAmount;
                dbItemType.Barcode = itemType.Barcode;
                dbItemType.Procedure = itemType.Procedure;
                dbItemType.Department = itemType.Department;
                dbItemType.BatchSize = itemType.BatchSize;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddIngredient() {
            throw new NotImplementedException();
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

            ItemTypeViewModel itemTypeViewModel = new ItemTypeViewModel();

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                itemTypeViewModel.ItemType = db.ItemTypeSet.Find(long.Parse(id));
            }

            return View(itemTypeViewModel);
        }
    }
}