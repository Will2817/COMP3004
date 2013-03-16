using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Server
{
    class GameManager
    {
        protected GameState gameState;
        protected Dictionary<long, AIPlayer> aiPlayers;
        protected MessageSerializerService messageSerializer;
        protected NetService netService;
        protected Deck deck;
        protected CardLibrary cards;
        protected List<Card> discards;

        public GameManager()
        {
            gameState = new GameState();
            aiPlayers = new Dictionary<long, AIPlayer>();
        }

        public void addPlayer(Player _player)
        {
            _player.setSeat(gameState.getPlayers().Count);//seat #ing starts at 0
            gameState.addPlayer(_player);
            if (gameState.getPlayers().Count() == Game1.MAXPLAYER)
            {
                netService.blockConnections();
            }
            messageSerializer.notifyPlayerJoined(gameState.lobbyToJson());
            updateAIs();
        }

        public void addAI(string type)
        {
            Player newAI = new Player(System.DateTime.UtcNow.Ticks, type);
            newAI.setReady(true);
            aiPlayers.Add(newAI.getID(), new AIPlayer(type, newAI.getID(), this));
            addPlayer(newAI);
        }

        public void playerDropped(long id)
        {
            if (!gameState.isGameInProgress())
            {
                int emptySeat = gameState.getPlayers()[id].getSeat();
                gameState.removePlayer(id);
                adjustSeats(emptySeat);
            }
            else
            {
                //Handle in-game player drop here
            }
        }

        public void bootPlayerInSeat(int seatNumber)
        {
            long id = 0;
            foreach (Player p in gameState.getPlayers().Values)
                if (p.getSeat() == seatNumber)
                {
                    id = p.getID();
                    break;
                }
            gameState.removePlayer(id);
            if (aiPlayers.ContainsKey(id)) aiPlayers.Remove(id);
            adjustSeats(seatNumber);
        }

        private void adjustSeats(int seatNumber)
        {
            foreach (Player p in gameState.getPlayers().Values)
                if (p.getSeat() > seatNumber) p.setSeat(p.getSeat() - 1);
            messageSerializer.notifyPlayerDropped(gameState.playersToJson());
            updateAIs();
        }

        public void setPlayerReady(long id, bool ready)
        {
            gameState.getPlayers()[id].setReady(ready);
            messageSerializer.notifyReadyChanged(gameState.playersToJson());
            updateAIs();
        }

        public void setOptions(bool onlySideA, bool assign)
        {
            gameState.setOptions(onlySideA, assign);
            messageSerializer.notifyOptionsChanged(gameState.optionsToJson());
            updateAIs();
        }

        public int startGame()
        {
      //      if (gameState.getAssign()) //uncomment these sections when ready to implement selecting a board;
      //      {
                deck = new Deck(gameState.getPlayers().Count);
                discards = new List<Card>();

                foreach (Player p in gameState.getPlayers().Values)
                {
                    p.setHand(deck.dealCards(1));
                    List<Wonder> wonders = new List<Wonder>();
                    wonders.AddRange(gameState.getWonders().Values);
                    Random rand = new Random();
                    int i;
                    do 
                    {
                        i = rand.Next(wonders.Count);
                    } 
                    while (wonders[i].isInUse());
                    int side = rand.Next(2);
                    if (gameState.getOnlySideA() || side == 0) wonders[i].setSideA();
                    else wonders[i].setSideB();
                    wonders[i].setInUse(true);
                    p.setBoard(wonders[i]);
                    p.addResource(wonders[i].getSide().getIntialResource(), 1);
                    p.addResource(Resource.COIN, 3);
                }
                messageSerializer.notifyWonderAssign(gameState.wonderAssignToJson());
                messageSerializer.broadcastSuperState(gameState.superJson());
                sendHands();
                updateAIs();
                
                return 0;
     /*       }
            else
            {
                //handle board picking
                return 0;
            }*/
        }

        public bool playersReady()
        {
            foreach (Player p in gameState.getPlayers().Values)
                if (!p.getReady()) return false;
            return true;
        }

        private void updateAIs()
        {
            foreach (AIPlayer ai in aiPlayers.Values) ai.updateGameState(gameState);
        }

        public GameState getGameState() { return gameState; }

        public void setMessageSerializer(MessageSerializerService messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public void setNetService(NetService netService) { this.netService = netService; }

        public void sendHands()
        {
            foreach (long id in gameState.getPlayers().Keys)
            {
                if (!aiPlayers.ContainsKey(id))
                    messageSerializer.notifyHand(id, gameState.handToJson(id));
            }
        }

        public void handleActions(long id, Dictionary<string, ActionType> actions)
        {
            // Checks if actions are valid and updates the player/discard pile/etc
            // set the ready flag for that player so that it can check whether all players are ready and
            // broadcasts the results of the turn
            Player p = gameState.getPlayers()[id];
            List<string> playedCards = new List<string>();
            List<ActionType> playedActions = new List<ActionType>();
            
            Console.WriteLine("Testing Handled Actions");
            foreach (KeyValuePair<string, ActionType> action in actions)
            {
                string card = action.Key;
                Card c = cards.getCard(card); 

                switch (action.Value)
                {                       
                    case ActionType.BUILD_CARD:
                        if (!p.getReady())
                        {
                            // Consider validating move/checking whether player has already played card.

                            bool freeConstruction = false;
                            // Check if the card has a chain, and if so has the chain card been built yet
                            /*foreach (Card _checkChain in p.getPlayed())
                            {
                                foreach (string chain in _checkChain.chains)
                                {
                                    if (chain.Equals(c.name))
                                    {
                                        freeConstruction = true;
                                        break;
                                    }
                                }
                                if (freeConstruction)
                                    break; // Stops foreach loop as soon as freeConstruction is true
                            }*/
                            

                                // Take into account coin costs and deduct it
                                if (!freeConstruction && c.cost.ContainsKey(Resource.COIN))
                                    p.addResource(Resource.COIN, (-1 * c.cost[Resource.COIN]));
                                
                                // Add to list of lastPlayedCards and lastActions
                                playedCards.Add(card);
                                playedActions.Add(ActionType.BUILD_CARD);

                                // Remove card from player's hand
                                p.getHand().Remove(card);

                                // Play card and update the # of card colour 
                                p.addPlayed(c);

                                // This is broken, we need to find a way to apply effects that are need to be applied at the end of the game
                                // maybe have a list of effects that would be run at the end of the game?
                                foreach (Game_Cards.Effect e in c.effects)
                                    EffectHandler.ApplyEffect(gameState, p, e);                        
                        }
                        else
                            Console.WriteLine(id + ": Cannot BUILD_CARD, already marked as ready");
                        break;

                    case ActionType.BUILD_WONDER:
                        if (!p.getReady())
                        {
                            Console.WriteLine(id + ": BUILDING WONDER" + action.Key);
                            Side pBoard = p.getBoard().getSide();
                            //Consider checking whether player has stages left to build/can afford to build next stage
                                // Add to list of lastActions
                                playedActions.Add(ActionType.BUILD_WONDER);

                                // Build Board and place effect into players effect list                      
                                pBoard.stagesBuilt += 1;

                                // Must take into account freebuild still or anything specific to wonders atm
                                foreach (Game_Cards.Effect e in pBoard.getStageEffects(pBoard.stagesBuilt))
                                    EffectHandler.ApplyEffect(gameState, p, e);
                        }
                        else
                            Console.WriteLine(id + ": Cannot BUILD_WONDER, already marked as ready");
                        break;

                    case ActionType.SELL_CARD:
                        if (!p.getReady())//need to move already ready check out of case statement
                        {
                                Console.WriteLine(id + ": DISCARDING " + action.Key);
                                // Add to list of lastPlayedCards and lastActions
                                playedCards.Add(action.Key);
                                playedActions.Add(ActionType.SELL_CARD);

                                // Setting Hand with the card removed and in play
                                p.getHand().Remove(card);

                                // Adding the sold card to the discard pile
                                discards.Add(c);
                                EffectHandler.SellCard(p);               
                        }
                        else
                            Console.WriteLine(id + ": Cannot SELL_CARD, already marked as ready");
                        break;

                    default:
                        Console.WriteLine("Action Error: " + action.Value);
                        break;
                } // End switch
            } // End foreach

            // Setting the players last action/cards played and player to READY
            p.setLastActions(playedActions);
            p.setLastCardsPlayed(playedCards);
            setPlayerReady(id, true);
        }
    }
}
