using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIC_TAC_TOE.APP.Abstract;

namespace TIC_TAC_TOE.APP.Logic
{
    internal class ComputerPlayer : PlayerBase
    {
        private readonly Random randomNum = new();

        public ComputerPlayer(string playerName, int playerNumber) : base(playerName, playerNumber)
        { 
        }

        //Abstract method implementation for the computer
        public override (int rowNum, int colNum, int value) GetGameMove(int[,] board, List<int> availableNumbers)
        {
            int boardSize = board.GetLength(0);
            //loop through the available numbers to find thee suitable winning value
            foreach (var number in availableNumbers)
            {
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (board[i, j] == 0)
                        {
                            board[i, j] = number;
                            //Check the winning move if have, and if available, assign the proper value to the computer
                            if (CheckWinningMove(board, boardSize))
                            {
                                board[i, j] = 0;
                                return (i, j, number);
                            }
                            board[i, j] = 0;
                        }
                    }
                }
            }
            //if the computer doesnt have a winning move just a random move will be chosen
            while (true)
            {

                int row = randomNum.Next(boardSize);
                int col = randomNum.Next(boardSize);
                if (board[row, col] == 0)
                {
                    int value = availableNumbers[randomNum.Next(availableNumbers.Count)];
                    return (row, col, value);
                }
            }
        }

        private bool CheckWinningMove(int[,] board, int boardSize)
        {
            int targetValue = boardSize * (boardSize * boardSize + 1) / 2;

            // Check rows and columns whether its equal to the targetValue to check the winning move
            for (int i = 0; i < boardSize; i++)
            {
                int rowSummation = 0, colSummation = 0;
                for (int j = 0; j < boardSize; j++)
                {
                    rowSummation = board[i, j] + rowSummation;
                    colSummation = board[j, i] + colSummation;
                }
                if (rowSummation == targetValue || colSummation == targetValue)
                    return true;
            }

            // Check diagonals summation whether its equal to the targetValue to check the winning move
            int mainDiagonalSum = 0, antiDiagonalSum = 0;
            for (int i = 0; i < boardSize; i++)
            {
                mainDiagonalSum = mainDiagonalSum + board[i, i];
                antiDiagonalSum = antiDiagonalSum + board[i, boardSize - 1 - i];
            }

            if (mainDiagonalSum == targetValue || antiDiagonalSum == targetValue)
                return true;

            return false;
        }
    }

}
