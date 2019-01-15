using System;
using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace GameSystem
{
    public class GameStorage
    {
        public AppDbContext context = new AppDbContext();
        private Game game;
        
        
        public GameStorage()
        {
            
        }

        public string SaveGame(string command/*Game game, bool userSave = false*/)
        {
            if (context.Games.Find(game.GameId) == null)
            {
                context.Games.Add(game);
            }
            else
            {
                //game.SavedGame = userSave ? 1 : 0;
                context.Games.Update(game);
            }

            context.SaveChanges();
            Console.WriteLine("Game saved!");
            return ApplicationMenu.MainMenu.RunMenu();
        }


        /*
         * Get games from DB.
         */
        public DbSet<Game> GetGames()
        {
            return context.Games;
        }

        /*
         * Return players from database.
         */
        public DbSet<Player> GetPlayers()
        {
            return context.Players;
        }
    }
}