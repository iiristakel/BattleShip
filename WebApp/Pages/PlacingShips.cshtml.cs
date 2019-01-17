using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameSystem;
using GameUI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace WebApp.Pages
{
    public class PlacingShipsModel : PageModel
    {
        [BindProperty] public int Index { get; set; }
        // public bool Error { get; set; }


        public string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [BindProperty] public Player Player { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(1)]
        public string XCord { get; set; }

        [BindProperty] [Range(0, 15)] public int YCord { get; set; }
        [BindProperty] [MaxLength(1)] public string Direction { get; set; }

        public List<Ship> ShipsNotPlaced { get; set; } = new List<Ship>();

        public IActionResult OnGet(int index)
        {
            Index = index;

            var save = GameStorage.Load(Index);
            Player = save.Turn;
            GetShipsThatCanBePlaced(Player);
            return Page();
        }

        public IActionResult OnPost(string submit)
        {
            var save = GameStorage.Load(Index);
            Player = save.Turn;
            GetShipsThatCanBePlaced(Player);

            if (submit.Equals("Next Player"))
            {
                save.Turn = save.PlayerTwo;
                GameStorage.Save(save);
                return RedirectToPage("PlacingShips", new {index = Index, shipIndex = 0});
            }

            if (submit.Equals("Play"))
            {
                save.Turn = save.PlayerOne;
                GameStorage.Save(save);
                return RedirectToPage("PlayingGame", new {index = Index, move = true});
            }

            if (submit.Equals("Place ship"))
            {
                var ship = ShipsNotPlaced[0];
                int xCord = Letters.IndexOf(XCord.ToUpper(), StringComparison.Ordinal);
                if (CanShipBePlaced(Player, ship, xCord, YCord, Direction))
                {
                    MainSystem.PlaceShip(Player, ship, xCord, YCord, Direction);
                    GameStorage.Save(save);

                    ship.IsPlaced = true;
                }
                else
                {
                    //some error
                }
            }

            return RedirectToPage("PlacingShips", new {index = Index});
        }

        private bool CanShipBePlaced(Player player, Ship ship, int X, int Y, string dir)
        {
            for (int i = 0; i < ship.Width; i++)
            {
                if ((X >= 0) && (X < (player.GameBoard.Board[0].Row.Count)) &&
                    (player.GameBoard.Board[Y].Row[X].CellStatus == Cell.CellState.Empty) && (Y >= 0) &&
                    (Y < (player.GameBoard.Board.Count)))
                {
                    if (dir.ToUpper().Equals("H") || dir.Equals("")) X = X + 1;
                    if (dir.ToUpper().Equals("V")) Y = Y + 1;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        private void GetShipsThatCanBePlaced(Player player)
        {
            foreach (var ship in player.Ships)
            {
                if (ship.IsPlaced == false)
                    ShipsNotPlaced.Add(ship);
            }
        }
    }
}