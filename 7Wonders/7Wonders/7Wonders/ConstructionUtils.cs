﻿using System;
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
            //split the cost into raw and manufactured goods to deal with them separately, as they do not affect each other
            Dictionary<Resource, int> rawCost = new Dictionary<Resource, int>();
            foreach (Resource r in rawGoods)
                if (cost.ContainsKey(r)) rawCost.Add(r, cost[r]);
            Dictionary<Resource, int> manCost = new Dictionary<Resource, int>();
            foreach (Resource r in manGoods)
                if (cost.ContainsKey(r)) manCost.Add(r, cost[r]);
            //get resource lists
            List<List<Resource>> playerRaw = buildResourceList(player, rawGoods, true);
            List<List<Resource>> playerMan = buildResourceList(player, manGoods, true);
            List<List<Resource>> eastRaw = buildResourceList(east, rawGoods, false);
            List<List<Resource>> westRaw = buildResourceList(west, rawGoods, false);
            List<List<Resource>> neighbourMan = buildResourceList(east, manGoods, false);
            neighbourMan.AddRange(buildResourceList(west, manGoods, false));
            //calculate cost of manufactured goods
            int numManPurchased = countPurchases(manCost, playerMan, neighbourMan);
            int totalCoinCost = numManPurchased * player.mcost;
            //calculate cost of raw goods
            if (player.rcostEast == player.rcostWest)
            {
                //All neighbour resources can be grouped together, since they cost the same amount
                List<List<Resource>> neighbourRaw = eastRaw;
                neighbourRaw.AddRange(westRaw);
                int numRawPurchased = countPurchases(rawCost, playerRaw, neighbourRaw);
                totalCoinCost += numRawPurchased * player.rcostWest;
            }
            else
            {
                //east and west neighbour resorces must be considered separately, as they cost different amounts
                //countPurchases() will return the coin cost, not just the number of resources purchased
                int numRawPurchased;
                if (player.rcostEast < player.rcostWest)
                    numRawPurchased = countPurchases(rawCost, playerRaw, eastRaw, westRaw);
                else
                    numRawPurchased = countPurchases(rawCost, playerRaw, westRaw, eastRaw);
                totalCoinCost += numRawPurchased;
            }

            return totalCoinCost>player.getResourceNum(Resource.COIN)?-1:totalCoinCost;
        }

        private static List<List<Resource>> buildResourceList(Player p, List<Resource> rSet, bool self)
        {
            List<List<Resource>> resources = new List<List<Resource>>();
            Dictionary<Resource, int> playerResources = p.getResources();
            List<List<Resource>> playerChoices = self?p.getTotalChoices():p.getPublicChoices();
            foreach (Resource r in playerResources.Keys)
                if (r != Resource.COIN)
                    for (int i = 0; i < playerResources[r]; i++)
                        resources.Add(new List<Resource> { r });
            foreach (List<Resource> choice in playerChoices)
            {
                List<Resource> l = new List<Resource>();
                l.AddRange(choice);
                resources.Add(l);
            }
            return resources;
        }

        private static int countPurchases(Dictionary<Resource, int> _cost, List<List<Resource>> freeRes, List<List<Resource>> costRes)
        {
            bool onlyChoicesLeft = false;
            Dictionary<Resource, int> cost = new Dictionary<Resource,int>();
            foreach (Resource r in _cost.Keys)
                cost.Add(r, _cost[r]);
            while (!onlyChoicesLeft)
            {
                freeRes = removeIrrelevant(cost, freeRes); //remove irrelevant resources (i.e. resources not in cost)
                onlyChoicesLeft = true;
                List<List<Resource>> keepMe = new List<List<Resource>>();
                foreach (List<Resource> choice in freeRes)
                {
                    if (choice.Count == 1)
                    {
                        onlyChoicesLeft = false;
                        if (cost.ContainsKey(choice[0]) && cost[choice[0]] > 0)
                            cost[choice[0]]--;
                    }
                    else
                        keepMe.Add(choice);
                }
                freeRes = keepMe;
            }
            if (freeRes.Count > 0)
            {
                List<int> costOptions = new List<int>();
                List<int> validOptions = new List<int>();
                List<List<List<Resource>>> configurations = enumerateCombos(freeRes);
                foreach (List<List<Resource>> choiceConfig in configurations)
                    costOptions.Add(countPurchases(cost, choiceConfig, costRes));
                foreach (int costOption in costOptions)
                    if (costOption > -1) validOptions.Add(costOption);
                return validOptions.Count > 0?validOptions.Min():-1;
            }
            //take into account choices that don't resolve themselves?
            //e.g. need stone and clay and have s/c and w/o/s/c (->s/c)
            onlyChoicesLeft = false;
            int purchases = 0;
            while (!onlyChoicesLeft)
            {
                costRes = removeIrrelevant(cost, costRes);
                onlyChoicesLeft = true;
                List<List<Resource>> keepMe = new List<List<Resource>>();
                foreach (List<Resource> choice in costRes)
                {
                    if (choice.Count == 1)
                    {
                        onlyChoicesLeft = false;
                        if (cost.ContainsKey(choice[0]) && cost[choice[0]] > 0)
                        {
                            cost[choice[0]]--;
                            purchases++;
                        }
                        else
                            keepMe.Add(choice);
                    }
                    else
                        keepMe.Add(choice);
                }
                costRes = keepMe;
            }
            if (costRes.Count > 0)
            {
                List<int> costOptions = new List<int>();
                List<int> validOptions = new List<int>();
                List<List<List<Resource>>> configurations = enumerateCombos(costRes);
                foreach (List<List<Resource>> choiceConfig in configurations)
                    costOptions.Add(countPurchases(cost, new List<List<Resource>>(), choiceConfig));
                foreach (int costOption in costOptions)
                    if (costOption > -1) validOptions.Add(costOption);
                return validOptions.Count > 0 ? validOptions.Min() : -1;
            }
            bool costRemaining = false;
            foreach (int amt in cost.Values)
                if (amt > 0) costRemaining = true;
            return costRemaining?-1:purchases;
        }

        private static int countPurchases(Dictionary<Resource, int> _cost, List<List<Resource>> freeRes, List<List<Resource>> cheapRes, List<List<Resource>> costRes)
        {
            //free resources
            bool onlyChoicesLeft = false;
            Dictionary<Resource, int> cost = new Dictionary<Resource, int>();
            foreach (Resource r in _cost.Keys)
                cost.Add(r, _cost[r]);
            while (!onlyChoicesLeft)
            {
                freeRes = removeIrrelevant(cost, freeRes);
                onlyChoicesLeft = true;
                List<List<Resource>> keepMe = new List<List<Resource>>();
                foreach (List<Resource> choice in freeRes)
                {
                    if (choice.Count == 1)
                    {
                        onlyChoicesLeft = false;
                        if (cost.ContainsKey(choice[0]) && cost[choice[0]] > 0)
                            cost[choice[0]]--;
                        else
                            keepMe.Add(choice);
                    }
                    else
                        keepMe.Add(choice);
                }
                freeRes = keepMe;
            }
            if (freeRes.Count > 0)
            {
                List<int> costOptions = new List<int>();
                List<int> validOptions = new List<int>();
                List<List<List<Resource>>> configurations = enumerateCombos(freeRes);
                foreach (List<List<Resource>> choiceConfig in configurations)
                    costOptions.Add(countPurchases(cost, choiceConfig, cheapRes, costRes));
                foreach (int costOption in costOptions)
                    if (costOption > -1) validOptions.Add(costOption);
                return validOptions.Count > 0 ? validOptions.Min() : -1;
            }
            //cheap resources
            onlyChoicesLeft = false;
            int purchases = 0;
            while (!onlyChoicesLeft)
            {
                cheapRes = removeIrrelevant(cost, cheapRes);
                onlyChoicesLeft = true;
                List<List<Resource>> keepMe = new List<List<Resource>>();
                foreach (List<Resource> choice in cheapRes)
                {
                    if (choice.Count == 1)
                    {
                        onlyChoicesLeft = false;
                        if (cost.ContainsKey(choice[0]) && cost[choice[0]] > 0)
                        {
                            cost[choice[0]]--;
                            purchases++;
                        }
                        else
                            keepMe.Add(choice);
                    }
                    else
                        keepMe.Add(choice);
                }
                cheapRes = keepMe;
            }
            if (cheapRes.Count > 0)
            {
                List<int> costOptions = new List<int>();
                List<int> validOptions = new List<int>();
                List<List<List<Resource>>> configurations = enumerateCombos(cheapRes);
                foreach (List<List<Resource>> choiceConfig in configurations)
                    costOptions.Add(countPurchases(cost, new List<List<Resource>>(), choiceConfig, costRes));
                foreach (int costOption in costOptions)
                    if (costOption > -1) validOptions.Add(costOption);
                return validOptions.Count > 0 ? validOptions.Min() : -1;
            }
            //costly resources
            onlyChoicesLeft = false;
            int purchases2 = 0;
            while (!onlyChoicesLeft)
            {
                costRes = removeIrrelevant(cost, costRes);
                onlyChoicesLeft = true;
                List<List<Resource>> keepMe = new List<List<Resource>>();
                foreach (List<Resource> choice in costRes)
                {
                    if (choice.Count == 1)
                    {
                        onlyChoicesLeft = false;
                        if (cost.ContainsKey(choice[0]) && cost[choice[0]] > 0)
                        {
                            cost[choice[0]]--;
                            purchases2++;
                        }
                        else
                            keepMe.Add(choice);
                    }
                    else
                        keepMe.Add(choice);
                }
                costRes = keepMe;
            }
            if (costRes.Count > 0)
            {
                List<int> costOptions = new List<int>();
                List<int> validOptions = new List<int>();
                List<List<List<Resource>>> configurations = enumerateCombos(costRes);
                foreach (List<List<Resource>> choiceConfig in configurations)
                    costOptions.Add(countPurchases(cost, new List<List<Resource>>(), new List<List<Resource>>(), choiceConfig));
                foreach (int costOption in costOptions)
                    if (costOption > -1) validOptions.Add(costOption);
                return validOptions.Count > 0 ? validOptions.Min() : -1;
            }
            bool costRemaining = false;
            foreach (int amt in cost.Values)
                if (amt > 0) costRemaining = true;
            return costRemaining ? -1 : purchases + 2 * purchases2;
        }

        private static List<List<Resource>> removeIrrelevant(Dictionary<Resource, int> cost, List<List<Resource>> resources)
        {
            List<List<Resource>> relevant = new List<List<Resource>>();

            foreach (List<Resource> choice in resources)
            {
                List<Resource> l = new List<Resource>();
                foreach (Resource r in choice)
                    if (cost.ContainsKey(r) && cost[r] > 0)
                        l.Add(r);
                if (l.Count > 0) relevant.Add(l);
            }

            return relevant;
        }

        private static List<List<List<Resource>>> enumerateCombos(List<List<Resource>> choices)
        {
            List<List<List<Resource>>> combos = new List<List<List<Resource>>>();
            if (choices.Count == 0) return combos;
            foreach (Resource r in choices[0])
            {
                List<List<List<Resource>>> subCombos = enumerateCombos(choices.GetRange(1, choices.Count - 1));
                foreach (List<List<Resource>> option in subCombos)
                {
                    option.Add(new List<Resource> { r });
                    combos.Add(option);
                }
                if (subCombos.Count == 0)
                    combos.Add(new List<List<Resource>> { new List<Resource> { r } });
            }
            return combos;
        }


        /*
        public static int constructCost(Player player, Player west, Player east, Dictionary<Resource, int> cost)
        {
            //check if cost is in coins
            if (cost.ContainsKey(Resource.COIN))
                return (cost[Resource.COIN] > player.getResourceNum(Resource.COIN)) ? -1 : cost[Resource.COIN];


            //subtract the player's resources from the cost (any individual resources and any choices, only one choice in which could be used to satisfy the cost)
            List<List<Resource>> remainingPlayerChoices = new List<List<Resource>>();
            Dictionary<Resource, int> costAfterPlayerResources = outsourcedCosts(player, cost, remainingPlayerChoices, true);

            int totalCost = 0;
            foreach (Resource r in cost.Keys) totalCost += cost[r];

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

            
        }*/

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
