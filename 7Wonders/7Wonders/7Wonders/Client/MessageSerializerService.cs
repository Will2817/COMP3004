﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    class MessageSerializerService
    {
        private NetService netService;

        public void setNetService(NetService netService)
        {
            this.netService = netService;
        }

        public void notifyReadyChanged(bool ready)
        {
            netService.sendMessage(ready.ToString(), (int)ClientMessageType.READY_CHANGED);
        }

        public void sendActions(string actions)
        {
            netService.sendMessage(actions, (int)ClientMessageType.TURN_ACTION);
        }

        public void sendChatMessage(String message)
        {
            //stuff
        }
    }
}
