using System;
using System.Runtime.InteropServices;
using dein.tools;
using ToolBox.Bridge;
using ToolBox.Transform;
using static HardHat.Program;

namespace HardHat
{
    static class Version
    {

        #region Required

        public static void CmdGradle()
        {
            try
            {
                Response result = _shell.Term($"gradle -v");
                string response = Strings.ExtractLine(result.stdout, "Gradle", "Gradle ");
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdGulp()
        {
            try
            {
                Response result = _shell.Term($"gulp --v");
                string response = Strings.GetWord(result.stdout, 3);
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdJava()
        {
            try
            {
                Response result = _shell.Term($"java -version 2>&1");
                string response = Strings.ExtractLine(result.stdout, "java version", "java version ", "\"");
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdNode()
        {
            try
            {
                Response result = _shell.Term($"node -v");
                string response = Strings.RemoveWords(result.stdout, "v");
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdNPM()
        {
            try
            {
                Response result = _shell.Term($"npm -v");
                _shell.Result(result.stdout, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        #endregion

        #region Optional

        public static void CmdAngular()
        {
            try
            {
                Response result = _shell.Term($"ng -v");
                string response = Strings.ExtractLine(result.stdout, "Angular CLI:", "Angular CLI: ");
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdCordova()
        {
            try
            {
                Response result = _shell.Term($"cordova -v");
                _shell.Result(result.stdout, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdGit()
        {
            try
            {
                Response result = _shell.Term($"git --version");
                string response = Strings.GetWord(result.stdout, 2);
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdNativescript()
        {
            try
            {
                Response result = _shell.Term($"tns --version");
                _shell.Result(result.stdout, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdTypescript()
        {
            try
            {
                Response result = _shell.Term($"tsc -v");
                string response = Strings.RemoveWords(result.stdout, "Version ");
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdSonarScanner()
        {
            try
            {
                Response result = _shell.Term($"sonar-scanner -v");
                string response = Strings.ExtractLine(result.stdout, "INFO: SonarQube Scanner ", "INFO: SonarQube Scanner ");
                _shell.Result(response, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdYarn()
        {
            try
            {
                Response result = _shell.Term($"tns --version");
                _shell.Result(result.stdout, "is not Installed");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        #endregion
    }
}
