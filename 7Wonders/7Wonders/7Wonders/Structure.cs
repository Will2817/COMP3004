using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    public class Structure
    {
        protected List<string> costs;
        protected List<string> effects;

        public Structure(JArray _costs, JArray _effects)
        {
            costs = new List<string>();
            effects = new List<string>();
            
            foreach (JObject j in (JArray)_costs)
            {
                //Add to costs
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
