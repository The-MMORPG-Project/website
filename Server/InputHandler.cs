using System.Linq;
using System;

namespace Valk.Networking 
{
    class InputHandler 
    {
        private string[] commands = {
            "help",
            "exit"
        };

        public void Run()
        {
            System.Threading.Thread.Sleep(100); // Temporary until I can find a better solution

            while(true) {
                ConsoleKeyInfo info = System.Console.ReadKey(true);

                if (info.Key != ConsoleKey.Enter)
                    Logger.inputBuffer += info.KeyChar;

                Console.Write(info.KeyChar);

                if (info.Key == ConsoleKey.Backspace) 
                {
                    Logger.ClearChar();
                }

                if (info.Key == ConsoleKey.Enter) 
                {
                    Logger.ClearInputConsole();
                    var cmd = Logger.inputBuffer;
                    Logger.inputBuffer = "";
                    HandleCommands(cmd);
                }
            }

            /*string input;
            while (!System.Console.KeyAvailable)
            {
                //Console.Write(": ");
                System.Console.SetCursorPosition(0, Console.index);
                input = System.Console.ReadLine();
                Console.index = System.Console.CursorTop;
                HandleCommands(input);
            }*/
        }

        private void HandleCommands(string input)
        {
            string cmd = input.ToLower();
            var server = Program.Server;

            if (cmd.Equals("help"))
            {
                Logger.Log($"Commands:\n - {String.Join("\n - ", commands)}");
            }

            if (cmd.Equals("exit")) 
            {
                Logger.Log("Exiting..");
                server.Stop();
                Environment.Exit(0);
            }
        }
    }
}