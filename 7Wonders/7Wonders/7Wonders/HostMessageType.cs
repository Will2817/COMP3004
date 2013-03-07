using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    public enum HostMessageType
    {
        PLAYER_JOINED,
        PLAYER_DROPPED,
        READY_CHANGED,
        SIDE_CHANGED,
        GAME_STARTED,
        ASSIGN_WONDER,
        RESERVE_WONDER,
        CONFIRM_WONDER,
        REJECT_WONDER,
        HAND,
        TURN_RESULT,
        MILITARY_RESULT,
        FINAL_SCORE,
        CHAT_MESSAGE
    }
}
