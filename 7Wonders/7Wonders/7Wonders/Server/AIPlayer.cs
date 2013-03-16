using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server
{
    class AIPlayer//Need to add support for multiple strategies by moving card selection (and later, board selection) to another class
    {               //Will have an interface with required methods implemented by a different class for each strategy
        public enum AIType //The constructor will instantiate the correct implementing class based on the given type
        {
            RANDOM,
            MILITARY,
            SCIENCE,
            CIVILIAN,
            COMMERCE
        }
        private GameState gameState;
        private AIType type;
        private long id;
        private GameManager gameManager;

        public static Dictionary<string, AIType> aiTypes = new Dictionary<string, AIType> { 
            {"Random AI", AIType.RANDOM}, 
            {"Military AI", AIType.MILITARY},
            {"Science AI", AIType.SCIENCE},
            {"Civilian AI", AIType.CIVILIAN},
            {"Commerce AI", AIType.COMMERCE} };

        public AIPlayer(String type, long id, GameManager gameManager)
        {
            this.id = id;
            this.type = aiTypes[type];
            this.gameManager = gameManager;
        }
        public void updateGameState(GameState gameState)
        {
            this.gameState = gameState;
            if (gameState.isGameInProgress() && gameState.getPlayers()[id].getReady())
                selectAction();
        }

        private void selectAction()
        {
            Player self = gameState.getPlayers()[id];
            Dictionary<string, ActionType> actions = new Dictionary<string, ActionType>();
            /*
            foreach (string c in self.getHand())
            {
                bool playable = true;
                foreach (Resource r in c.cost.Keys)
                {
                    if (c.cost[r] > self.getResourceNum(r))
                    {
                        playable = false;
                        break;
                    }
                }
                if (playable)
                {
                    actions.Add(c.getImageId(), ActionType.BUILD_CARD);
                    //call game manager action select method thing once done
                    return;
                }
            }*/
            actions.Add(self.getHand()[0], ActionType.SELL_CARD);
            //call game manager action select method thing once done
        }
    }
}
