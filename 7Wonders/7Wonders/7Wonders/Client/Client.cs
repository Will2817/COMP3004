﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    public class Client
    {

        GameManager gameManager;
        EventHandlerServiceImpl eventHandlerService;
        MessageSerializerService messageSerializerService;
        NetServiceImpl netService;

        public Client()
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

        public long getId()
        {
            return netService.getID();
        }

        public Player getSelf()
        {
            if (gameManager.getGameState().getPlayers().ContainsKey(getId()))
                return gameManager.getGameState().getPlayers()[getId()];
            return null;
        }

        public void joinHost(bool host)
        {
            //Console.WriteLine("Start Test");
            int i = netService.joinHost(host);
            if (i == 0) gameManager.setConnected();
            //Console.WriteLine("Over here:" + i);
        }

        public void disconnect()
        {
            netService.disconnect();
        }

        public GameState getState(){
            gameManager.setUpdateChecked();
            return gameManager.getGameState();
        }

        public bool isUpdateAvailable()
        {
            return gameManager.isUpdateAvailable();
        }

        public bool isConnected()
        {
            return gameManager.isConnected();
        }

        public void setReady(bool ready)
        {
            gameManager.setReady(ready);
        }

        public void setOption(bool onlySideA, bool assign)
        {
            Console.WriteLine("Setting side in gamestate");
            gameManager.setOptions(onlySideA, assign);
        }
    }
}
