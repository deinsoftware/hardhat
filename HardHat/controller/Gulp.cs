using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Platform;

namespace HardHat
{
    public static partial class Gulp
    {
        public static void CmdUglify(string dir)
        {
            try
            {
                $"gulp build".Term(Output.Internal, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdMake(string path, string dir, string ptf = "")
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp make --prj {path}/");
                if (!String.IsNullOrEmpty(ptf))
                {
                    cmd.Append($" --ptf {ptf}");
                }
                cmd.ToString().Term(Output.Internal, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdServer(string path, string dir, WebConfiguration gbs, string lip)
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                if (OS.IsMac())
                {
                    cmd.Append($"sudo ");
                }
                cmd.Append($"gulp --pth {path}/");
                if (!String.IsNullOrEmpty(gbs.internalPath))
                {
                    cmd.Append($" --ipt {gbs.internalPath}");
                }
                cmd.Append($" --dmn {gbs.dimension}");
                cmd.Append($" --ptc {gbs.protocol}");
                if (!String.IsNullOrEmpty(gbs.flavor))
                {
                    cmd.Append($" --flv {gbs.flavor.ToUpper()}");
                }
                if (!String.IsNullOrEmpty(gbs.number))
                {
                    cmd.Append($" --srv {gbs.number}");
                }
                cmd.Append($" --host {lip}");
                cmd.Append($" --sync {(gbs.sync ? "Y" : "N")}");
                cmd.Append($" --open {(gbs.open ? "Y" : "N")}");
                cmd.Append($" --os {OS.GetCurrent()}");
                cmd.ToString().Term(Output.External, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdLog(string dir, WebConfiguration gbs)
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp log");
                cmd.Append($" --dmn {gbs.dimension}");
                if (!String.IsNullOrEmpty(gbs.flavor))
                {
                    cmd.Append($" --flv {gbs.flavor.ToUpper()}");
                }
                if (!String.IsNullOrEmpty(gbs.number))
                {
                    cmd.Append($" --srv {gbs.number}");
                }
                cmd.ToString().Term(Output.External, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdInstall(string dir)
        {
            try
            {
                $"npm i".Term(Output.Hidden, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}