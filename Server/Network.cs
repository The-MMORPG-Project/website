using ENet;

class Network
{
    public static void Send(ref Event netEvent, ENet.Packet packet)
    {
        netEvent.Peer.Send(Program.CHANNEL_ID, ref packet);
    }
}