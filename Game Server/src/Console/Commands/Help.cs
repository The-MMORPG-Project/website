using System;
using System.Linq;

namespace Valk.Networking
{
    public class Help : Commands
    {
        // Loads available commands into array at runtime
        protected static string[] commands = typeof(Commands).Assembly.GetTypes()
                        .Where(x => typeof(Commands).IsAssignableFrom(x) && !x.IsAbstract)
                        .Select(Activator.CreateInstance).Cast<Commands>()
                        .Aggregate("", (str, x) => str + x.GetType().Name.ToLower() + " ")
                        .Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();

        public override void Run(string[] args)
        {
            Console.Log($"Commands:\n - {String.Join("\n - ", commands)}");
        }
    }
}