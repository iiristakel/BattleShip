using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages
{
    public class NewGameSetupModel : PageModel
    {
        private DAL.AppDbContext _context;

        public NewGameSetupModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Game> Games { get; set; }
        public IList<Player> Players { get; set; }
        public IList<Ship> Ships { get; set; }
        

        public void OnGet()
        {
        }


        [Required]
        [MinLength(1)]
        [MaxLength(35)]
        [BindProperty]
        public string PlayerOneName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(35)]
        [BindProperty]
        public string PlayerTwoName { get; set; }

        //public Player Player1 { get; set; }
        //public Player Player2 { get; set; }

        [BindProperty] [Range(5, 26)] public int BoardWidth { get; set; }

        [BindProperty] [Range(5, 15)] public int BoardHeight { get; set; }

        [BindProperty] [Range(0, 5)] public int CarrierAmount { get; set; }

        [BindProperty] [Range(0, 5)] public int BattleshipAmount { get; set; }

        [BindProperty] [Range(0, 5)] public int SubmarineAmount { get; set; }

        [BindProperty] [Range(0, 5)] public int CruiserAmount { get; set; }

        [BindProperty] [Range(0, 5)] public int PatrolAmount { get; set; }


        public ActionResult OnPost(string submit)
        {

            var game = new Game();
            var player1 = new Player(PlayerOneName);
            var player2 = new Player(PlayerTwoName);

            RuleSet.BoardWidth = BoardWidth;
            RuleSet.BoardHeight = BoardHeight;
            game.PlayerOne = player1;
            game.PlayerTwo = player2;
            game.PlayerOne.GameBoard = new GameBoard(RuleSet.BoardWidth, RuleSet.BoardHeight);
            game.PlayerOne.FiringBoard = new GameBoard(RuleSet.BoardWidth, RuleSet.BoardHeight);
            game.PlayerTwo.GameBoard = new GameBoard(RuleSet.BoardWidth, RuleSet.BoardHeight);
            game.PlayerTwo.FiringBoard = new GameBoard(RuleSet.BoardWidth, RuleSet.BoardHeight);
            game.Turn = player1;
            
            if (!submit.Equals("Back to main menu"))
            {
                RuleSet.BattleshipAmount = BattleshipAmount;
                RuleSet.CarrierAmount = CarrierAmount;
                RuleSet.CruiserAmount = CruiserAmount;
                RuleSet.PatrolAmount = PatrolAmount;
                RuleSet.SubmarineAmount = SubmarineAmount;
                RuleSet.BoardHeight = BoardHeight;
                RuleSet.BoardWidth = BoardWidth;
                MainSystem.AddShipsToPlayer(game.PlayerOne);
                MainSystem.AddShipsToPlayer(game.PlayerTwo);

                if (submit.Equals("Generate random board"))
                {
                    MainSystem.PlaceShipsRandomly(player1);
                    MainSystem.PlaceShipsRandomly(player2);
                    return RedirectToPage("PlayingGame", new {index = GameStorage.Save(game), move = true});
                }

                if (submit.Equals("Custom ship placement"))
                {
                    return RedirectToPage("PlacingShips", new {index = GameStorage.Save(game)}); 
                }
            }

            return RedirectToPage("Index");
        }
    }
}