using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    public class Host
    {
        private GameManager gameManager;
        private EventHandlerServiceImpl eventHandlerService;
        private MessageSerializerServiceImpl messageSerializerService;
        private NetServiceImpl netService;

        public Host()
        {
            gameManager = new GameManager();
            eventHandlerService = new EventHandlerServiceImpl();
            messageSerializerService = new MessageSerializerServiceImpl();
            netService = new NetServiceImpl();
            netService.setEventHandler(eventHandlerService);
            eventHandlerService.setGameManager(gameManager);
            gameManager.setNetService(netService);
            gameManager.setMessageSerializer(messageSerializerService);
            messageSerializerService.setNetService(netService);
        }

        public void shutdown()
        {
            netService.shutdown();
        }
    }
}
