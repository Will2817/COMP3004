using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Client
{
    class GameManager
    {
        GameState gameState;
        MessageSerializerService messageSerializer;
        NetService netService;
        CardLibrary cardLibrary;
        bool handUpdated;
        bool playerUpdated;
        bool connected;

        public GameManager()
        {
            cardLibrary = new CardLibrary();
            gameState = new GameState();
            connected = false;
            handUpdated = false;
            playerUpdated = false;
        }

        public void setMessageSerializer(MessageSerializerService messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public void setNetService(NetService netService)
        {
            this.netService = netService;
        }

        public void updateLobby(string lobby)
        {
            gameState.lobbyFromJson(lobby);
            playerUpdated = true;
        }

        public void updatePlayers(string players)
        {
            gameState.playersFromJson(players);
            playerUpdated = true;
        }

        public void updateOptions(string options)
        {
            gameState.optionsFromJson(options);
            playerUpdated = true;
        }

        public void assignWonders(string json)
        {
            gameState.assignWonders(json);
            playerUpdated = true;
        }

        public void superUpdate(string json)
        {
            gameState.superParse(json);
            playerUpdated = true;
        }

        public GameState getGameState()
        {
            return gameState;
        }

        public bool isUpdateAvailable()
        {
            return playerUpdated || handUpdated;
        }

        public bool isHandUpdated()
        {
            return handUpdated;
        }

        public bool isPlayerUpdated()
        {
            return playerUpdated;
        }

        public void setHandChecked()
        {
            handUpdated = false;
        }

        public void setPlayerChecked()
        {
            playerUpdated = false;
        }

        public bool isConnected()
        {
            return connected;
        }

        public void disconnected()
        {
            connected = false;
        }

        public void setConnected()
        {
            connected = true;
        }

        public void setReady(bool ready)
        {
            messageSerializer.notifyReadyChanged(ready);
        }

        public void assignHand(string message)
        {
            gameState.setHand(cardLibrary, netService.getID(), message);
            handUpdated = true;
        }

        //Returns the number of coins it would cost a player to build a card with cardID. -1 if the player cannot possibly build the
        //card (including if he does not have enough coins to purchase the required resources). 0 if the player has the required 
        //resources or a chain. Otherwise the minimum number of coins the player would have to spend to build the card. The player 
        //could choose to pay more by purchasing resources from a neighbour the player does not have a discount with as opposed to one 
        //he does have a discount with (e.g. out of spite).
        public int constructCost(string cardID)
        {
            Player self = gameState.getPlayers()[netService.getID()];
            Player west = getWestNeighbour(self);
            Player east = getEastNeighbour(self);
            Card card = cardLibrary.getCard(cardID);
            foreach (string cid in card.chains)
            {
                Card c = cardLibrary.getCard(cid);
                if (self.getPlayed().Contains(c)) return 0;//Building from chain is free
            }
            Dictionary<Resource, int> cost = card.cost;
            int coinCost = 0;
            foreach (Resource r in cost.Keys)//Construction cost calculation currently does not include choices or cost reductions
            {                                   //as these have not been implemented yet
                if (cost[r] <= self.getResourceNum(r)) continue;
                if (cost[r] > self.getResourceNum(r) + west.getResourceNum(r) + east.getResourceNum(r)) return -1;
                coinCost += 2 * (cost[r] - self.getResourceNum(r));
            }
            return (coinCost > self.getResourceNum(Resource.COIN))?-1:coinCost;
        }

        private Player getWestNeighbour(Player p)
        {
            foreach (Player o in gameState.getPlayers().Values)
            {
                if (o.getSeat() == p.getSeat() - 1 || (p.getSeat() == 0 && o.getSeat() == gameState.getPlayers().Count - 1))
                    return o;
            }
            return null;
        }

        private Player getEastNeighbour(Player p)
        {
            foreach (Player o in gameState.getPlayers().Values)
            {
                if (o.getSeat() == p.getSeat() + 1 || (o.getSeat() == 0 && p.getSeat() == gameState.getPlayers().Count - 1))
                    return o;
            }
            return null;
        }
    }
}
