using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Host
{
    class GameManager
    {
        private const int MAX_PLAYERS = 7;
        GameState gameState;
        MessageSerializerService messageSerializer;
        NetService netService;

        public GameManager(MessageSerializerService messageSerializer, NetService netService)
        {
            gameState = new GameState();
            this.messageSerializer = messageSerializer;
            this.netService = netService;
        }

        public void addPlayer(Player _player)
        {
            gameState.addPlayer(_player);
            if (gameState.getPlayers().Count() == MAX_PLAYERS)
            {
                netService.blockConnections();
            }
            messageSerializer.notifyPlayerJoined(_player);
        }

    }
}
