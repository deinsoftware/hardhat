using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Platform;
using static HardHat.Program;

namespace HardHat
{
    public static partial class Gulp
    {
        private static string DirPath()
        {
            return _path.Combine(Variables.Value("gp"));
        }

        public static void CmdUglify()
        {
            try
            {
                $"gulp build".Term(Output.Internal, DirPath());
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
                cmd.Append($"gulp watch --prj {path}/");
                if (!String.IsNullOrEmpty(platform))
                {
                    cmd.Append($" --ptf {platform}");
                }
                cmd.ToString().Term(Output.External, DirPath());
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
                cmd.Append($"gulp make --prj {path}/");
                if (!String.IsNullOrEmpty(platform))
                {
                    cmd.Append($" --ptf {platform}");
                }
                cmd.ToString().Term(Output.Internal, DirPath());
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
                cmd.Append($"gulp --pth {path}/");
                if (!String.IsNullOrEmpty(webServer.internalPath))
                {
                    cmd.Append($" --ipt {webServer.internalPath}");
                }
                cmd.Append($" --dmn {webServer.file}");
                cmd.Append($" --ptc {webServer.protocol}");
                if (!String.IsNullOrEmpty(webServer.flavor))
                {
                    cmd.Append($" --flv {webServer.flavor.ToUpper()}");
                }
                if (!String.IsNullOrEmpty(webServer.number))
                {
                    cmd.Append($" --srv {webServer.number}");
                }
                cmd.Append($" --host {localIp}");
                cmd.Append($" --sync {(webServer.sync ? "Y" : "N")}");
                cmd.Append($" --open {(webServer.open ? "Y" : "N")}");
                cmd.Append($" --os {OS.GetCurrent()}");
                cmd.ToString().Term(Output.External, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdFtp(string path, FtpConfiguration ftpServer)
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp sftp");
                cmd.Append($" --hst {ftpServer.host}");
                cmd.Append($" --prt {ftpServer.port}");
                if (!String.IsNullOrEmpty(ftpServer.authenticationPath))
                {
                    cmd.Append($" --atp {ftpServer.authenticationPath}");
                }
                if (!String.IsNullOrEmpty(ftpServer.authenticationKey))
                {
                    cmd.Append($" --atk {ftpServer.authenticationKey}");
                }
                cmd.Append($" --lcp {path}");
                cmd.Append($" --rmp {_path.Combine(ftpServer.remotePath, ftpServer.dimension, ftpServer.resourcePath)}");
                cmd.ToString().Term(Output.External, DirPath());
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
                cmd.Append($" --dmn {webServer.file}");
                if (!String.IsNullOrEmpty(webServer.flavor))
                {
                    cmd.Append($" --flv {webServer.flavor.ToUpper()}");
                }
                if (!String.IsNullOrEmpty(webServer.number))
                {
                    cmd.Append($" --srv {webServer.number}");
                }
                cmd.ToString().Term(Output.External, DirPath());
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
                $"npm i -f".Term(Output.Hidden, DirPath());
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}