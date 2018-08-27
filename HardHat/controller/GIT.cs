using System;
using dein.tools;
using ToolBox.Bridge;
using ToolBox.Transform;
using static HardHat.Program;

namespace HardHat
{
    static class Git
    {
        public static string CmdBranch(string path)
        {
            string response = "";
            try
            {
                Response result = _shell.Term($"git -C {path} branch");
                response = Strings.ExtractLine(result.stdout, "*", "* ");
                response = Strings.CleanSpecialCharacters(response);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return response;
        }

        public static void CmdDiscard(string path)
        {
            try
            {
                _shell.Term($"git -C {path} reset --hard HEAD", Output.Internal);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static string CmdPull(string path)
        {
            string response = "";
            try
            {
                Response result = _shell.Term($"git -C {path} pull", Output.Internal);
                response = Strings.CleanSpecialCharacters(result.stdout);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return response;
        }

        public static void CmdReset(string path)
        {
            try
            {
                _shell.Term($"git -C {path} clean -f -d -x", Output.Internal);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdFetch(string path)
        {
            try
            {
                _shell.Term($"git -C {path} fetch");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static bool CmdStatus(string path)
        {
            bool status = false;
            string response = "";
            try
            {
                string search = "Your branch is behind";
                Response result = _shell.Term($"git -C {path} status");
                response = Strings.ExtractLine(result.stdout, search);
                if (String.IsNullOrEmpty(response))
                {
                    status = true;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return status;
        }
    }
}