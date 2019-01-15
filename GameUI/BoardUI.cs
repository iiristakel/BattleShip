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
                sb.Append("| " + GetBoardSquareStateSymbol(cell.CellStatus) + " ");
            }


            sb.Append("|");
            return sb.ToString();
        }

        public string GetBoardSquareStateSymbol(CellState state)
        {
            switch (state)
            {
                case CellState.Empty: return " ";
                case CellState.Patrol: return "P";
                case CellState.Cruiser: return "C";
                case CellState.Carrier: return "A";
                case CellState.Submarine: return "S";
                case CellState.Battleship: return "B";
                case CellState.Sunk: return "X";
                case CellState.Miss: return "M";
                case CellState.Hit: return "H";
                default:
                    throw new InvalidEnumArgumentException("Unsupported enum value found!");
            }
        }
    }
}