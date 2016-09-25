///////////////////////////////////////////////////////////
//  DungeonMaster.cs
//  Implementation of the Class DungeonMaster
//  Generated by Enterprise Architect
//  Created on:      28-Apr-2014 10:13:36 PM
//  Original author: Gsimmons
///////////////////////////////////////////////////////////


using System;
using Mazegame.Boundary;
using Mazegame.Entity;

namespace Mazegame.Control
{
    public class DungeonMaster
    {
        private IMazeClient gameClient;
        private IMazeData gameData;

        private GameContext gameContext;

        public DungeonMaster(IMazeData gameData, IMazeClient gameClient)
        {
            this.gameData = gameData;
            this.gameClient = gameClient;
        }

        public void PrintWelcome()
        {
            gameClient.PlayerMessage(gameData.GetWelcomeMessage());
        }

        public void SetupPlayer()
        {
            String playerName = gameClient.GetReply("What name do you choose to be known by?");

            gameContext = new GameContext(gameData.GetStartingLocation(), new Player(playerName));

        }

        public void RunGame()
        {
            PrintWelcome();
            SetupPlayer();

            gameClient.PlayerMessage("Let the story begins!");
            gameClient.PlayerMessage("Welcome " + gameContext.Player.Name + "!\n");

            gameClient.PlayerMessage("You find yourself looking at " + gameData.GetStartingLocation().Description);

            gameClient.PlayerMessage("You can use 'help' to see all available commands");
            gameClient.PlayerMessage("You can use 'help <command>' to see command description");
        }
    } //end DungeonMaster
} //end namespace Control