using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class BoardRow
    {
        public int BoardRowId { get; set; }
        public List<Cell> Row { get; set; } = new List<Cell>();

        [ForeignKey("Board")]
        public int GameBoardId { get; set; }
        public GameBoard Board { get; set; }

    }
}