using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Ship
    {
        [Key]
        public int ShipId { get; set; }
        
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        
        public string Name { get; set; }
        public int Width { get; set; }
        public int Hits { get; set; } = 0;
        public Cell.CellState CellState { get; set; }
        public bool IsSunk => Hits >= Width;
        public bool IsPlaced { get; set; } = false;
    }
    
    public class Patrol : Ship
    {
        public Patrol()
        {
            Name = "Patrol";
            Width = 1;
            CellState = Cell.CellState.Patrol;
        }
    }

    public class Cruiser : Ship
    {
        public Cruiser()
        {
            Name = "Cruiser";
            Width = 2;
            CellState = Cell.CellState.Cruiser;
        }
    }

    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Width = 3;
            CellState = Cell.CellState.Submarine;
        }
    }

    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Width = 4;
            CellState = Cell.CellState.Battleship;
        }
    }

    public class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Carrier";
            Width = 5;
            CellState = Cell.CellState.Carrier;
        }
    }
}