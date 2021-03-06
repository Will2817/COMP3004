﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Server
{
    class EventHandlerServiceImpl : EventHandlerService
    {
        private GameManager gameManager;

        public void setGameManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void handleNewClient(long clientID, string name)
        {
            Player newPlayer = new Player(clientID, name);
            gameManager.addPlayer(newPlayer);
        }

        public void handleClientDrop(long clientID)
        {
            gameManager.playerDropped(clientID);
        }

        public void handleMessage(String message, int type, long clientID)
        {
            switch ((ClientMessageType)type)
            {
                case ClientMessageType.READY_CHANGED:
                    gameManager.setPlayerReady(clientID, Boolean.Parse(message));
                    break;
                case ClientMessageType.BOARD_SELECTION:
                    //stuff
                    break;
                case ClientMessageType.TURN_ACTION:
                    parseActions(clientID, message);
                    break;
                case ClientMessageType.CHAT_MESSAGE:
                    //stuff
                    break;
                default:
                    break;
            }
        }

        public void parseActions(long clientID, string message)
        {
            Dictionary<string, ActionType> actions = new Dictionary<string, ActionType>();
            JObject jMessage = JObject.Parse(message);
            JArray actionArray = (JArray)jMessage["actions"];
            foreach (JObject obj in actionArray)
                actions.Add((string )obj["card"], (ActionType) (int) obj["action"]);
            int westGold = (int)jMessage["westGold"];
            int eastGold = (int)jMessage["eastGold"];
            gameManager.handleActions(clientID, actions, westGold, eastGold);
        }

    }
}
