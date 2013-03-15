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
            
            //foreach ()
            {

            }
        }
    }
}
