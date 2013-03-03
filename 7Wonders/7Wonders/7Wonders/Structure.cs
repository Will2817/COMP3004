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
        public Structure(JArray _costs, JArray _effects)
        {
            costs = new List<string>();
            effects = new List<string>();
            chains = new List<string>();
            previous = new List<string>();

            costsSize = 0;
            effectsSize = 0;
            chainsSize = 0;
            previousSize = 0;
            
            foreach (JObject j in (JArray)_costs)
            {
                //(string)j.["s"]; Stone?
            }

            foreach (JObject j in _effects)
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
