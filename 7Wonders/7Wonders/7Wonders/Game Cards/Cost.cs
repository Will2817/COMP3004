using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Game_Cards
{
    public class Cost
    {
        // Used for returning the total cost
        private static Dictionary<Resource, int> cost;
    
        public Cost(JObject _json)
        {
            // Initializing the cost dictionary
            cost = new Dictionary<Resource, int>();

            foreach (KeyValuePair<string, JToken> token in (JObject)_json)
            {
                switch (token.Key)
                {
                    case "w":
                        cost.Add(Resource.WOOD, (int)token.Value);
                        break;
                    case "s":
                        cost.Add(Resource.STONE, (int)token.Value);
                        break;
                    case "o":
                        cost.Add(Resource.ORE, (int)token.Value);
                        break;
                    case "c":
                        cost.Add(Resource.CLAY, (int)token.Value);
                        break;
                    case "g":
                        cost.Add(Resource.GLASS, (int)token.Value);
                        break;
                    case "p":
                        cost.Add(Resource.PAPYRUS, (int)token.Value);
                        break;
                    case "l":
                        cost.Add(Resource.LOOM, (int)token.Value);
                        break;
                    case "coin":
                        cost.Add(Resource.COIN, (int)token.Value);
                        break;

                    default:
                        Console.WriteLine("ERROR: Cost " + token.Key + " not recognized!");
                        break;
                }
            }
        }

        // Returns a dictionary with the costs
        public Dictionary<Resource, int> getCost()
        {
            return cost;
        }
    }
}
