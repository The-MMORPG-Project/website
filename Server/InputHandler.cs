using System;
using System.Threading;

namespace Valk.Networking 
{
    class InputHandler 
    {
        public void Run()
        {
            Thread.Sleep(500); // Temporary until I can find a better solution
            string input;
            while (true)
            {
                //Console.Write(": ");
                input = Console.ReadLine();
                HandleCommands(input);
            }
        }

        private static void HandleCommands(string input)
        {
            string cmd = input.ToLower();

            if (cmd.Equals("help"))
            {
                Console.WriteLine("Useful command");
            }
        }
    }
}