using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    public static partial class Sonar {

        public static bool CmdStatus(string prt) {
            bool cnt = false;
            try
            {
                Response result = new Response();
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"netstat -at | ");
                switch (Os.Platform())
                {
                    case "win":
                        cmd.Append($"findstr -i \"LISTENING\" | findstr \"{prt}\"");
                        break;
                    case "mac":
                        cmd.Append($"egrep -i 'LISTENING' | egrep '{prt}'");
                        break;
                }
                result = cmd.ToString().Term(Output.Hidden);
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
        public static void CmdQube(){
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

        public static void CmdBrowse(string url){
            try
            {
                Validation.Url(url);
                $"http://localhost:9000".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}