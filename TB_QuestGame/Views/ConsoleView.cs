using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB_QuestGame
{
    /// <summary>
    /// view class
    /// </summary>
    public class ConsoleView
    {
        #region FIELDS

        //
        // declare game objects for the ConsoleView object to use
        //
        Player _gamePirate;
        Universe _gameUniverse;

        #endregion

        #region PROPERTIES

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// default constructor to create the console view objects
        /// </summary>
        public ConsoleView(Player gamePirate, Universe gameUniverse)
        {
            _gamePirate = gamePirate;
            _gameUniverse = gameUniverse;

            InitializeDisplay();
        }

        #endregion

        #region METHODS
        /// <summary>
        /// display all of the elements on the game play screen on the console
        /// </summary>
        /// <param name="messageBoxHeaderText">message box header title</param>
        /// <param name="messageBoxText">message box text</param>
        /// <param name="menu">menu to use</param>
        /// <param name="inputBoxPrompt">input box text</param>
        public void DisplayGamePlayScreen(string messageBoxHeaderText, string messageBoxText, Menu menu, string inputBoxPrompt)
        {
            //
            // reset screen to default window colors
            //
            Console.BackgroundColor = ConsoleTheme.WindowBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.WindowForegroundColor;
            Console.Clear();

            ConsoleWindowHelper.DisplayHeader(Text.HeaderText);
            ConsoleWindowHelper.DisplayFooter(Text.FooterText);

            DisplayMessageBox(messageBoxHeaderText, messageBoxText);
            DisplayMenuBox(menu);
            DisplayInputBox();
        }

        /// <summary>
        /// wait for any keystroke to continue
        /// </summary>
        public void GetContinueKey()
        {
            Console.ReadKey();
        }

        /// <summary>
        /// get a action menu choice from the user
        /// </summary>
        /// <returns>action menu choice</returns>
        public PlayerAction GetActionMenuChoice(Menu menu)
        {
            PlayerAction choosenAction = PlayerAction.None;
            Console.CursorVisible = false;

            //
            // an array of valid keys from menu dictionary
            //
            char[] validKeys = menu.MenuChoices.Keys.ToArray();

            //
            // validate key pressed in MenuChoices dictionary
            //
            char keyPressed;
            do
            {
                ConsoleKeyInfo keyPressedInfo = Console.ReadKey(true);
                keyPressed = keyPressedInfo.KeyChar;

            } while (!validKeys.Contains(keyPressed));

            choosenAction = menu.MenuChoices[keyPressed];
            Console.CursorVisible = true;

            return choosenAction;
        }

        /// <summary>
        /// get a string value from the user
        /// </summary>
        /// <returns>string value</returns>
        public string GetString()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// get an integer value from the user
        /// </summary>
        /// <returns>integer value</returns>
        public bool GetInteger(string prompt, int minimumValue, int maximumValue, out int integerChoice)
        {
            bool validResponse = false;
            integerChoice = 0;

            DisplayInputBoxPrompt(prompt);
            while (!validResponse)
            {
                if (int.TryParse(Console.ReadLine(), out integerChoice))
                {
                    if (integerChoice >= minimumValue && integerChoice <= maximumValue)
                    {
                        validResponse = true;
                    }
                    else
                    {
                        ClearInputBox();
                        DisplayInputErrorMessage($"You must enter an integer value between {minimumValue} and {maximumValue}. Please try again.");
                        DisplayInputBoxPrompt(prompt);
                    }
                }
                else
                {
                    ClearInputBox();
                    DisplayInputErrorMessage($"You must enter an integer value between {minimumValue} and {maximumValue}. Please try again.");
                    DisplayInputBoxPrompt(prompt);
                }
            }

            return true;
        }

        /// <summary>
        /// get a character race value from the user
        /// </summary>
        /// <returns>character race value</returns>
        public Character.GenderType GetGender()
        {
            Character.GenderType genderType;            

            if (!Enum.TryParse<Character.GenderType>(Console.ReadLine().ToUpper(), out genderType))
            {
                DisplayInputErrorMessage($"Input not recognized. Your gender as been set to: '{Character.GenderType.OTHER}'. " +
                "You may change this at any time. Press any key to continue.");
                GetContinueKey();
            }      
            
            return genderType;
        }

        /// <summary>
        /// display splash screen
        /// </summary>
        /// <returns>player chooses to play</returns>
        public bool DisplaySpashScreen()
        {
            bool playing = true;
            ConsoleKeyInfo keyPressed;

            Console.BackgroundColor = ConsoleTheme.SplashScreenBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.SplashScreenForegroundColor;
            Console.Clear();
            Console.CursorVisible = false;


            Console.SetCursorPosition(0, 10);
            string tabSpace = new String(' ', 48);
            Console.WriteLine(tabSpace + @" _____ _           _   _  _        _      ____                 ");
            Console.WriteLine(tabSpace + @"|_   _| |         | | | |(_)      | |    /  __\                ");
            Console.WriteLine(tabSpace + @"  | | | |__   ___ | |_| | _  ___  | |__  | |__  ___  ___   ___ ");
            Console.WriteLine(tabSpace + @"  | | | '_ \ / _ \|  _  || |/ _ \ | '_ \ \__  \/ _ \/ _ \ / __\");
            Console.WriteLine(tabSpace + @"  | | | | | |  __/| | | || | (_) || | | |___| |  __/ (_) |\__ \");
            Console.WriteLine(tabSpace + @"  \_/ |_| |_|\___||_| |_||_|\__  ||_| |_|\____/\___|\__/\_\___/");
            Console.WriteLine(tabSpace + @"                              _/ /                             ");
            Console.WriteLine(tabSpace + @"                             |__/                              ");        

            Console.SetCursorPosition(80, 25);
            Console.Write("Press any key to continue or Esc to exit.");
            keyPressed = Console.ReadKey();
            if (keyPressed.Key == ConsoleKey.Escape)
            {
                playing = false;
            }

            return playing;
        }

        /// <summary>
        /// initialize the console window settings
        /// </summary>
        private static void InitializeDisplay()
        {
            //
            // control the console window properties
            //
            ConsoleWindowControl.DisableResize();
            ConsoleWindowControl.DisableMaximize();
            ConsoleWindowControl.DisableMinimize();
            Console.Title = "The High Seas";

            //
            // set the default console window values
            //
            ConsoleWindowHelper.InitializeConsoleWindow();

            Console.CursorVisible = false;
        }

        /// <summary>
        /// display the correct menu in the menu box of the game screen
        /// </summary>
        /// <param name="menu">menu for current game state</param>
        private void DisplayMenuBox(Menu menu)
        {
            Console.BackgroundColor = ConsoleTheme.MenuBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.MenuBorderColor;

            //
            // display menu box border
            //
            ConsoleWindowHelper.DisplayBoxOutline(
                ConsoleLayout.MenuBoxPositionTop,
                ConsoleLayout.MenuBoxPositionLeft,
                ConsoleLayout.MenuBoxWidth,
                ConsoleLayout.MenuBoxHeight);

            //
            // display menu box header
            //
            Console.BackgroundColor = ConsoleTheme.MenuBorderColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(ConsoleLayout.MenuBoxPositionLeft + 2, ConsoleLayout.MenuBoxPositionTop + 1);
            Console.Write(ConsoleWindowHelper.Center(menu.MenuTitle, ConsoleLayout.MenuBoxWidth - 4));

            //
            // display menu choices
            //
            Console.BackgroundColor = ConsoleTheme.MenuBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.MenuForegroundColor;
            int topRow = ConsoleLayout.MenuBoxPositionTop + 3;

            foreach (KeyValuePair<char, PlayerAction> menuChoice in menu.MenuChoices)
            {
                if (menuChoice.Value != PlayerAction.None)
                {
                    string formatedMenuChoice = ConsoleWindowHelper.ToLabelFormat(menuChoice.Value.ToString());
                    Console.SetCursorPosition(ConsoleLayout.MenuBoxPositionLeft + 3, topRow++);
                    Console.Write($"{menuChoice.Key}. {formatedMenuChoice}");
                }
            }
        }

        /// <summary>
        /// display the text in the message box of the game screen
        /// </summary>
        /// <param name="headerText"></param>
        /// <param name="messageText"></param>
        private void DisplayMessageBox(string headerText, string messageText)
        {
            //
            // display the outline for the message box
            //
            Console.BackgroundColor = ConsoleTheme.MessageBoxBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.MessageBoxBorderColor;
            ConsoleWindowHelper.DisplayBoxOutline(
                ConsoleLayout.MessageBoxPositionTop,
                ConsoleLayout.MessageBoxPositionLeft,
                ConsoleLayout.MessageBoxWidth,
                ConsoleLayout.MessageBoxHeight);

            //
            // display message box header
            //
            Console.BackgroundColor = ConsoleTheme.MessageBoxBorderColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(ConsoleLayout.MessageBoxPositionLeft + 2, ConsoleLayout.MessageBoxPositionTop + 1);
            Console.Write(ConsoleWindowHelper.Center(headerText, ConsoleLayout.MessageBoxWidth - 4));

            //
            // display the text for the message box
            //
            Console.BackgroundColor = ConsoleTheme.MessageBoxBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.MessageBoxForegroundColor;
            List<string> messageTextLines = new List<string>();
            messageTextLines = ConsoleWindowHelper.MessageBoxWordWrap(messageText, ConsoleLayout.MessageBoxWidth - 4);

            int startingRow = ConsoleLayout.MessageBoxPositionTop + 3;
            int endingRow = startingRow + messageTextLines.Count();
            int row = startingRow;
            foreach (string messageTextLine in messageTextLines)
            {
                Console.SetCursorPosition(ConsoleLayout.MessageBoxPositionLeft + 2, row);
                Console.Write(messageTextLine);
                row++;
            }

        }

        /// <summary>
        /// draw the input box on the game screen
        /// </summary>
        public void DisplayInputBox()
        {
            Console.BackgroundColor = ConsoleTheme.InputBoxBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.InputBoxBorderColor;

            ConsoleWindowHelper.DisplayBoxOutline(
                ConsoleLayout.InputBoxPositionTop,
                ConsoleLayout.InputBoxPositionLeft,
                ConsoleLayout.InputBoxWidth,
                ConsoleLayout.InputBoxHeight);
        }

        /// <summary>
        /// display the prompt in the input box of the game screen
        /// </summary>
        /// <param name="prompt"></param>
        public void DisplayInputBoxPrompt(string prompt)
        {
            Console.SetCursorPosition(ConsoleLayout.InputBoxPositionLeft + 4, ConsoleLayout.InputBoxPositionTop + 1);
            Console.ForegroundColor = ConsoleTheme.InputBoxForegroundColor;
            Console.Write(prompt);
            Console.CursorVisible = true;
        }

        /// <summary>
        /// display the error message in the input box of the game screen
        /// </summary>
        /// <param name="errorMessage">error message text</param>
        public void DisplayInputErrorMessage(string errorMessage)
        {
            Console.SetCursorPosition(ConsoleLayout.InputBoxPositionLeft + 4, ConsoleLayout.InputBoxPositionTop + 2);
            Console.ForegroundColor = ConsoleTheme.InputBoxErrorMessageForegroundColor;
            Console.Write(errorMessage);
            Console.ForegroundColor = ConsoleTheme.InputBoxForegroundColor;
            Console.CursorVisible = true;
        }

        /// <summary>
        /// clear the input box
        /// </summary>
        private void ClearInputBox()
        {
            string backgroundColorString = new String(' ', ConsoleLayout.InputBoxWidth - 4);

            Console.ForegroundColor = ConsoleTheme.InputBoxBackgroundColor;
            for (int row = 1; row < ConsoleLayout.InputBoxHeight - 2; row++)
            {
                Console.SetCursorPosition(ConsoleLayout.InputBoxPositionLeft + 4, ConsoleLayout.InputBoxPositionTop + row);
                DisplayInputBoxPrompt(backgroundColorString);
            }
            Console.ForegroundColor = ConsoleTheme.InputBoxForegroundColor;
        }

        /// <summary>
        /// get the player's initial information at the beginning of the game
        /// </summary>
        /// <returns>traveler object with all properties updated</returns>
        public Player GetInitialPirateInfo()
        {
            Player pirate = new Player();

            //
            // intro
            //
            DisplayGamePlayScreen("Quest Setup", Text.InitializeMissionIntro(), ActionMenu.MissionIntro, "");
            GetContinueKey();          

            //
            // get pirates's age
            //
            DisplayGamePlayScreen("Quest Setup - Age", Text.InitializeMissionGetPirateAge(pirate), ActionMenu.MissionIntro, "");
            int gameTravelerAge;
            GetInteger($"Enter your age {pirate.Name}: ", 0, 1000000, out gameTravelerAge);
            pirate.Age = gameTravelerAge;

            //
            // get pirate's gender
            //
            DisplayGamePlayScreen("Quest Setup - Gender", Text.InitializeMissionGetPirateGender(pirate), ActionMenu.MissionIntro, "");
            DisplayInputBoxPrompt($"Enter your gender {pirate.Name}: ");
            pirate.Gender = GetGender();

            //
            // get pirates's personality
            //
            bool selectingPersona = true;
            string prompt = $"Enter 'Yes' or 'No' {pirate.Name}: ";
            DisplayGamePlayScreen("Quest Setup - Personality", Text.InitializeMissionGetPiratePersonality(pirate), ActionMenu.MissionIntro, "");

            // validate
            while (selectingPersona)
            {
                string userResponse;
                DisplayInputBoxPrompt(prompt);
                userResponse = GetString().ToUpper();

                if (userResponse == "YES")
                {
                    pirate.Personality = true;
                    selectingPersona = false;
                                   }
                else if (userResponse == "NO")
                {
                    pirate.Personality = false;
                    selectingPersona = false;
                }
                else
                {
                    ClearInputBox();
                    DisplayInputErrorMessage("You must enter either 'Yes' or 'No'. Please try again.");
                    DisplayInputBoxPrompt(prompt);
                }
            }

            return pirate;
        }

        /// <summary>
        /// get pirate's name
        /// </summary>
        public string GetPirateName(Player gamePirate)
        {
            bool choosingName = true;

            //
            // allows user to enter a custom name
            //
            DisplayGamePlayScreen("Quest Setup - Name", Text.InitializeMissionGetPirateName(), ActionMenu.InitializePlayerName, "");
            DisplayInputBoxPrompt("Enter your name: ");
            gamePirate.Name = GetString();
            
            if (gamePirate.Name == "1")
            {
                //
                // this code will generate a random name
                //
                gamePirate.Name = GenerateRandomName();

                while (choosingName)
                {
                    string userResponese;
                    string prompt = $"Generate new Name? (Yes or No): ";

                    DisplayGamePlayScreen("Quest Setup - Name", Text.InitializeMissionGetRandomName(gamePirate), ActionMenu.InitializeRandomName, "");
                    DisplayInputBoxPrompt(prompt);
                    userResponese = GetString().ToUpper();

                    if (userResponese == "YES")
                    {
                        //
                        // this code will generate a random name
                        //
                        gamePirate.Name = GenerateRandomName();
                        choosingName = true;
                    }
                    else if (userResponese == "NO")
                    {
                        choosingName = false;
                        return gamePirate.Name;
                    }
                    else if (userResponese == "1")
                    {
                        //
                        // allows user to return to previous screen and enter a custom name
                        //
                        DisplayGamePlayScreen("Quest Setup - Name", Text.InitializeMissionGetPirateName(), ActionMenu.InitializePlayerName, "");
                        DisplayInputBoxPrompt("Enter your name: ");
                        gamePirate.Name = GetString();

                        if (gamePirate.Name == "1")
                        {
                            gamePirate.Name = GenerateRandomName();
                            choosingName = true;
                        }
                        else
                        {
                            choosingName = false;
                            return gamePirate.Name;
                        }
                    }
                    else
                    {
                        ClearInputBox();
                        DisplayInputErrorMessage("You must enter either 'Yes','No', or a menu option. Please try again.");
                        DisplayInputBoxPrompt(prompt);
                        GetContinueKey();
                    }
                }
                
            }
            
            return gamePirate.Name;
        }

        /// <summary>
        /// generates a random name string and returns that string
        /// </summary>
        /// <returns></returns>
        public string GenerateRandomName()
        {
            // list of random 'pirate' names
            List<string> names = new List<string>()
            {
                //male names
                "Wayne 'Silver Tooth' Colby",
                "Farr 'Liar' Richmond",
                "Trevin 'Rage' Sweet",
                "Delton 'Double-Crossed' Fischer",
                "Wyatt 'Mutiny' Lucifer",
                "Garton 'Speechless' Brudge",
                "Burton 'Twisting' Ogden",
                "Paley 'Four-Teeth' Smith",
                "Washington 'The Hero' Marlowe",
                "Bramwell 'Rum Lover' Jeronimo",
                "Tomlin 'One-Eared' Shell",
                "Meldrick 'Rough Dog' Griffin",
                "Hawthorne 'One-Eyed' Kimberley",
                "Denver 'Fierce' Swales",
                "Winton 'Shaded' Penney",
                "Radburn 'Roaring' Charles",
                "Filmore 'Pirate' Frederik",
                "Harold 'Foxy' Sans",
                "Leland 'Four Fingers' Burling",
                "Sully 'Swine' Ottinger",

                // female names
                "Eugenie 'Crazy' Ethel",
                "Mettie 'The Hawk' Gnash",
                "Teresa 'Temptation' Crowley",
                "Sadye 'Barnacle' Atherton",
                "Blossom 'Shrew' Branson",
                "Adella 'Riot' Griffin",
                "Patsy 'Silver Hair' Watt",
                "Georgie 'The Marked' Arlin",
                "Harriett 'Thoothless' Shurman",
                "Taylor 'The Confident' Achey",
                "Melissa 'Vixen' Stratford",
                "Enola 'Treasure' Bradford",
                "Prudence 'Shark Bait' Merton",
                "Vira 'The Fool' Bing",
                "Vala 'Brown Teeth' Alistair",
                "Claire 'Dawg' Hastings",
                "Dolores 'The Bright' Tindall",
                "Elfrida 'Buccaneer' Smith",
                "Hulda 'Three-Teeth' Cloven",
                "Blanche 'One-Legged' Compton"
            };
            string pirateName;
            Random randomName = new Random();

            int randomNumber = randomName.Next(names.Count); //create a variable that will select a random name from the "names" list                    
            pirateName = names[randomNumber]; //assigns a random List<string> taken from the "names" list to the pirateName(string) 

            return pirateName;
        }

        #region ----- display responses to menu action choices -----

        /// <summary>
        /// displays all player information
        /// </summary>
        public void DisplayPirateInfo()
        {
            DisplayGamePlayScreen("Player Information", Text.PirateInfo(_gamePirate), ActionMenu.MainMenu, "");
        }

        /// <summary>
        /// displays 'edit profile' screen
        /// </summary>
        public void DisplayEditPirateInfo()
        {
            DisplayGamePlayScreen("Edit Player Information", Text.EditPirateInfo(_gamePirate), ActionMenu.MainMenu, "");
            EditPirateInformation(_gamePirate);
            
        }

        /// <summary>
        /// edits player information
        /// </summary>        
        public Player EditPirateInformation(Player gamePirate)
        {
            string userResponse;
            bool editingInfo = true;

            while (editingInfo)
            {
                DisplayInputBoxPrompt($"Enter your selection {gamePirate.Name}: ");
                userResponse = GetString().ToUpper();
                if (userResponse == "A" || userResponse == "B" || userResponse == "C")
                {
                    switch (userResponse)
                    {
                        case "A":
                            DisplayGamePlayScreen("Edit Player Information", $"You have selcted to edit your Name.\n" + "If you change your mind, you may enter 'Back' to be returned to the previous menu.\n", ActionMenu.MainMenu, "");
                            DisplayInputBoxPrompt($"Enter your new name: ");
                            gamePirate.Name = GetString();
                            editingInfo = false;
                            return gamePirate;

                        case "B":
                            int pirateAge = 0;
                            DisplayGamePlayScreen("Edit Player Information", $"You have selcted to edit your Age.\n" + "If you change your mind, you may enter 'Back' to be returned to the previous menu.\n", ActionMenu.MainMenu, "");
                            DisplayInputBoxPrompt($"Enter your new age: ");
                            GetInteger($"Enter your age {gamePirate.Name}: ", 0, 1000000, out pirateAge);
                            gamePirate.Age = pirateAge;
                            editingInfo = false;
                            return gamePirate;

                        case "C":
                            DisplayGamePlayScreen("Edit Player Information", $"You have selcted to edit your Gender.\n" + "If you change your mind, you may enter 'Back' to be returned to the previous menu.\n", ActionMenu.MainMenu, "");
                            DisplayInputBoxPrompt($"Enter your new gender: ");
                            gamePirate.Gender = GetGender();
                            editingInfo = false;
                            return gamePirate;

                        case "1":
                            DisplayPirateInfo();
                            editingInfo = false;
                            break;

                        case "3":
                            ReturnPlayerToMainScreen();
                            editingInfo = false;
                            break;

                        case "4":
                            DisplayClosingScreen();
                            editingInfo = false;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    ClearInputBox();
                    DisplayInputErrorMessage("You must enter a correct corresponding letter. Please try again.");
                    DisplayInputBoxPrompt($"Enter your selection {gamePirate.Name}: ");
                }
            }
            return gamePirate;
        }

        /// <summary>
        /// returns the player to main screen
        /// </summary>
        public void ReturnPlayerToMainScreen()
        {
            DisplayGamePlayScreen("Current Location Information", Text.CurrrentLocationInfo(), ActionMenu.MainMenu, "");
        }

        /// <summary>
        /// displays a closing screen upon exiting
        /// </summary>
        public void DisplayClosingScreen()
        {                        
            Console.BackgroundColor = ConsoleTheme.SplashScreenBackgroundColor;
            Console.ForegroundColor = ConsoleTheme.SplashScreenForegroundColor;
            Console.Clear();
            Console.CursorVisible = false;

            Console.SetCursorPosition(0, 6);
            string tabSpace = new String(' ', 45);
            Console.WriteLine(tabSpace + @" _____ _                    _                                           ");
            Console.WriteLine(tabSpace + @"|_   _| |                  | |  _                                       ");
            Console.WriteLine(tabSpace + @"  | | | |__   ___    ____  | | / /___                                   ");
            Console.WriteLine(tabSpace + @"  | | |  _ \ / _ \  |  _ \ | |/ // __\                                  ");
            Console.WriteLine(tabSpace + @"  | | | | | | (_) | | | | ||  _ \\__ \                                  ");
            Console.WriteLine(tabSpace + @"  \_/ |_| |_|\__/\_\|_| |_||_| \_\___/                                  ");
            Console.WriteLine(tabSpace + @"                  _______                                               ");
            Console.WriteLine(tabSpace + @"                 /  ____/                                               ");
            Console.WriteLine(tabSpace + @"                 | |__  ___  _ __                                       ");
            Console.WriteLine(tabSpace + @"                 |  __|/ _ \| '__/                                      ");
            Console.WriteLine(tabSpace + @"                 | |  | (_) | |                                         ");
            Console.WriteLine(tabSpace + @"                 \_/   \___/|_|                                         ");
            Console.WriteLine(tabSpace + @"                            ______ _               _               _    ");
            Console.WriteLine(tabSpace + @"                            |  __ \ |             (_)             | |   ");
            Console.WriteLine(tabSpace + @"                            | |_/ / | ___ __     _ _  ____   ___  | |   ");
            Console.WriteLine(tabSpace + @"                            |  __/| |/ _ \\ \   / / ||  _ \ / _ \ |_|   ");
            Console.WriteLine(tabSpace + @"                            | |   | | (_) |\ \ / /| || | | | (_) | _    ");
            Console.WriteLine(tabSpace + @"                            \_|   |_|\__/\_\\_  / |_||_| |_|\__  |(_)   ");
            Console.WriteLine(tabSpace + @"                                            _/ /              _/ /      ");
            Console.WriteLine(tabSpace + @"                                           |__/              |__/       ");

            Console.SetCursorPosition(85, 30);
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }        

        #endregion

        #endregion
    }
}
