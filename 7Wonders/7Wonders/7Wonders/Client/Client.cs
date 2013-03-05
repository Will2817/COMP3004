using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    class Client
    {

        public Client()
        {
            GameManager gameManager = new GameManager(new Player(0, System.Environment.MachineName));
            EventHandlerServiceImpl eventHandlerService = new EventHandlerServiceImpl();
            MessageSerializerServiceImpl messageSerializerService = new MessageSerializerServiceImpl();
            NetServiceImpl netService = new NetServiceImpl();
            netService.setEventHandler(eventHandlerService);
            eventHandlerService.setGameManager(gameManager);
            gameManager.setNetService(netService);
            gameManager.setMessageSerializer(messageSerializerService);
            messageSerializerService.setNetService(netService);
        }
    }
}
