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
                    Player newPlayer = new Player((JObject)JObject.Parse(message)["player"]);
                    gameManager.addPlayer(newPlayer);
                    break;
                //other cases
                default:
                    break;
            }
        }

        public void handleDisconnect()
        {
            //stuff
        }
    }
}
