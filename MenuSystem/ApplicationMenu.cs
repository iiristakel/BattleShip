using System.Collections.Generic;

namespace MenuSystem
{
    public class ApplicationMenu
    {
        public static readonly Menu GameMenu;
        public static readonly Menu MainMenu;
        public static readonly Menu InGameMenu;
        
        
        static ApplicationMenu()
        {
            GameMenu = new Menu()
            {
                Title = "Game Menu",
                IsGameMenu = true
            };
            GameMenu.MenuItems.Add(
                "A", new MenuItem()
                {
                    LongDescription = "Start shooting",
                    Shortcuts = new HashSet<string>() {"A", "a", "start", "shoot", "start shooting"},
                    CommandToExecute = null,
                    IsDefaultChoice = true
                }
            );

            GameMenu.MenuItems.Add(
                "S", new MenuItem()
                {
                    LongDescription = "Save game",
                    Shortcuts = new HashSet<string>() {"S", "s"},
                    CommandToExecute = null
                }
            );
            
            InGameMenu = new Menu()
            {
                Title = "Game Menu",
                CleanScreenInMenuStart = false
            };

            InGameMenu.MenuItems.Add(
                "S", new MenuItem()
                {
                    LongDescription = "Save game",
                    Shortcuts = new HashSet<string>() {"S", "s"},
                    CommandToExecute = null
                }
            );
            
            InGameMenu.MenuItems.Add(
                "C", new MenuItem()
                {
                    LongDescription = "Continue",
                    Shortcuts = new HashSet<string>() {"C", "c"},
                    CommandToExecute = null,
                    IsDefaultChoice = true
                }
            );

            MainMenu = new Menu()
            {
                Title = "Main menu: Battleship",
                IsMainMenu = true,
            };
            MainMenu.MenuItems.Add(
                "N", new MenuItem()
                {
                    LongDescription = "New game",
                    Shortcuts = new HashSet<string> {"N", "n"},
                    CommandToExecute = null,
                    IsDefaultChoice = true
                }
            );
            MainMenu.MenuItems.Add(
                "L", new MenuItem()
                {
                    LongDescription = "Load game",
                    Shortcuts = new HashSet<string> {"L", "l"},
                    CommandToExecute = null
                });
                
        }
    }
}