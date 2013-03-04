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
            NetEventHandlerService netEventHandlerService = new NetEventHandlerServiceImpl(gameManager);
            NetService netService = new NetServiceImpl(netEventHandlerService);
        }
    }
}
