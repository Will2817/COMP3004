using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server.AI
{
    class MilitaryStrategy
    {
        Dictionary<string, int> buildPriorities;
        Dictionary<string, int> hidePriorities;

        void initBuildPriorities()
        {
            buildPriorities = new Dictionary<string, int>();
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
