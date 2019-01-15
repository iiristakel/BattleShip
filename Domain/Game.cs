using System;
using System.Collections.Generic;

namespace Domain
{
    public class Game
    {
        public int GameId { get; set; }

        public int PlayerOneId { get; set; }
        public Player PlayerOne { get; set; }

        public int PlayerTwoId { get; set; }
        public Player PlayerTwo { get; set; }

        public Player Winner { get; set; }

        public Player Turn { get; set; }

        public bool CanTouch { get; set; } = true;
    }
}