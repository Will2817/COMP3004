using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    public class Player
    {
        protected string name;
        protected long id;
        protected int seatNumber;
        protected Wonder board;
        protected List<string> hand;
        protected List<string> played;
        protected bool ready;
        protected ActionType action; // Players Action selected for the turn
        
        protected List<string> lastCardsPlayed; //Cards built on last turn
        protected List<ActionType> lastActions; //Actions taken on last turn

        // Trading values for raw resources and manufactured resources
        public int rcostEast { get; set; }
        public int rcostWest { get; set;}
        public int mcost { get; set;}

        protected Dictionary<Score, int> score; // Players Score - Victory Points, Army, etc...
        protected Dictionary<Resource, int> resources; // Resources - Clay, Ore, etc... that are also purchasable by other players
        protected Dictionary<CardColour, int> cardColour; // Used to keep count the number of structures

        // List of choices which are in lists
        List<List<Resource>> publicChoices;
        List<List<Resource>> privateChoices;

        // Default Player Constructor
        public Player(long _id, string _name)
        {
            id          = _id;
            name        = _name;
            seatNumber  = -1;
            board       = null;
            hand        = null;
            played      =  null;
            ready       = false;
            action      = 0;

            Initializer();
        }

        public Player(JObject j)
        {
            id = (long)j["id"];
            name = (string)j["name"];
            seatNumber = (int)j["seat"];
            ready = bool.Parse((string)j["ready"]);
            Initializer();
        }

        public void updatePlayer(JObject j)
        {
            name = (string)j["name"];
            seatNumber = (int)j["seat"];
            ready = bool.Parse((string)j["ready"]);
        }

        private void Initializer()
        {
            //Initializing Variables
            hand = new List<string>();
            played = new List<string>();
            lastCardsPlayed = new List<string>();
            lastActions = new List<ActionType>();
            rcostEast = 2;
            rcostWest = 2;
            mcost = 2;

            publicChoices = new List<List<Resource>>();
            privateChoices = new List<List<Resource>>();

            // Setting the Players Score Dictionary
            score = new Dictionary<Score, int>();
            foreach (Score s in Enum.GetValues(typeof(Score)))
                score.Add(s, 0);

            // Setting the  resource dictionary
            resources = new Dictionary<Resource, int>();
            foreach (Resource r in Enum.GetValues(typeof(Resource)))
                resources.Add(r, 0);

            // Card Colours
            cardColour = new Dictionary<CardColour, int>();
            foreach (CardColour c in Enum.GetValues(typeof(CardColour)))
                cardColour.Add(c, 0);
        }

        // ACCESSORS
        public string getName() { return name; }
        public long getID() { return id; }
        public int getSeat() { return seatNumber; }
        public Wonder getBoard() { return board; }
        public List<string> getHand() { return hand; }
        public List<string> getPlayed() { return played; }
        public bool getReady() { return ready; }
        public List<string> getLastCardsPlayed() { return lastCardsPlayed; }
        public List<ActionType> getLastActions() { return lastActions; }

        // Returns the dictionary object of Score & Resources 
        // along with List of choices and unpurchasable resources
        public Dictionary<Score, int> getScore() { return score; }
        public int getScoreType(Score s) { return score[s]; }
        public Dictionary<Resource, int> getResources() { return resources; }
        public List<List<Resource>> getPublicChoices() { return publicChoices; }
        public List<List<Resource>> getPrivateChoices() { return privateChoices; }
        public Dictionary<CardColour, int> getCardColours() { return cardColour; }

        // Returns a list of all resource choices available to the player,
        // both public (purchaseable) and private (unpurchaseable)
        public List<List<Resource>> getTotalChoices()
        {
            List<List<Resource>> total = new List<List<Resource>>();
            total.AddRange(publicChoices);
            total.AddRange(privateChoices);
            return total;
        }

        public int getCardColourCount(CardColour c)
        {
            return cardColour[c];
        }

        // Returns a boolean value if that specific card has been played yet or not
        public bool cardPlayed(string cardID) { return played.Contains(cardID); }

        // Get the Score number of a certain 's'
        public int getScoreNum(Score s)
        {
            return score[s];
        }

        // Get the players Resource number of a certain 'r'
        public int getResourceNum(Resource r)
        {
            return resources[r];
        }

        // MUTATORS
        public void setNameID(string n)         {   name = n;   }
        public void setSeat(int s)              {   seatNumber = s; }
        public void setHand(List<string> h)       {   hand = new List<string>(h);   }
        public void setPlayed(List<string> p)     {   played = new List<string>(p); }
        public void setReady(bool _ready)       {   ready = _ready; }
        public void setBoard(Wonder w)          {   board = w; }
        public void addPublicChoices(List<Resource> r) { publicChoices.Add(r); }
        public void addPrivateChoices(List<Resource> r) { privateChoices.Add(r); }
        public void setLastCardsPlayed(List<string> _cards) { lastCardsPlayed = _cards; }
        public void setLastActions(List<ActionType> _actions) { lastActions = _actions; }

        public void addPlayed(Card card)
        {
            played.Add(card.getImageId());
            cardColour[card.colour] += 1;
        }

        public string toJString()
        {
            JObject player =
                new JObject(
                    new JProperty("player", toJObject()));
            return player.ToString();
        }
        public JObject toJObject()
        {
            JObject player =
                new JObject(
                    new JProperty("name", name),
                    new JProperty("id", id),
                    new JProperty("seat", seatNumber),
                    new JProperty("ready", ready));
            return player;
        }

        // Sets the Score of a certain 's'
        public void setScoreNum(Score s, int x)
        {
            score[s] = x;
        }

        // Adds to the Score of a certain 's'
        public void addScore(Score s, int x)
        {
            score[s] += x;
        }

        // Sets the Resource of a certain 'r'
        public void setResourceNum(Resource r, int x)
        {
            resources[r] = x;
        }

        //Adds to the Resource of a certain 'r'
        public void addResource(Resource r, int x)
        {
            resources[r] += x;
        }

        public void addCardColour(CardColour c, int x)
        {
            cardColour[c] += x;
        }

        public JObject superJson()
        {
            JObject resource = new JObject();
            foreach (Resource r in Enum.GetValues(typeof(Resource)))
            {
                resource.Add(new JProperty(((int)r).ToString(), resources[r]));
            }

            JArray actions = new JArray();
            JArray cards = new JArray();
            foreach (string s in lastCardsPlayed)
            {
                cards.Add(s);
            }

            foreach (ActionType a in lastActions)
            {
                actions.Add(((int)a).ToString());
            }

            JObject j = new JObject(
                new JProperty("id", id),
                new JProperty("score",
                    new JObject(
                        new JProperty(((int)Score.ARMY).ToString(), score[Score.ARMY]),
                        new JProperty(((int)Score.CONFLICT).ToString(), score[Score.CONFLICT]),
                        new JProperty(((int)Score.TABLET).ToString(), score[Score.TABLET]),
                        new JProperty(((int)Score.COMPASS).ToString(), score[Score.COMPASS]),
                        new JProperty(((int)Score.GEAR).ToString(), score[Score.GEAR]),
                        new JProperty(((int)Score.VICTORY_BLUE).ToString(), score[Score.VICTORY_BLUE]))),
                new JProperty("resource", resource),
                new JProperty("actions", actions),
                new JProperty("cards", cards));
            return j;
        }

        public void superParse(JObject j)
        {
            foreach (JProperty p in ((JObject)j["score"]).Properties())
            {
                score[((Score)int.Parse(p.Name))] = (int)p.Value;
            }
            foreach (JProperty p in ((JObject)j["resource"]).Properties())
            {
                resources[((Resource)int.Parse(p.Name))] = (int)p.Value;
            }
            lastActions.Clear();
            foreach (string s in (JArray)j["actions"])
            {
                lastActions.Add((ActionType)int.Parse(s));
            }
            lastCardsPlayed.Clear();
            foreach (string s in (JArray)j["cards"])
            {
                lastCardsPlayed.Add(s);
                addPlayed(CardLibrary.getCard(s));
            }
        }
    }
}
