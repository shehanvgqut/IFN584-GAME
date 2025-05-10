using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIC_TAC_TOE.APP.Interfaces
{
    public interface IGamePlayService
    {
        void StartGame(bool loadFromFile = false);

    }
}
