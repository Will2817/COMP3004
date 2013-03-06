﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Host
{
    class GameManager
    {
        GameState gameState;
        MessageSerializerService messageSerializer;
        NetService netService;

        public GameManager()
        {
            gameState = new GameState();
        }

        public void setMessageSerializer(MessageSerializerService messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public void setNetService(NetService netService)
        {
            this.netService = netService;
        }

        public void addPlayer(Player _player)
        {
            gameState.addPlayer(_player);
            if (gameState.getPlayers().Count() == Game1.MAXPLAYER)
            {
                netService.blockConnections();
            }
            messageSerializer.notifyPlayerJoined(gameState.playersToJson());
            Console.WriteLine("Start Players:");
            foreach (Player p in gameState.getPlayers().Values)
            {
                Console.WriteLine(p.getName() + ", " + p.getID());
            }
            Console.WriteLine("End Players...");
        }

        public void removePlayer(long id)
        {
            if (!gameState.isGameInProgress())
            {
                gameState.removePlayer(id);
                messageSerializer.notifyPlayerDropped(gameState.playersToJson());
            }
        }

        public void setPlayerReady(long id, bool ready)
        {
            gameState.getPlayers()[id].setReady(ready);
            messageSerializer.notifyReadyChanged(gameState.playersToJson());
        }

    }
}
