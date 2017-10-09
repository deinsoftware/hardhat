using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static partial class Vpn {
        public static bool CmdStatus(string sitename, string dir) {
            bool cnt = false;
            try
            {
                Response result = new Response();
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"trac info -s {sitename} -tr true | ");
                switch (Os.Platform())
                {
                    case "win":
                        cmd.Append($"findstr status");
                        break;
                    case "mac":
                        cmd.Append($"egrep -i 'status:'");
                        break;
                }
                result = cmd.ToString().Term(Output.Hidden, dir);
                result.stdout = result.stdout
                    .Replace("\r","")
                    .Replace("\n","");
                cnt = (result.code == 0) && (!String.IsNullOrEmpty(result.stdout) && result.stdout.Contains("Connected"));
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return cnt;
        }

        public static void CmdDisconnect(string dir) {
            try
            {
                $"trac disconnect".Term(Output.Hidden, dir);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdConnect(string sitename, string dir) {
            try
            {
                $"trac connectgui -s {sitename}".Term(Output.Hidden, dir);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}