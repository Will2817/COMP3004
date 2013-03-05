using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
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

        public void addPlayer(string players)
        {
            gameState.playersFromJson(players);
        }

        public GameState getGameState()
        {
            return gameState;
        }
    }
}
