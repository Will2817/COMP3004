using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Server
{
    class EventHandlerServiceImpl : EventHandlerService
    {
        private GameManager gameManager;

        public void setGameManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void handleNewClient(long clientID, string name)
        {
            Player newPlayer = new Player(clientID, name);
            gameManager.addPlayer(newPlayer);
        }

        public void handleClientDrop(long clientID)
        {
            gameManager.playerDropped(clientID);
        }

        public void handleMessage(String message, int type, long clientID)
        {
            switch ((ClientMessageType)type)
            {
                case ClientMessageType.READY_CHANGED:
                    Console.WriteLine("{0}: READY_CHANGED", clientID);
                    gameManager.setPlayerReady(clientID, Boolean.Parse(message));
                    break;
                case ClientMessageType.BOARD_SELECTION:
                    //stuff
                    Console.WriteLine("HandleMessage: BOARD_SELECTION");
                    break;
                case ClientMessageType.TURN_ACTION:
                    //stuff
                    Console.WriteLine("HandleMessage: TURN_ACTION");
                    Dictionary<string, ActionType> actions = new Dictionary<string, ActionType>();
                    //gameManager.handleActions(clientID, actions);
                    gameManager.handleActions(clientID, parseActions(message));
                    break;
                case ClientMessageType.CHAT_MESSAGE:
                    //stuff
                    break;
                default:
                    break;
            }
        }

        public Dictionary<string, ActionType> parseActions(string message)
        {
            Dictionary<string, ActionType> actions = new Dictionary<string, ActionType>();
            JArray actionArray = (JArray)JObject.Parse(message)["actions"];
            foreach (JObject obj in actionArray)
                actions.Add((string )obj["card"], (ActionType) (int) obj["action"]);
            return actions;
        }

    }
}
