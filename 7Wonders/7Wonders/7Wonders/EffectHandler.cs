using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{

    public class EffectHandler
    {
        // Add a certain number of x Resource r to Player p
        // "w", "o", "l", "p" etc
        public static void AddResource(Player p, Resource r, int x)
        {
            int resourceNum = p.getResourceNum(r) + x;
            p.setResourceNum(r, resourceNum);
        }

        // Resource choice - "rchoice" in json
        // Adds a temporary Resource r to the players choice Resource Dictionary
        public static void ChoiceResource(Player p, Resource r, int x)
        {
            p.addChoiceResosurce(r, x);
        }

        // Victory Points added from neighbours for card Colour
        public static void addVictoryNeighbours(Player p, Player east, Player west, CardColour c, int victoryPoints)
        {
            int ePoints = east.getCardColourCount(c) * victoryPoints;
            int wPoints = west.getCardColourCount(c) * victoryPoints;

            p.addScore(Score.VICTORY, (ePoints + wPoints));
        }
    }
}
