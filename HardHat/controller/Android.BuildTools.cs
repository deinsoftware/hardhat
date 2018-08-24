using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Bridge;
using ToolBox.Platform;
using ToolBox.Transform;
using static HardHat.Program;

namespace HardHat
{
    public static partial class BuildTools
    {
        public static string CmdGetPackage(string path, string value, int word)
        {
            string result = "";
            try
            {
                Response aapt = _shell.Term($"aapt dump badging {path}");
                string packagename = _shell.ExtractLine(aapt.stdout, "package:");
                if (!String.IsNullOrEmpty(packagename))
                {
                    packagename = _shell.GetWord(packagename, word);
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
                _shell.Term($"apksigner verify --print-certs {path}", Output.Internal);
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
                _shell.Term($"aapt dump badging {path}", Output.Internal);
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
                        result = _shell.Term($"sigcheck -h {path}");
                        result.stdout = _shell.ExtractLine(result.stdout, "SHA256:", "\tSHA256:\t");
                        break;
                    case "mac":
                        result = _shell.Term($"shasum -a 256 {path}");
                        result.stdout = _shell.GetWord(result.stdout, 0);
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