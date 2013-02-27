using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatThingy
{
    class ChatServiceImpl : ChatService
    {
        private ClientService clientService;
        private String name;
        private bool writing;
        private bool displaying;

        public ChatServiceImpl(String name)
        {
            this.name = name;
            writing = false;
            displaying = false;
            Console.WriteLine("Welcome to chat " + name);
        }

        public void start()
        {
            waitInput();
        }

        public void setClientService(ClientService clientService)
        {
            this.clientService = clientService;
        }

        private void changeName(String name)
        {
            this.name = name;
        }

        public void displayIncoming(Message message)
        {
            while (!writing);
            displaying = true;
            if (message != null)
                Console.WriteLine("{0}: {1}", message.getSenderName(), message.getContents());
            displaying = false;
        }

        private void waitInput()
        {
            while (true)
            {
                while (displaying);
                writing = true;
                String line = Console.ReadLine();
                writing = false;
                // if (line.StartsWith("/quit")) clientService.quit();
                if (line.StartsWith("/name")) this.name = line.Substring(6);
                else clientService.SendMessage(new Message(name, line));
                writing = false;
            }
        }
    }
}
