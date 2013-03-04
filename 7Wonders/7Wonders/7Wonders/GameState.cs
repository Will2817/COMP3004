using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    public class GameState
    {
        private Dictionary<String, Player> players;

        public GameState()
        {
            players = new Dictionary<string, Player>();
        }

        public void addPlayer(Player _player)
        {
            players.Add(_player.getName(), _player);
        }

        public void removePlayer(string _name)
        {
            players.Remove(_name);
        }

        public Dictionary<String, Player> getPlayers()
        {
            return players;
        }
    }
}
