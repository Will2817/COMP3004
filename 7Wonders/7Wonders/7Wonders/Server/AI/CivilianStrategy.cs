using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _7Wonders.Game_Cards;

namespace _7Wonders.Server.AI
{
    class CivilianStrategy : AIStrategy
    {
        Dictionary<string, int> buildPriorities;
        Dictionary<string, int> hidePriorities;

        public void initPriorities(int players)
        {
            buildPriorities = new Dictionary<string, int>();
            hidePriorities = new Dictionary<string, int>();
            CardColour pref = CardColour.BLUE;
            List<CardColour> secondPrefs = new List<CardColour> { CardColour.BROWN, CardColour.GRAY };
            List<Card> cards = CardLibrary.getCardList(players);
            int p;
            foreach (Card c in cards)
            {
                if (c.effects == null) break;
                if (!buildPriorities.ContainsKey(c.name))
                {
                    p = 0;
                    if (c.colour == pref)
                        p += c.effects[0].amount;
                    else if (secondPrefs.Contains(c.colour))
                        p += c.effects[0].amount;
                    else if (c.effects[0].type == Effect.TypeType.RCOSTEAST || c.effects[0].type == Effect.TypeType.RCOSTWEST || c.effects[0].type == Effect.TypeType.MCOST)
                        p += 2;
                    
                    buildPriorities.Add(c.name, p);
                }
                if (!hidePriorities.ContainsKey(c.name))
                {
                    p = 0;
                    hidePriorities.Add(c.name, p);
                }
            }
            //initialize priorities
        }

        private int calcBuildPriority(Card c)
        {
            if (c.effects == null) return 0;
            int p = 0;
            if (!buildPriorities.ContainsKey(c.name))
            {
                p = 0;
                if (c.colour == CardColour.BLUE)
                    p += c.effects[0].amount;
                else if (c.colour == CardColour.BROWN || c.colour == CardColour.GRAY)
                    p += 1;
                else if (c.effects[0].type == Effect.TypeType.RCOSTEAST || c.effects[0].type == Effect.TypeType.RCOSTWEST || c.effects[0].type == Effect.TypeType.MCOST)
                    p += 2;

                buildPriorities.Add(c.name, p);
            }
            if (!hidePriorities.ContainsKey(c.name))
            {
                p = 0;
                hidePriorities.Add(c.name, p);
            }

            return p;
        }

        public Dictionary<string, ActionType> chooseActions()
        {
            return new Dictionary<string, ActionType>();
        }

        public void chooseTrades()
        {
            //decide how to trade
            //possibly should be part of chooseActions
            //need a way to return trade values
        }
    }
}
