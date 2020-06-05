using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Terminal.Gui;
using System.Runtime.InteropServices;

namespace Valk.Networking
{
    class Program
    {
        public static Server Server;

        public static void Main(string[] args)
        {
            // Initialize console
            new Thread(new Console().Start).Start();

            // Initialize server
            Server = new Server(7777, 100);
            new Thread(Server.Start).Start();
        }
    }
}