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
            switch ((ServerMessageType)type)
            {
                case ServerMessageType.PLAYER_JOINED:
                    gameManager.updateLobby(message);
                    break;
                case ServerMessageType.PLAYER_DROPPED:
                    gameManager.updatePlayers(message);
                    break;
                case ServerMessageType.READY_CHANGED:
                    gameManager.updatePlayers(message);
                    break;
                case ServerMessageType.OPTIONS_CHANGED:
                    gameManager.updateOptions(message);
                    break;
                case ServerMessageType.CONFIRM_WONDER:
                    gameManager.assignWonders(message);
                    break;
                case ServerMessageType.HAND:
                    gameManager.assignHand(message);
                    break;
                case ServerMessageType.TURN_RESULT:
                    gameManager.superUpdate(message);
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
