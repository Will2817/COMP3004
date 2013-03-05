using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    interface EventHandlerService
    {
        void handleNewClient(long clientID);
        void handleClientDrop(long clientID);
        void handleMessage(String message, int type, long clientID);
    }
}
