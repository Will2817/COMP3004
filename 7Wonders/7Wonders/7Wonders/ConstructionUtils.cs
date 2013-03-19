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
            //check if cost is in coins
            if (cost.ContainsKey(Resource.COIN))
                return (cost[Resource.COIN] > player.getResourceNum(Resource.COIN)) ? -1 : cost[Resource.COIN];

            int totalCost = 0;
            foreach (Resource r in cost.Keys) totalCost += cost[r];

            //subtract the player's resources from the cost (any individual resources and any choices, only one choice in which could be used to satisfy the cost)
            List<List<Resource>> remainingPlayerChoices = new List<List<Resource>>();
            Dictionary<Resource, int> costAfterPlayerResources = outsourcedCosts(player, cost, remainingPlayerChoices, true);
            

            List<List<Resource>> totalChoices = new List<List<Resource>>();
            totalChoices.AddRange(player.getTotalChoices());
            totalChoices.AddRange(west.getPublicChoices());
            totalChoices.AddRange(east.getPublicChoices());
            totalChoices = getRelevantChoices(cost, totalChoices);

            
            bool eastCheaper = player.rcostEast < player.rcostWest;
            Player cheaper = eastCheaper? east : west;
            Player notCheaper = eastCheaper? west : east;
            int cheaperCost = eastCheaper? player.rcostEast : player.rcostWest;
            int notCheaperCost = eastCheaper? player.rcostWest : player.rcostEast;
            
            List<List<Resource>> remainingChoices = new List<List<Resource>>();

            Dictionary<Resource, int> afterPlayer = outsourcedCosts(player, cost, remainingChoices, true);
            foreach (Resource r in cost.Keys)
                totalCost -= cost[r] - (afterPlayer.ContainsKey(r) ? afterPlayer[r] : 0);
            if (afterPlayer.Count == 0 || canChoicesCover(remainingChoices, afterPlayer)) return 0;

            Dictionary<Resource, int> afterCheaper = outsourcedCosts(cheaper, afterPlayer, remainingChoices, false);
            if (cheaperCost > 1)
                foreach (Resource r in afterPlayer.Keys.Intersect(rawGoods))
                    totalCost += (afterPlayer[r] - (afterCheaper.ContainsKey(r) ? afterCheaper[r] : 0));
            Dictionary<Resource, int> afterNotCheaper;
            if (!(afterCheaper.Count == 0 || canChoicesCover(remainingChoices, afterPlayer)))
            {
                afterNotCheaper = outsourcedCosts(notCheaper, afterCheaper, remainingChoices, false);
                if (notCheaperCost > 1)
                    foreach (Resource r in afterCheaper.Keys.Intersect(rawGoods))
                        totalCost += (afterCheaper[r] - (afterNotCheaper.ContainsKey(r) ? afterNotCheaper[r] : 0));
            }
            else
            {
                afterNotCheaper = afterCheaper;
            }

            if (player.mcost > 1)
                foreach (Resource r in afterPlayer.Keys.Intersect(manGoods))
                    totalCost += (afterPlayer[r] - (afterNotCheaper.ContainsKey(r) ? afterNotCheaper[r] : 0));

            if (afterNotCheaper.Count > 0 && !canChoicesCover(remainingChoices, afterNotCheaper)) return -1;



            return totalCost > player.getResourceNum(Resource.COIN) ? -1 : totalCost;

            
        }

        //Checks whether a player can build a card from a chain
        public static bool canChainBuild(Player player, Card card)
        {
            foreach (string cardID in player.getPlayed())
                if (card.chains.Contains(CardLibrary.getCard(cardID).name)) return true;
            return false;
        }

        public static Dictionary<Resource, int> outsourcedCosts(Player player, Dictionary<Resource, int> cost)
        {
            return outsourcedCosts(player, cost, null, false);
        }

        //Returns the part of the given cost that the player cannot meet with his own single resources
        //Does not consider the player's resource choices
        private static Dictionary<Resource, int> outsourcedCosts(Player player, Dictionary<Resource, int> cost, List<List<Resource>> remainingPlayerChoices, bool self)
        {
            Dictionary<Resource, int> remaining = new Dictionary<Resource, int>();
            Dictionary<Resource, int> coveredByChoices = new Dictionary<Resource, int>(); //Costs that are covered by choices (each item in here is the only relevant application of the choice used)
            foreach (Resource r in cost.Keys) coveredByChoices.Add(r, 0);
            if (remainingPlayerChoices != null)
            {
                foreach (List<Resource> choice in self?player.getTotalChoices():player.getPublicChoices())
                {
                    int numRelevant = 0;
                    Resource coveredResource = Resource.COIN;
                    foreach (Resource r in choice)
                        if (cost.ContainsKey(r))
                        {
                            numRelevant++;
                            coveredResource = r;
                        }
                    if (numRelevant == 1)
                        coveredByChoices[coveredResource]++;
                    else
                        remainingPlayerChoices.Add(choice);
                }
            }
            foreach (Resource r in cost.Keys)
            {
                if (cost[r] > player.getResourceNum(r) + coveredByChoices[r] && r != Resource.COIN)
                    remaining.Add(r, cost[r] - player.getResourceNum(r));
            }
            return remaining;
        }

        //Returns the subset of a cost that cannot possibly be covered with a given set of choices
        //E.g. if the cost includes 3W and there are only 2 choices that include W, will return a dictionary where [W]=1
        //E.g. 2 if the cost includes manufactured goods and choices are only for raw goods, cost for man goods will remain same in return value
        private static Dictionary<Resource, int> cantCoverWithChoices(Dictionary<Resource, int> cost, List<List<Resource>> choices)
        {
            Dictionary<Resource, int> remaining = new Dictionary<Resource, int>();
            foreach (Resource r in cost.Keys)
                remaining.Add(r, cost[r]);
            foreach (List<Resource> choice in choices)
                foreach (Resource r in choice)
                    if (remaining.ContainsKey(r) && remaining[r] > 0) remaining[r]--;
            return remaining;
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
