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
        ClientRequestPositions,
        ClientDisconnect,
        ServerClientDisconnected,
        ServerClientName,
        ClientRequestNames,
        ServerInitialPositionUpdate
    }

    enum ErrorType 
    {
        AccountCreateNameAlreadyRegistered,
        AccountLoginDoesNotExist,
        AccountLoginWrongPassword
    }

    class Network
    {
        public static void Send(PacketType type, PacketFlags packetFlagType, params object[] values)
        {
            var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)type, values);
            var packet = default(ENet.Packet);
            packet.Create(buffer, packetFlagType);
            Client.Peer.Send(Client.CHANNEL_ID, ref packet);
        }
    }
}
