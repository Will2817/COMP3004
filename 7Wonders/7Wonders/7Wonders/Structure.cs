using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    // Structure Class
    public class Structure
    {
        // Variables
        protected List<string> costs;
        protected List<string> effects;
        protected List<string> chains;
        protected List<string> previous;

        protected int costsSize, effectsSize, chainsSize, previousSize;

        // Constructor
        public Structure(JObject _costs, JObject _effects)
        {
            costs = new List<string>();
            effects = new List<string>();
            chains = new List<string>();
            previous = new List<string>();

            costsSize = 0;
            effectsSize = 0;
            chainsSize = 0;
            previousSize = 0;
            
            foreach (JProperty p in _costs.Properties())
            {
                switch (p.Name)
                {
                    case "c":
                        //do stuff;
                        break;
                    default: break;

                }
            }

            foreach (JProperty p in _effects.Properties())
            {
                //add to effect
            }
        }

        public List<string> getEffects()
        {
            return effects;
        }

        public List<string> getCosts()
        {
            return costs;
        }
    }
}
