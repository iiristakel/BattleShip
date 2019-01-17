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

        BoardUI _board = new BoardUI();

        private int _width;
        private int _height;

        public BattleshipUI()
        {
            Game.PlayerOne = new Player("");
            Game.PlayerTwo = new Player("");
            ApplicationMenu.GameMenu.CleanScreenInMenuStart = false;
        }

        public string RunGame(string command)
        {
            Console.Clear();

            Console.Write("Enter 1.player name: ");
            MainSystem.SetPlayerName(Game.PlayerOne);
            Console.Write("Enter 2.player name: ");
            MainSystem.SetPlayerName(Game.PlayerTwo);

            Console.Clear();
            Console.WriteLine("Hello " + Game.PlayerOne.Name + " and " + Game.PlayerTwo.Name + "!");

            AskWidth();
            AskHeight();

            MainSystem.AskBoatCount(Game.PlayerOne, Game.PlayerTwo);

            Game.PlayerOne.GameBoard = new GameBoard(_width, _height);
            Game.PlayerOne.FiringBoard = new GameBoard(_width, _height);
            Game.PlayerTwo.GameBoard = new GameBoard(_width, _height);
            Game.PlayerTwo.FiringBoard = new GameBoard(_width, _height);

            Console.Clear();
            while (true)
            {
                Console.WriteLine("R - Place ships randomly \n" +
                                  "M - Place ships manually [Enter]");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input.ToUpper().Trim().Equals("M"))
                {
                    AddBoats(Game.PlayerOne);

                    Console.WriteLine(_board.GetBoardString(Game.PlayerOne.GameBoard));
                    Console.WriteLine("This is your battleship gameboard!");

                    Console.WriteLine(
                        "############################\n############################\n############################\n 2. player's turn!");

                    MainSystem.WaitForUser();
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n");

                    AddBoats(Game.PlayerTwo);

                    Console.WriteLine(_board.GetBoardString(Game.PlayerTwo.GameBoard));
                    Console.WriteLine("This is your battleship gameboard!");
                    Console.WriteLine();
                    break;
                }

                if (input.ToUpper().Equals("R"))
                {
                    //TODO random ship placement
                    break;
                }

                Console.WriteLine("Must be R or M!");
            }

            return ApplicationMenu.GameMenu.RunMenu();
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
                RuleSet.BoardWidth = width;
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
                RuleSet.BoardHeight = height;
            }
            else
            {
                Console.WriteLine("Height must be number between 5 and 15!");
                AskHeight();
            }
        }

        public void AddBoats(Player player)
        {
            Console.WriteLine(player.Name + " where do you want to place your ships?");

            foreach (var ship in player.Ships)
            {
                Console.WriteLine(_board.GetBoardString(player.GameBoard));
                MainSystem.CanShipBePlaced(ship, player);
            }
        }


        public string Shooting(string command)
        {
            StartShooting(Game.PlayerOne, Game.PlayerTwo, Game);
            return "";
        }

        public string StartShooting(Player player1, Player player2, Game game)
        {
            while (!player1.HasLost && !player2.HasLost)
            {
                game.Turn = player1;

                PlayTurn(player1, player2);

                if (!player1.HasLost && !player2.HasLost)
                {
                    game.Turn = player2;
                    ApplicationMenu.InGameMenu.RunMenu();
                    PlayTurn(player2, player1);
                    if (!player1.HasLost) ApplicationMenu.InGameMenu.RunMenu();
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
            int x = MainSystem.SetXCoordinate(player2);
            int y = MainSystem.SetYCoordinate(player2);
            var shotPanel = player2.GameBoard.Board[y].Row[x].CellStatus;

            if (player1.FiringBoard.Board[y].Row[x].CellStatus == Cell.CellState.Hit ||
                player1.FiringBoard.Board[y].Row[x].CellStatus == Cell.CellState.Miss)
            {
                Console.WriteLine("You already shot that panel!");
                MainSystem.WaitForUser();
                PlayTurn(player1, player2);
            }

            else if (shotPanel != Cell.CellState.Empty)
            {
                Console.WriteLine();
                player2.GameBoard.Board[y].Row[x].CellStatus = Cell.CellState.Hit;
                player1.FiringBoard.Board[y].Row[x].CellStatus = Cell.CellState.Hit;
                PrintBothBoards(player1);
                foreach (var ship in player2.Ships)
                {
                    if (shotPanel == ship.CellState) ship.Hits++;
                    if (ship.IsSunk && shotPanel == ship.CellState)
                    {
                        Console.WriteLine("You sunk a " + ship.Name + "! [Length: " + ship.Width + "]");
                        MainSystem.WaitForUser();
                        break;
                    }

                    if (shotPanel == ship.CellState)
                    {
                        Console.WriteLine("You hit a ship!");
                        MainSystem.WaitForUser();
                        break;
                    }
                }

                if (!player1.HasLost && !player2.HasLost) PlayTurn(player1, player2);
            }
            else
            {
                player1.FiringBoard.Board[y].Row[y].CellStatus = Cell.CellState.Miss;
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


        public string SaveGame(string command)
        {
            GameStorage.Save(Game);
            Console.WriteLine("Game saved!");

            MainSystem.WaitForUser();
            return ApplicationMenu.MainMenu.RunMenu();
        }


        public string LoadGame(string command)
        {
            if (context.Games.Count() == 0) // don't change it
            {
                Console.WriteLine("No saved games found!");
                MainSystem.WaitForUser();
                return ApplicationMenu.MainMenu.RunMenu();
            }

            bool notCorrect = true;
            int index = 0;
            while (notCorrect)
            {
                Console.WriteLine("Insert game Id you want to play: " + context.Games.First().GameId + "-" +
                                  context.Games.Last().GameId);
                var input = Console.ReadLine();

                try
                {
                    index = int.Parse(input);
                    notCorrect = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Must be number from" + context.Games.First().GameId + "-" +
                                      context.Games.Last().GameId);
                }
            }

            //Game lastGame = context.Games.Last();

            Console.WriteLine("Game Started");

            var game = GameStorage.Load(index);

            if (game.Winner != null)
            {
                Console.WriteLine();
                Console.WriteLine(game.Winner.Name + " already won the game.");
                MainSystem.WaitForUser();
                return ApplicationMenu.MainMenu.RunMenu();
            }

            if (game.Turn == game.PlayerTwo)
            {
                return StartShooting(game.PlayerTwo, game.PlayerOne, game);
            }

            return StartShooting(game.PlayerOne, game.PlayerTwo, game);
        }
    }
}