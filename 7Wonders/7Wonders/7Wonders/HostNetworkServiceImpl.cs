using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Networking
{
    class HostNetworkServiceImpl : HostNetworkService
    {
        private String tag;
        private int port;
        //need an application service that interacts with the networking service
        private NetServer server;
        private NetPeerConfiguration config;
        private NetOutgoingMessage outMessage;
        private Dictionary<long, NetConnection> connections;
        private Boolean acceptingClients;
        private int maxClients;

        public HostNetworkServiceImpl(int maxClients)
        {
            JObject constants = JObject.Parse(File.ReadAllText("Content/Json/constants-networking.json"));
            tag = constants.Value<String>("tag");
            port = constants.Value<int>("port");
            config = new NetPeerConfiguration(tag);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            server = new NetServer(config);
            server.Start();
            outMessage = server.CreateMessage();
            acceptingClients = true;

        }

        private void listenMessages()
        {
            //should probably have some sort of thread pool
            NetIncomingMessage inMessage;
            while (true)
            {
                if ((inMessage = server.ReadMessage()) != null)
                {
                    switch (inMessage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            if (acceptingClients)
                            {
                                inMessage.SenderConnection.Approve();
                                connections.Add(inMessage.ReadInt64(), inMessage.SenderConnection);
                                //tell the application that a new client has been added
                            }
                            else
                                inMessage.SenderConnection.Deny();
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            outMessage.Write(acceptingClients);
                            server.SendDiscoveryResponse(outMessage, inMessage.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.Data:
                            //pass the message on to the application
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)inMessage.ReadByte();
                            if (status == NetConnectionStatus.Disconnecting || status == NetConnectionStatus.Disconnected)
                            {
                                //tell the application that a client has disconnected
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
            {
                outMessage.Write(message);
                outMessage.Write(type);
                NetDeliveryMethod method = NetDeliveryMethod.ReliableUnordered;
                server.SendMessage(outMessage, connection, method);
                outMessage = server.CreateMessage();
            }
        }

        public void sendMessage(String message, int type, long clientID)
        {
            outMessage.Write(message);
            outMessage.Write(type);
            NetDeliveryMethod method = NetDeliveryMethod.ReliableUnordered;
            server.SendMessage(outMessage, connections[clientID], method);
        }

        public void shutdown()
        {
            //TODO
        }
    }
}
