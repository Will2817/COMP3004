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
        }

        public void selectAction(GameState gameState)
        {
            Console.WriteLine("AI: Selecting Card");
            this.gameState = gameState;
            Player self = gameState.getPlayers()[id];
            Player west = null;
            Player east = null;
            foreach (Player o in gameState.getPlayers().Values)
            {
                if (o.getSeat() == self.getSeat() - 1 || (self.getSeat() == 0 && o.getSeat() == gameState.getPlayers().Count - 1))
                    west = o;
                if (o.getSeat() == self.getSeat() + 1 || (o.getSeat() == 0 && self.getSeat() == gameState.getPlayers().Count - 1))
                    east =  o;
            }
            Dictionary<string, ActionType> actions = new Dictionary<string, ActionType>();
            foreach (string c in self.getHand())
            {
                Card card = CardLibrary.getCard(c);
                
                if (ConstructionUtils.canChainBuild(self, card) || ConstructionUtils.constructCost(self, west, east, card.cost) == 0)
                {
                    actions.Add(c, ActionType.BUILD_CARD);
                    gameManager.handleActions(id, actions, 0, 0);
                    return;
                }
            }
            actions.Add(self.getHand()[0], ActionType.SELL_CARD);
            gameManager.handleActions(id, actions, 0, 0);
            Console.WriteLine("AI: Actions handled");
        }
    }
}
