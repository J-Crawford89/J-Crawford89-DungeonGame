using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class HangmanTests
    {
        [TestMethod]
        public void HangmanTestOne()
        {
            Game.Game testGame = new Game.Game();

            testGame.Hangman("test");
        }
    }
}
