using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Platform;
using ToolBox.Transform;

namespace HardHat
{
    public static partial class BuildTools
    {
        public static string CmdGetPackage(string path, string value, int word)
        {
            string result = "";
            try
            {
                Response aapt = $"aapt dump badging {path}".Term();
                string packagename = Shell.ExtractLine(aapt.stdout, "package:");
                if (!String.IsNullOrEmpty(packagename))
                {
                    packagename = Shell.GetWord(packagename, word);
                    packagename = Strings.RemoveWords(packagename, $"{value}=", "'");
                }
                result = packagename;
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return result;
        }

        public static void CmdSignerVerify(string path)
        {
            try
            {
                $"apksigner verify --print-certs {path}".Term(Output.Internal);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdInformation(string path)
        {
            try
            {
                $"aapt dump badging {path}".Term(Output.Internal);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static Response CmdSha(string path)
        {
            Response result = new Response();
            try
            {
                switch (OS.GetCurrent())
                {
                    case "win":
                        result = $"sigcheck -h {path}".Term();
                        result.stdout = Shell.ExtractLine(result.stdout, "SHA256:", "\tSHA256:\t");
                        break;
                    case "mac":
                        result = $"shasum -a 256 {path}".Term();
                        result.stdout = Shell.GetWord(result.stdout, 0);
                        break;
                }
                result.stdout = result.stdout
                    .Replace("\r", "")
                    .Replace("\n", "");
                if (!String.IsNullOrEmpty(result.stdout))
                {
                    result.code = 0;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return result;
        }
    }
}