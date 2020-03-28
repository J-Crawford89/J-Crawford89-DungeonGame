using System;
using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Game.ItemClasses;

namespace Tests
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void DisplayInventory_ShouldShowInventory()
        {
            IItem sword = new Weapon() { Name = "Magic Sword", Description = "This sword gives you one free letter", };
            IItem swordTwo = new Weapon() { Name = "Magic Sword", Description = "This sword gives you one free letter", };
            IItem swordThree = new Weapon() { Name = "Magic Sword", Description = "This sword gives you one free letter", };
            IItem swordFour = new Weapon() { Name = "Magic Sword", Description = "This sword gives you one free letter", };

            Inventory inventory = new Inventory();
            inventory.AddToInventory(sword);
            inventory.AddToInventory(swordTwo);
            inventory.AddToInventory(swordThree);
            inventory.AddToInventory(swordFour);

            inventory.InventoryMenu();
        }
    }
}
