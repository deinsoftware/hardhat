using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static class Version {
        
        #region Required

        public static void CmdGradle() {
            try
            {
                Response result = new Response();
                result = $"gradle -v".Term();
                string response = Shell.ExtractLine(result.stdout, "Gradle", "Gradle ");
                Shell.Result(response);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdGulp() {
            try
            {
                Response result = new Response();
                result = $"gulp --v".Term();
                string response = Shell.GetWord(result.stdout, 3);
                Shell.Result(response);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdJava() {
            try
            {
                Response result = new Response();
                result = $"java -version 2>&1".Term();
                string response = Shell.ExtractLine(result.stdout, "java version", "java version ", "\"");
                Shell.Result(response);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdNode() {
            try
            {
                Response result = new Response();
                result = $"node -v".Term();
                string response = Strings.Remove(result.stdout, "v");
                Shell.Result(response);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdNPM() {
            try
            {
                Response result = new Response();
                result = $"npm -v".Term();
                Shell.Result(result.stdout);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        #endregion
        
        #region Optional

        public static void CmdCordova() {
            try
            {
                Response result = new Response();
                result = $"cordova -v".Term();
                Shell.Result(result.stdout);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdGit() {
            try
            {
                Response result = new Response();
                result = $"git --version".Term();
                string response = Shell.GetWord(result.stdout, 2);
                Shell.Result(response);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdNativescript() {
            try
            {
                Response result = new Response();
                result = $"tns --version".Term();
                Shell.Result(result.stdout);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdTypescript() {
            try
            {
                Response result = new Response();
                result = $"tsc -v".Term();
                string response = Strings.Remove(result.stdout, "Version ");
                Shell.Result(response);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdSonarScanner() {
            try
            {
                Response result = new Response();
                result = $"sonar-scanner -v".Term();
                string response = Shell.ExtractLine(result.stdout, "INFO: SonarQube Scanner ", "INFO: SonarQube Scanner ");
                Shell.Result(response);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        #endregion
    }
}