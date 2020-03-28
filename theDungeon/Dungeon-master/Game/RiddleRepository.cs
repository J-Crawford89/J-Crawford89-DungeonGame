using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class RiddleRepository
    {
        private List<Riddle> _riddleRepo = new List<Riddle>();
        public void AddToRiddles(Riddle riddle)
        {
            _riddleRepo.Add(riddle);
        }
        public int GetCount()
        {
            return _riddleRepo.Count();
        }
        public Riddle GetRiddleByIndex(int index)
        {
            return _riddleRepo[index];
        }
    }
}
