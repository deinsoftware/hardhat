using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

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
        public static void CmdServer(string path, string dir, ServerConfiguration gbs, string lip){
            try
            {
                StringBuilder cmd = new StringBuilder();
                if (Os.IsMacOS()){
                    cmd.Append($"sudo ");
                }
                cmd.Append($"gulp --pth {path.Slash()}/");
                if (!String.IsNullOrEmpty(gbs.ipt)){
                    cmd.Append($" --ipt {gbs.ipt.Slash()}");
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
                cmd.Append($" --os {Os.Platform()}");
                cmd.ToString().Term(Output.External, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}