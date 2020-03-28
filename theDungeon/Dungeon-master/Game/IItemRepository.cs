using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface IItemRepository
    {
        void SeedItems();
        int GetCount();
        IItem GetItemByIndex(int i);
    }
}
