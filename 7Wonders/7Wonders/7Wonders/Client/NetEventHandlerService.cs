using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    interface NetEventHandlerService
    {
        void handleMessage(String message, int type);
        void handleDisconnect();
    }
}
