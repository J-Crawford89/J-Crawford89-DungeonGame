using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ItemClasses;

namespace Game
{
    class ArmorRepository : IItemRepository
    {
        private List<Armor> _armorRepo = new List<Armor>();
        public void SeedItems()
        {
            string[] contentArray = System.IO.File.ReadAllLines(@"C:\Users\flyca\OneDrive\Documents\ELEVENFIFTY\DotNetFeb2020\C#\Game\Game\Items\Armor.txt");
            foreach (string content in contentArray)
            {
                string[] splitContent = content.Split('|');
                Armor newItem = new Armor() { Name = splitContent[0], Description = splitContent[1] };

                _armorRepo.Add(newItem);
            }
        }
        public int GetCount()
        {
            return _armorRepo.Count();
        }
        public IItem GetItemByIndex(int i)
        {
            return _armorRepo[i];
        }
    }
}
