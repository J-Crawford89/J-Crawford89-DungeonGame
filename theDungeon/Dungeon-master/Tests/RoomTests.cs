using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class RoomTests
    {
        [TestMethod]
        public void SetActiveRoom_ShouldSelectRandomCorrectDirection()
        {
            List<string> directions = new List<string>() { "north", "east", "west" };
            Room testRoom = new Room();

            bool correctRoom = testRoom.SetActiveRoom("north", directions);

            Console.WriteLine(correctRoom);
        }
        [TestMethod]
        public void GetRoom_WillSameRoomRegen()
        {
            int i = 0;
            while (i <= 20)
            {
                i++;
                if (i > 0)
                {

                    Room testRoom = new Room();
                    Console.WriteLine(testRoom.Description);
                }
            }
        }
    }
}
