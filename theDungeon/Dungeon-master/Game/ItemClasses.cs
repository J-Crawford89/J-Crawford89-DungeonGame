using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class ItemClasses
    {
        public class Weapon : IItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type => "Weapon";
        }
        public class Armor : IItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type => "Armor";
        }
        public class Potion : IItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type => "Potion";
        }
    }
}
