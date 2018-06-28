using System;
using System.Runtime.InteropServices;
using dein.tools;
using static HardHat.Program;

namespace HardHat
{
    public static partial class Build
    {
        public static void CmdClean(string path, string device = null)
        {
            try
            {
                $"gradle -p {path} clean".Term(Output.External);
                _config.personal.selected.path = "";
                _config.personal.selected.file = "";
                _config.personal.selected.mapping = "";
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdGradle(string path, string conf, string device = null)
        {
            try
            {
                $"gradle -p {path} assemble{conf}".Term(Output.External);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdRefresh(string path, string conf, string device = null)
        {
            try
            {
                $"gradle -p {path} --refresh-dependencies".Term(Output.External);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}