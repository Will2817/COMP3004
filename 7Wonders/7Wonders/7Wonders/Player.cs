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
        protected List<Card> hand;
        protected List<Card> played;
        protected bool ready;
        protected ActionType action; // Players Action selected for the turn
        
        protected List<string> lastCardsPlayed;
        protected List<ActionType> lastActions;

        // Trading values for raw resources and manufactured resources
        public int rcostEast { get; set; }
        public int rcostWest { get; set;}
        public int mcost { get; set;}

        protected Dictionary<Score, int> score; // Players Score - Victory Points, Army, etc...
        protected Dictionary<Resource, int> resources; // Resources - Clay, Ore, etc... that are also purchasable by other players
        protected Dictionary<Resource, int> choiceResources; // Used for players selecting a temp resource to use
        protected Dictionary<CardColour, int> cardColour; // Used to keep count the number of structures

        // List of choices which are in lists
        List<List<Resource>> choices;
        List<List<Resource>> unpurchasable;


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

        private void Initializer()
        {
            //Initializing Variables
            hand = new List<Card>();
            played = new List<Card>();
            lastCardsPlayed = new List<string>();
            lastActions = new List<ActionType>();
            rcostEast = 2;
            rcostWest = 2;
            mcost = 2;

            // Setting the Players Score Dictionary
            score = new Dictionary<Score, int>();
            score.Add(Score.VICTORY, 0);    // Victory Points
            score.Add(Score.ARMY, 0);       // Military Score (shields)
            score.Add(Score.COIN, 0);       // Coins - this will be totaled at the end
            score.Add(Score.CONFLICT, 0);   //Conflict points
            score.Add(Score.TABLET, 0);     // Science - Tablet
            score.Add(Score.COMPASS, 0);    // Science - Compass
            score.Add(Score.GEAR, 0);       // Science - Gear
            score.Add(Score.VICTORY_BLUE, 0); // Victory - Counting the number of blue cards

            // Setting the  resource dictionary
            resources = new Dictionary<Resource, int>();
            resources.Add(Resource.CLAY, 0);    // Clay
            resources.Add(Resource.ORE, 0);     // Ore
            resources.Add(Resource.STONE, 0);   // Stone
            resources.Add(Resource.WOOD, 0);    // Wood
            resources.Add(Resource.GLASS, 0);   // Glass
            resources.Add(Resource.LOOM, 0);    // Loom
            resources.Add(Resource.PAPYRUS, 0); // Papyrus
            resources.Add(Resource.COIN, 0);    // Coin - Players start with 3 coins

            choiceResources = new Dictionary<Resource, int>();
            choiceResources.Add(Resource.CLAY, 0);    // Clay
            choiceResources.Add(Resource.ORE, 0);     // Ore
            choiceResources.Add(Resource.STONE, 0);   // Stone
            choiceResources.Add(Resource.WOOD, 0);    // Wood
            choiceResources.Add(Resource.GLASS, 0);   // Glass
            choiceResources.Add(Resource.LOOM, 0);    // Loom
            choiceResources.Add(Resource.PAPYRUS, 0); // Papyrus - Choice Resource never gets coin

            // Card Colours
            cardColour = new Dictionary<CardColour, int>();
            cardColour.Add(CardColour.BROWN, 0);
            cardColour.Add(CardColour.GRAY, 0);
            cardColour.Add(CardColour.BLUE, 0);
            cardColour.Add(CardColour.GREEN, 0);
            cardColour.Add(CardColour.YELLOW, 0);
            cardColour.Add(CardColour.RED, 0);
            cardColour.Add(CardColour.PURPLE, 0);
        }

        // ACCESSORS
        public string getName() { return name; }
        public long getID() { return id; }
        public int getSeat() { return seatNumber; }
        public Wonder getBoard() { return board; }
        public List<Card> getHand() { return hand; }
        public List<Card> getPlayed() { return played; }
        public bool getReady() { return ready; }
        public List<string> getLastCardsPlayed() { return lastCardsPlayed; }
        public List<ActionType> getLastActions() { return lastActions; }

        // Returns the dicitonary object of Score & Resources 
        //  along with List of choices and unpurchasable resources
        public Dictionary<Score, int> getScore() { return score; }
        public int getSpecificScore(Score s) { return score[s]; }
        public Dictionary<Resource, int> getResources() { return resources; }
        public Dictionary<Resource, int> getChoiceResources() { return choiceResources; }
        public List<List<Resource>> getChoices() { return choices; }
        public List<List<Resource>> getUnpurchaseable() { return unpurchasable; }
        public Dictionary<CardColour, int> getCardColours() { return cardColour; }

        // Returns the players total number of resources a player has after
        // they hae decided on what choice they want
        public Dictionary<Resource, int> getTotalResources()
        {
            Dictionary<Resource, int> temp = new Dictionary<Resource, int>();
            if (!choiceResources.Equals(null) && (!resources.Equals(null)))
            {
                if (choiceResources.ContainsKey(Resource.CLAY))
                {
                    temp.Add(Resource.CLAY, (choiceResources[Resource.CLAY] + resources[Resource.CLAY]));
                }

                if (choiceResources.ContainsKey(Resource.GLASS))
                {
                    temp.Add(Resource.GLASS, (choiceResources[Resource.GLASS] + resources[Resource.GLASS]));
                }

                if (choiceResources.ContainsKey(Resource.LOOM))
                {
                    temp.Add(Resource.CLAY, (choiceResources[Resource.LOOM] + resources[Resource.LOOM]));
                }

                if (choiceResources.ContainsKey(Resource.ORE))
                {
                    temp.Add(Resource.CLAY, (choiceResources[Resource.ORE] + resources[Resource.ORE]));
                }

                if (choiceResources.ContainsKey(Resource.PAPYRUS))
                {
                    temp.Add(Resource.CLAY, (choiceResources[Resource.PAPYRUS] + resources[Resource.PAPYRUS]));
                }

                if (choiceResources.ContainsKey(Resource.STONE))
                {
                    temp.Add(Resource.CLAY, (choiceResources[Resource.STONE] + resources[Resource.STONE]));
                }

                if (choiceResources.ContainsKey(Resource.WOOD))
                {
                    temp.Add(Resource.CLAY, (choiceResources[Resource.WOOD] + resources[Resource.WOOD]));
                }
            }
            return temp;
        }

        // Resets the Players choice on their resources for the next turn
        public void resetChoiceResources()
        {
            // Resetting all choices so players can again, choose which resource to gain
            choiceResources[Resource.CLAY] = 0;    // Clay
            choiceResources[Resource.ORE] =  0;     // Ore
            choiceResources[Resource.STONE] = 0;   // Stone
            choiceResources[Resource.WOOD] =  0;    // Wood
            choiceResources[Resource.GLASS] =  0;   // Glass
            choiceResources[Resource.LOOM] = 0;    // Loom
            choiceResources[Resource.PAPYRUS] =  0; // Papyrus - Choice Resource never gets coin
        }

        public int getCardColourCount(CardColour c)
        {
            if (cardColour.ContainsKey(c))
            {
                //Console.WriteLine("Returning " + c + ": " + cardColour[c]);
                return cardColour[c];
            }
            else
                Console.WriteLine("Error returning cardColour: " + c);

            return -1;
        }

        // Returns a boolean value if that specific card has been played yet or not
        public bool cardPlayed(Card c) { return played.Contains(c); }

        // Get the Score number of a certain 's'
        public int getScoreNum(Score s)
        {
            if (score.ContainsKey(s))
            {
                Console.WriteLine("Returing " + s + ": " + score[s]);
                return score[s];
            }
            else
                Console.WriteLine("Error returning score: " + s);

            return -1;
        }

        // Get the players Resource number of a certain 'r'
        public int getResourceNum(Resource r)
        {
            if (resources.ContainsKey(r))
            {
                Console.WriteLine("Resources " + r + ": " + resources[r]);
                return resources[r];
            }
            else
                Console.WriteLine("Error returning resource: " + r);

            return -1;
        }

        // Get the choice Resource number of a certain 'r'
        public int getChoiceResourceNum(Resource r)
        {
            if (choiceResources.ContainsKey(r))
            {
                Console.WriteLine("Resources " + r + ": " + choiceResources[r]);
                return choiceResources[r];
            }
            else
                Console.WriteLine("Error returning choiceResource: " + r);

            return -1;
        }


        // MUTATORS
        public void setNameID(string n)         {   name = n;   }
        public void setSeat(int s)              {   seatNumber = s; }
        public void setHand(List<Card> h)       {   hand = new List<Card>(h);   }
        public void setPlayed(List<Card> p)     {   played = new List<Card>(p); }
        public void setReady(bool _ready)       {   ready = _ready; }
        public void setBoard(Wonder w)          {   board = w; }
        public void addChoices(List<Resource> r) { choices.Add(r); }
        public void addUnpurchasable(List<Resource> r) { unpurchasable.Add(r); }
        public void setLastCardsPlayed(List<string> _cards) { lastCardsPlayed = _cards; }
        public void setLastActions(List<ActionType> _actions) { lastActions = _actions; }

        public void addPlayed(Card c)
        {
            if (!cardPlayed(c))
            {
                played.Add(c);
                cardColour[c.colour] += 1;
            }
            else
                Console.WriteLine("Card '" + c + "' has been played already!");
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
            if (score.ContainsKey(s))
                score[s] = x;
            else
                Console.WriteLine("Error: Set Score failed, no such score " + s);
        }

        // Adds to the Score of a certain 's'
        public void addScore(Score s, int x)
        {
            if (score.ContainsKey(s))
                score[s] += x;
            else
                Console.WriteLine("Error: Add Score failed, no such score " + s);
        }

        // Sets the Resource of a certain 'r'
        public void setResourceNum(Resource r, int x)
        {
            if (resources.ContainsKey(r))
                resources[r] = x;
            else
                Console.WriteLine("Error: Set Resource failed, no such resource " + r);
        }

        //Adds to the Resource of a certain 'r'
        public void addResource(Resource r, int x)
        {
            if (resources.ContainsKey(r))
                resources[r] += x;
            else
                Console.WriteLine("Error: Add Resource failed, no such resource " + r);
        }

        // Sets the Resource of a certain 'r'
        public void setChoiceResourceNum(Resource r, int x)
        {
            if (choiceResources.ContainsKey(r))
                choiceResources[r] = x;
            else
                Console.WriteLine("Error: Set Resource failed, no such resource " + r);
        }

        //Adds to the Resource of a certain 'r'
        public void addChoiceResosurce(Resource r)
        {
            if (choiceResources.ContainsKey(r))
                choiceResources[r] += 1;
            else
                Console.WriteLine("Error: Add Resource failed, no such resource " + r);
        }

        public void addCardColour(CardColour c, int x)
        {
            if (cardColour.ContainsKey(c))
                cardColour[c] += x;
            else
                Console.WriteLine("Error: Adding to cardColour failed, no such card colour " + c);
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
            }
        }
    }
}
