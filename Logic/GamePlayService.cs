using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIC_TAC_TOE.APP.Abstract;
using TIC_TAC_TOE.APP.Interfaces;
using static TIC_TAC_TOE.APP.Gameplay;

namespace TIC_TAC_TOE.APP.Logic
{
    public class GamePlayService : IGamePlayService
    {
        #region Member variables

        // n value ex: (3 x 3)
        private int boardSize;

        //Multi dimensional array to hold a 2D game board
        private int[,] gameBoard;

        // Arrays to hold the available numbers for both players
        private List<int> availableOddNums;
        private List<int> availableEvenNums;

        private int currentPlayer;

        // abstract class references to the base class (to seperate player 1 and player 2)
        private PlayerBase playerOne;
        private PlayerBase playerTwo;
        #endregion

        #region Main methods
        public void StartGame(bool filePath = false)
        {
            if (filePath && File.Exists(Config.SaveFilePath))
            {
                LoadGameToContinue();

                GameMainLogicLoop();

                return;
            }

            Console.Write("Please enter the desired board size (n) : ");
            boardSize = int.Parse(Console.ReadLine());
            gameBoard = new int[boardSize, boardSize];


            availableEvenNums = new List<int>();
            availableOddNums = new List<int>();

            for (int i = 1; i <= boardSize * boardSize; i++)
            {
                if (i % 2 == 0)
                    availableEvenNums.Add(i);
                else
                    availableOddNums.Add(i);
            }

            Console.Write("Choose mode (1 = Human vs Human, 2 = Human vs Computer): ");
            string mode = Console.ReadLine();

            playerOne = new HumanPlayer("Player number 1", 1);
            playerTwo = mode == "1" ? new HumanPlayer("Player number 2", 2) : new ComputerPlayer("Computer", 2);


            currentPlayer = 1;

            GameMainLogicLoop();

        }
        private void GameMainLogicLoop()
        {
            while (true)
            {
                
                PrintGameBoardInConsole();

                var current = currentPlayer == 1 ? playerOne : playerTwo;
                var validList = currentPlayer == 1 ? availableOddNums : availableEvenNums;

                if (current is HumanPlayer)
                {
                    Console.Write("Enter your move in this format - (row number, column number and the value (or else you can - save/ help/ exit) : ");
                   
                    var input = Console.ReadLine()?.Trim().ToLower();
                    
                    if (input == "help")
                    {
                        ShowHelpOptionToTheUser();
                        continue;
                    }
                    if (input == "exit")
                    {
                        Console.WriteLine("you wll be exited from this game!");
                        Environment.Exit(0);
                    }
                    if (input == "save")
                    {
                        SaveTheGame();
                        Console.WriteLine("Game saved for later use.");
                        continue;
                    }
                   

                    var parts = input?.Split(',');

                    //Seperate the input string and validate the inputs
                    if (parts?.Length == 3 &&
                        int.TryParse(parts[0], out int row) && int.TryParse(parts[1], out int col) &&
                        int.TryParse(parts[2], out int value))
                    {
                        //setting the formats to suit the array (indexes start from 0...n)
                        row = row - 1;
                        col = col - 1;

                        if (!ExecuteThePlayerMove(row, col, value, validList))
                        {
                            Console.WriteLine("Invalid move! Please try again with a different move.");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input format, please use only numeric values thats valid in the game");
                        continue;
                    }
                }
                else
                {
                    var (row, col, value) = current.GetGameMove(gameBoard, validList);
                    ExecuteThePlayerMove(row, col, value, validList);
                }

                if (CheckWinningMove())
                {
                    // if CheckWinningMove == true then current player has won.
                    PrintGameBoardInConsole();
                    Console.WriteLine($"{current.PlayerName} wins the game :D ");


                    //Start a new game if the user wants
                    Console.WriteLine("Do you want to start a new game? (Y/N) : ");
                    var input = Console.ReadLine()?.Trim().ToLower();
                    if (input == "y")
                    {
                        Console.WriteLine("Starting a new game :) ");
                        StartGame(false);
                    }
                    Environment.Exit(0);
                }
                if (IsTheGameATie())
                {
                    PrintGameBoardInConsole();
                    Console.WriteLine("Congratulations, it's a tie :D ");


                    //Start a new game if the user wants
                    Console.WriteLine("Do you want to start a new game? (Y/N) : ");
                    var input = Console.ReadLine()?.Trim().ToLower();
                    if (input == "y")
                    {
                        Console.WriteLine("Starting a new game :) ");
                        StartGame(false);
                    }
                    Environment.Exit(0);
                }

                currentPlayer = 3 - currentPlayer;
            }
        }

        #endregion

        private bool ExecuteThePlayerMove(int row, int col, int value, List<int> availableValues)
        {
            //validate whether the inputs are in the boundry of the game
            if (gameBoard[row, col] != 0 || 
                row < 0 || 
                col < 0 || 
                col >= boardSize || 
                !availableValues.Contains(value) || 
                row >= boardSize)
            {
                return false;
            }
            gameBoard[row, col] = value;
            //remove the inserted value from the available values list
            availableValues.Remove(value);
            return true;
        }

        private bool CheckWinningMove()
        {
            int targetValue = boardSize * (boardSize * boardSize + 1) / 2;

            // Check rows and columns whether its equal to the targetValue to check the winning move
            for (int i = 0; i < boardSize; i++)
            {
                int rowSummation = 0;
                int colSummation = 0;

                for (int j = 0; j < boardSize; j++)
                {
                    rowSummation = gameBoard[i, j] + rowSummation;
                    colSummation = gameBoard[j, i] + colSummation;
                }
                if (rowSummation == targetValue || colSummation == targetValue)
                    return true;
            }

            // Check diagonals summation whether its equal to the targetValue to check the winning move
            int mainDiagonalSum = 0, antiDiagonalSum = 0;
            for (int i = 0; i < boardSize; i++)
            {
                mainDiagonalSum = mainDiagonalSum + gameBoard[i, i];
                antiDiagonalSum = antiDiagonalSum + gameBoard[i, boardSize - 1 - i];
            }

            if (mainDiagonalSum == targetValue || antiDiagonalSum == targetValue)
                return true;

            return false;
        }

        private bool IsTheGameATie()
        {
            foreach (var cell in gameBoard)
            {
                if (cell == 0) return false;
            }
            return true;
        }

        private void PrintGameBoardInConsole()
        {
            //displaye the current game board/ print the values entered by both players
            Console.WriteLine("\nCurrent Board:");
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Console.Write(gameBoard[i, j] == 0 ? " . " : $"{gameBoard[i, j],2} ");
                }
                Console.WriteLine();
            }
        }

