using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    interface MessageSerializerService
    {
        void notifyReadyChanged(bool ready);
        void notifyOnlySideA(bool onlyside);
        void sendChatMessage(String message);
    }
}
