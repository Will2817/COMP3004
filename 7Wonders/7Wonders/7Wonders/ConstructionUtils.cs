using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    class ConstructionUtils
    {
        //Checks how a player with neighbours west and east would have to spend in order to meet the given cost
        //Returns -1 if the player cannot possibly build the card (including if his neighbours have the required
        //resources, but he cannot afford to purchase them)
        //Returns 0 if the player has all of the required resources and does not need to purchase any
        //Otherwise, returns the total number of coins it would cost the player at minimum to purchase the
        //required resources to meet the cost
        public static int constructCost(Player player, Player west, Player east, Dictionary<Resource, int> cost)
        {
            Dictionary<Resource, int> cost2 = outsourcedCosts(player, cost);
            if (cost2.Count == 0) return 0;
            //check if player can meet remaining cost with choices
            int coinCost = 0;
            foreach (int amt in cost2.Values) coinCost += amt;
            Dictionary<Resource, int> cost3 = outsourcedCosts(west, cost2);
            if (cost3.Count > 0 && outsourcedCosts(east, cost3).Count > 0) return -1;
            return coinCost > player.getResourceNum(Resource.COIN) ? -1 : coinCost;
        }

        //Checks whether a player can build a card from a chain
        public static bool canChainBuild(Player player, Card card)
        {
            foreach (string cid in card.chains)
                if (player.getPlayed().Contains(cid)) return true;
            return false;
        }

        //Returns the part of the given cost that the player cannot meet with his own single resources
        //Does not consider the player's resource choices
        public static Dictionary<Resource, int> outsourcedCosts(Player player, Dictionary<Resource, int> cost)
        {
            Dictionary<Resource, int> remaining = new Dictionary<Resource, int>();
            foreach (Resource r in cost.Keys)
            {
                if (cost[r] > player.getResourceNum(r) && r != Resource.COIN)
                    remaining.Add(r, cost[r] - player.getResourceNum(r));
            }
            return remaining;
        }

        //Calculates, for each resource in cost, the maximum number of that resource that could be covered were it given priority in choices
        //E.g. if a value is zero, that resource cannot be acquired from the given set of choices
        //If a value is one and there are two of that resource in the cost, at least one of that resource must be obtained elsewhere
        private Dictionary<Resource, int> maxCoveragePerResourceByChoices(Dictionary<Resource, int> cost, List<List<Resource>> choices)
        {
            Dictionary<Resource, int> coverage = new Dictionary<Resource, int>();
            foreach (Resource r in cost.Keys)
                coverage.Add(r, 0);
            foreach (List<Resource> choice in choices)
                foreach (Resource r in choice)
                    if (coverage.ContainsKey(r)) coverage[r]++;
            foreach (Resource r in coverage.Keys)
                if (coverage[r] > cost[r]) coverage[r] = cost[r];
            return coverage;
        }

        private List<List<Resource>> getRelevantChoices(Dictionary<Resource, int> cost, List<List<Resource>> choices)
        {
            List<List<Resource>> relevantChoices = new List<List<Resource>>();
            foreach (List<Resource> choice in choices)
                foreach (Resource r in choice)
                    if (cost.ContainsKey(r) && cost[r] > 0)
                    {
                        relevantChoices.Add(choice);
                        break;
                    }
            return relevantChoices;
        }
    }
}
