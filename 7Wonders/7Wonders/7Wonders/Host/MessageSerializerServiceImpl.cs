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

        public void notifyPlayerJoined(Player player)
        {
            string message = player.toJString();
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
