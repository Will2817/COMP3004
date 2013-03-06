using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    public class GameState
    {
        private Dictionary<long, Player> players;
        private bool gameInProgress;

        public GameState()
        {
            players = new Dictionary<long, Player>();
            gameInProgress = false;
        }

        public bool isGameInProgress() { return gameInProgress; }

        public void addPlayer(Player _player)
        {
            players.Add(_player.getID(), _player);
        }

        public void removePlayer(long _id)
        {
            players.Remove(_id);
        }

        public Dictionary<long, Player> getPlayers()
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

        public void playersFromJson(string json)
        {
            JObject jplayers = JObject.Parse(json);
            players.Clear();
            foreach (JObject j in jplayers["players"])
            {
                players.Add((long)j["id"], new Player(j));
            }
        }
    }
}
