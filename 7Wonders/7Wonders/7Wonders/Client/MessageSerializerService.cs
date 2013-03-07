using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

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

        public void notifyOptionChange(bool onlySideA, bool assign)
        {
            JObject j = new JObject(
                                    new JProperty("onlySideA", onlySideA.ToString()),
                                    new JProperty("assign", assign.ToString()));
            netService.sendMessage(j.ToString(), (int)ClientMessageType.OPTIONS_CHANGED);
        }

        public void sendChatMessage(String message)
        {
            //stuff
        }
    }
}
