using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages
{
    public class PlayingGameModel : PageModel
    {
        [BindProperty] public int Index { get; set; }

        [BindProperty] [Range(0, 26)] public int XCord { get; set; }

        [BindProperty] [Range(0, 15)] public int YCord { get; set; }

        //public string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string Message { get; set; }
        public bool GameWon { get; set; }
        public bool Move { get; set; }

        [BindProperty] public Player Player { get; set; }

        public IActionResult OnGet(int index, string message, bool move)
        {
            Message = message;
            Move = move;
            Index = index;
            var save = GameStorage.Load(Index);

            if (save.PlayerOne.HasLost || save.PlayerTwo.HasLost)
            {
                GameWon = true;
            }


            //GameStorage.Save(save);
            Player = save.Turn;
            return Page();
        }

        public IActionResult OnPost(string submit)
        {
            var save = GameStorage.Load(Index);
            Player = save.Turn;

            if (submit.Equals("Shoot"))
            {
                Player opponent = save.PlayerTwo;
                //int xCord = Letters.IndexOf(XCord.ToUpper(), StringComparison.Ordinal);

                if (Player == save.PlayerTwo)
                {
                    opponent = save.PlayerOne;
                }

                if (XCord <= Player.GameBoard.Width && YCord <= Player.GameBoard.Height)
                {
                    Shoot(Player, opponent, XCord, YCord, save);
                    GameStorage.Save(save);
                }
                else
                {
                    Message = "Coordinates can't be bigger than gameboard measures!";
                }

                GameStorage.Save(save);
                return RedirectToPage("PlayingGame", new {index = Index, message = Message, move = false});
            }

            if (submit.Equals("Continue"))
            {
                save.Turn = save.Turn == save.PlayerOne ? save.PlayerTwo : save.PlayerOne;
                
                GameStorage.Save(save);

                return RedirectToPage("PlayingGame", new {index = Index, message = Message, move = true});
            }

            if (submit.Equals("Save Game"))
            {
                GameStorage.Save(save);
                return RedirectToPage("Index");
            }

            if (submit.Equals("Back to main menu"))
            {
                GameStorage.Save(save);
                return RedirectToPage("Index");
            }

            return RedirectToPage("PlayingGame", new {index = Index});
        }

        public void Shoot(Player attacker, Player opponent, int x, int y, Game game)
        {
            var shotPanel = opponent.GameBoard.Board[y].Row[x].CellStatus;

            if (attacker.FiringBoard.Board[y].Row[x].CellStatus == Cell.CellState.Hit ||
                attacker.FiringBoard.Board[y].Row[x].CellStatus == Cell.CellState.Miss)
            {
                Message = "You already shot that panel!";
            }

            else if (shotPanel != Cell.CellState.Empty)
            {
                opponent.GameBoard.Board[y].Row[x].CellStatus = Cell.CellState.Hit;
                attacker.FiringBoard.Board[y].Row[x].CellStatus = Cell.CellState.Hit;

                foreach (var ship in opponent.Ships)
                {
                    if (shotPanel == ship.CellState) ship.Hits++;
                    if (ship.IsSunk && shotPanel == ship.CellState)
                    {
                        Message = "You sunk a " + ship.Name;
                        break;
                    }

                    if (shotPanel == ship.CellState)
                    {
                        Message = "You hit a " + ship.Name;
                        break;
                    }
                }
            }
            else
            {
                attacker.FiringBoard.Board[y].Row[y].CellStatus = Cell.CellState.Miss;
                Message = "That was a miss!";
            }
        }
    }
}