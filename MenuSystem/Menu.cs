using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MenuSystem
{
    public class Menu
    {
        public string Title { get; set; }
        
        public Dictionary<string, MenuItem> MenuItems { get; private set; } = new Dictionary<string, MenuItem>()
        {
            {
                "X", new MenuItem()
                {
                    LongDescription = "Exit!",
                    Shortcuts = new HashSet<string>() {"X", "BACK", "GOBACK"},
                    MenuItemType = MenuItemType.Exit
                }
            },
            /*{
                "Q", new MenuItem()
                {
                    LongDescription = "Quit to main menu!",
                    Shortcuts = new HashSet<string>() {"Q", "QUIT", "HOME"},
                    MenuItemType = MenuItemType.GoBackToMain
                }
            }*/
        };
        
        public bool CleanScreenInMenuStart { get; set; } = true;


        //public bool DisplayQuitToMainMenu { get; set; } = false;
        public bool IsMainMenu { get; set; } = false;
        public bool IsGameMenu { get; set; } = false;

        private KeyValuePair<string, MenuItem> exitItem;
        private KeyValuePair<string, MenuItem> continueItem;

        private void PrintMenu()
        {
            var defaultMenuChoice = MenuItems.FirstOrDefault(m => m.Value.IsDefaultChoice == true);

            if (CleanScreenInMenuStart)
            {
                Console.Clear();
            }

            Console.WriteLine("-------- " + Title + "--------");
            foreach (var dictionaryItem in MenuItems.Where(m => m.Value.MenuItemType == MenuItemType.Regular))
            {
                var menuItem = dictionaryItem.Value;
                if (menuItem.IsDefaultChoice)
                {
                    Console.ForegroundColor =
                        ConsoleColor.Red;
                    Console.Write(dictionaryItem.Key);
                    Console.WriteLine(menuItem);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(dictionaryItem.Key);
                    Console.WriteLine(menuItem);
                }
            }

            Console.WriteLine("--------");

            var item = MenuItems.FirstOrDefault(m => m.Value.MenuItemType == MenuItemType.Exit);
            if (item.Value != null)
            {
                Console.WriteLine(exitItem.Key + exitItem.Value);
            }
            
            
            /*if (DisplayQuitToMainMenu == true)
            {
                item = MenuItems.FirstOrDefault(m => m.Value.MenuItemType == MenuItemType.GoBackToMain);
                if (item.Value != null)
                {
                    Console.WriteLine(quitToMainItem.Key + quitToMainItem.Value);
                }
            }*/
            Console.Write(
                defaultMenuChoice.Value == null ? ">" : "[" + defaultMenuChoice.Value.Shortcuts.First() + "]>"
            );
        }

        
        
        private void WaitForUser()
        {
            Console.Write("Press any key to continue!");
            Console.ReadKey();
        }

        public string RunMenu()
        {
            exitItem = MenuItems.FirstOrDefault(m => m.Value.MenuItemType == MenuItemType.Exit);
            //quitToMainItem = MenuItems.FirstOrDefault(m => m.Value.MenuItemType == MenuItemType.GoBackToMain);
            continueItem = MenuItems.FirstOrDefault(m => m.Value.LongDescription == "Continue");
            
            var done = true;
            string input;
            do
            {
                done = false;

                PrintMenu();
                input = Console.ReadLine().ToUpper().Trim();

            
                // shall we exit from this menu
                if (exitItem.Value.Shortcuts.Any(s => s == input))
                {
                    Environment.Exit(0);  // jump out of the loop
                }
                /*if (DisplayQuitToMainMenu && quitToMainItem.Value.Shortcuts.Any(s => s == (input)))
                {
                    break; // jump out of the loop
                }*/

                // find the correct menuitem
                MenuItem item = null;
                item = string.IsNullOrWhiteSpace(input)
                    ? MenuItems.FirstOrDefault(m => m.Value.IsDefaultChoice == true).Value
                    // dig out item, where this input is in its shortcuts
                    : MenuItems.FirstOrDefault(m => m.Value.Shortcuts.Contains(input)).Value;

                if (item == null)
                {
                    Console.WriteLine(input + " was not found in the list of commands!");
                    WaitForUser();
                    continue; // jump back to the start of loop
                }
                
                if (item.LongDescription == "Continue")
                {
                    break;
                }
                

                // execute the command specified in the menu item
                if (item.CommandToExecute(input) == null)
                {
                    Console.WriteLine(input + " has no command assigned to it!");
                    WaitForUser();
                    continue; // jump back to the start of loop
                }

                // everything should be ok now, lets run it!
                //var chosenCommand = item.CommandToExecute(input);
                var chosenCommand = item.CommandToExecute(input);
                
                input = chosenCommand;

                /*if (IsMainMenu == false && quitToMainItem.Value.Shortcuts.Contains(chosenCommand))
                {
                    break;
                }*/

                if (!exitItem.Value.Shortcuts.Contains(chosenCommand) /*&& 
                    !quitToMainItem.Value.Shortcuts.Contains(chosenCommand)*/)
                {
                    WaitForUser();
                }
            } while (done != true);

            return input;
        }
    }
}