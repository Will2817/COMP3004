using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    class ConstructionUtils
    {
        private static CardLibrary cards;

        public static void init(CardLibrary _cards)
        {
            cards = _cards;
        }

        public static int constructCost(Player player, Player west, Player east, Dictionary<Resource, int> cost)
        {
            //calculate min construct cost in coins
            return -1;
        }

        public static bool canChainBuild(Player player, Card card)
        {
            //check whether player can build card from chain
            return false;
        }

        public static Dictionary<Resource, int> outsorcedCosts(Player player, Card card)
        {
            Dictionary<Resource, int> remaining = new Dictionary<Resource, int>();
            //fill remaining with costs that can't be covered by player's plain resources (i.e. not including player's choices)
            //!assumes there is a remaining cost, i.e. that the caller has called constructCost() with a result > 0
            return remaining;
        }
    }
}
