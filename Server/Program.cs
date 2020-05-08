using System;
using System.Threading;

namespace Valk.Networking
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            new Thread(new Server(7777, 8).Run).Start();
            new Thread(new InputHandler().Run).Start();
        }
    }
}