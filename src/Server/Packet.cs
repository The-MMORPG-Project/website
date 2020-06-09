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

    class Packet
    {
        public static ENet.Packet Create(PacketType type, ENet.PacketFlags packetFlagType, params object[] values)
        {
            using var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)type, values);
            var packet = default(ENet.Packet);
            packet.Create(buffer, packetFlagType);
            return packet;
        }
    }
}