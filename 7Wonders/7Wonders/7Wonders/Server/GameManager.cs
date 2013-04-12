using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _7Wonders.Server.AI;

namespace _7Wonders.Server
{
    class GameManager
    {
        protected GameState gameState;
        protected Dictionary<long, AIPlayer> aiPlayers;
        protected MessageSerializerService messageSerializer;
        protected NetService netService;
        protected Deck deck;
        protected Dictionary<long, int> pendingTrades;

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
            foreach (KeyValuePair<long, Player> kp in gameState.getPlayers())
            {
                if (!aiPlayers.ContainsKey(kp.Key))
                {
                    kp.Value.setReady(false);
                }
            }
            messageSerializer.notifyPlayerJoined(gameState.lobbyToJson());
        }

        public void addAI(string type)
        {
            Player newAI = new Player(System.DateTime.UtcNow.Ticks, type);
            newAI.setReady(true);
            AIStrategy strategy = null;
            switch (AIPlayer.aiTypes[type])
            {
                case AIPlayer.AIType.CIVILIAN:
                    strategy = new CivilianStrategy();
                    break;
                case AIPlayer.AIType.SCIENCE:
                    strategy = new ScienceStrategy();
                    break;
                case AIPlayer.AIType.MILITARY:
                    strategy = new MilitaryStrategy();
                    break;
                case AIPlayer.AIType.COMMERCE:
                    strategy = new CommerceStrategy();
                    break;
                case AIPlayer.AIType.GREEDY:
                default:
                    strategy = new GreedyStrategy();
                    break;
            }
            //replace 'type' by strategy
            aiPlayers.Add(newAI.getID(), new AIPlayer(strategy, newAI.getID(), this));
            addPlayer(newAI);
        }

        public void playerDropped(long id)
        {
            if (!gameState.isGameInProgress())
            {
                int emptySeat = gameState.getPlayers()[id].getSeat();
                gameState.removePlayer(id);
                foreach (KeyValuePair<long, Player> kp in gameState.getPlayers())
                {
                    if (!aiPlayers.ContainsKey(kp.Key))
                    {
                        kp.Value.setReady(false);
                    }
                }
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
        }

        public void setPlayerReady(long id, bool ready)
        {
            gameState.getPlayers()[id].setReady(ready);
            messageSerializer.notifyReadyChanged(gameState.playersToJson());
        }

        public void setOptions(bool onlySideA, bool assign)
        {
            gameState.setOptions(onlySideA, assign);
            messageSerializer.notifyOptionsChanged(gameState.optionsToJson());
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
            if (gameState.getAssign()) //uncomment these sections when ready to implement selecting a board;
            {
                gameState.clearDiscard();
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
                    p.setReady(false);
                }
                pendingTrades = new Dictionary<long, int>();
                foreach (long id in gameState.getPlayers().Keys) pendingTrades.Add(id, 0);
                gameState.setInProgress(true);

                messageSerializer.notifyWonderAssign(gameState.wonderAssignToJson());
                messageSerializer.broadcastSuperState(gameState.superJson());
                foreach (AIPlayer ai in aiPlayers.Values)
                    ai.init();
                sendHands();
                return 0;
            }
            else
            {
                //handle board picking
                return 0;
            }
        }

        public bool playersReady()
        {
            foreach (Player p in gameState.getPlayers().Values)
                if (!p.getReady()) return false;
            return true;
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
                if (!aiPlayers.ContainsKey(id))
                    messageSerializer.notifyHand(id, gameState.handToJson(id));
            foreach (long id in aiPlayers.Keys)
                aiPlayers[id].selectAction();
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
            pendingTrades[p.getID()] -= westGold + eastGold;
            pendingTrades[west.getID()] += westGold;
            pendingTrades[east.getID()] += eastGold;
            //List of cards the player built this turn
            List<string> playedCards = new List<string>();
            Console.WriteLine("Testing Handled Actions");
            foreach (string c in actions.Keys)
            {
                // Remove card from player's hand
                p.getHand().Remove(c);
                //If the player is building, add the built cards to list
                if (actions[c] == ActionType.BUILD_CARD) playedCards.Add(c);
                // Adding the sold card to the discard pile
                else if (actions[c] == ActionType.SELL_CARD) gameState.addDiscard(c);
            }

            // Setting the players last action/cards played and player to READY
            p.setLastActions(actions.Values.ToList());
            p.setLastCardsPlayed(playedCards);
            setPlayerReady(id, true);

            if (playersReady()) endTurn();
        }

