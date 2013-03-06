using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Client
{
    class NetServiceImpl : NetService
    {
        private String tag;
        private int port;
        private int clientport;
        private long discoveryWait;
        private EventHandlerService eventHandler;
        private NetClient client;
        private NetPeerConfiguration config;
        private NetOutgoingMessage outMessage;
        private NetConnection connection;

        public NetServiceImpl()
        {
            JObject constants = JObject.Parse(File.ReadAllText("Content/Json/constants-networking.json"));
            tag = constants.Value<String>("tag");
            port = constants.Value<int>("port");
            clientport = constants.Value<int>("clientport");
            discoveryWait = constants.Value<int>("discoverywait");
            config = new NetPeerConfiguration(tag);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.DisableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = clientport;
            client = new NetClient(config);
            client.Start();
        }

        public void setEventHandler(EventHandlerService eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        public int joinHost(bool local)
        {
            //Console.WriteLine("CLIENT: Discovering");
            if (local) client.DiscoverKnownPeer("127.0.0.1", port);
            else client.DiscoverLocalPeers(port);
            if (waitDiscoveryResponse() == -1)
            {
                Console.WriteLine("Error on wait");
                return -1;
            }
/*            while (client.ConnectionStatus == NetConnectionStatus.InitiatedConnect
                || client.ConnectionStatus == NetConnectionStatus.RespondedAwaitingApproval
                || client.ConnectionStatus == NetConnectionStatus.RespondedConnect);
            if (!(client.ConnectionStatus == NetConnectionStatus.Connected))
                return -1;*/
            Thread messageListenerThread = new Thread(new ThreadStart(listenMessages));
            messageListenerThread.Start();
            while (!messageListenerThread.IsAlive);
            return 0;
        }

        private int waitDiscoveryResponse()
        {
            DateTime start = DateTime.UtcNow;
            NetIncomingMessage inMessage;
            while (DateTime.UtcNow - start < TimeSpan.FromMilliseconds(discoveryWait))
            {
                while ((inMessage = client.ReadMessage()) != null)
                {
                    switch (inMessage.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryResponse:
                            if (inMessage.ReadBoolean())
                            {
                                outMessage = client.CreateMessage();
                                outMessage.Write(client.UniqueIdentifier);
                                outMessage.Write(System.Environment.MachineName);
                                connection = client.Connect(inMessage.SenderEndPoint, outMessage);
                                return 0;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return -1;
        }

        private void listenMessages()
        {
            NetIncomingMessage inMessage;
            while (true)
            {
                if ((inMessage = client.ReadMessage()) != null)
                {
                    switch (inMessage.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("Client: I Got DATA");
                            int type = inMessage.ReadInt32();
                            String message = inMessage.ReadString();
                            Console.WriteLine("Data type: " + type);
                            Console.WriteLine("Message: " + message);
                            eventHandler.handleMessage(message, type);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus) inMessage.ReadByte();
                            if (status == NetConnectionStatus.Disconnecting || status == NetConnectionStatus.Disconnected)
                            {
                                disconnect();
                                eventHandler.handleDisconnect();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public int sendMessage(String message, int type)
        {
            outMessage = client.CreateMessage();
            outMessage.Write(type);
            outMessage.Write(message);
            NetDeliveryMethod method = NetDeliveryMethod.ReliableUnordered;
            client.SendMessage(outMessage, connection, method);
            return 0;
        }

        public int disconnect()
        {
            Console.WriteLine("Client Shutting down...");
            client.Shutdown("");
            return 0;
        }
    }
}
