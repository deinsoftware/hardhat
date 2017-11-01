using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static class Git {
        public static string CmdBranch(string path) {
            string response = "";
            try
            {
                Response result = new Response();
                result = $"git -C {path.Slash()} branch".Term();
                response = Shell.ExtractLine(result.stdout, "*", "* ");
                response = response
                    .Replace("\r","")
                    .Replace("\n","");
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }

        public static void CmdDiscard(string path) {
            try
            {
                $"git -C {path.Slash()} reset --hard HEAD".Term(Output.Internal);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        
        public static string CmdPull(string path) {
            string response = "";
            try
            {
                Response result = new Response();
                result = $"git -C {path.Slash()} pull".Term(Output.Internal);
                response = result.stdout
                    .Replace("\r","")
                    .Replace("\n","");
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }

        public static void CmdReset(string path) {
            try
            {
                $"git -C {path.Slash()} clean -f -d -x".Term(Output.Internal);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}