        private void endTurn()
        {
            foreach (Player p in gameState.getPlayers().Values)
            {
                p.setReady(false);
                foreach (string c in p.getLastCardsPlayed())
                {
                    Card card = CardLibrary.getCard(c);
                    p.addPlayed(card);
                    if (card.cost.ContainsKey(Resource.COIN)) p.addResource(Resource.COIN, -1 * card.cost[Resource.COIN]);
                    foreach (Game_Cards.Effect e in card.effects)
                        EffectHandler.ApplyEffect(p, getEastNeighbour(p), getWestNeighbour(p), e);
                }
                foreach (ActionType action in p.getLastActions())
                {
                    switch (action)
                    {
                        case ActionType.BUILD_WONDER:
                            Side boardSide = p.getBoard().getSide();
                            boardSide.stagesBuilt += 1;
                            foreach (Game_Cards.Effect e in boardSide.getStageEffects(boardSide.stagesBuilt - 1))
                                EffectHandler.ApplyEffect(p, getEastNeighbour(p), getWestNeighbour(p), e);
                            break;
                        case ActionType.SELL_CARD:
                            EffectHandler.SellCard(p);
                            break;
                        default:
                            break;
                    }
                }
                p.addResource(Resource.COIN, pendingTrades[p.getID()]);
            }
            List<long> ids = new List<long>();
            ids.AddRange(pendingTrades.Keys);
            foreach (long id in ids) pendingTrades[id] = 0;
            if (gameState.getTurn() == 6)
            {
                foreach (Player p in gameState.getPlayers().Values)
                    gameState.addDiscard(p.getHand()[0]);
                resolveMilitaryConflicts();
                if (gameState.getAge() == 3)
                {
                    endGame();
                    gameState.setInProgress(false);
                    messageSerializer.broadcastSuperState(gameState.superJson());
                    return;
                }
                gameState.incrementAge();
                gameState.resetTurn();
                foreach (Player p in gameState.getPlayers().Values)
                    p.setHand(deck.dealCards(gameState.getAge()));                
            }
            else
            {
                rotateHands();
                gameState.incrementTurn();
            }
            messageSerializer.broadcastSuperState(gameState.superJson());
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
                    hands.Insert(0, lastHand);
                    break;
            }
            for (int i = 0; i < players.Count; i++)
                players[i].setHand(hands[i]);
        }

        private void resolveMilitaryConflicts()
        {
            //calculate and broadcast military conflict results
            foreach (Player p in gameState.getPlayers().Values)
            {
                int eastArmy = getEastNeighbour(p).getScoreNum(Score.ARMY);
                int westArmy = getWestNeighbour(p).getScoreNum(Score.ARMY);

                if (p.getScoreNum(Score.ARMY) > eastArmy)
                    p.addScore(Score.CONFLICT, (gameState.getAge() * 2 ) -1);
                else if (p.getScoreNum(Score.ARMY) < eastArmy)
                {
                    p.addScore(Score.CONFLICT, -1);
                    p.addScore(Score.DEFEAT, 1);
                }

                if (p.getScoreNum(Score.ARMY) > westArmy)
                    p.addScore(Score.CONFLICT, (gameState.getAge() * 2) - 1);
                else if (p.getScoreNum(Score.ARMY) < westArmy)
                {
                    p.addScore(Score.CONFLICT, -1);
                    p.addScore(Score.DEFEAT, 1);
                }
            }
        }

        private void endGame()
        {
            EffectHandler.ApplyEndGameEffect(gameState);
        }
    }
}
