using System.Runtime.InteropServices;
using System;
using System.Diagnostics;

using ct = dein.tools.Colorify.Type;
using HardHat;

namespace dein.tools
{
    static class Machine 
    {
        public static string User(){
            return Env.Get("USERNAME") ?? Env.Get("USER");
        }

        public static string Domain(){
            return Env.Get("USERDOMAIN") ?? Env.Get("HOSTNAME");
        }
    }

    static partial class Env {
        public static string Get(string value) {
            return Environment.GetEnvironmentVariable(value);
        }

        public static bool Check(string value){
            string env = Env.Get(value);
            return !String.IsNullOrEmpty(env);
        }

        public static void Status(string value){
            string env = Env.Get(value);
            if (!String.IsNullOrEmpty(env)){
                $"{env.Slash()}".txtDefault(ct.WriteLine);
            } else {
                $"is not defined".txtWarning(ct.WriteLine);
            }
        }

        public static void CmdUpdate() {
            try
            {
                if (OS.IsWindows()){
                    $"refreshenv".Term();
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}