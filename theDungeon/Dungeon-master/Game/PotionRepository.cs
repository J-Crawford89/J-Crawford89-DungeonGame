using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ItemClasses;

namespace Game
{
    class PotionRepository : IItemRepository
    {
        private List<Potion> _potionRepo = new List<Potion>();
        public void SeedItems()
        {
            string[] contentArray = System.IO.File.ReadAllLines(@"C:\Users\flyca\OneDrive\Documents\ELEVENFIFTY\DotNetFeb2020\C#\Game\Game\Items\Potions.txt");
            foreach (string content in contentArray)
            {
                string[] splitContent = content.Split('|');
                Potion newItem = new Potion() { Name = splitContent[0], Description = splitContent[1] };

                _potionRepo.Add(newItem);
            }
        }
        public int GetCount()
        {
            return _potionRepo.Count();
        }
        public IItem GetItemByIndex(int i)
        {
            return _potionRepo[i];
        }
    }
}
