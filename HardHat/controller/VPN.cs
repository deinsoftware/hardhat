using System;
using System.Text;
using dein.tools;
using ToolBox.Bridge;
using ToolBox.Platform;
using ToolBox.Transform;
using static HardHat.Program;

namespace HardHat
{
    public static partial class Vpn
    {
        public static bool CmdStatus(string sitename, string dir)
        {
            bool connectionStatus = false;
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append($"trac info -s {sitename} -tr true | ");
                switch (OS.GetCurrent())
                {
                    case "win":
                        cmd.Append($"findstr status");
                        break;
                    case "mac":
                        cmd.Append($"egrep -i 'status:'");
                        break;
                }
                Response result = _shell.Term(cmd.ToString(), Output.Hidden, dir);
                result.stdout = Strings.CleanSpecialCharacters(result.stdout);
                connectionStatus = (result.code == 0) && (!String.IsNullOrEmpty(result.stdout) && result.stdout.Contains("Connected"));
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return connectionStatus;
        }

        public static void CmdDisconnect(string dir)
        {
            try
            {
                _fileSystem.DirectoryExists(dir);
                _shell.Term($"trac disconnect", Output.Hidden, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdConnect(string sitename, string dir)
        {
            try
            {
                _fileSystem.DirectoryExists(dir);
                _shell.Term($"trac connectgui -s {sitename}", Output.Hidden, dir);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}