using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIC_TAC_TOE.APP.Abstract;
using TIC_TAC_TOE.APP.Interfaces;

namespace TIC_TAC_TOE.APP
{
    public class Gameplay: IGamePlay
    {
        //interface for the IGamePlayService
        private readonly IGamePlayService _iGamePlayService;
        //saved game file path
        private readonly string saveFilePath = Config.SaveFilePath;

        #region Constructor
        public Gameplay(IGamePlayService iGamePlayService)
        {
            _iGamePlayService = iGamePlayService;
            saveFilePath = Config.SaveFilePath;
        }
        #endregion

        #region Start game method
        public void StartTheGame()
        {
            Console.WriteLine("**********  N11884347 - IFN584 Assignment 1 **********************************");

            Console.WriteLine("**********  Welcome to the Tic-Tac-Toe console app   *************************");

            Console.Write("Do you like to load an already saved game ? (Y/N) : ");
            bool loadGame = false;

            while (true)
            {
                var input = Console.ReadLine()?.Trim().ToUpper();
                loadGame = input == "Y" && File.Exists(saveFilePath);

                if (input == "Y")
                {
                    _iGamePlayService.StartGame(loadGame);
                }
                else if (input == "N")
                {
                    loadGame = input == "Y" && File.Exists(saveFilePath);
                    Console.WriteLine("Starting a new game :) ");
                    _iGamePlayService.StartGame(false);

                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter Y or N.");
                }
            }
        }
        #endregion
    }
}
