using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TIC_TAC_TOE.APP.Abstract;

namespace TIC_TAC_TOE.APP.Logic
{
    internal class HumanPlayer : PlayerBase
    {
        public HumanPlayer(string name, int number) : base(name, number) { }

        //human player get move method implementation
        public override (int rowNum, int colNum, int value) GetGameMove(int[,] board, List<int> availableNumbers)
        {
            while (true)
            {
                Console.Write("Please enter your move in this format - row number, column number and the value ( Odd numbers only ) : ");

                var inputValue = Console.ReadLine();
                var seperatedValues = inputValue?.Split(',');

                // validate the input parameters
                if (seperatedValues?.Length == 3 && int.TryParse(seperatedValues[0], out int rowNum) &&
                    int.TryParse(seperatedValues[1], out int colNum) && int.TryParse(seperatedValues[2], out int value))
                {
                    return (rowNum - 1, colNum - 1, value);
                }

                Console.WriteLine("Invalid input, Please try again with a valid input!");
            }
        }
    }
}
