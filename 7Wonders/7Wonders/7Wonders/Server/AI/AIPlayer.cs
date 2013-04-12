using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server.AI
{
    class AIPlayer//Need to add support for multiple strategies by moving card selection (and later, board selection) to another class
    {               //Will have an interface with required methods implemented by a different class for each strategy
        public enum AIType //The constructor will instantiate the correct implementing class based on the given type
        {
            GREEDY,
            MILITARY,
            SCIENCE,
            CIVILIAN,
            COMMERCE
        }
        private GameState gameState;
        private long id;
        private GameManager gameManager;
        private AIStrategy strategy;

        public static Dictionary<string, AIType> aiTypes = new Dictionary<string, AIType> { 
            {"Random AI", AIType.GREEDY}, 
            {"Military AI", AIType.MILITARY},
            {"Science AI", AIType.SCIENCE},
            {"Civilian AI", AIType.CIVILIAN},
            {"Commerce AI", AIType.COMMERCE} };

        public AIPlayer(AIStrategy strategy, long id, GameManager gameManager)
        {
            this.id = id;
            this.strategy = strategy;
            this.gameManager = gameManager;
            this.gameState = gameManager.getGameState();
        }

        public void init()
        {
            Player self = gameState.getPlayers()[id];
            Player west = null;
            Player east = null;
            List<Player> opponents = new List<Player>();
            foreach (Player o in gameState.getPlayers().Values)
            {
                if (o.getID() != id) opponents.Add(o);
                if (o.getSeat() == self.getSeat() - 1 || (self.getSeat() == 0 && o.getSeat() == gameState.getPlayers().Count - 1))
                    west = o;
                if (o.getSeat() == self.getSeat() + 1 || (o.getSeat() == 0 && self.getSeat() == gameState.getPlayers().Count - 1))
                    east = o;
            }
            strategy.initPriorities(gameState.getPlayers().Count, self, east, west, opponents);
        }

        public void selectAction()
        {
            Dictionary<string, ActionType> actions = new Dictionary<string,ActionType>();
            List<int> trades = new List<int>();
            strategy.chooseActions(actions, trades);
            int westGold = 0;
            int eastGold = 0;
            gameManager.handleActions(id, actions, westGold, eastGold);
        }
    }
}
