using System;
using System.Text;
using dein.tools;
using ToolBox.Bridge;
using ToolBox.Platform;
using ToolBox.Validations;
using static HardHat.Program;

namespace HardHat
{
    public static partial class Sonar
    {

        public static void CmdQube()
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                switch (OS.GetCurrent())
                {
                    case "win":
                        cmd.Append($"StartSonar");
                        break;
                    case "mac":
                        cmd.Append($"sonar.sh console");
                        break;
                }
                _shell.Term(cmd.ToString(), Output.External);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdScanner(string dir)
        {
            try
            {
                _fileSystem.DirectoryExists(dir);
                _shell.Term($"sonar-scanner", Output.External, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdBrowse(string url)
        {
            try
            {
                Web.IsUrl(url);
                _shell.Browse($"{url}");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}