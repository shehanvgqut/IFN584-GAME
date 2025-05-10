using System;
using TIC_TAC_TOE.APP;
using TIC_TAC_TOE.APP.Interfaces;
using TIC_TAC_TOE.APP.Logic;

// Create the GamePlayService class and assign it to the respective interface - to inject in the below line
IGamePlayService gamePlayService = new GamePlayService();

//Inject gamePlayService into Gameplay
Gameplay gameplay = new Gameplay(gamePlayService);

// Start to play the game
gameplay.StartTheGame();
