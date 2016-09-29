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
using Mazegame.Commands;
using System.Linq;

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

            var player = new Player(playerName, 10, 20, 10) { Location = gameData.GetStartingLocation(), Gold = 100 };

            gameContext = new GameContext(player);
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

            var dispatcher = new CommandDispatcher();

            while (true)
            {
                var userInput = gameClient.GetReply(">");
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    gameClient.PlayerMessage("Please enter some command");
                    gameClient.PlayerMessage("You can use 'help' to see all available commands");
                    gameClient.PlayerMessage("You can use 'help <command>' to see command description");
                    continue;
                }

                var userInputItems = userInput.Split(new[] { ' ' },  StringSplitOptions.RemoveEmptyEntries);
                if (userInputItems.Length > 2)
                {
                    gameClient.PlayerMessage("You have entered too many words, you can enter command or command and its argument");
                    continue;
                }

                var commandName = userInputItems[0];
                var arguments = userInputItems.Skip(1);

                if (commandName == "quit") break;

                gameClient.PlayerMessage(dispatcher.Execute(gameContext, commandName, arguments.FirstOrDefault()));

                if (gameContext.Player.LifePoints <= 0) break;
            }

            gameClient.PlayerMessage("Game over!");
            gameClient.PlayerMessage("Good bye!");
        }
    } //end DungeonMaster
} //end namespace Control