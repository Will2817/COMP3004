using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    class MessageSerializerServiceImpl : MessageSerializerService
    {
        private NetService netService;

        public void setNetService(NetService netService)
        {
            this.netService = netService;
        }

        public void notifyPlayerJoined(string players)
        {
            string message = players;
            netService.broadcastMessage(message, (int) HostMessageType.PLAYER_JOINED);
        }

        public void notifyPlayerDropped(string players)
        {
            netService.broadcastMessage(players, (int)HostMessageType.PLAYER_DROPPED);
        }

        public void broadcastChatMessage(String message, long senderID)
        {
            //broadcast message
        }
    }
}
