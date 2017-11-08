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
        public static void CmdServer(string path, string dir, string ipt, string dmn, string flv, string srv, bool syn, string lip, string ptc, bool opn){
            try
            {
                StringBuilder cmd = new StringBuilder();
                if (Os.IsMacOS()){
                    cmd.Append($"sudo ");
                }
                cmd.Append($"gulp --pth {path.Slash()}/");
                if (!String.IsNullOrEmpty(ipt)){
                    cmd.Append($" --ipt {ipt.Slash()}");
                }
                cmd.Append($" --dmn {dmn}");
                cmd.Append($" --ptc {ptc}");
                if (!String.IsNullOrEmpty(flv)){
                    cmd.Append($" --flv {flv.ToUpper()}");
                }
                if (!String.IsNullOrEmpty(srv)){
                    cmd.Append($" --srv {srv}");
                }
                cmd.Append($" --host {lip}");
                cmd.Append($" --sync {(syn ? "Y" : "N")}");
                cmd.Append($" --open {(opn ? "Y" : "N")}");
                cmd.Append($" --os {Os.Platform()}");
                cmd.ToString().Term(Output.External, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}