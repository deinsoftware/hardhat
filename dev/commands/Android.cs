using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    partial class Adb{
        public static bool CmdDevices() {
            bool dev = false;
            string response = "";
            try
            {
                Response result = new Response();
                result = $"adb devices -l".Term();
                response = Strings.Remove(result.stdout, $"List of devices attached{Environment.NewLine}", Environment.NewLine);
                
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return dev;
        }

        public static Response CmdInstall(string path, string device){
            Response result = new Response();
            result.code = 1;
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    result = $"adb -s {device} get-state".Term();
                    if (result.stdout.Contains("device")){
                        cmd.Append($" -s {device}");
                    } else {
                        Message.Critical(
                            msg: $" Device '{device}' not found."
                        );
                    }
                }
                cmd.Append($" install -r {path} 2>&1");
                result = cmd.ToString().Term(Output.Internal);
                string status = Shell.ExtractLine(result.stdout, "Success");
                if (status.Contains("Success")){
                    result.code = 0;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return result;
        }

        public static void CmdLaunch(string path, string device){
            try
            {
                Response result = new Response();
                result = $"aapt dump badging {path}".Term();
                string packagename = Shell.ExtractLine(result.stdout, "package:");
                if (!String.IsNullOrEmpty(packagename)){
                    packagename = Shell.GetWord(packagename, 1);
                    packagename = Strings.Remove(packagename, "name=", "'");
                }

                if (!String.IsNullOrEmpty(packagename)){
                    StringBuilder cmd = new StringBuilder();
                    cmd.Append("adb");
                    if (!String.IsNullOrEmpty(device))
                    {
                        result = $"adb -s {device} get-state".Term();
                        if (result.stdout.Contains("device")){
                            cmd.Append($" -s {device}");
                        } else {
                            Message.Critical(
                                msg: $" Device '{device}' not found."
                            );
                        }
                    }
                    cmd.Append($" shell monkey -p {packagename} 1");
                    result = cmd.ToString().Term();
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static bool CmdConnect(string ip, string port){
            bool connected = false;
            try
            {
                Response result = new Response();
                result = $"adb connect {ip}:{port}".Term(Output.Internal);
                if (result.stdout.StartsWith($"connected to {ip}:{port}")){
                    connected = true;
                } else {
                    connected = false;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return connected;
        }

        public static bool CmdDisconnect(string ip, string port){
            bool connected = true;
            try
            {
                
                Response result = new Response();
                result = $"adb disconnect {ip}:{port}".Term(Output.Internal);
                if (result.stdout.StartsWith($"disconnected {ip}:{port}")){
                    connected = false;
                } else {
                    connected = true;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return connected;
        }

        public static void CmdKillServer(){
            try
            {
                $"adb kill-server".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdStartServer(){
            try
            {
                $"adb start-server".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static string CmdList() {
            string response = "";
            try
            {
                Response result = new Response();
                result = $"adb devices -l".Term();
                response = Strings.Remove(result.stdout, $"List of devices attached{Environment.NewLine}");
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }
    }

    partial class BuildTools {
        public static void CmdSignerVerify(string path){
            try
            {
                $"apksigner verify --print-certs {path}".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdInformation(string path){
            try
            {
                $"aapt dump badging {path}".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}