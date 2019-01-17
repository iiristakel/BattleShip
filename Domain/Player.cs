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

        public GameBoard GameBoard { get; set; }

        public GameBoard FiringBoard { get; set; }

        public List<Ship> Ships { get; set; } = new List<Ship>(); 

        public bool HasLost
        {
            get { return Ships.All(x => x.IsSunk);
                
            }
        }

        public Player()
        {
            
        }
        //constructor
        public Player(string name)
        {
            Name = name;
        }
    }
}