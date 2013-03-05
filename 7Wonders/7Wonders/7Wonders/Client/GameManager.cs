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

        public GameManager(Player thisPlayer)
        {
            gameState = new GameState();
            gameState.addPlayer(thisPlayer);
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
        }

        public GameState getGameState()
        {
            return gameState;
        }
    }
}
