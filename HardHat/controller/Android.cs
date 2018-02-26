using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Platform;
using ToolBox.Transform;

namespace HardHat 
{
    public static partial class Adb{
        public static bool CmdDevices() {
            bool dev = false;
            string response = "";
            try
            {
                Response result = $"adb devices -l".Term();
                response = Strings.RemoveWords(result.stdout, $"List of devices attached{Environment.NewLine}", Environment.NewLine);
                
                if (
                    !String.IsNullOrEmpty(result.stdout) &&
                    (
                        response.Contains("device usb:") || 
                        response.Contains("device product:") || 
                        response.Contains("device")
                    )
                )
                {
                    dev = true;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return dev;
        }

        public static Response CmdState(string device){
            Response result = new Response();
            result = $"adb -s {device} get-state".Term();
            if (result.stdout.Contains("not found")){
                result.code = 0;   
            } else {
                result.code = 1;
                Message.Error(
                    msg: $" Device '{device}' not found."
                );
            }
            return result;
        }

        public static Response CmdInstall(string path, string device){
            Response result = new Response();
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    result = CmdState(device);
                    if (result.code == 0){
                        cmd.Append($" -s {device}");
                    }
                }
                cmd.Append($" install -r {path} 2>&1");
                result = cmd.ToString().Term(Output.Internal);
                string status = Shell.ExtractLine(result.stdout, "Success");
                if (status.Contains("Success")){
                    result.code = 0;
                } else {
                    result.code = 1;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return result;
        }

        public static void CmdLaunch(string path, string device){
            try
            {
                Response result = $"aapt dump badging {path}".Term();
                string packagename = Shell.ExtractLine(result.stdout, "package:");
                if (!String.IsNullOrEmpty(packagename)){
                    packagename = Shell.GetWord(packagename, 1);
                    packagename = Strings.RemoveWords(packagename, "name=", "'");
                }

                if (!String.IsNullOrEmpty(packagename)){
                    StringBuilder cmd = new StringBuilder();
                    cmd.Append("adb");
                    if (!String.IsNullOrEmpty(device))
                    {
                        result = CmdState(device);
                        if (result.code == 0){
                            cmd.Append($" -s {device}");
                        }
                    }
                    cmd.Append($" shell monkey -p {packagename} 1");
                    cmd.ToString().Term();
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static Response CmdTcpIp(string port, string device){
            Response result = new Response();
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    result = CmdState(device);
                    if (result.code == 0){
                        cmd.Append($" -s {device}");
                    }
                }
                cmd.Append($" tcpip {port} 2>&1");
                result = cmd.ToString().Term(Output.Internal);
                string status = Shell.ExtractLine(result.stdout, $"{port}");
                if (String.IsNullOrEmpty(status) || status.Contains($"{port}")){
                    result.code = 0;
                } else {
                    result.code = 1;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return result;
        }

        public static bool CmdConnect(string ip, string port){
            bool connected = false;
            try
            {
                Response result = $"adb connect {ip}:{port}".Term(Output.Internal);
                if (result.stdout.Contains($"connected to {ip}:{port}")){
                    connected = true;
                } else {
                    connected = false;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return connected;
        }

        public static bool CmdDisconnect(string ip, string port){
            bool connected = false;
            try
            {
                Response result = new Response();
                result = $"adb disconnect {ip}:{port}".Term(Output.Internal);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return connected;
        }

        public static void CmdKillServer(){
            try
            {
                $"adb kill-server".Term(Output.Internal);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdStartServer(){
            try
            {
                $"adb start-server".Term(Output.Internal);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static string CmdList() {
            string response = "";
            try
            {
                Response result = $"adb devices -l".Term();
                response = Strings.RemoveWords(result.stdout, $"List of devices attached{Environment.NewLine}");
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }
    }

    public static partial class BuildTools {
        public static void CmdSignerVerify(string path){
            try
            {
                $"apksigner verify --print-certs {path}".Term(Output.Internal);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void CmdInformation(string path){
            try
            {
                $"aapt dump badging {path}".Term(Output.Internal);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static Response CmdSha(string path) {
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
                    .Replace("\r","")
                    .Replace("\n","");
                if (!String.IsNullOrEmpty(result.stdout)){
                    result.code = 0;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return result;
        }
    }
}