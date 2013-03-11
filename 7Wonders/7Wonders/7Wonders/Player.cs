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

        // Players Score
        // Victory Points, Army, Coins, Conflict tokens
        // Sciences: tablet, compass, gear
        protected Dictionary<Score, int> score;        

        // Resources
        // Clay, Ore, Stone, Wood, Glass, Loom, Papyrus including
        protected Dictionary<Resource, int> resources;

        
        // Default Player Constructor
        public Player(long _id, string _name)
        {
            id          = _id;
            name        = _name;
            seatNumber = -1;
            board       = null;            
            hand        = null;
            played      = null;
            ready       = false;
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
            // Setting the Players Score Dictionary
            score = new Dictionary<Score, int>();
            score.Add(Score.VICTORY, 0);    // Victory Points
            score.Add(Score.ARMY, 0);       // Military Score (shields)
            score.Add(Score.COIN, 0);       // Coins - this will be totaled at the end
            score.Add(Score.CONFLICT, 0);   //Conflict points
            score.Add(Score.TABLET, 0);     // Science - Tablet
            score.Add(Score.COMPASS, 0);    // Science - Compass
            score.Add(Score.GEAR, 0);       // Science - Gear
            score.Add(Score.VICTORY_BLUE, 0);

            // Setting the  resource dictionary
            resources = new Dictionary<Resource, int>();
            resources.Add(Resource.CLAY, 0);    // Clay
            resources.Add(Resource.ORE, 0);     // Ore
            resources.Add(Resource.STONE, 0);   // Stone
            resources.Add(Resource.WOOD, 0);    // Wood
            resources.Add(Resource.GLASS, 0);   // Glass
            resources.Add(Resource.LOOM, 0);    // Loom
            resources.Add(Resource.PAPYRUS, 0); // Papyrus
            resources.Add(Resource.COIN, 0);    // Coin
        }

       /* This is no longer needed, we will have hte player join the lobby or host
        *  the game and once the game starts will we assign the board and cards to the player
        * public Player(string _name, Wonder _board, List<Card> _hand)
        {
            // Initialized through the constructor
            name = _name;
            board = _board;
            hand = new List<Card>(_hand);

            // Setting the Players Score Dictionary
            score = new Dictionary<string, int>();
            score.Add("victory", 0);    // Victory Points
            score.Add("army", 0);
            score.Add("coin", 0);
            score.Add("defeat", 0);
            score.Add("tablet", 0);
            score.Add("compass", 0);
            score.Add("gear", 0);

            // Setting the  resource dictionary
            resources = new Dictionary<string, int>();
            resources.Add("c", 0);  // Clay
            resources.Add("o", 0);  // Ore
            resources.Add("s", 0);  // Stone
            resources.Add("w", 0);  // Wood
            resources.Add("g", 0);  // Glass
            resources.Add("l", 0);  // Loom
            resources.Add("p", 0);  // Papyrus
        }*/

        // ACCESSORS
        public string getName()         {   return name;    }        
        public long getID()             {   return id;      }
        public int getSeat()            {   return seatNumber; }
        public Wonder getBoard()        {   return board;   }
        public List<Card> getHand()     {   return hand;    }
        public List<Card> getPlayed()   {   return played;  }
        public bool getReady()          {   return ready;   }
            
        // Returns the dicitonary object of Score & Resources
        public Dictionary<Score, int> getScore()           { return score;     }
        public Dictionary<Resource, int> getResources()     { return resources; }

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

        // Get the Resource number of a certain 'r'
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

        // MUTATORS
        public void setNameID(string n)         {   name = n;   }
        public void setSeat(int s)              { seatNumber = s; }
        public void setHand(List<Card> h)       {   hand = new List<Card>(h);   }
        public void setPlayed(List<Card> p)     {   played = new List<Card>(p); }
        public void setReady(bool _ready)       {   ready = _ready; }
        public void setBoard(Wonder w)          {   board = w; }

        public void addPlayed(Card c)             
        {
            if (!cardPlayed(c))
                played.Add(c);
            else
                Console.WriteLine("Card '" + c + "' has been played already!");
        }


        

        public string toJString()
        {
            JObject player = 
                new JObject (
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
        public void addResosurce(Resource r, int x)
        {
            if (resources.ContainsKey(r))
                resources[r] += x;
            else
                Console.WriteLine("Error: Add Resource failed, no such resource " + r);
        }
    }
}
