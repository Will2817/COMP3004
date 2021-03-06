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

        public void registar(Observer o) { 
            gameManager.registar(o); 
        } 

        public void joinHost(bool host)
        {
            int i = netService.joinHost(host);
            if (i == 0) gameManager.setConnected();
        }

        public void disconnect()
        {
            netService.disconnect();
        }

        public GameState getState(){
            return gameManager.getGameState();
        }

        
        public bool isConnected()
        {
            return gameManager.isConnected();
        }

        public void setReady(bool ready)
        {
            gameManager.setReady(ready);
        }

        public int constructCost(string cardID)
        {
            return gameManager.constructCost(cardID);
        }

        public void playCard(Dictionary<string, ActionType> actions, int westGold, int eastGold)
        {
            gameManager.selectActions(actions, westGold, eastGold);
        }

        public Player westPlayer(Player p)
        {
            return gameManager.getWestNeighbour(p);
        }

        public Player eastPlayer(Player p)
        {
            return gameManager.getEastNeighbour(p);
        }
    }
}
