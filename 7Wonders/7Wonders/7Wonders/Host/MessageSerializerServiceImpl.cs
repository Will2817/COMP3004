using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    class MessageSerializerServiceImpl// : MessageSerializerService
    {
        private NetService netService;

        public void notifyPlayerJoined(Player player)
        {
            //broadcast notification
        }

        public void notifyPlayerDropped(Player player)
        {
            //broadcast notification
        }

        public void broadcastChatMessage(String message, HostMessageType type, long senderID)
        {
            //broadcast message
        }
    }
}