        #region Save/ Help and Load game methods
        private void SaveTheGame()
        {
            // write the existing informaton/ current game status to a .txt file using I/O
            using var writer = new StreamWriter(Config.SaveFilePath);
            writer.WriteLine(boardSize);
            //Save the cur
            writer.WriteLine(currentPlayer);
            writer.WriteLine(string.Join(",", gameBoard.Cast<int>()));
            writer.WriteLine(string.Join(",", availableOddNums));
            writer.WriteLine(string.Join(",", availableEvenNums));
            // whether the player 2 is the computer or a human playing
            writer.WriteLine(playerTwo is HumanPlayer ? "H" : "C");
        }
        private void LoadGameToContinue()
        {
            //Read the txt file and restore the values accordingly 
            using var reader = new StreamReader(Config.SaveFilePath);
            boardSize = int.Parse(reader.ReadLine());
            gameBoard = new int[boardSize, boardSize];
            currentPlayer = int.Parse(reader.ReadLine());

            //split the values and assign it to the gameBoard to display the game board
            var flatBoard = reader.ReadLine().Split(',').Select(int.Parse).ToArray();

            int index = 0;
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    gameBoard[row, col] = flatBoard[index++];
                }
            }


            availableOddNums = reader.ReadLine().Split(',').Select(int.Parse).ToList();
            availableEvenNums = reader.ReadLine().Split(',').Select(int.Parse).ToList();

            playerOne = new HumanPlayer("Player 1", 1);
            playerTwo = reader.ReadLine() == "H" ? new HumanPlayer("Player 2", 2) : new ComputerPlayer("Computer", 2);
        }
        public void ShowHelpOptionToTheUser()
        {
            Console.WriteLine("\n********** Help options menu : Please select one ************");
            Console.WriteLine("  Enter your move in the format of : row number, column number and the value (ex: 1,3,3)");
            Console.WriteLine("Available numbers: " + string.Join(", ", availableOddNums));
            Console.WriteLine("  Rows and columns start from 1 (and could be any odd number upto 9)");
            Console.WriteLine("  Value must be from your available numbers (odd or even)");
            Console.WriteLine("  Game options available in this game : ");
            Console.WriteLine("  Save option - save the current progress to a .txt file.");
            Console.WriteLine("  Help  - display the help option.");
            Console.WriteLine("  Exit  - Exit the current game.");
        }
        #endregion
    }


}
