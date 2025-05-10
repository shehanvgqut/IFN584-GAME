using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIC_TAC_TOE.APP
{
    public static class Config
    {
        public static readonly string SaveFilePath = Path.Combine(AppContext.BaseDirectory, "savegame.txt");
    }
}
