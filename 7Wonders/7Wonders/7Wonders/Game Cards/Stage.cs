using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Game_Cards
{
    // This Class will be used with Wonders to keep track of it's stages
    class Stage
    {
        public Dictionary<Resource, int> cost;
        public List<Effect> effects { get; set; }

        public Stage(JObject _json)
        {
            cost = new Dictionary<Resource, int>();
            cost = (new Cost((JObject)_json["cost"])).getCost(); // Setting up the cost

            effects = new List<Effect>();

            // Adding the effects for the cards into a List of Effects
            foreach (JObject e in (JArray)_json["effects"])
            {
                Effect effect = new Effect(e);
                effects.Add(effect);
            }
        }
    }
}
