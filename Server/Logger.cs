using System.Linq;
using System;

namespace Valk.Networking
{
    class Logger
    {
        public static string inputBuffer = "";

        public static void Log(object value)
        {
            int lines = value.ToString().Split('\n').Length;

            if (Console.CursorTop >= Console.BufferHeight - 1)
                Console.BufferHeight += lines;

            // Write to output buffer
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine(value);
            
            // Clear input console
            ClearInputConsole();

            // Rewrite input buffer
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.Write(inputBuffer);
        }

        public static void ClearChar()
        {
            if (Console.CursorLeft != -1) 
            {
                Console.Write(new String(' ', 1));
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        public static void ClearInputConsole()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(string.Concat(Enumerable.Repeat(' ', inputBuffer.Length)));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public static void ClearInputConsole(int amount)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(string.Concat(Enumerable.Repeat(' ', amount)));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}