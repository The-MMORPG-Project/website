using ENet;

class Network
{
    public static void Send(ref Event netEvent, Packet.Type type, params object[] values)
    {
        var protocol = new Protocol();
        var buffer = protocol.Serialize((byte) type, values);
        var packet = default(ENet.Packet);
        packet.Create(buffer);
        netEvent.Peer.Send(Program.CHANNEL_ID, ref packet);
    }
}