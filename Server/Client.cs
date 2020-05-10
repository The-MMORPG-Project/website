using ENet;

namespace Valk.Networking
{
    enum ClientStatus
    {
        InLobby,
        InGame
    }

    class Client
    {
        private Peer peer;

        public uint ID { get; }
        public string IP { get; }

        public float x { get; set; }
        public float y { get; set; }

        public ClientStatus ClientStatus { get; set; }

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

        public override string ToString()
        {
            return $"Client ID: {ID}, X:{x}, Y:{y}";
        }
    }
}