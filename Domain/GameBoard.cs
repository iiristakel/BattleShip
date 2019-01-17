using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class GameBoard
    {
        [Key]
        public int GameBoardId { get; set; }

        
        //public int PlayerId { get; set; }
       // [NotMapped]
       // public Player Player { get; set; }
        
        public  int Width { get; set; }
        public  int Height { get; set; }
        public List<BoardRow> Board { get; set; } = new List<BoardRow>();

        public GameBoard(int width, int height)
        {
           Width = width;
           Height = height;
            
            /*for (int i = 0; i < height; i++)
            {
                Board.Add(new List<CellState>());
                for (int j = 0; j < width; j++)
                {
                    Board[i].Add(new CellState());
                }
            }*/
            for (int i = 0; i < height; i++)
            {
                Board.Add(new BoardRow());
                for (int j = 0; j < width; j++)
                {
                    Board[i].Row.Add(new Cell());
                }
            }
        }
    }
}