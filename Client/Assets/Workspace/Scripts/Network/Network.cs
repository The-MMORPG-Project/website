using ENet;

namespace Valk.Networking
{
    enum PacketType
    {
        HeartBeat,
        ClientCreateAccount,
        ServerCreateAccountDenied,
        ServerCreateAccountAccepted,
        ClientLoginAccount,
        ServerLoginDenied,
        ServerLoginAccepted,
        ClientPositionUpdate,
        ServerPositionUpdate,
        ClientRequestPositions
    }

    class Network
    {
        private byte channelID;
        private Peer peer;

        public Network(byte channelID, Peer peer)
        {
            this.channelID = channelID;
            this.peer = peer;
        }

        public void Send(PacketType type, PacketFlags packetFlagType, params object[] values)
        {
            var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)type, values);
            var packet = default(ENet.Packet);
            packet.Create(buffer, packetFlagType);
            peer.Send(channelID, ref packet);
        }
    }
}
