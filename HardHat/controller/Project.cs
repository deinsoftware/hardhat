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
                if (dir == null)
                {
                    throw new ArgumentException(nameof(dir));
                }

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

        public static void CmdEditor(string editor, string dir)
        {
            try
            {
                if (String.IsNullOrEmpty(editor))
                {
                    editor = _config.editor.selected;
                }

                if (dir == null)
                {
                    throw new ArgumentException(nameof(dir));
                }

                switch (editor)
                {
                    case "a":
                        editor = "studio .";
                        dir = _path.Combine(dir, _config.project.androidPath);
                        break;
                    case "c":
                        editor = "code .";
                        break;
                    case "s":
                        editor = "sublime .";
                        break;
                    case "w":
                        editor = "wstorm .";
                        break;
                    case "x":
                        editor = "open *.xcodeproj";
                        dir = _path.Combine(dir, _config.project.iosPath);
                        break;
                }

                $"{editor}".Term(Output.Hidden, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}