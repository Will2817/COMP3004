using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    interface MessageSerializerService
    {
        void sendChatMessage(String message);
    }
}
