using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.ItemClasses;

namespace Game
{
    public class Game
    {
        private readonly RiddleRepository _minionRiddles = new RiddleRepository();
        private readonly RiddleRepository _bossRiddles = new RiddleRepository();
        private readonly List<int> _usedMinionRiddles = new List<int>();
        private readonly List<int> _usedBossRiddles = new List<int>();
        private string _gameTitle = "the DUNGEON";
        private Inventory _inventory = new Inventory();
        private readonly WeaponRepository _weaponRepo = new WeaponRepository();
        private readonly ArmorRepository _armorRepo = new ArmorRepository();
        private readonly PotionRepository _potionRepo = new PotionRepository();


        public void Run()
        {
            _weaponRepo.SeedItems();
            _armorRepo.SeedItems();
            _potionRepo.SeedItems();
            // Game Title Screen
            
            string[] title = System.IO.File.ReadAllLines(@"Title.txt");
            foreach (string line in title)
            { Console.WriteLine(line); }
            Console.ReadLine();
            Console.Clear();
            RunMenu();
        }

        public void RunMenu()
        {
            Console.WriteLine($"__________________________________________________________________________________________\n\n" +
                $"----------------------------------------MAIN MENU-----------------------------------------\n" +
                $"__________________________________________________________________________________________\n\n\n");
            Console.WriteLine($"Welcome to {_gameTitle}\n\n" +
                $"1. New Game\n" +
                $"2. Load Game\n" +
                $"3. How to Play\n" +
                $"4. High Scores\n" +
                $"5. Exit\n\n");
            Console.Write("Please make a selection: ");
            string selection = Console.ReadLine().ToLower();
            Console.Clear();
            switch (selection)
            {
                case "1":
                case "new game":
                    SetNewGame();
                    break;
                case "2":
                case "load game":
                    LoadGameMenu();
                    break;
                case "3":
                case "how to play":
                    Instructions();
                    break;
                case "4":
                case "high scores":
                    ShowHighScores();
                    break;
                case "5":
                case "exit":
                    break;
                default:
                    RunMenu();
                    break;
            }
        }
        public void Instructions()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to {_gameTitle}. The goal of the game is to travel through the dungeon, one room at a time.\n" +
                $"In each room, you may find a minion, a boss monster, or may find some respite in an empty room.\n" +
                $"To defeat a monster, you must successfully complete a minigame. After which you will be awarded points and then may continue to another room.\n" +
                $"Whenever you would enter a room or while you are in a mini-game, you may type 'inventory' to pull up your inventory and use an item you have found.\n" +
                $"Your initial room choice is important, as the dungeon has three branches to it, a north branch, an east branch, and a west branch.\n" +
                $"At the end of one of the branches is {_gameTitle}'s main boss. Defeat this boss to win the game and add your name to the high scores list!\n" +
                $"If you fail to defeat a monster, you lose a life and attempt again. Too many fails and it's game over!\n" +
                $"Your difficulty level determines the difficulty of each encounter, as well as the length of {_gameTitle}.\n\n" +
                $"Do you have what it takes to defeat {_gameTitle}? Then what are you waiting for?");
            Console.ReadLine();
            RunMenu();
        }
        public void SetNewGame()
        {
            Player newPlayer = new Player();
            newPlayer.PlayerName = " ";

            newPlayer.GetPlayerName();

            Console.Clear();

            Difficulty difficulty = SetDifficulty(newPlayer.PlayerName);

            int lives = difficulty.Lives;
            int correctDirectionsNeeded = difficulty.CorrectDirectionsNeeded;
            int maxRooms = difficulty.MaxRooms;
            newPlayer.Score = 0;
            int roomScore = 0;
            int roomNumber = 1;
            int correctDirectionChoices = 0;
            string overallDirectionChoice = "";
            string directionChoice = "";

            RunGame(newPlayer, lives, correctDirectionsNeeded, maxRooms, roomScore,
                roomNumber, correctDirectionChoices, overallDirectionChoice, directionChoice);
        }

        public void RunGame(Player newPlayer, int lives, int correctDirectionsNeeded, int maxRooms, int roomScore,
            int roomNumber, int correctDirectionChoices, string overallDirectionChoice, string directionChoice)
        {
            Room firstRoom = new Room();

            int score = newPlayer.Score;

            List<string> directionChoicesStart = new List<string>() { "north", "east", "west" };

            List<string> startChoices = new List<string>();

            bool correctOverallDirection = false;

            bool wonGame = false;

            string response;
            // full game loop
            while (lives > 0 && !wonGame)
            {

                bool backToStart = false;
                bool correctChoice = false;

                // loop after starting room
                while (lives > 0 && !wonGame && !backToStart)
                {
                    score += roomScore;
                    // lose a life and try again
                    if (roomScore == 25)
                    {
                        lives--;
                        if (lives > 0)
                        {
                            Console.WriteLine($"You have lost a life. You have {lives} lives remaining.\n" +
                            $"You fall back, regroup, and press forward once more.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        roomScore = 0;
                    }

                    // advance
                    else if (score > 0)
                    {

                        Room currentRoom = new Room();

                        // second room (only one direction to go)
                        if (roomNumber == 1)
                        {
                            correctChoice = true;
                            Console.WriteLine("At the end of the room is a single door leading onward.\n" +
                                "Press ENTER to continue through.");
                            string input = Console.ReadLine().ToLower();
                            while (input == "inventory")
                            {
                                Console.Clear();
                                IItem itemChoice = _inventory.InventoryMenu();
                                if (itemChoice == null) { break; }

                                if (itemChoice.Type == "Armor")
                                {
                                    lives++;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You equip your shield, granting you one extra life. You now have {lives} lives remaining.\n\n" +
                                        $"{itemChoice.Name} has been removed from your inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }

                                else if (itemChoice.Name.Contains("Speed"))
                                {
                                    maxRooms--;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You drink the lightning charged Potion of Speed. You dash through the current room on to the next one.\n\n" +
                                        $"{itemChoice.Name} has been removed from your inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                else if (itemChoice.Name.Contains("Foresight"))
                                {
                                    correctDirectionChoices++;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You drink the crystal clear Potion of Foresight. You are certain you are headed in the correct direction." +
                                        $"{itemChoice.Name} has been removed from you inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                else if (itemChoice.Name.Contains("Teleportation"))
                                {
                                    backToStart = true;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You drink the chaotic Potion of Teleportation. You find yourself back in the original room.\n\n" +
                                        $"{itemChoice.Name} has been removed from your inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                else { Console.WriteLine("You cannot use that here."); Console.ReadLine(); Console.Clear(); }
                                Console.WriteLine("At the end of the room is a single door leading onward.\n" +
                                "Press ENTER to continue through.");
                                input = Console.ReadLine().ToLower();
                            }
                            Console.Clear();
                            // enter second room
                            if (!backToStart)
                            {
                                roomScore = EnterRoom(currentRoom.Description);
                                Console.WriteLine("\n\nMake a selection:\n" +
                                    "1. Continue\n" +
                                    "2. Save\n");
                                response = Console.ReadLine().ToLower();
                                while (response != "1" && response != "continue" && response != "2" && response != "save")
                                {
                                    Console.WriteLine("I didn't understand. Please make a selection:");
                                    response = Console.ReadLine().ToLower();
                                }
                                switch (response)
                                {
                                    case "2":
                                    case "save":
                                        SaveMenu(newPlayer, lives, correctDirectionsNeeded, maxRooms, roomScore,
                                            roomNumber, correctDirectionChoices, overallDirectionChoice, directionChoice);
                                        break;
                                }
                                Console.Clear();
                            }
                        }

                        // after second room
                        else if (correctDirectionChoices < correctDirectionsNeeded && roomNumber <= maxRooms)
                        {
                            List<string> nextDirection = new List<string>() { "north", "south", "east", "west" };
                            nextDirection.Remove(GetOppositeDirection(directionChoice));
                            Console.WriteLine($"You see three doors in this room:\n" +
                                $"1. A door to the {nextDirection[0]},\n" +
                                $"2. A door to the {nextDirection[1]},\n" +
                                $"3. A door to the {nextDirection[2]}\n\n" +
                                $"Please make a selection.");
                            directionChoice = Console.ReadLine().ToLower();
                            // direction choice
                            while (directionChoice == "inventory")
                            {
                                Console.Clear();
                                IItem itemChoice = _inventory.InventoryMenu();
                                if (itemChoice == null) 
                                {
                                    Console.WriteLine($"You see three doors in this room:\n" +
                                $"1. A door to the {nextDirection[0]},\n" +
                                $"2. A door to the {nextDirection[1]},\n" +
                                $"3. A door to the {nextDirection[2]}\n\n" +
                                $"Please make a selection.");
                                    directionChoice = Console.ReadLine().ToLower();
                                    break; 
                                }

                                if (itemChoice.Type == "Armor")
                                {
                                    lives++;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You equip your shield, granting you one extra life. You now have {lives} lives remaining.\n\n" +
                                        $"{itemChoice.Name} has been removed from your inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }

                                else if (itemChoice.Name.Contains("Speed"))
                                {
                                    maxRooms--;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You drink the lightning charged Potion of Speed. You dash through the current room on to the next one.\n\n" +
                                        $"{itemChoice.Name} has been removed from your inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                else if (itemChoice.Name.Contains("Foresight"))
                                {
                                    correctDirectionChoices++;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You drink the crystal clear Potion of Foresight. You are certain you are headed in the correct direction." +
                                        $"{itemChoice.Name} has been removed from you inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                else if (itemChoice.Name.Contains("Teleportation"))
                                {
                                    backToStart = true;
                                    _inventory.RemoveItem(itemChoice);
                                    Console.WriteLine($"You drink the chaotic Potion of Teleportation. You find yourself back in the original room.\n\n" +
                                        $"{itemChoice.Name} has been removed from your inventory.");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                else { Console.WriteLine("You cannot use that here."); Console.ReadLine(); Console.Clear(); }
                                Console.WriteLine($"You see three doors in this room:\n" +
                                $"1. A door to the {nextDirection[0]},\n" +
                                $"2. A door to the {nextDirection[1]},\n" +
                                $"3. A door to the {nextDirection[2]}\n\n" +
                                $"Please make a selection.");
                                directionChoice = Console.ReadLine().ToLower();
                            }
                            if (!backToStart)
                            {

                                while (directionChoice != "north" && directionChoice != "south" && directionChoice != "east" && directionChoice != "west")
                                {
                                    switch (directionChoice)
                                    {
                                        case "1":
                                            directionChoice = nextDirection[0];
                                            break;
                                        case "2":
                                            directionChoice = nextDirection[1];
                                            break;
                                        case "3":
                                            directionChoice = nextDirection[2];
                                            break;
                                        default:
                                            Console.WriteLine("I didn't understand. Please select which door you would like to walk through.");
                                            directionChoice = Console.ReadLine().ToLower();
                                            break;
                                    }
                                }
                                Console.Clear();
                                // enter rooms 3 until final
                                correctChoice = currentRoom.SetActiveRoom(directionChoice, nextDirection);
                                roomScore = EnterRoom(currentRoom.Description);
                                Console.WriteLine("\n\nMake a selection:\n" +
                                    "1. Continue\n" +
                                    "2. Save\n");
                                response = Console.ReadLine().ToLower();
                                while (response != "1" && response != "continue" && response != "2" && response != "save")
                                {
                                    Console.WriteLine("I didn't understand. Please make a selection:");
                                    response = Console.ReadLine().ToLower();
                                }
                                switch (response)
                                {
                                    case "2":
                                    case "save":
                                        SaveMenu(newPlayer, lives, correctDirectionsNeeded, maxRooms, roomScore,
                                            roomNumber, correctDirectionChoices, overallDirectionChoice, directionChoice);
                                        break;
                                }
                                Console.Clear();
                            }
                        }
                        else
                        {
                            // final boss room
                            if (correctOverallDirection)
                            {
                                // final boss
                                Room finalRoom = new Room();
                                finalRoom.Name = "final";
                                roomScore = EnterRoom("final");
                                if (roomScore != 25)
                                {
                                    wonGame = true;
                                    score += roomScore;
                                }
                            }
                            // leg complete, but wrong leg - start over
                            else
                            {
                                Console.WriteLine("You enter the final room, only to realize this is not the correct direction.\n" +
                                    "You are teleported back to the start room.\n" +
                                    "Press Enter to accept your fate.");
                                Console.ReadLine();
                                Console.Clear();
                                backToStart = true;
                            }
                        }

                        if (correctChoice)
                        {
                            correctDirectionChoices++;
                        }
                        roomNumber++;
                    }
                    else
                    {
                        backToStart = true;
                    }
                }
                if (lives > 0 && !wonGame)
                {

                    roomNumber = 1;
                    correctDirectionChoices = 0;
                    directionChoice = overallDirectionChoice;
                    // starting room - choose first room to go to
                    directionChoicesStart.Remove(overallDirectionChoice);
                    Console.WriteLine($"You are in an empty room.\n" +
                        $"You see three doors in this room:\n" +
                        $"1. A door to the north,\n" +
                        $"2. A door to the east,\n" +
                        $"3. A door to the west\n");

                    if (startChoices.Count > 0)
                    {
                        Console.WriteLine("You have already tried the following doors:");
                        foreach (string choice in startChoices)
                        {
                            Console.WriteLine(choice);
                        }
                    }
                    Console.WriteLine("Please select which door you would like to walk through.");

                    overallDirectionChoice = Console.ReadLine().ToLower();
                    while (overallDirectionChoice == "inventory")
                    {
                        Console.Clear();
                        IItem itemChoice = _inventory.InventoryMenu();
                        if (itemChoice == null) { break; }
                        if (itemChoice.Type == "Armor")
                        {
                            lives++;
                            _inventory.RemoveItem(itemChoice);
                            Console.WriteLine($"You equip your shield, granting you one extra life. You now have {lives} lives remaining.\n\n" +
                                $"{itemChoice.Name} has been removed from your inventory.");
                            Console.ReadLine();
                            Console.Clear();
                        }

                        else if (itemChoice.Name.Contains("Speed"))
                        {
                            maxRooms--;
                            _inventory.RemoveItem(itemChoice);
                            Console.WriteLine($"You drink the lightning charged Potion of Speed. You dash through the current room on to the next one.\n\n" +
                                $"{itemChoice.Name} has been removed from your inventory.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else if (itemChoice.Name.Contains("Foresight"))
                        {
                            correctDirectionChoices++;
                            _inventory.RemoveItem(itemChoice);
                            Console.WriteLine($"You drink the crystal clear Potion of Foresight. You are certain you are headed in the correct direction." +
                                $"{itemChoice.Name} has been removed from you inventory.");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else { Console.WriteLine("You cannot use that here."); Console.ReadLine(); Console.Clear(); }
                        Console.WriteLine($"You are in an empty room.\n" +
                        $"You see three doors in this room:\n" +
                        $"1. A door to the north,\n" +
                        $"2. A door to the east,\n" +
                        $"3. A door to the west\n");

                        if (startChoices.Count > 0)
                        {
                            Console.WriteLine("You have already tried the following doors:");
                            foreach (string choice in startChoices)
                            {
                                Console.WriteLine(choice);
                            }
                        }
                        Console.WriteLine("Please select which door you would like to walk through.");

                        overallDirectionChoice = Console.ReadLine().ToLower();
                    }
                    while (overallDirectionChoice != "north" && overallDirectionChoice != "east" && overallDirectionChoice != "west")
                    {
                        switch (overallDirectionChoice)
                        {
                            case "1":
                                overallDirectionChoice = "north";
                                break;
                            case "2":
                                overallDirectionChoice = "east";
                                break;
                            case "3":
                                overallDirectionChoice = "west";
                                break;
                            default:
                                Console.WriteLine("I didn't understand. Please select which door you would like to walk through");
                                overallDirectionChoice = Console.ReadLine().ToLower();
                                break;
                        }
                    }

                    directionChoice = overallDirectionChoice;

                    Console.Clear();

                    startChoices.Add(overallDirectionChoice);

                    correctOverallDirection = firstRoom.SetActiveRoom(overallDirectionChoice, directionChoicesStart);

                    // enter first room
                    roomScore = EnterRoom(firstRoom.Description);
                    Console.WriteLine("\n\nMake a selection:\n" +
                                    "1. Continue\n" +
                                    "2. Save\n");
                    response = Console.ReadLine().ToLower();
                    while (response != "1" && response != "continue" && response != "2" && response != "save")
                    {
                        Console.WriteLine("I didn't understand. Please make a selection:");
                        response = Console.ReadLine().ToLower();
                    }
                    switch (response)
                    {
                        case "2":
                        case "save":
                            SaveMenu(newPlayer, lives, correctDirectionsNeeded, maxRooms, roomScore,
                                roomNumber, correctDirectionChoices, overallDirectionChoice, directionChoice);
                            break;
                    }

                    Console.Clear();
                }
            }
            if (lives == 0 || roomScore == 25)
            {
                score += roomScore;
                Console.WriteLine($"Your heroic adventures have come to an end.\n" +
                    $"The tale of {newPlayer.PlayerName} ends here.\n\n\n" +
                    $"GAME OVER");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Congratulations {newPlayer.PlayerName} the Victorious! Your epic quest comes to an end.\n" +
                    $"You finished with {score} points. Maybe you can beat it next time!\n\n\n" +
                    $"GAME OVER");
                Console.ReadLine();
            }
            Console.Clear();
            string highScore = $"{score.ToString()}|{newPlayer.PlayerName}";
            System.IO.File.AppendAllText(@"HighScores.sav", $"{highScore}\n");

            ShowHighScores();

        }

        public Difficulty SetDifficulty(string playerName)
        {

            int lives = 3;
            int correctDirectionsNeeded = 3;
            int maxRooms = 6;

            Console.WriteLine($"Welcome {playerName}, brave adventurer. Set your challenge level.\n" +
                $"1. Easy\n" +
                $"2. Medium\n" +
                $"3. Hard");
            string difficulty = Console.ReadLine().ToLower();

            // to choose Difficulty. medium is default if incorrect value entered.
            switch (difficulty)
            {
                case "1":
                case "easy":
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\easyMinionRiddles.txt"), _minionRiddles);
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\easyBossRiddles.txt"), _bossRiddles);
                    lives = 5;
                    correctDirectionsNeeded = 2;
                    maxRooms = 4;
                    break;
                case "2":
                case "medium":
                default:
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\mediumMinionRiddles.txt"), _minionRiddles);
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\mediumBossRiddles.txt"), _bossRiddles);
                    break;
                case "3":
                case "hard":
                    SeedRiddles(System.IO.File.ReadAllLines(@"\RiddleTxt\hardMinionRiddles.txt"), _minionRiddles);
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\hardBossRiddles.txt"), _bossRiddles);
                    lives = 1;
                    correctDirectionsNeeded = 5;
                    maxRooms = 10;
                    break;
            }
            Console.Clear();
            return new Difficulty()
            {
                Lives = lives,
                CorrectDirectionsNeeded = correctDirectionsNeeded,
                MaxRooms = maxRooms
            };
        }

        public void ShowHighScores()
        {
            List<Player> highScores = new List<Player>();

            string[] fromFile = System.IO.File.ReadAllLines(@"HighScores.sav");

            foreach (string line in fromFile)
            {
                string[] splitLine = line.Split('|');
                highScores.Add(new Player(int.Parse(splitLine[0]), splitLine[1]));
            }
            Console.WriteLine($"__________________________________________________________________________________________\n\n" +
                $"---------------------------------------HIGH SCORES----------------------------------------\n" +
                $"__________________________________________________________________________________________\n\n\n");


            Console.WriteLine(String.Format("{0,-10}         {1,10}", "Name", "Score"));
            Console.WriteLine("-----------------------------");

            foreach (Player player in highScores.OrderByDescending(x => x.Score))
            {
                Console.WriteLine($"{player.PlayerName,-10}  -----  {player.Score,10}");
            }

            Console.ReadLine();
            Console.Clear();
            RunMenu();
        }

        public string GetOppositeDirection(string lastDirection)
        {
            switch (lastDirection)
            {
                case "north":
                    return "south";
                case "south":
                    return "north";
                case "east":
                    return "west";
                case "west":
                    return "east";
            }
            return null;
        }

        public void SeedRiddles(string[] content, RiddleRepository riddleList)
        {
            // cleaned up code - now pulls from txt file
            // txt file must have keyword first (lowercase) followed by '|' (no spaces) and then description
            // example: "water|You see the slight trickle of a stream flowing from the far wall. You hear as it drips into a small pool near the goblin"

            foreach (string enemy in content)
            {
                string[] enemySplit = enemy.Split('|');
                riddleList.AddToRiddles(new Riddle(enemySplit[0], enemySplit[1]));
            }
        }

        // Give description, initiate mini game (if applicable), award item (if applicable).
        private int EnterRoom(string description)
        {
            int roomScore = 0;
            if (description == "final")
            {
                Console.WriteLine($"You have made it to the end of {_gameTitle}!! Time to face your final test!!");
                Riddle finalRiddle = GetRiddle(_bossRiddles, _usedBossRiddles);
                Console.WriteLine(finalRiddle.Description);
                roomScore = 10 * (Hangman(finalRiddle.Keyword));
            }
            // translate minion or boss room description to riddle
            else if (description == "minion")
            {
                Riddle currentRiddle = GetRiddle(_minionRiddles, _usedMinionRiddles);
                Random random = new Random();
                int randomNumber = random.Next(1, 3);
                switch (randomNumber)
                {
                    case 1:
                        Console.WriteLine($"A lone goblin is in this room. It looks at you and snarls.\n\n" +
                            $"Here is your clue:\n {currentRiddle.Description}");
                        roomScore = Hangman(currentRiddle.Keyword); // pass in keyword from method
                        break;
                    case 2:
                        TwentyOne newTwentyOne = new TwentyOne();
                        roomScore = newTwentyOne.Run(_inventory);
                        break;
                }
            }
            else if (description == "boss")
            {
                Riddle currentRiddle = GetRiddle(_minionRiddles, _usedMinionRiddles);
                Random random = new Random();
                int randomNumber = random.Next(1, 3);
                switch (randomNumber)
                {
                    case 1:
                        Console.WriteLine($"A large goblin boss is in this room. It charges at you.\n\n" +
                            $"Here is your clue:\n {currentRiddle.Description}");
                        roomScore = Hangman(currentRiddle.Keyword); // pass in keyword from method
                        break;
                    case 2:
                        TwentyOne newTwentyOne = new TwentyOne();
                        roomScore = newTwentyOne.Run(_inventory);
                        break;
                }
            }
            else
            {
                Console.WriteLine(description);
                roomScore = 100;
                if (description.Contains("chest"))
                {
                    Console.WriteLine("What would you like to do?\n" +
                        "1. Inspect the chest\n" +
                        "2. Continue");
                    string response = Console.ReadLine().ToLower();
                    while (response != "1" && response != "2")
                    {
                        if (response.Contains("continue"))
                        {
                            response = "2";
                        }
                        else if (response.Contains("inspect") || response.Contains("chest"))
                        {
                            response = "1";
                        }
                    }
                    switch (response)
                    {
                        case "1":
                            IItem newItem = GetRandomItem();
                            _inventory.AddToInventory(newItem);
                            Console.Clear();
                            Console.WriteLine($"You open the chest. Inside you find a {newItem.Name}!\n\n" +
                                $"{"TYPE",-8}  ||  { "NAME",-30}  ||  { "DESCRIPTION",60}\n" +
                                $"{newItem.Type,-8}  ||  {newItem.Name,-30}  ||  {newItem.Description,60}\n\n\n\n" +
                                $"{newItem.Name} added to your inventory!\n\n" +
                                $"Press ENTER to continue");
                            Console.ReadLine();
                            break;
                    }
                    Console.Clear();
                }
            }
            return roomScore;
        }

        public IItem GetRandomItem()
        {
            IItem newItem;
            Random randomNumber = new Random();
            int itemNumber = randomNumber.Next(1, 4);
            switch (itemNumber)
            {
                case 1:
                    int weaponNumber = randomNumber.Next(_weaponRepo.GetCount());
                    newItem = _weaponRepo.GetItemByIndex(weaponNumber);
                    break;
                case 2:
                    int armorNumber = randomNumber.Next(_armorRepo.GetCount());
                    newItem = _armorRepo.GetItemByIndex(armorNumber);
                    break;
                case 3:
                    int potionNumber = randomNumber.Next(_potionRepo.GetCount());
                    newItem = _potionRepo.GetItemByIndex(potionNumber);
                    break;
                default:
                    newItem = new Weapon();
                    break;
            }

            return newItem;
        }

        public Riddle GetRiddle(RiddleRepository riddleList, List<int> usedList)
        {
            Random randomNumber = new Random();
            int riddleNumber = randomNumber.Next(riddleList.GetCount());
            bool isNewRiddle = false;

            while (usedList.Count() < riddleList.GetCount() && !isNewRiddle)
            {
                if (usedList.Contains(riddleNumber))
                {
                    riddleNumber = randomNumber.Next(0, riddleList.GetCount());
                }
                else
                {
                    usedList.Add(riddleNumber);
                    isNewRiddle = true;
                    return riddleList.GetRiddleByIndex(riddleNumber);
                }
            }
            return null;
        }

        public int Hangman(string keyword)
        {
            // need to add the keyword 
            char[] word = keyword.ToCharArray();
            char letter;
            char[] guessed = new char[25];
            int incorrectGuesses = 0;
            int index = 0;
            bool found = false;
            bool won = false;
            int score = 25;
            int letterCount = keyword.Length;

            Console.Write("You must guess the correct word to pass. \n\n");

            while (!won && incorrectGuesses < 5)
            {

                if (letterCount == 0)
                {
                    won = true;
                }
                else
                {


                    for (int i = 0; i < word.Length; i++)
                    {
                        if (incorrectGuesses == 5)
                        {
                            break;
                        }
                        for (int j = 0; j < word.Length; j++)
                        {
                            for (int m = 0; m < index; m++)
                            {
                                if (word[j] == guessed[m])
                                    found = true;
                            }


                            if (found == true)
                            {
                                Console.Write(word[j]);
                                found = false;
                                letterCount--;
                            }
                            else
                                Console.Write("*");
                        }
                        if (letterCount == 0)
                        {
                            break;
                        }
                        else
                        {
                            letterCount = keyword.Length;
                            Console.WriteLine("\n\n");
                            Console.Write("Enter a letter: ");
                            string command = Console.ReadLine().ToLower();
                            while (command == "inventory")
                            {
                                Console.Clear();
                                IItem itemChoice = _inventory.InventoryMenu();
                                if (itemChoice == null) 
                                {
                                    Console.Clear();
                                    Console.WriteLine("\n\n");
                                    Console.Write("Enter a letter: ");
                                    command = Console.ReadLine().ToLower();
                                    break; 
                                }

                                if (itemChoice.Type == "Weapon")
                                {
                                    if (itemChoice.Name.Contains("Epic"))
                                    {
                                        won = true;
                                        Console.WriteLine("You brandish your weapon, allowing you to slay the monster and advance.");
                                        Console.ReadLine();
                                        Console.Clear();
                                    }
                                    else
                                    {
                                        incorrectGuesses--;
                                        Console.WriteLine($"You brandish your weapon, giving you an extra attempt to solve the riddle. You have {5 - incorrectGuesses} remaining.");
                                        Console.ReadLine();
                                        Console.Clear();
                                    }
                                    _inventory.RemoveItem(itemChoice);
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
                                if (!won)
                                {
                                    Console.WriteLine("\n\n");
                                    Console.Write("Enter a letter: ");
                                    command = Console.ReadLine().ToLower();
                                }
                                else { command = ""; }
                            }
                            if (!won)
                            {

                                letter = char.Parse(command);
                                Console.Clear();

                                if (word.Contains(letter))
                                {
                                    Console.WriteLine("Correct!\n\n");
                                    guessed[index] = letter;
                                    ++index;
                                }
                                else
                                {
                                    Console.WriteLine("You guessed incorrectly!\n\n");
                                    ++incorrectGuesses;
                                    Console.WriteLine($"You have {5 - incorrectGuesses} tries remaining.\n\n");
                                }
                            }
                        }

                    }

                }

            }

            if (won)
            {
                score = 500 - (incorrectGuesses * 50);
                Console.WriteLine("\n\n" +
                "Congratulations! You have solved the riddle!");
            }
            Console.WriteLine("Your score was: " + score);

            return score;
        }

        public void SaveMenu(Player newPlayer, int lives, int correctDirectionsNeeded, int maxRooms, int roomScore,
            int roomNumber, int correctDirectionChoices, string overallDirectionChoice, string directionChoice)
        {
            bool exitMenu = false;

            string saveName = "";

            string[] saveFile = new string[]
            {
                $"{saveName}",
                $"playerName={newPlayer.PlayerName}",
                $"score={newPlayer.Score}",
                $"lives={lives}",
                $"correctDirectionsNeeded={correctDirectionsNeeded}",
                $"maxRooms={maxRooms}",
                $"roomScore={roomScore}",
                $"roomNumber={roomNumber}",
                $"correctDirectionChoices={correctDirectionChoices}",
                $"overallDirectionChoice={overallDirectionChoice}",
                $"directionChoice={directionChoice}"
            };

            string saveOne = GetSaveName(System.IO.File.ReadAllLines(@"SaveGame\SaveOne.sav"));
            string saveTwo = GetSaveName(System.IO.File.ReadAllLines(@"SaveGame\SaveTwo.sav"));
            string saveThree = GetSaveName(System.IO.File.ReadAllLines(@"SaveGame\SaveThree.sav"));

            Console.Clear();
            Console.WriteLine($"__________________________________________________________________________________________\n\n" +
                $"-------------------------------------SAVE GAME MENU---------------------------------------\n" +
                $"__________________________________________________________________________________________\n\n\n");
            Console.WriteLine($"Would you like to save your progress? Choose an option:\n" +
                $"1. {saveOne}\n" +
                $"2. {saveTwo}\n" +
                $"3. {saveThree}\n" +
                $"4. Exit\n");
            string selection = Console.ReadLine();
            while (selection != "1" && selection != "2" && selection != "3" && selection != "4" && selection.ToLower() != "exit")
            {
                Console.WriteLine("I didn't understand. Choose an option:\n");
                selection = Console.ReadLine();
            }
            while (!exitMenu)
            {

                switch (selection)
                {
                    case "1":
                        //save to SaveOne
                        saveName = SaveGame(saveOne);
                        if (saveName != null)
                        {
                            saveFile[0] = saveName;
                            System.IO.File.WriteAllLines(@"SaveGame\SaveOne.sav", saveFile);
                            Console.WriteLine("\n\n SAVED");
                        }
                        break;
                    case "2":
                        //save to SaveTwo
                        saveName = SaveGame(saveTwo);
                        if (saveName != null)
                        {
                            saveFile[0] = saveName;
                            System.IO.File.WriteAllLines(@"SaveGame\SaveTwo.sav", saveFile);
                            Console.WriteLine("\n\n SAVED");
                        }
                        break;
                    case "3":
                        //save to SaveThree
                        saveName = SaveGame(saveThree);
                        if (saveName != null)
                        {
                            saveFile[0] = saveName;
                            System.IO.File.WriteAllLines(@"SaveGame\SaveThree.sav", saveFile);
                            Console.WriteLine("\n\n SAVED");
                        }
                        break;
                    case "4":
                    case "exit":
                        break;
                }
                exitMenu = true;
            }
        }

        public string SaveGame(string oldSave)
        {
            if (oldSave != "EMPTY")
            {
                Console.WriteLine($"Are you sure you wish to overwrite {oldSave}? Enter yes or no.");
                string response = Console.ReadLine().ToLower();
                Console.Clear();
                while (response != "yes" && response != "no")
                {
                    Console.WriteLine($"I didn't understand. Are you sure you wish to overwrite {oldSave}? Enter yes or no.");
                    response = Console.ReadLine().ToLower();
                }
                if (response != "yes")
                {
                    return null;
                }
            }
            Console.WriteLine("Enter a save name.");
            string saveName = Console.ReadLine().ToUpper();
            while (saveName.Length > 15)
            {
                Console.Clear();
                Console.WriteLine("Your entry was too long. Please limit your save name to 15 characters and try again.");
                saveName = Console.ReadLine().ToUpper();
            }
            return saveName;
        }

        public string GetSaveName(string[] saveGame)
        {
            if (saveGame.Length < 5)
            {
                return "EMPTY";
            }
            else { return saveGame[0].ToUpper(); }
        }
        public void LoadGameMenu()
        {
            string[] saveOne = System.IO.File.ReadAllLines(@"SaveGame\SaveOne.sav");
            string[] saveTwo = System.IO.File.ReadAllLines(@"SaveGame\SaveTwo.sav");
            string[] saveThree = System.IO.File.ReadAllLines(@"SaveGame\SaveThree.sav");

            Console.WriteLine($"__________________________________________________________________________________________\n\n" +
                $"-------------------------------------LOAD GAME MENU---------------------------------------\n" +
                $"__________________________________________________________________________________________\n\n\n");
            Console.WriteLine($"Select which file to load:\n" +
                $"1. {GetSaveName(saveOne)}\n" +
                $"2. {GetSaveName(saveTwo)}\n" +
                $"3. {GetSaveName(saveThree)}\n" +
                $"4. Exit\n");
            string response = Console.ReadLine().ToUpper();
            while (response != "1" && response != "2" && response != "3" && response != "4")
            {
                if (response == saveOne[0].ToUpper()) { response = "1"; }
                else if (response == saveTwo[0].ToUpper()) { response = "2"; }
                else if (response == saveThree[0].ToUpper()) { response = "3"; }
                else if (response == "EXIT") { response = "4"; }
                else
                {
                    Console.WriteLine("I didn't understand. Select which file to load:\n");
                    response = Console.ReadLine().ToUpper();
                }
            }
            Console.Clear();
            switch (response)
            {
                case "1":
                    if (GetSaveName(saveOne) != "EMPTY") { LoadGame(saveOne); }
                    else { CannotLoad(); }
                    break;
                case "2":
                    if (GetSaveName(saveTwo) != "EMPTY") { LoadGame(saveTwo); }
                    else { CannotLoad(); }
                    break;
                case "3":
                    if (GetSaveName(saveThree) != "EMPTY") { LoadGame(saveThree); }
                    else { CannotLoad(); }
                    break;
                case "4":
                    RunMenu();
                    break;
            }
        }
        public void CannotLoad()
        {
            Console.WriteLine("There is no game to load. Please select again.");
            Console.ReadLine();
            Console.Clear();
            LoadGameMenu();
        }
        public void LoadGame(string[] saveFile)
        {
            List<string> saveContent = new List<string>();

            for (int i = 1; i < saveFile.Length; i++)
            {
                string[] splitFile = saveFile[i].Split('=');
                saveContent.Add(splitFile[1]);
            }

            Player newPlayer = new Player(int.Parse(saveContent[1]), saveContent[0]);

            int difficulty = int.Parse(saveContent[3]);

            switch (difficulty)
            {
                case 2:
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\easyMinionRiddles.txt"), _minionRiddles);
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\easyBossRiddles.txt"), _bossRiddles);
                    break;
                case 3:
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\mediumMinionRiddles.txt"), _minionRiddles);
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\mediumBossRiddles.txt"), _bossRiddles);
                    break;
                case 5:
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\hardMinionRiddles.txt"), _minionRiddles);
                    SeedRiddles(System.IO.File.ReadAllLines(@"RiddleTxt\hardBossRiddles.txt"), _bossRiddles);
                    break;
            }

            RunGame(newPlayer, int.Parse(saveContent[2]), int.Parse(saveContent[3]), int.Parse(saveContent[4]), int.Parse(saveContent[5]),
                int.Parse(saveContent[6]), int.Parse(saveContent[7]), saveContent[8], saveContent[9]);
        }
    }
}
