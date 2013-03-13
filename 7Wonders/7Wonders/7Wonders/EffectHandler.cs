using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    /* EffectHandler Class
     * This Class has Global Functions!
     * This class will be used to control the effects 
     * of cards along with the effects of Wonders
     */
    public static class EffectHandler
    {
        // Add a certain number of x Resource r to Player p
        // "w", "o", "l", "p" etc
        public static void AddResource(Player p, Resource r, int x)
        {
            int resourceNum = p.getResourceNum(r) + x;
            p.setResourceNum(r, resourceNum);
        }

        public static void AddResourceChoice(Player p, List<Resource> r) { p.addChoices(r); }
        public static void AddResourceUnPurchaseable(Player p, List<Resource> r) { p.addUnpurchasable(r); }

        // Resource choice - "rchoice" in json
        // Adds a temporary Resource r to the players choice Resource Dictionary
        // This might be a little moredifficult to implement... =(
        public static void ChoiceResource(Player p, Resource r, int x)   {   p.addChoiceResosurce(r);  }

        // Science Choice - player chooses which science to gain from the card at the end of the game
        // NOTE: Should we have this as a max function? eg. Find max of gear, tablet, compass and just add 1?
        public static void AddScienceChoice(Player p) 
        {
            int gear = p.getScoreNum(Score.GEAR);
            int compass = p.getScoreNum(Score.COMPASS);
            int tablet = p.getScoreNum(Score.TABLET);
            Score maxScience;

            if (gear > compass)
            {   maxScience = Score.GEAR;

                if (tablet >= gear)
                    maxScience = Score.TABLET;
            }
            else
            {   maxScience = Score.COMPASS;

                if (tablet >= compass)
                    maxScience = Score.TABLET;
            }
            p.addScore(maxScience, 1);        
        }

        // Victory Points or Army or any other score
        // This could probably be used to replace alot of generic score functions
        public static void AddScore(Player p, Score s, int points) { p.addScore(s, points); }

        // Coin awarded with no "basis" expect the construction of the structure
        public static void AddCoin(Player p, int coin) { p.addResource(Resource.COIN, coin); }

        // Coin awarded on the number of wonderstages a player has buit
        public static void AddCoinWonder(Player p, int amount)
        {
            int coin = p.getBoard().getSide().stagesBuilt * amount;
            p.addResource(Resource.COIN, coin);
        }

        // Coin awarded with the basis of Card Colour the Player owns
        public static void AddCoinColour(Player p, CardColour c, int amount)
        {
            int coin = p.getCardColourCount(c) * amount;
            p.setResourceNum(Resource.COIN, coin);
        }

        // Coin awarded from the number of specific structure colour each neighbours have constructed
        public static void AddCoinAllColour(Player p, Player east, Player west, CardColour c, int amount)
        {
            int coin = p.getCardColourCount(c) + east.getCardColourCount(c) + west.getCardColourCount(c);
            coin *= amount;
            p.addResource(Resource.COIN, coin);
        }

        // Victory Points awarded from the number of specific structure colour each neighbours constructed
        public static void AddVictoryNeighboursColour(Player p, Player east, Player west, CardColour c, int amount)
        {
            int points = (east.getCardColourCount(c) + west.getCardColourCount(c)) * amount;
            p.addScore(Score.VICTORY, points);
        }

        // Victory Points awarded 
        // Through the number of specific structure colour the player has constructed
        public static void AddVictoryColour(Player p, CardColour c, int amount)
        {
            int points = p.getCardColourCount(c) * amount;
            p.addScore(Score.VICTORY, points);
        }

        // Victory Points awarded from the number of conflict points each neighbour has
        public static void AddVictoryNeighboursConflict(Player p, Player east, Player west)
        {
            int points = east.getSpecificScore(Score.CONFLICT) + west.getSpecificScore(Score.CONFLICT);
            p.addScore(Score.VICTORY, points);
        }

        // Victory Points awarded from the number of wonderstages built from each neighbour including the player
        public static void AddVictoryWonders(Player p, Player east, Player west)
        {
            int points = p.getBoard().getSide().stagesBuilt;
            points += east.getBoard().getSide().stagesBuilt;
            points += west.getBoard().getSide().stagesBuilt;

            p.addScore(Score.VICTORY, points);
        }

        // Trading Cost for East and West Raw Resources
        public static void SetRawTradeEast(Player p) { p.rcostEast = 1; }
        public static void SetRawTradeWest(Player p) { p.rcostWest = 1; }

        // Trading Cost for East && West of manufactured Resources
        public static void SetManufactedTrade(Player p) { p.mcost = 1; }

    }
}
