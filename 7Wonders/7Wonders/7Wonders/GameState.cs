using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    // Keeps the Game State of the current game in progress
    public class GameState
    {
        private Dictionary<long, Player> players;
        private Dictionary<string, Wonder> wonders;
        private bool gameInProgress;
        private bool onlySideA;
        private bool assign;
        private int age;
        private int turn;
        private List<string> discards;

        public GameState()
        {
            players = new Dictionary<long, Player>();
            gameInProgress = false;
            onlySideA = false;
            assign = false;
            JObject wondersJson = JObject.Parse(File.ReadAllText("Content/Json/wonderlist.json"));
            wonders = new Dictionary<string, Wonder>();
            age = 1;
            turn = 1;
            foreach (JObject j in (JArray)wondersJson["wonders"])
            {
                wonders.Add((string)j["name"], new Wonder(j));
            }
            discards = new List<string>();
        }

        public int getAge() { return age; }
        public int getTurn() { return turn; }
        public bool isGameInProgress() { return gameInProgress; }
        public bool getOnlySideA() { return onlySideA; }
        public bool getAssign() { return assign; }
        public void setOptions(bool _onlySideA, bool _assign) { onlySideA = _onlySideA; assign = _assign;}
        public void incrementAge() { age++; }
        public void incrementTurn() { turn++; }
        public void resetTurn() { turn = 1; }

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

        public Dictionary<string, Wonder> getWonders()
        {
            return wonders;
        }

        public List<string> getDiscards()
        {
            return discards;
        }

        public void addDiscard(string cardID)
        {
            discards.Add(cardID);
        }

        public void clearDiscard()
        {
            discards.Clear();
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

 /*       public string wondersToJson()
        {
            JObject jwonders =
                new JObject(
                    new JProperty("wonders",
                        new JArray(
                            from w in wonders.Values
                            select new JObject(w.toJObject()))));

            return jwonders.ToString();
        }*/

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
            List<long> temp = new List<long>();
            foreach (JObject j in jplayers["players"])
            {
                temp.Add((long)j["id"]);
                if (players.ContainsKey((long)j["id"])) players[(long)j["id"]].updatePlayer(j);
                else players.Add((long)j["id"], new Player(j));
            }
            players =  players.Where(x => temp.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
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

        public string wonderAssignToJson()
        {
            JArray j = new JArray();
            foreach (Player p in players.Values)
            {
                j.Add(new JObject(
                    new JProperty("wonder", p.getBoard().getName()),
                    new JProperty("player", p.getID()),
                    new JProperty("side", p.getBoard().getSideName())));
            }
            return j.ToString();
        }

        public void assignWonders(string json)
        {
            JArray o = JArray.Parse(json);
            foreach (JObject w in o)
            {
                Console.WriteLine(wonders[(string)w["wonder"]].getName());
                players[(long) w["player"]].setBoard(wonders[(string)w["wonder"]]);
                if ((string)w["side"] == "A") players[(long)w["player"]].getBoard().setSideA();
                else players[(long)w["player"]].getBoard().setSideB();
            }
            gameInProgress = true;
        }

        public string handToJson(long id)
        {
            JArray j = new JArray();
            foreach (string card in players[id].getHand())
            {
                j.Add(card);
            }
            return j.ToString();
        }

        public void setHand(long id, string message)
        {
            List<string> hand = new List<string>();
            JArray j = JArray.Parse(message);
            foreach (JToken card in j)
                hand.Add((string) card);
            players[id].setHand(hand);
        }

        public string superJson()
        {
            JArray jPlayers = new JArray();
            foreach (Player p in players.Values)
            {
                jPlayers.Add(p.superJson());
            }
            JArray jDiscards = new JArray();
            foreach (string s in discards)
            {
                jDiscards.Add(new JValue(s));
            }
            JObject super = new JObject(new JProperty("players", jPlayers),
                                        new JProperty("discards", jDiscards),
                                        new JProperty("age", age),
                                        new JProperty("turn", turn),
                                        new JProperty("inprogress", gameInProgress));
            return super.ToString();
        }

        public void superParse(string json)
        {
            discards.Clear();
            JObject obj = JObject.Parse(json);

            JArray ja = (JArray)obj["players"];
            foreach (JObject jo in ja)
            {
                players[(long)jo["id"]].superParse(jo);
            }

            JArray jDiscards = (JArray)obj["discards"];
            foreach (JValue jo in jDiscards)
            {
                discards.Add((string)jo);
            }

            age = (int)obj["age"];
            turn = (int)obj["turn"];
            gameInProgress = (bool)obj["inprogress"];
        }

        public void setInProgress(bool _progress)
        {
            gameInProgress = _progress;
        }
    }
}
