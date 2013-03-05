using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Host
{
    class NetServiceImpl : NetService
    {
        private String tag;
        private int port;
        private int clientport;
        private EventHandlerService eventHandler;
        private NetServer server;
        private NetPeerConfiguration config;
        private NetOutgoingMessage outMessage;
        private Dictionary<long, NetConnection> connections;
        private Boolean acceptingClients;

        public NetServiceImpl()
        {
            JObject constants = JObject.Parse(File.ReadAllText("Content/Json/constants-networking.json"));
            tag = constants.Value<String>("tag");
            port = constants.Value<int>("port");
            clientport = constants.Value<int>("clientport");
            config = new NetPeerConfiguration(tag);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.Port = port;
            server = new NetServer(config);
            server.Start();
            outMessage = server.CreateMessage();
            connections = new Dictionary<long, NetConnection>();
            acceptingClients = true;
            Thread messageListener = new Thread(new ThreadStart(listenMessages));
            messageListener.Start();
        }

        public void setEventHandler(EventHandlerService eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        private void listenMessages()
        {
            Console.WriteLine("SERVER: Server listening");
            //should probably have some sort of thread pool
            NetIncomingMessage inMessage;
            while (true)
            {
                if ((inMessage = server.ReadMessage()) != null)
                {
                    Console.WriteLine(inMessage.MessageType.ToString());
                    switch (inMessage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            Console.WriteLine("SERVER: Approve a connection");
                            if (acceptingClients)
                            {
                                inMessage.SenderConnection.Approve();
                                long clientID = inMessage.ReadInt64();
                                string clientName = inMessage.ReadString();
                                connections.Add(clientID, inMessage.SenderConnection);
                                eventHandler.handleNewClient(clientID, clientName);
                            }
                            else
                                inMessage.SenderConnection.Deny();
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            Console.WriteLine("SERVER: DiscoveryRequest!");
                            outMessage.Write(acceptingClients);
                            server.SendDiscoveryResponse(outMessage, inMessage.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("SERVER: I got Data");
                            int type = inMessage.ReadInt32();
                            String message = inMessage.ReadString();
                            long senderID = inMessage.SenderConnection.RemoteUniqueIdentifier;
                            eventHandler.handleMessage(message, type, senderID);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            Console.WriteLine("SERVER: I got a status change");
                            NetConnectionStatus status = (NetConnectionStatus)inMessage.ReadByte();
                            if (status == NetConnectionStatus.Disconnecting || status == NetConnectionStatus.Disconnected)
                            {
                                long clientID = inMessage.SenderConnection.RemoteUniqueIdentifier;
                                eventHandler.handleClientDrop(clientID);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        public void bootClient(long clientID)
        {
            NetConnection connection = connections[clientID];
            connection.Disconnect(null);
        }

        public void blockConnections()
        {
            acceptingClients = false;
            //look into whether message types can be enabled
            //or disabled after instantiating the NetServer
        }

        public void allowConnections()
        {
            acceptingClients = true;
            //ditto above
        }

        public void broadcastMessage(String message, int type)
        {
            foreach (NetConnection connection in server.Connections)
                sendMessage(message, type, connection.RemoteUniqueIdentifier);
        }

        public void sendMessage(String message, int type, long clientID)
        {
            outMessage.Write(type);
            outMessage.Write(message);
            NetDeliveryMethod method = NetDeliveryMethod.ReliableUnordered;
            server.SendMessage(outMessage, connections[clientID], method);
            outMessage = server.CreateMessage();
        }

        public void shutdown()
        {
            //TODO
        }
    }
}
