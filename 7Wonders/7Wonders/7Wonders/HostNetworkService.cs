using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Networking
{
    interface HostNetworkService
    {
        public int bootPlayer(String playerIP);//represent player as IP or other?
        public int blockConnections();
        public int allowConnections();
        public int broadcastGameState(String gameState);//broadcast as game state, or action list?
        public int sendHand(String hand, String playerIP);//hand is a string representation of a JArray; player as IP or other?
        public int sendWonder(String wonder, String playerIP);//needs to be separate from hand/game state?
        public int shutdown();
    }
}
