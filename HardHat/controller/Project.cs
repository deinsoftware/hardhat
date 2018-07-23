using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Platform;
using static HardHat.Program;

namespace HardHat
{
    public static partial class Project
    {
        public static bool CmdOpen(string dir)
        {
            bool cnt = false;
            try
            {
                StringBuilder cmd = new StringBuilder();
                switch (OS.GetCurrent())
                {
                    case "win":
                        cmd.Append($"start .");
                        break;
                    case "mac":
                        cmd.Append($"open .");
                        break;
                }
                cmd.ToString().Term(Output.Hidden, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return cnt;
        }

        public static void CmdEditor(string dir)
        {
            try
            {
                $"{_config.editor.open} {dir}".Term(Output.Hidden);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}