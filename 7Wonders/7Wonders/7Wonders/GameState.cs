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
        private bool onlySideA;
        private bool assign;

        public GameState()
        {
            players = new Dictionary<long, Player>();
            gameInProgress = false;
            onlySideA = false;
            assign = false;
        }

        public bool isGameInProgress() { return gameInProgress; }
        public bool getOnlySideA() { return onlySideA; }
        public bool getAssign() { return assign; }
        public void setOptions(bool _onlySideA, bool _assign) { onlySideA = _onlySideA; assign = _assign;} 

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

        public string lobbyToJson()
        {

            JObject jlobby =
                new JObject(
                    new JProperty("players",
                        new JArray(
                            from p in players.Values
                            select new JObject(p.toJObject()))),
                    new JProperty("onlySideA",onlySideA),
                    new JProperty("assign",assign));

            return jlobby.ToString();
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

        public void lobbyFromJson(string json)
        {
            JObject jlobby = JObject.Parse(json);
            players.Clear();
            foreach (JObject j in jlobby["players"])
            {
                players.Add((long)j["id"], new Player(j));
            }
            onlySideA = bool.Parse((string)jlobby["onlySideA"]);
            assign = bool.Parse((string)jlobby["assign"]);
        }

        public string optionsToJson()
        {
            JObject j = new JObject(
                new JProperty("onlySideA", onlySideA.ToString()),
                new JProperty("assign", assign.ToString()));
            return j.ToString();
        }

        public void optionsFromJson(string json)
        {
            JObject joptions = JObject.Parse(json);
            onlySideA = bool.Parse((string)joptions["onlySideA"]);
            assign = bool.Parse((string)joptions["assign"]);
        }
    }
}
