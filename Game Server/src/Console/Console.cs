using System;
using System.Collections;
using System.Collections.Generic;
using Terminal.Gui;
using NStack;

namespace Valk.Networking
{
    public class Console
    {
        private const int SPACE_BETWEEN_INPUTFIELD_AND_MESSAGES = 3;
        private static int messageEndLine;

        public static TextField Input;
        public static ConsoleView ConsoleView;
        public static List<Label> Messages;

        public Console()
        {
            
        }

        public void Start() 
        {
            Application.Init();
            Application.OnResized += UpdatePositions;

            Messages = new List<Label>();
            ConsoleView = new ConsoleView();

            Application.Top.Add(ConsoleView);

            CreateInputField();

            messageEndLine = ConsoleView.Driver.Clip.Bottom - SPACE_BETWEEN_INPUTFIELD_AND_MESSAGES;

            Application.Run();
        }

        public static void Log(ustring text, Color foregroundColor = Color.White, Color backgroundColor = Color.Black)
        {
            AddMessage(text, foregroundColor, backgroundColor);
        }

        private void CreateInputField()
        {
            Input = new TextField("")
            {
                X = 0,
                Y = ConsoleView.Driver.Clip.Bottom - 1,
                Width = ConsoleView.Driver.Clip.Width
            };

            ConsoleView.Add(Input);
        }

        private static void AddMessage(ustring text, Color foregroundColor = Color.White, Color backgroundColor = Color.Black)
        {
            var totalLines = 0;
            foreach (var message in Messages) 
            {
                totalLines += CalcLinesInMessage(message.Text.ToString());
            }

            if (totalLines > messageEndLine)
            {
                RemoveFirstMessage();
            }

            var label = new Label(text);
            label.TextColor = Application.Driver.MakeAttribute(foregroundColor, backgroundColor);

            Messages.Add(label);
            ConsoleView.Add(label);
            UpdatePositions();

            Application.Refresh();
        }

        // Removes first message from list
        private static void RemoveFirstMessage()
        {
            ConsoleView.Remove(Console.Messages[0]);
            Messages.Remove(Console.Messages[0]);
            UpdatePositions();

            Application.Refresh();
        }

        public static void UpdatePositions()
        {
            var i = 0;
            var offset = ConsoleView.Bounds.Bottom - SPACE_BETWEEN_INPUTFIELD_AND_MESSAGES - messageEndLine;

            foreach (var message in Messages)
            {
                message.Y = i + offset;
                i += CalcLinesInMessage(message.Text.ToString());
            }

            Input.Y = ConsoleView.Bounds.Bottom - 1;
        }

        private static int CalcLinesInMessage(string message) 
        {
            var lines = 0;
            lines += (int)Math.Ceiling(message.Length / (ConsoleView.Driver.Clip.Width * 1.0));
            lines += message.Split('\n').Length - 1;
            return lines;
        }
    }
}