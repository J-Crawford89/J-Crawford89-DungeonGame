using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Riddle
    {
        public string Keyword { get; set; }
        public string Description { get; set; }
        public Riddle() { }
        public Riddle(string keyword, string description)
        {
            Keyword = keyword;
            Description = description;
        }
    }
}
