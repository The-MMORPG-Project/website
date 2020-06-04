using System.Linq;
using System;
using System.Collections.Generic;

namespace Valk.Networking
{
    class InputHandler
    {
        private List<string> memory;
        private int memoryIndex = 0;

        // Loads commands into dictionary at runtime
        private Dictionary<string, Commands> commands = typeof(Commands).Assembly.GetTypes()
                                                        .Where(x => typeof(Commands).IsAssignableFrom(x) && !x.IsAbstract)
                                                        .Select(Activator.CreateInstance).Cast<Commands>()
                                                        .ToDictionary(x => x.GetType().Name.ToLower(), x => x);
        private ConsoleKey[] exclude = {
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.Enter
        };

        public void Run()
        {
            System.Threading.Thread.Sleep(100); // Temporary until I can find a better solution

            memory = new List<string>();
            Console.BufferHeight = Console.WindowHeight;

            while (true)
            {
                ConsoleKeyInfo info = System.Console.ReadKey(true);

                if (info.Key != ConsoleKey.Enter)
                    Logger.inputBuffer += info.KeyChar;

                if (!exclude.Contains(info.Key))
                    Console.Write(info.KeyChar);

                if (info.Key == ConsoleKey.Backspace)
                {
                    Logger.ClearChar();
                }

                if (memory.Count > 0)
                {
                    if (info.Key == ConsoleKey.UpArrow)
                    {
                        memoryIndex = Math.Max(0, memoryIndex - 1);
                        Logger.ClearInputConsole();
                        Logger.inputBuffer = memory[memoryIndex];
                        Console.Write(memory[memoryIndex]);
                    }

                    if (info.Key == ConsoleKey.DownArrow)
                    {
                        memoryIndex = Math.Min(memory.Count - 1, memoryIndex + 1);
                        Logger.ClearInputConsole();
                        Logger.inputBuffer = memory[memoryIndex];
                        Console.Write(memory[memoryIndex]);
                    }
                }

                if (info.Key == ConsoleKey.Enter)
                {
                    Logger.ClearInputConsole();
                    var cmd = Logger.inputBuffer;
                    Logger.inputBuffer = "";
                    HandleCommands(cmd);
                    memoryIndex++;
                    memory.Add(cmd);
                }
            }
        }

        private void HandleCommands(string input)
        {
            string cmd = input.ToLower().Split(' ')[0];
            string[] args = input.ToLower().Split(' ').Skip(1).ToArray();
            
            if (commands.ContainsKey(cmd))
                commands[cmd].Run(args);
            else
                Logger.Log($"Unknown Command: '{cmd}'");
        }
    }
}