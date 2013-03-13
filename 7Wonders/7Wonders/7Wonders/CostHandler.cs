using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    /*
     * CostHandler Class
     * This will handle all the transactions of purchasing Cards, Structures and Resources
     * from Neighbouring Cities
     */
    public class CostHandler
    {
        // Grabs the players resources and computes
        // if the Card c can be purchased
        public static void buildCard(Player p, Card c)
        {
            Dictionary<Resource,int> resources = p.getTotalResources();
            Dictionary<Resource, int> cost = c.cost;
        }

        //private Dictionary<Resource, int> computeResources(Dictionary<Resource, int> curr, Dictionary<Resource, int> cost)

    }
}
