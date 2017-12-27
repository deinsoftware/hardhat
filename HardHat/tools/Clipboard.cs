using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Text;
using ToolBox.Platform;
using static HardHat.Program;

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