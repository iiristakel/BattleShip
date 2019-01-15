using System;

namespace Domain
{
    public class Cell
    {
        public int CellId { get; set; }
        public CellState CellStatus { get; set; }
        
        public int BoardRowId { get; set; }
        public BoardRow BoardRow { get; set; }

        public Cell()
        {
            CellStatus = CellState.Empty;
        }
    }
}