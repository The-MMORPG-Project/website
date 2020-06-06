using Terminal.Gui;

namespace Valk.Networking
{
    public class ConsoleView : View
    {
        public ConsoleView() : base()
        {
            DefaultColorScheme();
        }

        public override bool ProcessKey(KeyEvent e)
        {
            base.ProcessKey(e);

            // Reset view back to 'input area' if any key is pressed
            if (Console.ViewOffset + Console.GetTotalLines() > ConsoleView.Driver.Clip.Height - 1)
            {
                Console.ViewOffset = -Console.GetTotalLines() + ConsoleView.Driver.Clip.Height - 1;
                Console.UpdatePositions();
            }

            if (e.Key == Key.Esc) // KEY RESERVED FOR DEBUGGING
            {
                Console.Log(Application.Driver.Clip.ToString());
                return true;
            }

            if (e.Key == Key.CursorDown)
            {
                if (Console.CommandHistoryIndex + Console.CommandHistory.Count + 1 < Console.CommandHistory.Count)
                {
                    Console.CommandHistoryIndex++;
                    Console.Input.Text = Console.CommandHistory[Console.CommandHistoryIndex + Console.CommandHistory.Count];
                }
                return true;
            }

            if (e.Key == Key.CursorUp)
            {
                if (Console.CommandHistoryIndex + Console.CommandHistory.Count - 1 >= 0)
                {
                    Console.CommandHistoryIndex--;
                    Console.Input.Text = Console.CommandHistory[Console.CommandHistory.Count + Console.CommandHistoryIndex];
                }
                return true;
            }

            if (e.Key == Key.Enter)
            {
                var input = Console.Input.Text.ToString();
                var cmd = input.Split(' ')[0];

                Console.Input.Text = "";

                if (cmd == "" || input == "")
                    return false;

                Console.Log(input);
                Console.HandleCommands(input);

                Console.CommandHistory.Add(input);
                return true;
            }

            return false;
        }

        public override bool MouseEvent(MouseEvent e)
        {
            base.MouseEvent(e);

            if (e.Flags == MouseFlags.WheeledUp)
            {
                if (Console.ViewOffset < 0)
                {
                    Console.ViewOffset++;
                    Console.UpdatePositions();
                }
            }

            if (e.Flags == MouseFlags.WheeledDown)
            {
                if (Console.ViewOffset + Console.GetTotalLines() > ConsoleView.Driver.Clip.Height - 1)
                {
                    Console.ViewOffset--;
                    Console.UpdatePositions();
                }
            }

            return true;
        }

        private void DefaultColorScheme()
        {
            var colorScheme = new ColorScheme();
            colorScheme.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black); // Labels
            colorScheme.Focus = Application.Driver.MakeAttribute(Color.White, Color.Black); // Text Fields

            // These attributes are not needed, however if they do come up for whatever reason
            // they will be visible with vibrant notable colors.
            colorScheme.Disabled = Application.Driver.MakeAttribute(Color.Red, Color.Blue);
            colorScheme.HotFocus = Application.Driver.MakeAttribute(Color.Green, Color.Magenta);
            colorScheme.HotNormal = Application.Driver.MakeAttribute(Color.Cyan, Color.BrightRed);

            this.ColorScheme = colorScheme;
        }
    }
}