using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace ChatThingy
{
    class HostServiceImpl
    {
        private NetServer mPeer;
        private NetPeerConfiguration mConfig;
        private int port = 14242;
        private NetOutgoingMessage mOutgoingMessage;

        public HostServiceImpl()
        {
            mConfig = new NetPeerConfiguration(Program.tag);
            Initialize();
        }

        private void Initialize()
        {
            mConfig.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            mConfig.Port = port;
            mPeer = new NetServer(mConfig);
            mPeer.Start();
            mOutgoingMessage = mPeer.CreateMessage();
        }

        public void WriteMssage(String message)
        {
            mOutgoingMessage.Write(message);
        }

        public void BroadcastMessage(String message)
        {
            foreach (NetConnection connection in mPeer.Connections)
            {
                mOutgoingMessage.Write(message);
                NetDeliveryMethod method = NetDeliveryMethod.ReliableOrdered;
                mPeer.SendMessage(mOutgoingMessage, connection, method);
                mOutgoingMessage = mPeer.CreateMessage();
            }
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
                        BroadcastMessage(incomingMessage.ReadString());
                        break;
                    default:
                        break;
                }
            }
        }

        public void Shutdown()
        {
            mPeer.Shutdown("Closing Connection");
        }
    }
}
