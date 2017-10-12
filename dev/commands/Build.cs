using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static partial class Build {
        public static void CmdClean(string path, string device = null){
            try
            {
                $"gradle -p {path} clean".Term(Output.External);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
        public static void CmdGradle(string path, string conf, string device = null){
            try
            {
                $"gradle -p {path} clean assemble{conf}".Term(Output.External);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
        public static void CmdRefresh(string path, string conf, string device = null){
            try
            {
                $"gradle -p {path} --refresh-dependencies".Term(Output.External);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}