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
                switch (action.Value)
                {
                    case ActionType.BUILD_CARD:
                        if (!p.getReady())
                        {
                            Card c = cards.getCard(action.Key);                            
                            // If able to purchase and card hasn't been built yet
                            if (p.canPurchase(c.cost) && !p.cardPlayed(c))
                            {
                                // Add to list of lastPlayedCards and lastActions
                                playedCards.Add(action.Key);
                                playedActions.Add(ActionType.BUILD_CARD);

                                // Setting Hand with the card removed and in play
                                List<Card> newHand = p.getHand();
                                newHand.Remove(c);
                                p.setHand(newHand);

                                // Play card and update the # of card colour 
                                p.addPlayed(c);

                                // This is broken, we need to find a way to apply effects that are need to be applied at the end of the game
                                // maybe have a list of effects that would be run at the end of the game?
                                foreach (Game_Cards.Effect e in c.effects)
                                    EffectHandler.ApplyEffect(p, e);
                            }
                            else
                                Console.WriteLine(id + ": Cannot build Card: " + action.Key);                           
                        }
                        else
                            Console.WriteLine(id + ": Cannot BUILD_CARD, already marked as ready");
                        break;

                    case ActionType.BUILD_WONDER:
                        if (!p.getReady())
                        {
                            Side pBoard = p.getBoard().getSide();
                            // If player has wonders left to build and hasthe resources, then build
                            if (pBoard.stagesBuilt < pBoard.getStageNum() && p.canPurchase(pBoard.getStageCost(pBoard.stagesBuilt + 1)))
                            {
                                // Add to list of lastPlayedCards and lastActions
                                playedCards.Add(action.Key);
                                playedActions.Add(ActionType.BUILD_WONDER);

                                // Build Board and place effect into players effect list                      
                                pBoard.stagesBuilt += 1;

                                // Must take into account freebuild still or anything specific to wonders atm
                                foreach (Game_Cards.Effect e in pBoard.getStageEffects(pBoard.stagesBuilt))
                                    EffectHandler.ApplyEffect(p, e);
                            }
                            else
                                Console.WriteLine(id + ": Cannot build Wonder [Max Wonder stage] OR [Not enough resources]");
                        }
                        else
                            Console.WriteLine(id + ": Cannot BUILD_WONDER, already marked as ready");
                        break;

                    case ActionType.SELL_CARD:
                        if (!p.getReady())
                        {
                            Card c = cards.getCard(action.Key); // Getting the Card

                            // Add to list of lastPlayedCards and lastActions
                            playedCards.Add(action.Key);
                            playedActions.Add(ActionType.SELL_CARD);

                            // Setting Hand with the card removed and in play
                            List<Card> newHand = p.getHand();
                            newHand.Remove(c);
                            p.setHand(newHand);
                            
                            // Need to place card into a discard pile
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
