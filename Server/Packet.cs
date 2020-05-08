namespace Valk.Networking
{
    class Packet
    {
        public enum Type
        {
            HeartBeat = 0,
            ClientCreateAccount = 1,
            ServerCreateAccountDenied = 2,
            ServerCreateAccountAccepted = 3,
            ClientLoginAccount = 4,
            ServerLoginDenied = 5,
            ServerLoginAccepted = 6,
            ClientPositionUpdate = 7,
            ServerPositionUpdate = 8
        }

        public static ENet.Packet Create(Packet.Type type, params object[] values)
        {
            using var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)type, values);
            var packet = default(ENet.Packet);
            packet.Create(buffer);
            return packet;
        }
    }
}