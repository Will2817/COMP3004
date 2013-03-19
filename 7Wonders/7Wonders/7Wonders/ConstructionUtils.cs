using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    public class ConstructionUtils
    {
        private static List<Resource> rawGoods = new List<Resource>{ Resource.CLAY, Resource.ORE, Resource.STONE, Resource.WOOD };
        private static List<Resource> manGoods = new List<Resource> { Resource.GLASS, Resource.LOOM, Resource.PAPYRUS };
        //Checks how a player with neighbours west and east would have to spend in order to meet the given cost
        //Returns -1 if the player cannot possibly build the card (including if his neighbours have the required
        //resources, but he cannot afford to purchase them)
        //Returns 0 if the player has all of the required resources and does not need to purchase any
        //Otherwise, returns the total number of coins it would cost the player at minimum to purchase the
        //required resources to meet the cost
        public static int constructCost(Player player, Player west, Player east, Dictionary<Resource, int> cost)
        {
            if (cost.ContainsKey(Resource.COIN))
                return (cost[Resource.COIN] > player.getResourceNum(Resource.COIN)) ? -1 : cost[Resource.COIN];
            Dictionary<Resource, int> costAfterPlayerResources = outsourcedCosts(player, cost);
            if (costAfterPlayerResources.Count == 0 || canChoicesCover(player.getTotalChoices(), costAfterPlayerResources)) return 0;
            //check if player can meet remaining cost with choices
            int coinCost = 0;
            //raw resources
            Player cheaper;
            Player notCheaper;
            int cheaperCost;
            int notCheaperCost;
            if (player.rcostEast < player.rcostWest)
            {
                cheaper = east;
                notCheaper = west;
                cheaperCost = player.rcostEast;
                notCheaperCost = player.rcostWest;
            }
            else
            {
                cheaper = west;
                notCheaper = east;
                cheaperCost = player.rcostWest;
                notCheaperCost = player.rcostEast;
            }
            Dictionary<Resource, int> rawCost = new Dictionary<Resource, int>();
            foreach (Resource r in rawGoods.Intersect(costAfterPlayerResources.Keys))
                rawCost.Add(r, costAfterPlayerResources[r]);
            // List<List<Resource>> rawChoices = new List<List<Resource>>();
            //rawChoices.AddRange(west.getPublicChoices());
            //rawChoices.AddRange(east.getPublicChoices());
            Dictionary<Resource, int> leftoverRaw = outsourcedCosts(cheaper, rawCost);
            foreach (Resource r in rawGoods.Intersect(costAfterPlayerResources.Keys))
                coinCost += cheaperCost * (costAfterPlayerResources[r] - (leftoverRaw.ContainsKey(r)?leftoverRaw[r]:0));
            if (!(leftoverRaw.Count == 0 || canChoicesCover(player.getTotalChoices(), leftoverRaw)))
            {
                Dictionary<Resource, int> stillLeftoverRaw = outsourcedCosts(notCheaper, leftoverRaw);
                foreach (Resource r in rawGoods.Intersect(leftoverRaw.Keys))
                    coinCost += notCheaperCost * (leftoverRaw[r] - (stillLeftoverRaw.ContainsKey(r) ? stillLeftoverRaw[r] : 0));
                if (!(stillLeftoverRaw.Count == 0 || canChoicesCover(player.getTotalChoices(), stillLeftoverRaw))) return -1;
            }
            

            //manufactured goods
            Dictionary<Resource, int> manCost = new Dictionary<Resource, int>();
            foreach (Resource r in manGoods.Intersect(costAfterPlayerResources.Keys))
                manCost.Add(r, costAfterPlayerResources[r]);
            Dictionary<Resource, int> leftoverMan = outsourcedCosts(west, manCost);
            foreach (Resource r in manGoods.Intersect(costAfterPlayerResources.Keys))
                coinCost += player.mcost * (costAfterPlayerResources[r] - (leftoverMan.ContainsKey(r) ? leftoverMan[r] : 0));
            if (!(leftoverMan.Count == 0 || canChoicesCover(player.getTotalChoices(), leftoverMan)))
            {
                Dictionary<Resource, int> stillLeftoverMan = outsourcedCosts(east, leftoverMan);
                foreach (Resource r in manGoods.Intersect(leftoverMan.Keys))
                    coinCost += player.mcost * (leftoverMan[r] - (stillLeftoverMan.ContainsKey(r) ? stillLeftoverMan[r] : 0));
                if (!(stillLeftoverMan.Count == 0 || canChoicesCover(player.getTotalChoices(), stillLeftoverMan))) return -1;
            }

            return coinCost > player.getResourceNum(Resource.COIN) ? -1 : coinCost;
        }

        //Checks whether a player can build a card from a chain
        public static bool canChainBuild(Player player, Card card)
        {
            foreach (string cardID in player.getPlayed())
                if (card.chains.Contains(CardLibrary.getCard(cardID).name)) return true;
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
        private static Dictionary<Resource, int> maxCoveragePerResourceByChoices(Dictionary<Resource, int> cost, List<List<Resource>> choices)
        {
            Dictionary<Resource, int> coverage = new Dictionary<Resource, int>();
            foreach (Resource r in cost.Keys)
                coverage.Add(r, 0);
            foreach (List<Resource> choice in choices)
                foreach (Resource r in choice)
                    if (coverage.ContainsKey(r) && coverage[r] < cost[r]) coverage[r]++;
            return coverage;
        }

        public static List<List<Resource>> getRelevantChoices(Dictionary<Resource, int> cost, List<List<Resource>> choices)
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

        public static bool canChoicesCover(List<List<Resource>> choices, Dictionary<Resource, int> cost)
        {
            List<List<Resource>> relevantChoices = getRelevantChoices(cost, choices);
            if (relevantChoices.Count == 0) return false;
            int totalAmount = 0;
            foreach (KeyValuePair<Resource, int> r in cost)
            {
                if (relevantChoices.Count < r.Value) return false;
                else if (cost.Count == 1) return true;
                Dictionary<Resource, int> rCost = new Dictionary<Resource, int>();
                rCost.Add(r.Key, r.Value);
                if (!canChoicesCover(choices, rCost)) return false;
                totalAmount += r.Value;
            }
            if (relevantChoices.Count < totalAmount) return false;
            return true;
        }
    }
}
