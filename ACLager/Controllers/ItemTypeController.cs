using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.CustomClasses.Attributes;
using ACLager.Interfaces;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers
{
    [AdminOnly]
    public class ItemTypeController : Controller, ILoggable
    {
        public ItemTypeController() {
            new Logger().Subcribe(this);
        }

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

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Varetype tilføjet",
                        $"Varetypen {itemType.Name} i {itemType.Unit} er blevet tilføjet.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Varetype = itemType.ToLoggable()
                        }
                    )
            );
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
                itemTypeViewModel.Ingredients = db.IngredientSet.Where(it => it.ForItemType.UID == uid).ToList();
                foreach (Ingredient ingredient in itemTypeViewModel.Ingredients)
                {
                    ingredient.ItemType = db.ItemTypeSet.SingleOrDefault(it => it.UID == ingredient.UID);
                }
                itemTypeViewModel.Ingredient = new Ingredient();
                itemTypeViewModel.UnitSelectListItems = new[] {
                    new SelectListItem() {Text = "Gram"},
                    new SelectListItem() {Text = "Stk."},
                    new SelectListItem() {Text = "Liter"}
                };
                itemTypeViewModel.DepartmentSelectListItems = new[] {
                    new SelectListItem() {Text = "Produktion", Value = "Production"},
                    new SelectListItem() {Text = "Pakkeri", Value = "Packaging"},
                    new SelectListItem() {Text = "Bestilling", Value = "Order"}
                };
                itemTypeViewModel.ItemTypeSelectListItems = db.ItemTypeSet.Where(it => it.IsActive && it.UID != uid).ToList()
                    .Select(itemType => new SelectListItem {Text = itemType.Name, Value = itemType.UID.ToString()}).ToList();
            }

            return View(itemTypeViewModel);
        }

        [HttpPost]
        public ActionResult EditItemType(ItemType itemType) {
            ItemType oldItemType;
            ItemType newItemType;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                ItemType dbItemType = db.ItemTypeSet.Find(itemType.UID);

                oldItemType = dbItemType.ToLoggable();

                dbItemType.IsActive = itemType.IsActive;
                dbItemType.Name = itemType.Name;
                dbItemType.Unit = itemType.Unit;
                dbItemType.MinimumAmount = itemType.MinimumAmount;
                dbItemType.Barcode = itemType.Barcode;
                dbItemType.Procedure = itemType.Procedure;
                dbItemType.Department = itemType.Department;
                dbItemType.BatchSize = itemType.BatchSize;

                newItemType = dbItemType.ToLoggable();

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Varetype ændret",
                        $"Varetypen {itemType.Name} er blevet ændret.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Før = oldItemType,
                            Efter = newItemType
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddIngredient(Ingredient ingredient, string id) {
            if (id == null || ingredient == null) {
                return RedirectToAction("Index");
            }

            ItemType dbForItemType;
            ItemType dbItemType;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbForItemType = db.ItemTypeSet.Find(long.Parse(id));
                dbItemType = db.ItemTypeSet.Find(ingredient.ItemType.UID);

                ingredient.ForItemType = dbForItemType;
                ingredient.Amount = ingredient.Amount;
                ingredient.ItemType = dbItemType;

                db.IngredientSet.Add(ingredient);

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Ingrediens tilføjet",
                        $"Ingrediens tilføjet til varetypen {dbForItemType.Name}.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Varetype = dbForItemType.ToLoggable(),
                            Ingrediens = ingredient.ToLoggable(),
                            IngrediensVaretype = dbItemType.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("EditItemType", new { id = id });
        }

        [HttpPost]
        public ActionResult RemoveIngredient(string itemTypeId, string ingredientId) {
            if (itemTypeId == null || ingredientId == null) {
                return RedirectToAction("Index");
            }

            Ingredient dbIngredient;
            ItemType dbForItemType;
            ItemType dbItemType;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                dbIngredient = db.IngredientSet.Find(long.Parse(ingredientId));

                if (dbIngredient == null) {
                    return RedirectToAction("Index");
                }

                dbForItemType = dbIngredient.ForItemType.ToLoggable();
                dbItemType = dbIngredient.ItemType.ToLoggable();

                db.IngredientSet.Remove(dbIngredient);

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Ingrediens fjernet",
                        $"Ingrediens fjernet fra varetypen {dbForItemType.Name}.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Varetype = dbForItemType,
                            Ingrediens = dbIngredient.ToLoggable(),
                            IngrediensVaretype = dbItemType
                        }
                    )
            );

            return RedirectToAction("EditItemType", new { id = itemTypeId });
        }

        [HttpGet]
        public ActionResult DeleteItemType(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            ItemTypeViewModel viewModel = new ItemTypeViewModel();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                ItemType dbItemType = db.ItemTypeSet.Find(long.Parse(id));

                if (dbItemType == null) {
                    return RedirectToAction("Index");
                }

                viewModel.ItemType = dbItemType;
                viewModel.ItemType.Items = dbItemType.Items.ToList();
                
                foreach (Item item in viewModel.ItemType.Items) {
                    item.Location = db.LocationSet.SingleOrDefault(l => l.UID == item.Location.UID);
                }
                
                viewModel.ItemType.WorkOrderItem = dbItemType.WorkOrderItem.ToList();
                foreach (WorkOrderItem workOrderItem in viewModel.ItemType.WorkOrderItem) {
                    workOrderItem.WorkOrder = db.WorkOrderSet.SingleOrDefault(wo => wo.UID == workOrderItem.WorkOrder.UID);
                }

                viewModel.ItemType.IsIngredientFor = dbItemType.IsIngredientFor;
                foreach (Ingredient ingredient in viewModel.ItemType.IsIngredientFor) {
                    ingredient.ForItemType = db.ItemTypeSet.SingleOrDefault(it => it.UID == ingredient.ForItemType.UID);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteItemType(long id) {
            ItemType dbItemType;
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                dbItemType = db.ItemTypeSet.Find(id);

                if (dbItemType == null) {
                    return RedirectToAction("Index");
                }
                
                db.ItemSet.RemoveRange(dbItemType.Items);
                db.WorkOrderItemSet.RemoveRange(dbItemType.WorkOrderItem);
                db.IngredientSet.RemoveRange(dbItemType.IsIngredientFor);
                db.IngredientSet.RemoveRange(dbItemType.IngredientsForRecipe);
                db.ItemTypeSet.Remove(dbItemType);

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Varetype fjernet",
                        $"Varetypen {dbItemType.Name} blev fjernet.",
                        new {
                            KontekstBruger = UserController.GetContextUser().ToLoggable(),
                            Varetype = dbItemType.ToLoggable()
                        }
                    )
            );

            return RedirectToAction("Index");
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
                ItemType itemType = db.ItemTypeSet.Find(long.Parse(id));

                itemTypeViewModel.ItemType = itemType;
                itemTypeViewModel.ItemType.Items = itemType.Items;
                itemTypeViewModel.Ingredients = itemType.IngredientsForRecipe;

                foreach (Item item in itemTypeViewModel.ItemType.Items)
                {
                    item.Location = db.LocationSet.Single(l => l.UID == item.Location.UID);
                }
                foreach (Ingredient ingredient in itemTypeViewModel.Ingredients)
                {
                    ingredient.ItemType = db.ItemTypeSet.Single(i => i.UID == ingredient.ItemType.UID);
                }
            }

            return View(itemTypeViewModel);
        }

        public event ChangedEventHandler Changed;
    }
}