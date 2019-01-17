using System;
using System.Linq;
using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace GameSystem
{
    public class GameStorage
    {
        public static AppDbContext context = new AppDbContext();
        

        public static int Save(Game game)
        {
           
            if (context.Games.Find(game.GameId) == null)
            {
                context.Games.Add(game);
            }
            else
            {
                context.Games.Update(game);
                
            }

            context.SaveChanges();
            return game.GameId;
        }

        public static Game Load(int index)
        {
            
            
            var game = context.Games
                 .Where(g=>g.GameId == index)
                .Include(g => g.PlayerOne)
                .ThenInclude(f => f.GameBoard)
                .Include(g => g.PlayerOne)
                .ThenInclude(f => f.FiringBoard)
                .Include(g => g.PlayerTwo)
                .ThenInclude(f => f.GameBoard)
                .Include(g => g.PlayerOne)
                .ThenInclude(f => f.Ships)
                .Include(g => g.PlayerTwo)
                .ThenInclude(f => f.Ships)
                .Include(g => g.PlayerTwo)
                .ThenInclude(f => f.FiringBoard)
                .Include(g=>g.Turn)
                .Include(g=>g.Winner)
                .AsQueryable()
                .FirstOrDefault();

            /*game.PlayerOne.GameBoard = context.GameBoards.Where(c => c.PlayerId == game.PlayerOne.PlayerId)
                .Include(a => a.Board).ThenInclude(b => b.Row).First();
            
            game.PlayerTwo.GameBoard = context.GameBoards.Where(c => c.PlayerId == game.PlayerTwo.PlayerId)
                .Include(a => a.Board).ThenInclude(b => b.Row).First();

*/
                /*foreach (BoardRow row in game.PlayerOne.GameBoard.Board
                    .Where(a=>a.GameBoardId == game.PlayerOne.GameBoard.GameBoardId))
                {
                    foreach (Cell cell in row.Row)
                    {
                        cell.CellStatus = row.Row.First(a => a.BoardRowId == cell.BoardRowId).CellStatus;
                    }
                }*/
            

            
            return game;
        }
    }
}