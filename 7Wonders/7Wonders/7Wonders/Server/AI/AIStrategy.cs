using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server.AI
{
    public interface AIStrategy
    {
        void initPriorities(int players);
        Dictionary<string, ActionType> chooseActions();
        void chooseTrades();
    }
}
