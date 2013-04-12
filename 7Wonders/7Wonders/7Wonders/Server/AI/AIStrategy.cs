using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server.AI
{
    public interface AIStrategy
    {
        void initPriorities(int players, Player self, Player east, Player west, List<Player> opponents);
        void chooseActions(Dictionary<string, ActionType> outActions, List<int> outTrades);
    }
}
