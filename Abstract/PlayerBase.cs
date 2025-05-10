using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIC_TAC_TOE.APP.Abstract
{
    public abstract class PlayerBase
    {
        public string PlayerName { get; set; }
        public int PlayerNumber { get; set; }

        protected PlayerBase(string playerName, int playerNumber)
        {
            PlayerName = playerName;
            PlayerNumber = playerNumber;
        }

        public abstract (int rowNum, int colNum, int value) GetGameMove(int[,] board, List<int> availableNumbers);

    }
}
