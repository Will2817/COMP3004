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

        public void joinHost(bool host)
        {
            //Console.WriteLine("Start Test");
            int i = netService.joinHost(host);
            //Console.WriteLine("Over here:" + i);
        }

        public GameState getState(){
            return gameManager.getGameState();
        }
    }
}
