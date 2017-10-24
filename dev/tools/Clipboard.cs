using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Text;

namespace dein.tools
{
    public static class Clipboard
    {
        public static void Copy(string val)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append($"echo ");
            switch (Os.Platform())
            {
                case "win":
                    cmd.Append($"{val.BackSlash()}|clip");
                    break;
                case "mac":
                    cmd.Append($"\"{val.Slash()}\" | pbcopy");
                    break;
            }
            cmd.ToString().Term();
        }
    }
}