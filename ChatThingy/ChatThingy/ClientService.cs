﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatThingy
{
    interface ClientService
    {
        void SendMessage(Message message);
    }
}
