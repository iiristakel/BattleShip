using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        [ForeignKey("PlayerOne")]
        public int PlayerOneId { get; set; }
        public Player PlayerOne { get; set; }

        [ForeignKey("PlayerTwo")]
        public int PlayerTwoId { get; set; }
        public Player PlayerTwo { get; set; }

        public Player Winner { get; set; }

        public Player Turn { get; set; }

        //public bool CanTouch { get; set; } = true;
    }
}