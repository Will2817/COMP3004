using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server
{
    public interface NetService
    {
        void bootClient(long clientID);//represent client as IP or other?
        void blockConnections();//sets server to block incoming connections
        void allowConnections();//sets server to resume allowing incoming connections
        void broadcastMessage(String message, int type);//broadcast a message to all clients
        void sendMessage(String message, int type, long clientID);//sends a private message to a single client
        void shutdown();//shuts down the server
    }
}
