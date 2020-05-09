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

        public override string ToString() 
        {
            return $"Player X:{x}, Y:{y}";
        }
    }
}