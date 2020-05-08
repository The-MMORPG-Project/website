using ENet;

class Client 
{
    public uint ID { get; }
    public string IP { get; }

    private Peer peer;

    public Client(Peer peer) 
    {
        this.peer = peer;
        this.ID = peer.ID;
        this.IP = peer.IP;
    }

    public void Kick() 
    {
        peer.DisconnectNow(ID);
    }
}