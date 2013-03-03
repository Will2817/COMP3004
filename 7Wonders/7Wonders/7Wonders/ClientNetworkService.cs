using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Networking
{
    public interface ClientNetworkService
    {
        int joinHost(String hostIP);
        int disconnect();
        int sendMove(String move);//how to represent move?
        int sendMessage(String message);
    }
}
