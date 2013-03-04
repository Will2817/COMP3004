using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    class NetEventHandlerServiceImpl : NetEventHandlerService
    {
        private GameManager gameManager;

        public NetEventHandlerServiceImpl(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void handleNewClient(long clientID)
        {
            Player newPlayer = new Player(clientID);
            gameManager.addPlayer(newPlayer);
        }

        public void handleClientDrop(long clientID)
        {
            //stuff
        }

        public void handleMessage(String message, int type, long clientID)
        {
            switch ((MessageTypes)type)
            {
                case MessageTypes.BOARD_SELECTION:
                    //stuff
                    break;
                case MessageTypes.TURN_ACTION:
                    //stuff
                    break;
                case MessageTypes.CHAT_MESSAGE:
                    //stuff
                    break;
                default:
                    break;
            }
        }

    }
}
