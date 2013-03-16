using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Client
{
    class GameManager
    {
        GameState gameState;
        MessageSerializerService messageSerializer;
        NetService netService;
        bool handUpdated;
        bool playerUpdated;
        bool connected;

        public GameManager()
        {
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
            gameState.setHand(netService.getID(), message);
            handUpdated = true;
        }

        //Returns the number of coins it would cost a player to build a card with cardID. -1 if the player cannot possibly build the
        //card (including if he does not have enough coins to purchase the required resources). 0 if the player has the required 
        //resources or a chain. Otherwise the minimum number of coins the player would have to spend to build the card.
        public int constructCost(string cardID)
        {
            Player self = gameState.getPlayers()[netService.getID()];
            Card card = CardLibrary.getCard(cardID);
            if (ConstructionUtils.canChainBuild(self, card)) return 0;
            return ConstructionUtils.constructCost(self, getWestNeighbour(self), getEastNeighbour(self), card.cost);
        }

        public void selectActions(Dictionary<string, ActionType> actions, int westGold, int eastGold)
        {
            JArray actionArray = new JArray();
            foreach (KeyValuePair<string, ActionType> action in actions)
            {
                actionArray.Add(new JObject(new JProperty("card", action.Key),
                                            (new JProperty("action", (int)action.Value))));
            }
            JObject jActions = new JObject(
                new JProperty("actions", actionArray),
                new JProperty("westGold", westGold),
                new JProperty("eastGold", eastGold));
            messageSerializer.sendActions(jActions.ToString());
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
    }
}
