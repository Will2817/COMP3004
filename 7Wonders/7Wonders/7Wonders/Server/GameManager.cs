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

        public Player getWestNeighbour(Player p)
        {
            foreach (Player o in gameState.getPlayers().Values)
            {
                if (o.getSeat() == p.getSeat() - 1 || (p.getSeat() == 0 && o.getSeat() == gameState.getPlayers().Count - 1))
                    return o;
            }
            return null;
        }

        public Player getEastNeighbour(Player p)
        {
            foreach (Player o in gameState.getPlayers().Values)
            {
                if (o.getSeat() == p.getSeat() + 1 || (o.getSeat() == 0 && p.getSeat() == gameState.getPlayers().Count - 1))
                    return o;
            }
            return null;
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
                    p.setReady(false);
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
                else
                    aiPlayers[id].selectAction(gameState);
            }
        }

        public void handleActions(long id, Dictionary<string, ActionType> actions, int westGold, int eastGold)
        {
            // Checks if actions are valid and updates the player/discard pile/etc
            // set the ready flag for that player so that it can check whether all players are ready and
            // broadcasts the results of the turn
            Player p = gameState.getPlayers()[id];
            if (p.getReady()) return;
            Player west = null;
            Player east = null;
            foreach (Player o in gameState.getPlayers().Values)
            {
                if (o.getSeat() == p.getSeat() - 1 || (p.getSeat() == 0 && o.getSeat() == gameState.getPlayers().Count - 1))
                    west = o;
                if (o.getSeat() == p.getSeat() + 1 || (o.getSeat() == 0 && p.getSeat() == gameState.getPlayers().Count - 1))
                    east =  o;
            }
            west.addResource(Resource.COIN, westGold);
            east.addResource(Resource.COIN, eastGold);
            p.addResource(Resource.COIN, -1 * (westGold + eastGold));
            List<string> playedCards = new List<string>();
            List<ActionType> playedActions = new List<ActionType>();
            Console.WriteLine("Testing Handled Actions");
            foreach (KeyValuePair<string, ActionType> action in actions)
            {
                string card = action.Key;
                Card c = CardLibrary.getCard(card);
                switch (action.Value)
                {                       
                    case ActionType.BUILD_CARD:
                        // Consider validating move/checking whether player has already played card.
                                
                        // Add to list of lastPlayedCards and lastActions
                        playedCards.Add(card);
                        playedActions.Add(ActionType.BUILD_CARD);

                        // Remove card from player's hand
                        p.getHand().Remove(card);

                        // Play card and update the # of card colour 
                        p.addPlayed(c);

                        // Only applies instant effects of cards, such as victory points, coins,
                        // resource choices, army, trade and science
                        foreach (Game_Cards.Effect e in c.effects)
                            EffectHandler.ApplyEffect(gameState, p, e);
                        break;

                    case ActionType.BUILD_WONDER:
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
                        break;

                    case ActionType.SELL_CARD:
                        Console.WriteLine(id + ": DISCARDING " + action.Key);
                        // Add to list of lastActions
                        playedActions.Add(ActionType.SELL_CARD);

                        // Setting Hand with the card removed and in play
                        p.getHand().Remove(card);

                        // Adding the sold card to the discard pile
                        discards.Add(c);
                        EffectHandler.SellCard(p);
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

            if (playersReady()) endTurn();
        }

        private void endTurn()
        {
            if (gameState.getTurn() == 6)
            {
                if (gameState.getAge() == 3)
                {
                    endGame();
                    return;
                }
                gameState.incrementAge();
                gameState.resetTurn();
                foreach (Player p in gameState.getPlayers().Values)
                    p.setHand(deck.dealCards(1));
                resolveMilitaryConflicts();
            }
            else
            {
                messageSerializer.broadcastSuperState(gameState.superJson());
                rotateHands();
                gameState.incrementTurn();
            }
            sendHands();
        }

        private void rotateHands()
        {
            List<Player> players = gameState.getPlayers().Values.ToList();
            players.Sort((p, q) => p.getSeat().CompareTo(q.getSeat()));
            List<List<string>> hands = new List<List<string>>();
            foreach (Player p in players)
                hands.Add(p.getHand());
            switch (gameState.getAge())
            {
                case 1:
                case 3:
                    List<string> firstHand = hands.First();
                    hands.RemoveAt(0);
                    hands.Add(firstHand);
                    break;
                case 2:
                    List<string> lastHand = hands.Last();
                    hands.RemoveAt(hands.Count - 1);
                    hands.Add(lastHand);
                    break;
            }
            for (int i = 0; i < players.Count; i++)
                players[i].setHand(hands[i]);
        }

        private void resolveMilitaryConflicts()
        {
            //calculate and broadcast military conflict results
        }

        private void endGame()
        {

        }
    }
}
