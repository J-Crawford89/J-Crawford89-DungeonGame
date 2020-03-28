using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void WriteHighScores_ShouldWriteHighScoreToSav()
        {
            Player jon = new Player(500, "Jon");
            Player steve = new Player(775, "Steve");
            Player dave = new Player(250, "Dave");
            Player sally = new Player(1100, "Sally");
            Player jim = new Player(5000, "Jim");
            Player donna = new Player(8200, "Donna");
            Player jill = new Player(1900, "Jill");
            Player megan = new Player(2200, "Megan");

            List<Player> highScores = new List<Player>() { jon, steve, dave, sally, jim, donna, jill, megan };

            foreach (Player player in highScores)
            {

                string highScore = $"{player.Score.ToString()}|{player.PlayerName}";

                System.IO.File.AppendAllText(@"C:\Users\flyca\OneDrive\Documents\ELEVENFIFTY\DotNetFeb2020\C#\Game\Game\HighScores.sav", $"{highScore}\n");

            }

            
        }
        [TestMethod]
        public void ReadHighScores_ShouldReadHighScoresInOrder()
        {
            string[] text = System.IO.File.ReadAllLines(@"C:\Users\flyca\OneDrive\Documents\ELEVENFIFTY\DotNetFeb2020\C#\Game\Game\HighScores.sav");

            List<Player> highScores = new List<Player>();

            foreach (string line in text)
            {
                string[] splitLine = line.Split('|');
                highScores.Add(new Player(int.Parse(splitLine[0]), splitLine[1]));
            }

            Console.WriteLine(String.Format("{0, -10}         {1,10}", "Name", "Score"));
            Console.WriteLine("-----------------------------");

            foreach (Player player in highScores.OrderByDescending(x => x.Score))
            {
                string highScore = String.Format($"{player.PlayerName,-15} | {player.Score,8}");
                Console.WriteLine(highScore);
            }
            Console.ReadLine();
        }
        
    }
}
