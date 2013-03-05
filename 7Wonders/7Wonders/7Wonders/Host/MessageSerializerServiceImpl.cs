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
            Console.WriteLine("host broadcasting player: " + message);
            netService.broadcastMessage(message, (int) HostMessageType.PLAYER_JOINED);
        }

        public void notifyPlayerDropped(Player player)
        {
            //broadcast notification
        }

        public void broadcastChatMessage(String message, long senderID)
        {
            //broadcast message
        }
    }
}
