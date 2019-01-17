using System;
using System.ComponentModel;
using Domain;

namespace GameSystem
{
    public static class MainSystem
    {
        public static void SetPlayerName(Player player)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Player name can not be empty!");
                SetPlayerName(player);
            }
            else
            {
                player.Name = input;
            }
        }

        /*public static void SetTouchingRule(Game game)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || input.ToUpper().Equals("Y"))
            {
                RuleSet.CanTouch = true;
            }
            else if (input.ToUpper().Equals("N"))
            {
                RuleSet.CanTouch = false;
            }
            else
            {
                Console.WriteLine("Must be Y or N!");
                SetTouchingRule(game);
            }
        }*/

        public static void AskBoatCount(Player player1, Player player2)
        {
            Console.WriteLine("How many boats (min: 1, max: 10) do you want? Separate their lengths (1-5) by" +
                              " commas. (For example: 1,4,5,3,3) Boats sizes and count are same for both players.");
            var input = Console.ReadLine();

            var shipwidths = input.Split(',');
            foreach (var width in shipwidths)
            {
                if (width == "1")
                {
                    RuleSet.PatrolAmount += 1;
                }
                else if (width == "2")
                {
                    RuleSet.CruiserAmount += 1;
                }
                else if (width == "3")
                {
                    RuleSet.SubmarineAmount += 1;
                }
                else if (width == "4")
                {
                    RuleSet.BattleshipAmount += 1;
                }
                else if (width == "5")
                {
                    RuleSet.CarrierAmount += 1;
                }
                else
                {
                    Console.WriteLine("Must be 1 to 10 boats with lengths 1-5 separated by commas. You wrote: " +
                                      input);
                    RuleSet.PatrolAmount = 0;
                    RuleSet.CarrierAmount = 0;
                    RuleSet.BattleshipAmount = 0;
                    RuleSet.CruiserAmount = 0;
                    RuleSet.SubmarineAmount = 0;
                    AskBoatCount(player1, player2);
                }
            }

            AddShipsToPlayer(player1);
            AddShipsToPlayer(player2);
        }

        public static void AddShipsToPlayer(Player player)
        {
            for (int i = 0; i < RuleSet.PatrolAmount; i++)
            {
                player.Ships.Add(new Patrol());
            }

            for (int i = 0; i < RuleSet.CruiserAmount; i++)
            {
                player.Ships.Add(new Cruiser());
            }

            for (int i = 0; i < RuleSet.BattleshipAmount; i++)
            {
                player.Ships.Add(new Battleship());
            }

            for (int i = 0; i < RuleSet.CarrierAmount; i++)
            {
                player.Ships.Add(new Carrier());
            }

            for (int i = 0; i < RuleSet.SubmarineAmount; i++)
            {
                player.Ships.Add(new Submarine());
            }
        }


        public static void CanShipBePlaced(Ship ship, Player player)
        {
            bool done = false;
            int x = 0;
            int y = 0;
            string dir = "";
            while (!done)
            {
                Console.WriteLine("Where to put your " + ship.Name + ", length: " + ship.Width);

                x = SetXCoordinate(player);
                y = SetYCoordinate(player);
                dir = "";

                int tempX = x;
                int tempY = y;
                if (!ship.Name.Equals("Patrol")) dir = SetDirection();
                Console.WriteLine();

                for (int i = 0; i < ship.Width; i++)
                {
                    if ((tempX >= 0) && (tempX < (player.GameBoard.Board[0].Row.Count)) &&
                        (player.GameBoard.Board[y].Row[tempX].CellStatus == Cell.CellState.Empty) && (tempY >= 0) &&
                        (tempY < (player.GameBoard.Board.Count)))
                    {
                        if (i == ship.Width - 1) done = true;
                        if (dir.ToUpper().Equals("H") || dir.Equals("")) tempX = tempX + 1;
                        if (dir.ToUpper().Equals("V")) tempY = tempY + 1;
                    }
                    else
                    {
                        Console.WriteLine("Try again! There wasn't enough space for your ship!");
                        break;
                    }
                }
            }


            PlaceShip(player, ship, x, y, dir);
        }

        public static void PlaceShip(Player player, Ship ship, int X, int Y, string dir)
        {
            for (int j = 0; j < ship.Width; j++)
            {
                if (ship.Name == "Patrol") player.GameBoard.Board[Y].Row[X].CellStatus = Cell.CellState.Patrol;
                if (ship.Name == "Cruiser") player.GameBoard.Board[Y].Row[X].CellStatus = Cell.CellState.Cruiser;
                if (ship.Name == "Submarine") player.GameBoard.Board[Y].Row[X].CellStatus = Cell.CellState.Submarine;
                if (ship.Name == "Battleship") player.GameBoard.Board[Y].Row[X].CellStatus = Cell.CellState.Battleship;
                if (ship.Name == "Carrier") player.GameBoard.Board[Y].Row[X].CellStatus = Cell.CellState.Carrier;
                if (dir.ToUpper().Equals("H") || dir.Equals("")) X = X + 1;
                if (dir.ToUpper().Equals("V")) Y = Y + 1;
            }
        }

        public static int SetXCoordinate(Player player)
        {
            String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Console.Write("Enter letter (A - " + chars.Substring(player.GameBoard.Board[0].Row.Count - 1, 1) + "): ");
            int x = 0;
            var input = Console.ReadLine()?.ToUpper();
            if (input != null && input.Trim().Length != 0 &&
                chars.Substring(0, player.GameBoard.Board[0].Row.Count).Contains(input))
            {
                x = chars.IndexOf(input, StringComparison.Ordinal);
            }
            else
            {
                Console.WriteLine("Must be letter from A - " +
                                  chars.Substring(player.GameBoard.Board[0].Row.Count - 1, 1));
                SetXCoordinate(player);
            }

            return x;
        }

        public static int SetYCoordinate(Player player)
        {
            Console.Write("Enter number from 0 to " + (player.GameBoard.Board.Count - 1) + ": ");

            var input = Console.ReadLine();
            int number;
            int y = 0;
            if (int.TryParse(input, out number) && number >= 0 && number < player.GameBoard.Board.Count)
            {
                y = number;
            }
            else
            {
                Console.WriteLine("Must be number from 0 to " + (player.GameBoard.Board.Count - 1));
                SetYCoordinate(player);
            }

            return y;
        }

        public static string SetDirection()
        {
            Console.Write("Horizontally (H) or Vertically (V) ? ");
            var input = Console.ReadLine();
            string s = "";
            if ((input.ToUpper() == "H") || (input.ToUpper() == "V"))
            {
                s = input;
            }
            else
            {
                Console.WriteLine("Must be H or V");
                SetDirection();
            }

            return s;
        }

        public static void PlaceShipsRandomly(Player player)
        {
            foreach (var ship in player.Ships)
            {
                Random random = new Random();
                var dir = "";

                var number = random.Next(1, 2);
                if (number == 1)
                {
                    dir = "H";
                }
                else
                {
                    dir = "V";
                }

                var x = 0;
                var y = 0;
                var success = false;
                
                while (!success)
                {
                    y = random.Next(0, player.GameBoard.Height);
                    x = random.Next(0, player.GameBoard.Width);
                    var tempX = x;
                    var tempY = y;

                    for (int i = 0; i < ship.Width; i++)
                    {
                        if ((tempX >= 0) && (tempX < (player.GameBoard.Board[0].Row.Count)) &&
                            (player.GameBoard.Board[y].Row[tempX].CellStatus == Cell.CellState.Empty) && (tempY >= 0) &&
                            (tempY < (player.GameBoard.Board.Count)))
                        {
                            if (i == ship.Width - 1) success = true;
                            if (dir.ToUpper().Equals("H") || dir.Equals("")) tempX = tempX + 1;
                            if (dir.ToUpper().Equals("V")) tempY = tempY + 1;
                        }
                    }
                }
                PlaceShip(player, ship, x, y, dir);
            }
        }


        public static string GetBoardSquareStateSymbol(Cell.CellState state)
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
        }

        public static void WaitForUser()
        {
            Console.Write("Press any key to continue!");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}