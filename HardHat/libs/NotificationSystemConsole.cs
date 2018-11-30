using System;
using System.Linq;
using static Colorify.Colors;
using static HardHat.Program;

namespace ToolBox.Notification
{
    public sealed class ConsoleNotificationSystem : INotificationSystem
    {
        private string _pastMessage { get; set; } = "";

        public void ShowAction(string action, string message)
        {
            _colorify.Wrap($" [{action}] {message}", txtPrimary);
        }

        public void StandardOutput(string message)
        {
            var diff = message.Except(_pastMessage).ToArray();
            if (diff.Length <= 2) //isProgress message
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            _colorify.Wrap($" {message}", txtPrimary);
            _pastMessage = message;
        }

        public void StandardWarning(string message)
        {
            _colorify.Wrap($" {message}", txtWarning);
        }

        public void StandardError(string message)
        {
            _colorify.Wrap($" {message}", txtDanger);
        }

        public void StandardLine()
        {
            _colorify.BlankLines();
        }
    }
}