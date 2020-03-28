using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Inventory
    {
        public List<IItem> _playerInventory = new List<IItem>();
        public void AddToInventory(IItem item)
        {
            _playerInventory.Add(item);
        }
        public IItem InventoryMenu()
        {
            Console.Clear();
            Console.WriteLine($"______________________________________________________________________________________________________________________\n\n" +
                $"------------------------------------------------------INVENTORY-------------------------------------------------------\n" +
                $"______________________________________________________________________________________________________________________\n\n\n" +
                $"        {"TYPE",-8}      {"NAME",-30}      {"DESCRIPTION",60}\n" +
                $"----------------------------------------------------------------------------------------------------------------------");
            for (int i = 1; i <= _playerInventory.Count(); i++)
            {
                Console.WriteLine($"{i,-2}  ||  {_playerInventory[i - 1].Type,-8}  ||  {_playerInventory[i - 1].Name,-30}  ||  {_playerInventory[i - 1].Description,60}");
            }
            Console.ReadLine();
            Console.WriteLine("Select the number of the item you wish to use.");
            string command = Console.ReadLine();
            Console.Clear();
            try
            {
                int inventoryItemNumber = Convert.ToInt32(command) - 1;
                return _playerInventory[inventoryItemNumber];
            }
            catch { return null; }
        }
        public void RemoveItem(IItem item)
        {
            _playerInventory.Remove(item);
        }
    }
}
