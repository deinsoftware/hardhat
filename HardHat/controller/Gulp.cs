using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Platform;
using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    public static partial class Gulp {
        public static void CmdUglify(string dir){
            try
            {
                $"gulp build".Term(Output.Internal, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdMake(string path, string dir, string ptf = ""){
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp make --prj {path}/");
                if (!String.IsNullOrEmpty(ptf)){
                    cmd.Append($" --ptf {ptf}");
                }
                cmd.ToString().Term(Output.Internal, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdServer(string path, string dir, ServerConfiguration gbs, string lip){
            try
            {
                StringBuilder cmd = new StringBuilder();
                if (OS.IsMac()){
                    cmd.Append($"sudo ");
                }
                cmd.Append($"gulp --pth {path}/");
                if (!String.IsNullOrEmpty(gbs.ipt)){
                    cmd.Append($" --ipt {gbs.ipt}");
                }
                cmd.Append($" --dmn {gbs.dmn}");
                cmd.Append($" --ptc {gbs.ptc}");
                if (!String.IsNullOrEmpty(gbs.flv)){
                    cmd.Append($" --flv {gbs.flv.ToUpper()}");
                }
                if (!String.IsNullOrEmpty(gbs.srv)){
                    cmd.Append($" --srv {gbs.srv}");
                }
                cmd.Append($" --host {lip}");
                cmd.Append($" --sync {(gbs.syn ? "Y" : "N")}");
                cmd.Append($" --open {(gbs.opn ? "Y" : "N")}");
                cmd.Append($" --os {OS.GetCurrent()}");
                cmd.ToString().Term(Output.External, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdLog(string dir, ServerConfiguration gbs)
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"gulp log");
                cmd.Append($" --dmn {gbs.dmn}");
                if (!String.IsNullOrEmpty(gbs.flv)){
                    cmd.Append($" --flv {gbs.flv.ToUpper()}");
                }
                if (!String.IsNullOrEmpty(gbs.srv)){
                    cmd.Append($" --srv {gbs.srv}");
                }
                cmd.ToString().Term(Output.External, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdInstall(string dir){
            try
            {
                $"npm i".Term(Output.Hidden, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}