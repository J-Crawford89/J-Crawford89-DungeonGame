using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Room
    {
        public string Description 
        {
            get
            {
                Random random = new Random();
                int randomRoom = random.Next(1, 13);
                switch(randomRoom)
                {
                    case 1: // empty room
                    case 5:
                    case 6:
                        return "You survey your surroundings.\n" +
                            "This room is empty.";
                    case 2: // no enemy, only a pickup
                    case 11:
                        return "You survey your surroundings.\n" +
                            "This room is empty save for a small chest in the corner.";
                    case 3: // minion - will be changed to minion description in Game class
                    case 7:
                    case 8:
                    case 9:
                    case 12:
                        return "minion";
                    case 4: // boss - will be changed to boss description in Game class
                    case 10:
                        return "boss";
                    default:
                        return null;
                }
            }
        }
        public string Name { get; set; }
        public bool SetActiveRoom(string direction, List<string> directionChoices)
        {
            Random randomDirection = new Random();

            string correctDirection = directionChoices[randomDirection.Next(directionChoices.Count())];

            return direction == correctDirection;
            
        }
    }
}
