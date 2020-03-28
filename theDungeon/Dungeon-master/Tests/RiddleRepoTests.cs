using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class RiddleRepoTests
    {
        [TestMethod]
        public void GetByIndex_ShouldGetRiddle()
        {
            RiddleRepository repo = new RiddleRepository();
            List<int> usedList = new List<int>();
            Riddle riddle = new Riddle("keyword", "description");
            repo.AddToRiddles(riddle);
            
            Game.Game newGame = new Game.Game();

            Riddle getRiddle = newGame.GetRiddle(repo, usedList);

            string actualDescription = getRiddle.Description;
            string expectedDescription = "description";

            string actualKeyword = getRiddle.Keyword;
            string expectedKeyword = "keyword";

            Assert.AreEqual(expectedDescription, actualDescription);
            Assert.AreEqual(expectedKeyword, actualKeyword);
            
        }
    }
}
