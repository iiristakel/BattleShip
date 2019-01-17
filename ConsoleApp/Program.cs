using System;
using System.Linq;
using DAL;
using Domain;
using GameSystem;
using GameUI;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Battleship ConsoleApp");
            var gameUi = new BattleshipUI();
            
            var menuItemNewGame = ApplicationMenu.MainMenu.MenuItems.First(m => m.Value.LongDescription == "New game");
            menuItemNewGame.Value.CommandToExecute = gameUi.RunGame; //boats are placed with that
            
            var menuItemLoadGame = ApplicationMenu.MainMenu.MenuItems.First(m => m.Value.LongDescription == "Load game");
            menuItemLoadGame.Value.CommandToExecute = gameUi.LoadGame; 
            
            var menuItemStartShooting = ApplicationMenu.GameMenu.MenuItems.First(m => m.Value.LongDescription == "Start shooting");
           menuItemStartShooting.Value.CommandToExecute = gameUi.Shooting;
            
            var menuItemSaveGame = ApplicationMenu.GameMenu.MenuItems.First(m => m.Value.LongDescription == "Save game");
            menuItemSaveGame.Value.CommandToExecute = gameUi.SaveGame;
            
            var menuItemSaveInGame = ApplicationMenu.InGameMenu.MenuItems.First(m => m.Value.LongDescription == "Save game");
            menuItemSaveInGame.Value.CommandToExecute = gameUi.SaveGame;

            ApplicationMenu.MainMenu.RunMenu();
        }
    }
}