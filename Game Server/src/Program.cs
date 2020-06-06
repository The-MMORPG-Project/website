using System.Threading;

namespace Valk.Networking
{
    class Program
    {
        public static Server Server;

        public static void Main(string[] args)
        {
            new Thread(new Console().Start).Start(); // Initialize console on thread 1
        }
    }
}