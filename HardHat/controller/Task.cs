using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Bridge;
using ToolBox.Platform;
using static HardHat.Program;

namespace HardHat
{
    public static partial class Task
    {
        private static string DirPath()
        {
            string path = "";
            try
            {
                path = _path.Combine(Variables.Value("tp"));
                _fileSystem.DirectoryExists(path);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return path;
        }

        public static void CmdObfuscate(string type)
        {
            try
            {
                _shell.Term($"gulp dist --type {type}", Output.Internal, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdWatch(string path, string platform = "")
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp watch --path {path}/");
                if (!String.IsNullOrEmpty(platform))
                {
                    cmd.Append($" --os {platform}");
                }
                _shell.Term(cmd.ToString(), Output.External, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdMake(string path, string platform = "")
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp make --path {path}/");
                if (!String.IsNullOrEmpty(platform))
                {
                    cmd.Append($" --os {platform}");
                }
                _shell.Term(cmd.ToString(), Output.Internal, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdServer(string path, WebConfiguration webServer, string localIp)
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                if (OS.IsMac())
                {
                    cmd.Append($"sudo ");
                }
                cmd.Append($"gulp server");
                cmd.Append($" --path {path}/");
                if (!String.IsNullOrEmpty(webServer.internalPath))
                {
                    cmd.Append($" --internal {webServer.internalPath}");
                }
                cmd.Append($" --dimension {webServer.file}");
                if (!String.IsNullOrEmpty(webServer.flavor))
                {
                    cmd.Append($" --flavor {Selector.Name(Selector.Flavor, _config.personal.webServer.flavor)}");
                }
                if (!String.IsNullOrEmpty(webServer.number))
                {
                    cmd.Append($" --number {webServer.number}");
                }
                cmd.Append($" --host {localIp}");
                cmd.Append($" --sync {webServer.sync.ToString().ToLower()}");
                cmd.Append($" --browse {webServer.open.ToString().ToLower()}");
                cmd.Append($" --os {OS.GetCurrent()}");
                _shell.Term(cmd.ToString(), Output.External, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdLog(WebConfiguration webServer)
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp log");
                cmd.Append($" --dimension {webServer.file}");
                if (!String.IsNullOrEmpty(webServer.flavor))
                {
                    cmd.Append($" --flavor {Selector.Name(Selector.Flavor, _config.personal.webServer.flavor)}");
                }
                if (!String.IsNullOrEmpty(webServer.number))
                {
                    cmd.Append($" --number {webServer.number}");
                }
                _shell.Term(cmd.ToString(), Output.External, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdRemove()
        {
            try
            {
                _shell.Term($"npm r", Output.Hidden, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdInstall()
        {
            try
            {
                _shell.Term($"npm i -f", Output.Hidden, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}