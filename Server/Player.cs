using ENet;

namespace Valk.Networking
{
    class Player : Client
    {
        public float x { get; set; }
        public float y { get; set; }

        public Player(Peer peer) : base(peer)
        {

        }
    }
}