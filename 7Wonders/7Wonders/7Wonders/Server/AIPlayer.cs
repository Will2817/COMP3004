using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server
{
    class AIPlayer
    {
        public enum AIType
    {
        RANDOM,
        MILITARY,
        SCIENCE,
        CIVILIAN,
        COMMERCE
    }
        public static Dictionary<string, AIType> aiTypes = new Dictionary<string, AIType> { 
            {"Random AI", AIType.RANDOM}, 
            {"Military AI", AIType.MILITARY},
            {"Science AI", AIType.SCIENCE},
            {"Civilian AI", AIType.CIVILIAN},
            {"Commerce AI", AIType.COMMERCE} };

        public AIPlayer(String type)
        {

        }
        public void updateGameState(GameState gameState)
        {

        }
    }
}
