using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders
{
    public enum ClientMessageType
    {
        READY,
        NOT_READY,
        BOARD_SELECTION,
        TURN_ACTION,
        CHAT_MESSAGE
    }
}
