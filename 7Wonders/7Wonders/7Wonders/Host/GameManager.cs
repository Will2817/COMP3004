using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    class GameManager
    {
        GameState gameState;
        List<AIPlayer> aiPlayers;
        MessageSerializerService messageSerializer;
        NetService netService;

        public GameManager()
        {
            gameState = new GameState();
            aiPlayers = new List<AIPlayer>();
        }

        public GameState getState()
        {
            return gameState;
        }

        public void setMessageSerializer(MessageSerializerService messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public void setNetService(NetService netService)
        {
            this.netService = netService;
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
            Console.WriteLine("Start Players:");
            foreach (Player p in gameState.getPlayers().Values)
            {
                Console.WriteLine(p.getName() + ", " + p.getID());
            }
            Console.WriteLine("End Players...");
        }

        public void addAI(AIType type)
        {
            string name;
            switch(type)
            {
                case AIType.CIVILIAN:
                    name = "Civilian AI";
                    break;
                case AIType.COMMERCE:
                    name = "Commerce AI";
                    break;
                case AIType.MILITARY:
                    name = "Military AI";
                    break;
                case AIType.RANDOM:
                    name = "Random AI";
                    break;
                case AIType.SCIENCE:
                    name = "Science AI";
                    break;
                default:
                    name = "AI Player";
                    break;
            }
            Player newAI = new Player(Convert.ToInt64(System.DateTime.UtcNow), name);
            aiPlayers.Add(new AIPlayer(type));
            addPlayer(newAI);
        }

        public void removePlayer(long id)
        {
            if (!gameState.isGameInProgress())
            {
                int emptySeat = gameState.getPlayers()[id].getSeat();
                gameState.removePlayer(id);
                foreach (Player p in gameState.getPlayers().Values)
                {
                    if (p.getSeat() > emptySeat)
                    {
                        p.setSeat(p.getSeat() - 1);
                    }
                }
                messageSerializer.notifyPlayerDropped(gameState.playersToJson());
                updateAIs();
            }
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

        private void updateAIs()
        {
            foreach (AIPlayer ai in aiPlayers)
            {
                ai.updateGameState(gameState);
            }
        }

    }
}
