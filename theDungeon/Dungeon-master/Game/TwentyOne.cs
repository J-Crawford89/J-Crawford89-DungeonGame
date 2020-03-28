using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class TwentyOne
    {
        static int total = 0;
        public static bool hasWon = false;
        public static int playerScore;
        static int houseScore = 0;
        static int count = 1;
        static Random randomizeCards = new Random();
        public string nextMove = "";
        public static string hitOrStay = "";
        public static string[] userCards = new string[12];
        static int rematchCount = 1;
        static int maxTries = 3;

        public int Run(Inventory inventory)
        {
            total = 0;
            houseScore = randomizeCards.Next(15, 22);
            userCards[0] = DealCards();
            userCards[1] = DealCards();
            do
            {
                Console.WriteLine("Welcome to 21! To win you must beat the house." +
                    "\nThe objective is to get as close as possible, or up to, 21 " +
                    "\npoints. Hit to be dealt more cards or stay to play your hand" +
                    "\nagainst the house. Note, if you go over 21 you automatically lose!" +
                    "\nYou have 3 tries to beat the house!");
                Console.WriteLine("\n\n\nPress any key to continue.");
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Your cards are " + userCards[0] + " and " + userCards[1] + ". " +
                    "\nYour total is " + total + ".\nHit or stay?");
                hitOrStay = Console.ReadLine().ToLower();
                while (hitOrStay == "inventory")
                {
                    Console.Clear();
                    IItem itemChoice = inventory.InventoryMenu();
                    if (itemChoice == null)
                    {
                        Console.WriteLine("Your cards are " + userCards[0] + " and " + userCards[1] + ". " +
                    "\nYour total is " + total + ".\nHit or stay?");
                        hitOrStay = Console.ReadLine().ToLower();
                        break;
                    }
                    if (itemChoice.Type == "Weapon")
                    {
                        if (itemChoice.Name.Contains("Epic"))
                        {
                            hasWon = true;
                            playerScore = 500;
                            Console.WriteLine("You brandish your weapon, allowing you to slay the monster and advance.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            rematchCount--;
                            Console.WriteLine($"You brandish your weapon, giving you an extra attempt to beat the house. You have {maxTries - rematchCount} tries remaining.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        inventory.RemoveItem(itemChoice);
                        Console.WriteLine($"{itemChoice.Name} has been removed from your inventory.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("You cannot use that here");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    Console.WriteLine("Your cards are " + userCards[0] + " and " + userCards[1] + ". " +
                    "\nYour total is " + total + ".\nHit or stay?");
                    hitOrStay = Console.ReadLine().ToLower();
                }


            } while (!hitOrStay.Equals("hit") && !hitOrStay.Equals("stay"));
            HitOrStay();

            if (hasWon)
            {
                if (total == 21) { playerScore = 500; }
                else if (rematchCount == 1) { playerScore = 500; }
                else if (rematchCount == 2) { playerScore = 350; }
                else if (rematchCount == 3) { playerScore = 250; }
                Console.WriteLine("\n\n" +
                "Congratulations! You have beaten the house!");
                Console.WriteLine("Your score was: " + playerScore);
                Console.ReadLine();
                return playerScore;
            }
            else { return 25; }

        }
        public static void HitOrStay()
        {
            if (hitOrStay.Equals("hit"))
            {
                Hit();
            }
            else if (hitOrStay.Equals("stay"))
            {
                if (total > houseScore && total <= 21)
                {
                    switch (total)
                    {
                        case 1:
                            total = 13; playerScore = 100; hasWon = true;
                            break;
                        case 2:
                            total = 14; playerScore = 150; hasWon = true;
                            break;
                        case 3:
                            total = 15; playerScore = 200; hasWon = true;
                            break;
                        case 4:
                            total = 16; playerScore = 250; hasWon = true;
                            break;
                        case 5:
                            total = 17; playerScore = 300; hasWon = true;
                            break;
                        case 6:
                            total = 18; playerScore = 350; hasWon = true;
                            break;
                        case 7:
                            total = 19; playerScore = 400; hasWon = true;
                            break;
                        case 8:
                            total = 20; playerScore = 450; hasWon = true;
                            break;
                        case 9:
                            total = 21; playerScore = 500; hasWon = true;
                            break;
                    }
                    hasWon = true;
                    Console.WriteLine("\n You won. The house total " +
                        "is " + houseScore + ".\n Would you like to play again? y/n");
                    ReMatch();
                }
                else if (total < houseScore)
                {
                    hasWon = false;
                    playerScore = 25;
                    Console.WriteLine("\n You lost! The house's total was " +
                        "" + houseScore + ".\n Would you like to play again? y/n");
                    ReMatch();
                }
            }
        }
        public static string DealCards()
        {
            string Card = "";
            int cards = randomizeCards.Next(1, 14);
            switch (cards)
            {
                case 1:
                    Card = "Two"; total += 2;
                    break;
                case 2:
                    Card = "Three"; total += 3;
                    break;
                case 3:
                    Card = "Four"; total += 4;
                    break;
                case 4:
                    Card = "Five"; total += 5;
                    break;
                case 5:
                    Card = "Six"; total += 6;
                    break;
                case 6:
                    Card = "Seven"; total += 7;
                    break;
                case 7:
                    Card = "Eight"; total += 8;
                    break;
                case 8:
                    Card = "Nine"; total += 9;
                    break;
                case 9:
                    Card = "Ten"; total += 10;
                    break;
                case 10:
                    Card = "Jack"; total += 10;
                    break;
                case 11:
                    Card = "Queen"; total += 10;
                    break;
                case 12:
                    Card = "King"; total += 10;
                    break;
                case 13:
                    Card = "Ace"; total += 11;
                    break;
                default:
                    Card = "2"; total += 2;
                    break;
            }
            return Card;
        }
        public static void Hit()
        {
            hasWon = true;
            playerScore = 500;
            count += 1;
            userCards[count] = DealCards();
            Console.WriteLine("\nYou were dealt: " + userCards[count] + ".\n" +
                "Your new total is " + total + ".");
            if (total.Equals(21))
            {
                playerScore = 500;
                Console.WriteLine("\nYou have 21! You win! The dealer's score is " + houseScore + ".\n");
                Console.ReadLine();
            }
            else if (total > 21)
            {
                hasWon = false;
                playerScore = 25;
                Console.WriteLine("\nYou went over 21! You've lost. The dealer's total" +
                    " was " + houseScore + ".\n Play again? y/n");
                ReMatch();
            }
            else if (total < 21)
            {
                do
                {
                    Console.WriteLine("\nWould you like to hit or stay?");
                    hitOrStay = Console.ReadLine().ToLower();
                } while (!hitOrStay.Equals("hit") && !hitOrStay.Equals("stay"));
                HitOrStay();
            }
        }
        static void ReMatch()
        {
            string reMatch = "";
            do
            {
                reMatch = Console.ReadLine().ToLower();
            } while (!reMatch.Equals("y") && !reMatch.Equals("n"));
            if (reMatch.Equals("y") && rematchCount < maxTries)
            {
                Console.WriteLine("\nPress enter to retry!");
                Console.ReadLine();
                Console.Clear();
                rematchCount++;
                playerScore = 0;
                houseScore = 0;
                count = 1;
                total = 0;
                houseScore = randomizeCards.Next(12, 22);
                userCards[0] = DealCards();
                userCards[1] = DealCards();
                Console.WriteLine("Your cards are " + userCards[0] + " and " + userCards[1] + ". " +
                    "\nYour total is " + total + ".\nHit or stay?");
                hitOrStay = Console.ReadLine().ToLower();
                while (!hitOrStay.Equals("hit") && !hitOrStay.Equals("stay")) ;
                HitOrStay();
            }
            else if (reMatch.Equals("y") && rematchCount >= maxTries)
            {
                Console.WriteLine("You have tried the maximum number of times.");
                Console.ReadLine();
            }
            else if (reMatch.Equals("n"))
            {
                Console.WriteLine("\nPress enter to exit.");
                Console.ReadLine();
            }
        }

    }
}
