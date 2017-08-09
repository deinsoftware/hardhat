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
                Response result = new Response();
                result = $"git pull".Term();
                
                if (result.stdout.Contains("Please commit your changes or stash them before you merge."))
                {
                    $"git stash".Term();
                    result = $"git pull".Term();
                    $"git stash apply --index".Term();
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