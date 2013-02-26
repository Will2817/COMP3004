using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace ChatThingy
{
    class ClientServiceImpl : ClientService
    {
        String hostIP;
        ChatService chatService;
        private NetClient mPeer;
        private NetPeerConfiguration mConfig;
        private int port = 14242;
        private NetOutgoingMessage mOutgoingMessage;
        private NetConnection connection;


        public ClientServiceImpl(String hostIP, ChatService chatService)
        {
            this.hostIP = hostIP;
            this.chatService = chatService;
            this.chatService.setClientService(this);
            mConfig = new NetPeerConfiguration(Program.tag);
            Initialize();
        }

        private void Initialize()
        {
            mConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            mPeer = new NetClient(mConfig);
            mPeer.Start();
            mOutgoingMessage = mPeer.CreateMessage();
            Connect();
        }

        private void Connect()
        {
            mPeer.Connect(hostIP, port);
            connection = mPeer.Connections[0];
        }

        public void SendMessage(Message message)
        {
            mOutgoingMessage.Write(message.ToString());
            NetDeliveryMethod method = NetDeliveryMethod.ReliableOrdered;
            mPeer.SendMessage(mOutgoingMessage, connection, method);
            mOutgoingMessage = mPeer.CreateMessage();
        }

        public void CheckForMessages()
        {
            NetIncomingMessage incomingMessage;

            while ((incomingMessage = mPeer.ReadMessage()) != null)
            {
                switch (incomingMessage.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        mPeer.SendDiscoveryResponse(null, incomingMessage.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.Data:
                        chatService.displayIncoming(new Message(incomingMessage.ReadString()));
                        break;
                    default:
                        break;
                }
            }
        }

        public void Shutdown()
        {
            mPeer.Shutdown("Closing connection.");
        }
    }
}
