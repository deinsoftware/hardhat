using System.Text;
using ToolBox.Platform;

namespace dein.tools
{
    public static class Clipboard
    {
        public static void Copy(string val)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append($"echo ");
            switch (OS.GetCurrent())
            {
                case "win":
                    cmd.Append($"{val}|clip");
                    break;
                case "mac":
                    cmd.Append($"\"{val}\" | pbcopy");
                    break;
            }
            cmd.ToString().Term();
        }
    }
}