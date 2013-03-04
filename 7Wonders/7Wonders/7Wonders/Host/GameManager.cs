using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Host
{
    class GameManager
    {
        GameState gameState;

        public GameManager()
        {
            gameState = new GameState();
        }

        public void addPlayer(Player _player)
        {
            gameState.addPlayer(_player);
        }

    }
}
