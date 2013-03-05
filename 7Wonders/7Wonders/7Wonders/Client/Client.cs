using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    public class Client
    {

        GameManager gameManager;
        EventHandlerServiceImpl eventHandlerService;
        MessageSerializerServiceImpl messageSerializerService;
        NetServiceImpl netService;

        public Client()
        {
            gameManager = new GameManager(new Player(0, System.Environment.MachineName));
            eventHandlerService = new EventHandlerServiceImpl();
            messageSerializerService = new MessageSerializerServiceImpl();
            netService = new NetServiceImpl();
            netService.setEventHandler(eventHandlerService);
            eventHandlerService.setGameManager(gameManager);
            gameManager.setNetService(netService);
            gameManager.setMessageSerializer(messageSerializerService);
            messageSerializerService.setNetService(netService);

        }

        public void joinHost()
        {
            Console.WriteLine("Start Test");
            int i = netService.joinHost(false);
            Console.WriteLine("Over here:" + i);
        }
    }
}
