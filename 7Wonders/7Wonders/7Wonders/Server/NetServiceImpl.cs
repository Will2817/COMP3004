using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Server
{
    class NetServiceImpl : NetService
    {
        private String tag;
        private int port;
        private int clientport;
        private EventHandlerService eventHandler;
        private NetServer server;
        private NetPeerConfiguration config;
        private Dictionary<long, NetConnection> connections;
        private Dictionary<long, string> names;
        private Boolean acceptingClients;

        public NetServiceImpl()
        {
            JObject constants = JObject.Parse(File.ReadAllText("Content/Json/constants-networking.json"));
            tag = constants.Value<String>("tag");
            port = constants.Value<int>("port");
            clientport = constants.Value<int>("clientport");
            config = new NetPeerConfiguration(tag);
            config.ConnectionTimeout = 5;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.Port = port;
            server = new NetServer(config);
            server.Start();
            connections = new Dictionary<long, NetConnection>();
            names = new Dictionary<long, string>();
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
            NetIncomingMessage inMessage;
            while (server.Socket != null)
            {
                if ((inMessage = server.ReadMessage()) != null)
                {
                    Console.WriteLine(inMessage.MessageType.ToString());
                    switch (inMessage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            if (acceptingClients)
                            {
                                inMessage.SenderConnection.Approve();
                                long clientID = inMessage.ReadInt64();
                                string clientName = inMessage.ReadString();
                                connections.Add(clientID, inMessage.SenderConnection);
                                names.Add(clientID, clientName);
                            }
                            else
                                inMessage.SenderConnection.Deny();
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            NetOutgoingMessage outMessage;
                            outMessage = server.CreateMessage();
                            outMessage.Write(acceptingClients);
                            server.SendDiscoveryResponse(outMessage, inMessage.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.Data:
                            int type = inMessage.ReadInt32();
                            String message = inMessage.ReadString();
                            long senderID = inMessage.SenderConnection.RemoteUniqueIdentifier;
                            eventHandler.handleMessage(message, type, senderID);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)inMessage.ReadByte();
                            if (status == NetConnectionStatus.Disconnected || status == NetConnectionStatus.None)
                            {
                                long clientID = inMessage.SenderConnection.RemoteUniqueIdentifier;
                                connections.Remove(clientID);
                                names.Remove(clientID);
                                eventHandler.handleClientDrop(clientID);
                            }
                            if (status == NetConnectionStatus.Connected)
                            {
                                long id = inMessage.SenderConnection.RemoteUniqueIdentifier;
                                if (names.ContainsKey(id))
                                {
                                    eventHandler.handleNewClient(id, names[id]);
                                }
                            }
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            Console.WriteLine(inMessage.ReadString());
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
            NetOutgoingMessage outMessage;
            Console.WriteLine("BroadCasting all");
            outMessage = server.CreateMessage();
            outMessage.Write(type);
            outMessage.Write(message);
            NetDeliveryMethod method = NetDeliveryMethod.ReliableOrdered;
            server.SendToAll(outMessage, method);
        }

        public void sendMessage(String message, int type, long clientID)
        {
            NetOutgoingMessage outMessage;
            outMessage = server.CreateMessage();
            outMessage.Write(type);
            outMessage.Write(message);
            NetDeliveryMethod method = NetDeliveryMethod.ReliableOrdered;
            server.SendMessage(outMessage, connections[clientID], method);
        }

        public void shutdown()
        {
            Console.WriteLine("Server Shutting down...");
            server.Shutdown("");
        }
    }
}
