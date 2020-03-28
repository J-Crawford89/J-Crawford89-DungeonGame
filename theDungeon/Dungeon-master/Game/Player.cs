using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Player
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public Player() { }
        public Player(int score, string playerName) 
        {
            Score = score;
            PlayerName = playerName;
        }
        public void GetPlayerName()
        {

            while (PlayerName.Length == 1)
            {
                Console.WriteLine("Welcome! What is your name?");
                PlayerName = Console.ReadLine().ToLower();

                if (PlayerName.Length > 1 && PlayerName[0] != ' ')
                {
                    PlayerName = PlayerName.Substring(0, 1).ToUpper() + PlayerName.Substring(1).ToLower();
                }
                else if (PlayerName.Length == 1 && PlayerName[0] != ' ')
                {
                    PlayerName.ToUpper();
                }
                else
                {
                    PlayerName = " ";
                }

            }
        }
    }
}
