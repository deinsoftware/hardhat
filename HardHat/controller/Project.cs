using System;
using System.Text;
using dein.tools;
using ToolBox.Bridge;
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
                _fileSystem.DirectoryExists(dir);
                _shell.Term(cmd.ToString(), Output.Hidden, dir);
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
                        editor = "file -f *.xcworkspace && open *.xcworkspace || open *.xcodeproj";
                        dir = _path.Combine(dir, _config.project.iosPath);
                        break;
                }
                _fileSystem.DirectoryExists(dir);
                _shell.Term($"{editor}", Output.Hidden, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdCompress(string path, string fileName)
        {
            try
            {
                _shell.Term($"zip {fileName}.zip *.*", Output.Internal, path);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}