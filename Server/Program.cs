using System;
using System.Threading;

namespace Valk.Networking
{
    class Program
    {
        public static Server Server;

        public static void Main(string[] args)
        {
            Console.Clear();
            Console.SetCursorPosition(0, Console.CursorTop + 1);

            Server = new Server(7777, 100);

            new Thread(Server.Start).Start();
            new Thread(new InputHandler().Run).Start();
        }
    }
}