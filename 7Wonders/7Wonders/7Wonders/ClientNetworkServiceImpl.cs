using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using Newtonsoft.Json.Linq;

namespace _7Wonders.Networking
{
    class ClientNetworkServiceImpl : ClientNetworkService
    {
        String hostIP;
        private String tag;
        private int port;
        private long discoveryWait;
        //need an application service that interacts with the networking service
        private NetClient client;
        private NetPeerConfiguration config;
        private NetOutgoingMessage outMessage;
        private NetConnection connection;

        public ClientNetworkServiceImpl()
        {
            JObject constants = JObject.Parse(File.ReadAllText("Content/Json/constants-networking.json"));
            tag = constants.Value<String>("tag");
            port = constants.Value<int>("port");
            discoveryWait = constants.Value<int>("discoverywait");
            config = new NetPeerConfiguration(tag);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.DisableMessageType(NetIncomingMessageType.DiscoveryRequest);
            client = new NetClient(config);
            client.Start();
            outMessage = client.CreateMessage();
        }

        public int joinHost()
        {
            client.DiscoverLocalPeers(port);
            if (waitDiscoveryResponse() == -1)
                return -1;
            while (client.ConnectionStatus == NetConnectionStatus.InitiatedConnect
                || client.ConnectionStatus == NetConnectionStatus.RespondedAwaitingApproval
                || client.ConnectionStatus == NetConnectionStatus.RespondedConnect);
            if (!(client.ConnectionStatus == NetConnectionStatus.Connected))
                return -1;
            Thread messageListenerThread = new Thread(new ThreadStart(listenMessages));
            messageListenerThread.Start();
            while (!messageListenerThread.IsAlive);
            return 0;
        }

        private int waitDiscoveryResponse()
        {
            DateTime start = DateTime.UtcNow;
            NetIncomingMessage inMessage;
            while (start - DateTime.UtcNow < TimeSpan.FromMilliseconds(discoveryWait))
            {
                while ((inMessage = client.ReadMessage()) != null)
                {
                    switch (inMessage.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryResponse:
                            if (inMessage.ReadBoolean())
                            {
                                outMessage.Write(client.UniqueIdentifier);
                                connection = client.Connect(inMessage.SenderEndPoint, outMessage);
                                outMessage = client.CreateMessage();
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
                            //pass the message on to the application
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus) inMessage.ReadByte();
                            if (status == NetConnectionStatus.Disconnecting || status == NetConnectionStatus.Disconnected)
                            {
                                //tell the application it has been disconnected from the server
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
            outMessage.Write(message);
            outMessage.Write(type);
            NetDeliveryMethod method = NetDeliveryMethod.ReliableUnordered;
            client.SendMessage(outMessage, connection, method);
            outMessage = client.CreateMessage();
            return 0;
        }

        public int disconnect()
        {
            connection.Disconnect(null);
            while (connection.Status != NetConnectionStatus.Disconnected);
            return 0;
        }
    }
}
