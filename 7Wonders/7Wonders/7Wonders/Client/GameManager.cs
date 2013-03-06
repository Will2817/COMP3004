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
        bool updateAvailable;
        bool connected;
        bool ready;

        public GameManager()
        {
            gameState = new GameState();
            connected = true;
            ready = false;
        }

        public void setMessageSerializer(MessageSerializerService messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public void setNetService(NetService netService)
        {
            this.netService = netService;
        }

        public void updatePlayers(string players)
        {
            gameState.playersFromJson(players);
            updateAvailable = true;
        }

        public GameState getGameState()
        {
            return gameState;
        }

        public bool isUpdateAvailable()
        {
            return updateAvailable;
        }

        public void setUpdateChecked()
        {
            updateAvailable = false;
        }

        public bool isConnected()
        {
            return connected;
        }

        public void disconnected()
        {
            connected = false;
        }

        public void setReady(bool ready)
        {
            this.ready = ready;
            messageSerializer.notifyReadyChanged(ready);
        }
    }
}
