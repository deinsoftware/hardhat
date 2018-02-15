using static Colorify.Colors;
using static HardHat.Program;

namespace ToolBox.Files
{
    public sealed class ConsoleNotificationSystem : INotificationSystem
    {
        public void ShowAction(string action, string message)
        {
            _colorify.Wrap($" [{action}] {message}", txtPrimary);
        }
    }
}