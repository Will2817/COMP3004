using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Networking
{
    interface ClientNetworkService
    {
        public int joinHost(String hostIP);
        public int disconnect();
        public int sendMove(String move);//how to represent move?
        public int sendMessage(String message);
    }
}
