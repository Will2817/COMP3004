using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    interface MessageSerializerService
    {
        void notifyPlayerJoined(string players);//send a player (all)
        void notifyPlayerDropped(string players);//send a player (all)
        void notifyReadyChanged(string players);//(all)
        /*void notifyAIAdded();//needed?
        void notifyAIDropped();//needed?
        void notifyGameStarted();//no message? (all)
        void assignWonder(Wonder wonder, Player player);//wonder (one)
        void notifyWonderSelected(Wonder wonder, Player player);//wonder (all)
        void confirmWonderSelection(Wonder wonder, Player player);//wonder (one)
        void rejectWonderSelection(Wonder wonder, Player player);//wonder (one)
        void sendHand(List<Card> hand, Player player);//list<card> (one)
        void broadcastTurnResult(GameState gameState);//gamestate (all)
        void broadcastMilitaryResult(Dictionary<Player, int> westResults, Dictionary<Player, int> eastResults);//? (all)
        void broadcastFinalScore(Dictionary<Player, int> scores);//? (all)*/
        void broadcastChatMessage(String message, long senderID);
    }
}
