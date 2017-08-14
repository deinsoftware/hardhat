using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static partial class GIT {
        public static string CmdBranch(string path) {
            string response = "";
            try
            {
                Response result = new Response();
                result = $"git -C {path.Slash()} branch".Term();
                response = Shell.RemoveLine(result.stdout, "*", "* ");
                response = response
                    .Replace("\r","")
                    .Replace("\n","");
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }

        public static void CmdDiscard(string path) {
            try
            {
                $"git -C {path.Slash()} reset --hard HEAD".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }

        public static void CmdReset(string path) {
            try
            {
                $"git -C {path.Slash()} clean -f -d -x".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static bool CmdUpdate() {
            bool response = false;
            try
            {
                $"git config --local core.filemode false".Term();
                Response result = new Response();
                result = $"git pull".Term();
                
                if (!String.IsNullOrEmpty(result.stderr))
                {
                    $"git reset --hard HEAD".Term();
                    $"git clean -f -d -x".Term();
                    result = $"git pull".Term();
                }

                if (result.stdout.Contains("Already up-to-date."))
                {
                    response = false;
                } else {
                    response = true;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }
    }
}