using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Domain;

namespace GameUI
{
    public class BoardUI
    {
        public string GetBoardString(GameBoard gameBoard)
        {
            String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var sb = new StringBuilder();

            sb.Append(" ");
            for (int i = 0; i < gameBoard.Board[0].Row.Count; i++)
            {
                sb.Append("   " + chars[i]);
            }

            sb.Append("\n");
            for (int boardRow = 0; boardRow < gameBoard.Board.Count; boardRow++)
            {
                sb.Append(GetRowSeparator(gameBoard.Board[0].Row.Count) + "\n");
                sb.Append(boardRow.ToString("00"));
                sb.Append(GetRowWithData(gameBoard.Board[boardRow].Row) + "\n");
            }

            sb.Append(GetRowSeparator(gameBoard.Board[0].Row.Count));
            return sb.ToString();
        }


        public string GetRowSeparator(int elemCountInRow)
        {
            var sb = new StringBuilder();

            sb.Append("  ");
            for (int i = 0; i < elemCountInRow; i++)
            {
                sb.Append("+---");
            }

            sb.Append("+");
            return sb.ToString();
        }

        public string GetRowWithData(List<Cell> boardRow)
        {
            var sb = new StringBuilder();
            foreach (var cell in boardRow)
            {
                sb.Append("| " + GameSystem.MainSystem.GetBoardSquareStateSymbol(cell.CellStatus) + " ");
            }


            sb.Append("|");
            return sb.ToString();
        }

        /*public string GetBoardSquareStateSymbol(Cell.CellState state)
        {
            switch (state)
            {
                case Cell.CellState.Empty: return " ";
                case Cell.CellState.Patrol: return "P";
                case Cell.CellState.Cruiser: return "C";
                case Cell.CellState.Carrier: return "A";
                case Cell.CellState.Submarine: return "S";
                case Cell.CellState.Battleship: return "B";
                case Cell.CellState.Miss: return "M";
                case Cell.CellState.Hit: return "H";
                default:
                    throw new InvalidEnumArgumentException("Unsupported enum value found!");
            }
        }*/
    }
}