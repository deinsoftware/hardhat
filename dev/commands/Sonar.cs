using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    public partial class Sonar {

        public static bool CmdStatus(string sitename, string dir) {
            bool cnt = false;
            try
            {
                Response result = new Response();
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"netstat -at | ");
                switch (Os.Platform())
                {
                    case "win":
                        cmd.Append($"findstr -i \"LISTENING\" | findstr \"9000\"");
                        break;
                    case "mac":
                        cmd.Append($"egrep -i 'LISTENING' | egrep ");
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
        public static void CmdStart(){
            try
            {
                StringBuilder cmd = new StringBuilder();
                if (Os.IsMacOS()){
                    cmd.Append($"sudo ");
                }
                switch (Os.Platform())
                {
                    case "win":
                        cmd.Append($"StartSonar");
                        break;
                    case "mac":
                        cmd.Append($"sonar console");
                        break;
                }
                cmd.ToString().Term(Output.External);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdBrowse(string dir){
            try
            {
                Validation.Url(dir);
                $"http://localhost:9000".Term(Output.Internal, dir);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdScanner(string dir){
            try
            {
                $"sonar-scanner".Term(Output.Internal, dir);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}