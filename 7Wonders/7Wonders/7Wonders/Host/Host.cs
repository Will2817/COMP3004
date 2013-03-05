using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7Wonders.Host
{
    class Host
    {

        public Host()
        {
            GameManager gameManager = new GameManager();
            EventHandlerService netEventHandlerService = new EventHandlerServiceImpl(gameManager);
            NetService netService = new NetServiceImpl(netEventHandlerService);
        }
    }
}
