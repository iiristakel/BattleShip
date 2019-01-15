using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class BoardRow
    {
        [Key]
        public int BoardRowId { get; set; }
        public List<Cell> Row { get; set; } = new List<Cell>();

        public int GameBoardId { get; set; }
        public GameBoard Board { get; set; }

    }
}