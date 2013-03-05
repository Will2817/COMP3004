using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

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

        public string playersToJson()
        {
            JObject jplayers =
                new JObject(
                    new JProperty("players",
                        new JArray(
                            from p in players.Values
                            select new JObject(p.toJObject()))));

            return jplayers.ToString();
        }
    }
}
