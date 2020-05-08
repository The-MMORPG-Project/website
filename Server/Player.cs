using ENet;

namespace Valk.Networking
{
    class Player : Client
    {
        public float x, y;

        public Player(Peer peer) : base(peer)
        {

        }
    }
}