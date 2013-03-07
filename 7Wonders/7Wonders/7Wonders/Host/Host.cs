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
        private MessageSerializerService messageSerializerService;
        private NetServiceImpl netService;

        public Host()
        {
            gameManager = new GameManager();
            eventHandlerService = new EventHandlerServiceImpl();
            messageSerializerService = new MessageSerializerService();
            netService = new NetServiceImpl();
            netService.setEventHandler(eventHandlerService);
            eventHandlerService.setGameManager(gameManager);
            gameManager.setNetService(netService);
            gameManager.setMessageSerializer(messageSerializerService);
            messageSerializerService.setNetService(netService);
        }

        public void setOptions(bool _onlySideA, bool _assign)
        {
            gameManager.setOptions(_onlySideA, _assign);
        }

        public void addAIPlayer(string type)
        {
            gameManager.addAI(type);
        }

        public void shutdown()
        {
            netService.shutdown();
        }
    }
}
