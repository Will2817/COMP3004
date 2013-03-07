using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Host
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
            gameManager.removePlayer(clientID);
        }

        public void handleMessage(String message, int type, long clientID)
        {
            switch ((ClientMessageType)type)
            {
                case ClientMessageType.READY_CHANGED:
                    gameManager.setPlayerReady(clientID, Boolean.Parse(message));
                    break;
                case ClientMessageType.OPTIONS_CHANGED:
                    JObject j = JObject.Parse(message);
                    gameManager.setOptions((bool)j["onlySideA"], (bool)j["assign"]);
                    break;
                case ClientMessageType.BOARD_SELECTION:
                    //stuff
                    break;
                case ClientMessageType.TURN_ACTION:
                    //stuff
                    break;
                case ClientMessageType.CHAT_MESSAGE:
                    //stuff
                    break;
                default:
                    break;
            }
        }

    }
}
