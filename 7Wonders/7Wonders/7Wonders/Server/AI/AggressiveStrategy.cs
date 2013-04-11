using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _7Wonders.Game_Cards;

namespace _7Wonders.Server.AI
{
    class AggressiveStrategy : AIStrategy
    {
        Dictionary<string, int> buildPriorities;
        Dictionary<string, int> hidePriorities;

        public void initPriorities(int players)
        {

        }

        public void initBuildPriorities()
        {
            buildPriorities = new Dictionary<string, int>();
            //initialize priorities
        }

        public void initHidePriorities()
        {
            hidePriorities = new Dictionary<string, int>();
            //initialize priorities
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
