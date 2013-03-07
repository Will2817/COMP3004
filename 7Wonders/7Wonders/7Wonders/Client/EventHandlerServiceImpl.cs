using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Client
{
    class EventHandlerServiceImpl: EventHandlerService
    {
        private GameManager gameManager;

        public void setGameManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void handleMessage(String message, int type)
        {
            switch ((HostMessageType)type)
            {
                case HostMessageType.PLAYER_JOINED:
                    gameManager.updatePlayers(message);
                    break;
                case HostMessageType.PLAYER_DROPPED:
                    gameManager.updatePlayers(message);
                    break;
                case HostMessageType.READY_CHANGED:
                    gameManager.updatePlayers(message);
                    break;
                case HostMessageType.SIDE_CHANGED:
                    gameManager.updateSide(message);
                    break;
                default:
                    break;
            }
        }

        public void handleDisconnect()
        {
            gameManager.disconnected();
        }
    }
}
