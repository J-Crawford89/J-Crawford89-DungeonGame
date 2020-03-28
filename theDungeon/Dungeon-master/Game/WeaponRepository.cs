using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ItemClasses;

namespace Game
{
    class WeaponRepository : IItemRepository
    {
        private List<Weapon> _weaponRepo = new List<Weapon>();
        public void SeedItems()
        {
            string[] contentArray = System.IO.File.ReadAllLines(@"C:\Users\flyca\OneDrive\Documents\ELEVENFIFTY\DotNetFeb2020\C#\Game\Game\Items\Weapons.txt");
            foreach(string content in contentArray)
            {
                string[] splitContent = content.Split('|');
                Weapon newItem = new Weapon() { Name = splitContent[0], Description = splitContent[1] };

                _weaponRepo.Add(newItem);                
            }
        }
        public int GetCount()
        {
            return _weaponRepo.Count();
        }
        public IItem GetItemByIndex(int i)
        {
            return _weaponRepo[i];
        }
    }
}
