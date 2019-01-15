namespace Domain
{
    public class Ship
    {
        public int ShipId { get; set; }
        
        //public int PlayerId { get; set; }
        //public Player Player { get; set; }
        
        public string Name { get; set; }
        public int Width { get; set; }
        public int Hits { get; set; } = 0;
        public CellState CellState { get; set; }
        public bool IsSunk => Hits >= Width;
    }
    
    public class Patrol : Ship
    {
        public Patrol()
        {
            Name = "Patrol";
            Width = 1;
            CellState = CellState.Patrol;
        }
    }

    public class Cruiser : Ship
    {
        public Cruiser()
        {
            Name = "Cruiser";
            Width = 2;
            CellState = CellState.Cruiser;
        }
    }

    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Width = 3;
            CellState = CellState.Submarine;
        }
    }

    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Width = 4;
            CellState = CellState.Battleship;
        }
    }

    public class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Carrier";
            Width = 5;
            CellState = CellState.Carrier;
        }
    }
}