using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ACLager.Controllers;
using ACLager.CustomClasses;
using ACLager.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACLager.Tests.Controllers {
    [TestClass]
    public class InventoryControllerTest
    {
        private List<Item> items { get; set; }
        private List<ItemType> itemTypes { get; set; }
        private List<Location> locations { get; set; }
        private InventoryController controller { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            /* Arrange */
            items = new List<Item>();
            itemTypes = new List<ItemType>();
            locations = new List<Location>();

            itemTypes.Add(new ItemType {
                UID = 1,
                Name = "Chokolademandler",
                MinimumAmount = 0,
                Unit = "stk.",
                IsActive = true,
                Procedure = "",
                Barcode = "123456789",
                Department = "Produktion"
            });
            itemTypes.Add(new ItemType {
                UID = 2,
                Name = "Chokolademus",
                MinimumAmount = 0,
                Unit = "kg.",
                IsActive = true,
                Procedure = "",
                Barcode = "987654321",
                Department = "Produktion"
            });
            itemTypes.Add(new ItemType
            {
                UID = 3,
                Name = "Chokoladekugler",
                MinimumAmount = 0,
                Unit = "g.",
                IsActive = true,
                Procedure = "",
                Barcode = "123789456",
                Department = "Produktion"
            });

            locations.Add(new Location
            {
                UID = 1,
                Name = "L1",
                IsActive = true
            });
            locations.Add(new Location {
                UID = 2,
                Name = "L2",
                IsActive = true
            });
            locations.Add(new Location
            {
                UID = 3,
                Name = "L3",
                IsActive = true
            });
            locations.Add(new Location
            {
                UID = 4,
                Name = "L4",
                IsActive = true
            });
            locations.Add(new Location {
                UID = 5,
                Name = "L5",
                IsActive = true
            });

            items.Add(new Item
            {
                UID = 1,
                Amount = 100,
                DeliveryDate = new DateTime(1999, 01, 01),
                ExpirationDate = new DateTime(1999, 01, 01),
                Supplier = "AC",
                Reserved = 0,
                Location = locations.First(l => l.UID == 1),
                ItemType = itemTypes.First(it => it.UID == 1)
            });
            items.Add(new Item
            {
                UID = 2,
                Amount = 50,
                DeliveryDate = new DateTime(1999, 01, 01),
                ExpirationDate = new DateTime(1999, 01, 01),
                Supplier = "AC",
                Reserved = 0,
                Location = locations.First(l => l.UID == 2),
                ItemType = itemTypes.First(it => it.UID == 2)
            });
            items.Add(new Item
            {
                UID = 3,
                Amount = 200,
                DeliveryDate = new DateTime(1999, 01, 01),
                ExpirationDate = new DateTime(1999, 01, 01),
                Supplier = "AC",
                Reserved = 0,
                Location = locations.First(l => l.UID == 3),
                ItemType = itemTypes.First(it => it.UID == 2)
            });

            locations.First(l => l.UID == 1).Item = items.First(i => i.UID == 1);
            locations.First(l => l.UID == 2).Item = items.First(i => i.UID == 2);
            locations.First(l => l.UID == 3).Item = items.First(i => i.UID == 3);

            itemTypes.First(it => it.UID == 1).Items.Add(items.First(i => i.UID == 1));
            itemTypes.First(it => it.UID == 2).Items.Add(items.First(i => i.UID == 2));
            itemTypes.First(it => it.UID == 2).Items.Add(items.First(i => i.UID == 3));

            controller = new InventoryController();
        }

        [TestMethod]
        public void GenerateItemGroups()
        {
            /* Arrange */
            IEnumerable<ItemGroup> expectedItemGroups = new ItemGroup[]
            {
                new ItemGroup
                {
                    ItemType = itemTypes.First(it => it.UID == 1),
                    ItemLocationPairs = new ItemLocationPair[]
                    {
                        new ItemLocationPair
                        {
                            Item = items.First(i => i.UID == 1),
                            Location = locations.First(l => l.UID == 1)
                        }
                    }
                },
                new ItemGroup
                {
                    ItemType = itemTypes.First(it => it.UID == 2),
                    ItemLocationPairs = new ItemLocationPair[]
                    {
                        new ItemLocationPair
                        {
                            Item = items.First(i => i.UID == 2),
                            Location = locations.First(l => l.UID == 2)
                        }, 
                        new ItemLocationPair
                        {
                            Item = items.First(i => i.UID == 3),
                            Location = locations.First(l => l.UID == 3)
                        } 
                    }
                } 
            };

            /* Act */
            IEnumerable<ItemGroup> actualItemGroups = controller.GenerateItemGroups(items, itemTypes, locations);

            /* Assert */
            Assert.IsTrue(expectedItemGroups.SequenceEqual(actualItemGroups));
        }

        [TestMethod]
        public void GenerateLocationSelectListItems()
        {
            /* Arrange */
            IEnumerable<SelectListItem> expectedLocationSelectListItems = new SelectListItem[]
            {
                new SelectListItem
                {
                    Text = "L4",
                    Value = "4"
                },
                new SelectListItem
                {
                    Text = "L5",
                    Value = "5"
                }
            };

            /* Act */
            IEnumerable<SelectListItem> actualLocationSelectListItems =
                controller.GenerateLocationSelectListItems(locations);

            /* Assert */
            Assert.IsTrue(SelectListItemsSequenceEqual(expectedLocationSelectListItems, actualLocationSelectListItems));
        }

        [TestMethod]
        public void GenerateItemTypeSelectListItems()
        {
            /* Arrange */
            IEnumerable<SelectListItem> expectedItemTypeSelectListItems = new SelectListItem[]
            {
                new SelectListItem
                {
                    Text = "Chokolademandler",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Chokolademus",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "Chokoladekugler",
                    Value = "3"
                }
            };

            /* Act */
            IEnumerable<SelectListItem> actualItemTypeSelectListItems =
                controller.GenerateItemTypeSelectListItems(itemTypes);

            /* Assert */
            Assert.IsTrue(SelectListItemsSequenceEqual(expectedItemTypeSelectListItems, actualItemTypeSelectListItems));
        }

        [TestCleanup]
        public void TearDown()
        {
            controller.Dispose();
        }

        private bool SelectListItemsSequenceEqual(
            IEnumerable<SelectListItem> expectedSelectListItems,
            IEnumerable<SelectListItem> actualSelectListItems)
        {
            return
                expectedSelectListItems.Select(sli => sli.Text)
                    .SequenceEqual(actualSelectListItems.Select(sli => sli.Text)) &&
                expectedSelectListItems.Select(sli => sli.Value)
                    .SequenceEqual(actualSelectListItems.Select(sli => sli.Value));
        }
    }
}
