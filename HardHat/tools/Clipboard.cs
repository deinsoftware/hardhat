using System.Text;
using ToolBox.Platform;
using static HardHat.Program;

namespace dein.tools
{
    public static class Clipboard
    {
        public static void Copy(string value)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append($"echo ");
            switch (OS.GetCurrent())
            {
                case "win":
                    cmd.Append($"{value}|clip");
                    break;
                case "mac":
                    cmd.Append($"\"{value}\" | pbcopy");
                    break;
                case "gnu":
                    cmd.Append($"\"{value}\" | xclip");
                    break;
            }
            _shell.Term(cmd.ToString());
        }
    }
}