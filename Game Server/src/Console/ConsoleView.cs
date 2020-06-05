using System;
using System.Linq;
using System.Collections.Generic;
using Terminal.Gui;

namespace Valk.Networking
{
    public class ConsoleView : View
    {
        private Dictionary<string, Commands> commands = typeof(Commands).Assembly.GetTypes().Where(x => typeof(Commands).IsAssignableFrom(x) && !x.IsAbstract).Select(Activator.CreateInstance).Cast<Commands>().ToDictionary(x => x.GetType().Name.ToLower(), x => x);

        public ConsoleView() : base()
        {
            SetColorScheme();
        }

        public override bool ProcessColdKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Enter)
            {
                if (Console.Input.Text == "")
                    return false;

                Console.Log(Console.Input.Text);

                HandleCommands(Console.Input.Text.ToString());

                Console.Input.Text = "";
            }
            
            return true;
        }

        private void SetColorScheme() 
        {
            var colorScheme = new ColorScheme();
            // Labels
            colorScheme.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
            // Text Fields
            colorScheme.Focus = Application.Driver.MakeAttribute(Color.White, Color.Black);

            this.ColorScheme = colorScheme;
        }

        private void HandleCommands(string input)
        {
            var cmd = input.ToLower().Split(' ')[0];
            var args = input.ToLower().Split(' ').Skip(1).ToArray();

            if (commands.ContainsKey(cmd))
                commands[cmd].Run(args);
            else
                Console.Log($"Unknown Command: '{cmd}'");
        }
    }
}