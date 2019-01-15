using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }

        [Required]
        [MaxLength (30)]
        public string Name { get; set; }

        [InverseProperty("PlayerOne")] 
        public List<Game> PlayerOneGames { get; set; } = new List<Game>();

        [InverseProperty("PlayerTwo")] 
        public List<Game> PlayerTwoGames { get; set; } = new List<Game>();

        //public int GameBoardId { get; set; }
        public GameBoard GameBoard { get; set; }

        //public int FiringBoardId { get; set; }
        public GameBoard FiringBoard { get; set; }

        public List<Ship> Ships { get; set; } =
            new List<Ship>(); //list of player ships, if they're all sunk, player has lost the game

        public bool HasLost
        {
            get { return Ships.All(x => x.IsSunk);
                
            }
        }

        //constructor
        public Player(string name)
        {
            Name = name;
        }
    }
}