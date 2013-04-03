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

        void initBuildPriorities(int players)
        {
            buildPriorities = new Dictionary<string, int>();
            CardColour pref = CardColour.BLUE;
            List<Card> cards = CardLibrary.getCardList(players);
            foreach (Card c in cards)
            {
                if (!buildPriorities.ContainsKey(c.name))
                {

                }
                if (!hidePriorities.ContainsKey(c.name))
                {/*
                    if (c.effects != null)
                        foreach (Effect e in c.effects)
                            if (e.b*/
                }
            }
            //initialize priorities
        }

        void initHidePriorities()
        {
            hidePriorities = new Dictionary<string, int>();
            //initialize priorities
        }

        Dictionary<string, ActionType> chooseActions()
        {
            return new Dictionary<string, ActionType>();
        }

        void chooseTrades()
        {
            //decide how to trade
            //possibly should be part of chooseActions
            //need a way to return trade values
        }
    }
}
