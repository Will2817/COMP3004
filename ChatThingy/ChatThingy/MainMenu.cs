using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ChatThingy
{
    class MainMenu
    {
        private const String LOCALHOST = "127.0.0.1";
        private String name;
        private String ip;

        public MainMenu()
        {
            Console.WriteLine("Enter an option");
            Console.WriteLine("/name <name> to set display name");
            Console.WriteLine("/host to begin hosting chat");
            Console.WriteLine("/join <ip> to join a chat at specified ip");
            Console.WriteLine("/quit to quit");
            while (true)
            {
                string line = Console.ReadLine();
                if (line.StartsWith("/quit"))
                {
                    return;
                }
                else if (line.StartsWith("/name"))
                {
                    name = line.Substring(6);
                }
                else if (line.StartsWith("/host"))
                {
                    ip = LOCALHOST;
                    Thread hostThread = new Thread(new ThreadStart(startHost));
                    hostThread.Start();
                    while (!hostThread.IsAlive);
                    new ClientServiceImpl(ip, new ChatServiceImpl(name));
                    break;
                }
                else if (line.StartsWith("/join"))
                {
                    ip = line.Substring(6);
                    new ClientServiceImpl(ip, new ChatServiceImpl(name));
                    break;
                }
                else
                {
                    Console.WriteLine("Not a valid command");
                }

            }
        }

        private void startHost()
        {
            new HostServiceImpl();
        }
    }
}
