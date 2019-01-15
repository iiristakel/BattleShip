using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using GameSystem;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameUI
{
    public class BattleshipUI
    {
        public AppDbContext context = new AppDbContext();
        public Game Game = new Game();
        Player _player1 = new Player("");
        Player _player2 = new Player("");

        private int _width = 0;
        private int _height = 0;

        BoardUI _board = new BoardUI();

        //RuleSet _ruleSet = new RuleSet();
        GameStorage _gameStorage = new GameStorage();

        public BattleshipUI()
        {
            Game.PlayerOne = _player1;
            Game.PlayerTwo = _player2;
            ApplicationMenu.GameMenu.CleanScreenInMenuStart = false;
        }

        public string RunGame(string command)
        {
            Console.Clear();

            AskName(_player1);
            AskName(_player2);

            Console.Clear();
            Console.WriteLine("Hello " + _player1.Name + " and " + _player2.Name + "!");

            AskTouchingRule();
            //TODO function for not touching

            while (true)
            {
                Console.WriteLine("R - Generate random 10x10 boards with 5 different ships \n" +
                                  "M - Enter measures and ships manually");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input.ToUpper().Trim().Equals("M"))
                {
                    AskWidth();
                    AskHeight();
                    AskBoatCount(_player1, _player2);

                    _player1.GameBoard = new GameBoard(_width, _height);
                    _player1.FiringBoard = new GameBoard(_width, _height);
                    _player2.GameBoard = new GameBoard(_width, _height);
                    _player2.FiringBoard = new GameBoard(_width, _height);

                    Console.Clear();

                    AddBoats(_player1);

                    Console.WriteLine(_board.GetBoardString(_player1.GameBoard));
                    Console.WriteLine("This is your battleship gameboard!");

                    Console.WriteLine(
                        "############################\n############################\n############################\n 2. player's turn!");

                    WaitForUser();
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n");

                    AddBoats(_player2);

                    Console.WriteLine(_board.GetBoardString(_player2.GameBoard));
                    Console.WriteLine("This is your battleship gameboard!");
                    Console.WriteLine();
                    break;
                }

                if (input.ToUpper().Equals("R"))
                {
                    //TODO random ship placement 10x10 board, ships 12345
                    break;
                }

                Console.WriteLine("Must be R or M!");
            }

            Console.WriteLine("A)Start shooting \nB)Save game \n>");
            var chosenCommand = Console.ReadLine()?.Trim().ToUpper();
            if(chosenCommand == "A" || string.IsNullOrWhiteSpace(chosenCommand))
            {
                StartShooting(_player1, _player2, Game);
            }else if (chosenCommand == "B")
            {
                SaveGame(chosenCommand);
            }

            return "";
        }

        public void AskWidth()
        {
            Console.Write("Enter GameBoard width (5 - 26) [Enter->10]: ");

            var input = Console.ReadLine();
            int width;

            if (string.IsNullOrWhiteSpace(input))
            {
                _width = 10;
            }
            else if (int.TryParse(input, out width) && width > 4 && width < 27)
            {
                _width = width;
            }
            else
            {
                Console.WriteLine("Width must be number between 5 and 26!");
                AskWidth();
            }
        }

        public void AskHeight()
        {
            Console.Write("Enter GameBoard height (5 - 15) [Enter->10]: ");

            var input = Console.ReadLine();
            int height;

            if (string.IsNullOrWhiteSpace(input))
            {
                _height = 10;
            }
            else if (int.TryParse(input, out height) && height < 16 && height > 4)
            {
                _height = height;
            }
            else
            {
                Console.WriteLine("Height must be number between 5 and 15!");
                AskHeight();
            }
        }

        public void AskName(Player player)
        {
            if (_player1.Name == "") Console.Write("Enter 1.player name: ");
            if (_player1.Name != "") Console.Write("Enter 2.player name: ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Player name can not be empty!");
                AskName(player);
            }
            else
            {
                player.Name = input;
            }
        }

        private int _x;
        private int _y;
        private String _s = "";

        public void AskBoatCount(Player player1, Player player2)
        {
            Console.WriteLine("How many boats (min: 1, max: 10) do you want? Separate their lengths (1-5) by" +
                              " commas. (For example: 1,4,5,3,3) Boats sizes and count are same for both players.");
            var input = Console.ReadLine();

            var shipwidths = input.Split(',');
            foreach (var width in shipwidths)
            {
                if (width == "1")
                {
                    player1.Ships.Add(new Patrol());
                    player2.Ships.Add(new Patrol());
                }
                else if (width == "2")
                {
                    player1.Ships.Add(new Cruiser());
                    player2.Ships.Add(new Cruiser());
                }
                else if (width == "3")
                {
                    player1.Ships.Add(new Submarine());
                    player2.Ships.Add(new Submarine());
                }
                else if (width == "4")
                {
                    player1.Ships.Add(new Battleship());
                    player2.Ships.Add(new Battleship());
                }
                else if (width == "5")
                {
                    player1.Ships.Add(new Carrier());
                    player2.Ships.Add(new Carrier());
                }
                else
                {
                    Console.WriteLine("Must be 1 to 10 boats with lengths 1-5 separated by commas. You wrote: " +
                                      input);
                    _player1.Ships.Clear();
                    _player2.Ships.Clear();
                    AskBoatCount(player1, player2);
                }
            }
        }

        public void AddBoats(Player player)
        {
            Console.WriteLine(player.Name + " where do you want to place your ships?");

            foreach (var ship in player.Ships)
            {
                Console.WriteLine(_board.GetBoardString(player.GameBoard));
                SetShip(ship, player);
            }
        }

        public void SetShip(Ship ship, Player player)
        {
            bool done = false;
            while (done == false)
            {
                Console.WriteLine("Where to put your " + ship.Name + ", length: " + ship.Width);

                AskXCoordinate(player);
                AskYCoordinate(player);
                if (!ship.Name.Equals("Patrol")) AskDirection();
                Console.WriteLine();

                var xCord = _x;
                var yCord = _y;
                for (int i = 0; i < ship.Width; i++)
                {
                    if ((xCord >= 0) && (xCord < (player.GameBoard.Board[0].Row.Count)) &&
                        (player.GameBoard.Board[yCord].Row[xCord].CellStatus == CellState.Empty) && (yCord >= 0) &&
                        (yCord < (player.GameBoard.Board.Count)))
                    {
                        if (i == ship.Width - 1) done = true;
                        if (_s.ToUpper().Equals("H") || _s.Equals("")) xCord = xCord + 1;
                        if (_s.ToUpper().Equals("V")) yCord = yCord + 1;
                    }
                    else
                    {
                        Console.WriteLine("Try again! There wasn't enough space for your ship!");
                        SetShip(ship, player);
                        break;
                    }
                }
            }

            for (int j = 0; j < ship.Width; j++)
            {
                if (ship.Name == "Patrol") player.GameBoard.Board[_y].Row[_x].CellStatus = CellState.Patrol;
                if (ship.Name == "Cruiser") player.GameBoard.Board[_y].Row[_x].CellStatus = CellState.Cruiser;
                if (ship.Name == "Submarine") player.GameBoard.Board[_y].Row[_x].CellStatus = CellState.Submarine;
                if (ship.Name == "Battleship") player.GameBoard.Board[_y].Row[_x].CellStatus = CellState.Battleship;
                if (ship.Name == "Carrier") player.GameBoard.Board[_y].Row[_x].CellStatus = CellState.Carrier;
                if (_s.ToUpper().Equals("H") || _s.Equals("")) _x = _x + 1;
                if (_s.ToUpper().Equals("V")) _y = _y + 1;
            }
        }

        public void AskXCoordinate(Player player)
        {
            String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Console.Write("Enter letter (A - " + chars.Substring(player.GameBoard.Board[0].Row.Count - 1, 1) + "): ");

            var input = Console.ReadLine()?.ToUpper();
            if (input != null && input.Trim().Length != 0 &&
                chars.Substring(0, player.GameBoard.Board[0].Row.Count).Contains(input))
            {
                _x = chars.Substring(0, player.GameBoard.Board[0].Row.Count)
                    .IndexOf(input, StringComparison.Ordinal);
            }
            else
            {
                Console.WriteLine("Must be letter from A - " +
                                  chars.Substring(player.GameBoard.Board[0].Row.Count - 1, 1));
                AskXCoordinate(player);
            }
        }

        public void AskYCoordinate(Player player)
        {
            Console.Write("Enter number from 0 to " + (player.GameBoard.Board.Count - 1) + ": ");

            var input = Console.ReadLine();
            int number;
            if (int.TryParse(input, out number) && number >= 0 && number < player.GameBoard.Board.Count)
            {
                _y = number;
            }
            else
            {
                Console.WriteLine("Must be number from 0 to " + (player.GameBoard.Board.Count - 1));
                AskYCoordinate(player);
            }
        }

        public void AskDirection()
        {
            Console.Write("Horizontally (H) or Vertically (V) ? ");
            var input = Console.ReadLine();
            if ((input.ToUpper() == "H") || (input.ToUpper() == "V"))
            {
                _s = input;
            }
            else
            {
                Console.WriteLine("Must be H or V");
                AskDirection();
            }
        }

        public void AskTouchingRule()
        {
            Console.WriteLine("Can ships touch in your game? [y/n]");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || input.ToUpper().Equals("Y"))
            {
                Game.CanTouch = true;
            }
            else if (input.ToUpper().Equals("N"))
            {
                Game.CanTouch = false;
            }
            else
            {
                Console.WriteLine("Must be Y or N!");
                AskTouchingRule();
            }
        }

        public string StartShooting(Player player1, Player player2, Game game)
        {
            while (!player1.HasLost && !player2.HasLost)
            {
                game.Turn = player1;
                PlayTurn(player1, player2);
                ApplicationMenu.InGameMenu.RunMenu();
                if (!player1.HasLost && !player2.HasLost)
                {
                    game.Turn = player2;
                    PlayTurn(player2, player1);
                    ApplicationMenu.InGameMenu.RunMenu();
                }
            }

            if (player2.HasLost)
            {
                game.Winner = player1;
                Console.WriteLine(player1.Name + ", you won the game!");
            }

            if (player1.HasLost)
            {
                game.Winner = player2;
                Console.WriteLine(player2.Name + ", you won the game!");
            }

            Console.WriteLine();
            return ApplicationMenu.InGameMenu.RunMenu();
        }

        public void PlayTurn(Player player1, Player player2)
        {
            Console.WriteLine();
            Console.WriteLine(player1.Name + ", it is your turn:");
            PrintBothBoards(player1);
            AskXCoordinate(player2);
            AskYCoordinate(player2);
            var shotPanel = player2.GameBoard.Board[_y].Row[_x].CellStatus;

            if (player1.FiringBoard.Board[_y].Row[_x].CellStatus == CellState.Hit ||
                player1.FiringBoard.Board[_y].Row[_x].CellStatus == CellState.Miss)
            {
                Console.WriteLine("You already shot that panel!");
                WaitForUser();
                PlayTurn(player1, player2);
            }

            else if (shotPanel != CellState.Empty)
            {
                Console.WriteLine();
                player2.GameBoard.Board[_y].Row[_x].CellStatus = CellState.Hit;
                player1.FiringBoard.Board[_y].Row[_x].CellStatus = CellState.Hit;
                PrintBothBoards(player1);
                foreach (var ship in player2.Ships)
                {
                    if (shotPanel == ship.CellState) ship.Hits++;
                    if (ship.IsSunk && shotPanel == ship.CellState)
                    {
                        ship.CellState = CellState.Sunk;
                        Console.WriteLine("You sunk a " + ship.Name + "! [Length: " + ship.Width + "]");
                        WaitForUser();
                        break;
                    }

                    if (shotPanel == ship.CellState)
                    {
                        Console.WriteLine("You hit a ship!");
                        WaitForUser();
                        break;
                    }
                }

                if (!_player1.HasLost && !_player2.HasLost) PlayTurn(player1, player2);
            }
            else
            {
                player1.FiringBoard.Board[_y].Row[_x].CellStatus = CellState.Miss;
                PrintBothBoards(player1);
                Console.WriteLine("That was a miss!");
            }
        }

        public void PrintBothBoards(Player player)
        {
            Console.WriteLine("Your gameboard: ");
            Console.WriteLine(_board.GetBoardString(player.GameBoard));
            Console.WriteLine("Your firingboard: ");
            Console.WriteLine(_board.GetBoardString(player.FiringBoard));
        }

        private void WaitForUser()
        {
            Console.Write("Press any key to continue!");
            Console.WriteLine();
            Console.ReadKey();
        }

        public string SaveGame(string command)
        {
            if (context.Games.Find(Game.GameId) == null)
            {
                context.Games.Add(Game);
            }
            else
            {
                context.Games.Update(Game);
            }

            context.SaveChanges();
            Console.WriteLine("Game saved!");
            WaitForUser();
            return ApplicationMenu.MainMenu.RunMenu();
        }

        public string LoadGame(string command)
        {
            if (context.Games.Count() == 0) // don't change it
            {
                Console.WriteLine("No saved games found!");
                WaitForUser();
                return ApplicationMenu.MainMenu.RunMenu();
            }

            Game lastGame = context.Games.Last();

            Console.WriteLine("Game Started");

            lastGame = _gameStorage.GetGames()
                .Where(g => g.GameId == lastGame.GameId)
                .Include(g => g.PlayerOne)
                .ThenInclude(f => f.GameBoard)
                .Include(g => g.PlayerOne)
                .ThenInclude(f => f.FiringBoard)
                .Include(g => g.PlayerOne)
                .ThenInclude(f => f.Ships)
                .Include(g => g.PlayerTwo)
                .ThenInclude(f => f.GameBoard)
                .Include(g => g.PlayerTwo)
                .ThenInclude(f => f.FiringBoard)
                .Include(g => g.PlayerTwo)
                .ThenInclude(f => f.Ships)
                .FirstOrDefault(g => g.GameId == lastGame.GameId);

            Player player1 = lastGame.PlayerOne;
            Player player2 = lastGame.PlayerTwo;

            player1.GameBoard = context.GameBoards.First(a => a.GameBoardId == player1.GameBoard.GameBoardId);
            player2.GameBoard = context.GameBoards.First(a => a.GameBoardId == player2.GameBoard.GameBoardId);

            player1.FiringBoard = context.GameBoards.First(a => a.GameBoardId == player1.FiringBoard.GameBoardId);
            player2.FiringBoard = context.GameBoards.First(a => a.GameBoardId == player2.FiringBoard.GameBoardId);

            /*for (var i = 0; i < player1.GameBoard.Height; i++)
            {
                for (var j = 0; j < player1.GameBoard.Width; j++)
                {
                    player1.GameBoard.Board[i].Row[j].CellStatus = new CellState(){}
                }
            }*/


            if (lastGame.Winner != null)
            {
                Console.WriteLine();
                Console.WriteLine(lastGame.Winner.Name + " already won the game.");
                WaitForUser();
                return ApplicationMenu.MainMenu.RunMenu();
            }

            if (lastGame.Turn == lastGame.PlayerTwo)
            {
                return StartShooting(player2, player1, lastGame);
            }

            return StartShooting(player1, player2, lastGame);
        }
    }
}