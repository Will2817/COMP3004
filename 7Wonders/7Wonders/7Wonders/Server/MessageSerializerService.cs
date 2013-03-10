using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server
{
    class MessageSerializerService
    {
        private NetService netService;

        public void setNetService(NetService netService)
        {
            this.netService = netService;
        }

        public void notifyPlayerJoined(string players)
        {
            string message = players;
            netService.broadcastMessage(message, (int) ServerMessageType.PLAYER_JOINED);
        }

        public void notifyPlayerDropped(string players)
        {
            netService.broadcastMessage(players, (int)ServerMessageType.PLAYER_DROPPED);
        }

        public void notifyReadyChanged(string players)
        {
            netService.broadcastMessage(players, (int)ServerMessageType.READY_CHANGED);
        }

        public void notifyOptionsChanged(string options)
        {
            netService.broadcastMessage(options, (int)ServerMessageType.OPTIONS_CHANGED);
        }

        public void notifyWonderAssign(string json)
        {
            netService.broadcastMessage(json, (int)ServerMessageType.CONFIRM_WONDER);
        }

        public void broadcastChatMessage(String message, long senderID)
        {
            //broadcast message
        }
    }
}
