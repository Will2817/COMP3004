using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    class EventHandlerServiceImpl : EventHandlerService
    {
        private GameManager gameManager;

        public EventHandlerServiceImpl(GameManager gameManager)
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
            //stuff
        }

        public void handleMessage(String message, int type, long clientID)
        {
            switch ((ClientMessageType)type)
            {
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
