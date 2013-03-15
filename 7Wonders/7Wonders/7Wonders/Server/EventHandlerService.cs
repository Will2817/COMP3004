using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server
{
    interface EventHandlerService
    {
        void handleNewClient(long clientID, string name);
        void handleClientDrop(long clientID);
        void handleMessage(String message, int type, long clientID);
        void handleAction(String action, int turn_action);
    }
}
