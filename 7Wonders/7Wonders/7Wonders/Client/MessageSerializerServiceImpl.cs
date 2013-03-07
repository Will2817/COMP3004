using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    class MessageSerializerServiceImpl : MessageSerializerService
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

        public void notifyOnlySideA(bool onlyside)
        {
            netService.sendMessage(onlyside.ToString(), (int)ClientMessageType.SIDE_CHANGED);
        }

        public void sendChatMessage(String message)
        {
            //stuff
        }
    }
}
