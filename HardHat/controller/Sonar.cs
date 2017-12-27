using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    public static partial class Sonar {

        public static void CmdQube(){
            try
            {
                StringBuilder cmd = new StringBuilder();
                switch (Os.Platform())
                {
                    case "win":
                        cmd.Append($"StartSonar");
                        break;
                    case "mac":
                        cmd.Append($"sonar.sh console");
                        break;
                }
                cmd.ToString().Term(Output.External);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdScanner(string dir){
            try
            {
                $"sonar-scanner".Term(Output.Internal, dir);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdBrowse(string url){
            try
            {
                Validation.Url(url);
                $"{url}".Browse();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}