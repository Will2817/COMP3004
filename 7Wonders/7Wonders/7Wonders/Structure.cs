using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders
{
    class Structure
    {
        protected List<string> costs;
        protected List<string> effects;

        public Structure(JArray _costs, JArray _effect)
        {
            costs = new List<string>();
            effects = new List<string>();
            
            foreach (JObject j in _costs)
            {
                costs.Add((string) j);
            }

            foreach (JObject j in _effect)
            {
                effects.Add((string)j);
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
