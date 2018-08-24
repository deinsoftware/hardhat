using static Colorify.Colors;
using static HardHat.Program;

namespace ToolBox.Notification
{
    public sealed class ConsoleNotificationSystem : INotificationSystem
    {
        public void ShowAction(string action, string message)
        {
            _colorify.Wrap($" [{action}] {message}", txtPrimary);
        }

        public void StandardOutput(string message)
        {
            _colorify.Wrap($" {message}", txtPrimary);
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