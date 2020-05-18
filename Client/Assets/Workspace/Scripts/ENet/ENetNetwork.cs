using ENet;

namespace Valk.Networking
{
    public enum PacketType
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

    public enum ErrorType 
    {
        AccountCreateNameAlreadyRegistered,
        AccountLoginDoesNotExist,
        AccountLoginWrongPassword
    }

    public class ENetNetwork
    {
        public static void Send(PacketType type, PacketFlags packetFlagType, params object[] values)
        {
            var protocol = new ENetProtocol();
            var buffer = protocol.Serialize((byte)type, values);
            var packet = default(ENet.Packet);
            packet.Create(buffer, packetFlagType);
            ENetClient.Peer.Send(ENetClient.CHANNEL_ID, ref packet);
        }
    }
}
