using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Cell
    {
        public int CellId { get; set; }
        public CellState CellStatus { get; set; }
        
        [ForeignKey("BoardRow")]
        public int BoardRowId { get; set; }
        public BoardRow BoardRow { get; set; }

        public Cell()
        {
            CellStatus = CellState.Empty;
        }
        
        public enum CellState
        {
            Empty,
            Carrier,
            Battleship,
            Submarine,
            Cruiser,
            Patrol,
            Miss,
            Hit
        }
    }
}