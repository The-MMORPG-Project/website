using ENet;

namespace Valk.Networking
{
    class Network
    {
        public static void Send(ref Event netEvent, ENet.Packet packet)
        {
            netEvent.Peer.Send(Server.channelID, ref packet);
        }

        public static void Broadcast(Host server, ENet.Packet packet)
        {
            server.Broadcast(Server.channelID, ref packet);
        }

        public static void Broadcast(Host server, ENet.Packet packet, Peer excludedPeer)
        {
            server.Broadcast(Server.channelID, ref packet, excludedPeer);
        }

        public static void Broadcast(Host server, ENet.Packet packet, Peer[] peers)
        {
            server.Broadcast(Server.channelID, ref packet, peers);
        }
    }
}