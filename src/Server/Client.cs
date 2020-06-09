using System;
using ENet;

namespace Valk.Networking
{
    enum ClientStatus
    {
        InLobby,
        InGame
    }

    [SerializableAttribute]
    class Client
    {
        public Peer Peer;

        public uint ID { get; }
        public string IP { get; }
        public float px { get; set; }
        public float py { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public ClientStatus Status { get; set; }

        public Client(Peer peer)
        {
            this.Peer = peer;
            this.ID = peer.ID;
            this.IP = peer.IP;
        }

        public void Kick()
        {
            Peer.DisconnectNow(ID);
        }

        public override string ToString()
        {
            return $"Client ID: {ID}, X:{x}, Y:{y}";
        }
    }
}