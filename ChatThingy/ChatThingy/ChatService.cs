using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatThingy
{
    interface ChatService
    {
        void setClientService(ClientService clientService);
        void start();
        void displayIncoming(Message message);
    }
}
