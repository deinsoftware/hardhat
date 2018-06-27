using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Platform;
using ToolBox.Transform;

namespace HardHat
{
    public static partial class Adb
    {
        public static bool CmdDevices()
        {
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
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return dev;
        }

        public static Response CmdState(string device)
        {
            Response result = $"adb -s {device} get-state".Term();
            if (result.stdout.Contains("not found"))
            {
                result.code = 0;
            }
            else
            {
                result.code = 1;
                Message.Error(
                    msg: $" Device '{device}' not found."
                );
            }
            return result;
        }

        public static Response CmdInstall(string path, string device)
        {
            Response result = new Response();
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    result = CmdState(device);
                    if (result.code == 0)
                    {
                        cmd.Append($" -s {device}");
                    }
                }
                cmd.Append($" install -r {path} 2>&1");
                result = cmd.ToString().Term(Output.Internal);
                string status = Shell.ExtractLine(result.stdout, "Success");
                if (status.Contains("Success"))
                {
                    result.code = 0;
                }
                else
                {
                    result.code = 1;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return result;
        }

        public static string CmdGetPid(string packagename)
        {
            string result = "";
            try
            {
                Response pid = $"adb shell pidof -s {packagename}".Term();
                pid.stdout = pid.stdout
                    .Replace("\r", "")
                    .Replace("\n", "");
                result = pid.stdout;
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return result;
        }

        public static void CmdLaunch(string path, string device)
        {
            try
            {
                Response result = new Response();
                string packagename = BuildTools.CmdGetPackageName(path);
                if (!String.IsNullOrEmpty(packagename))
                {
                    StringBuilder cmd = new StringBuilder();
                    cmd.Append("adb");
                    if (!String.IsNullOrEmpty(device))
                    {
                        result = CmdState(device);
                        if (result.code == 0)
                        {
                            cmd.Append($" -s {device}");
                        }
                    }
                    cmd.Append($" shell monkey -p {packagename} 1");
                    cmd.ToString().Term();
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdLogcat(string device, string priority, string pid = "")
        {
            try
            {
                Response result = new Response();

                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    result = CmdState(device);
                    if (result.code == 0)
                    {
                        cmd.Append($" -s {device}");
                    }
                }
                cmd.Append(" logcat");
                cmd.Append($" *:");
                if (!String.IsNullOrEmpty(priority))
                {
                    cmd.Append($"{priority.ToUpper()}");
                }
                else
                {
                    cmd.Append($"V");
                }

                cmd.Append(" -v color");
                if (!String.IsNullOrEmpty(pid))
                {
                    cmd.Append($" --pid={pid}");
                }
                cmd.ToString().Term(Output.External);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static Response CmdTcpIp(string port, string device)
        {
            Response result = new Response();
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    result = CmdState(device);
                    if (result.code == 0)
                    {
                        cmd.Append($" -s {device}");
                    }
                }
                cmd.Append($" tcpip {port} 2>&1");
                result = cmd.ToString().Term(Output.Internal);
                string status = Shell.ExtractLine(result.stdout, $"{port}");
                if (String.IsNullOrEmpty(status) || status.Contains($"{port}"))
                {
                    result.code = 0;
                }
                else
                {
                    result.code = 1;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return result;
        }

        public static bool CmdConnect(string ip, string port)
        {
            bool connected = false;
            try
            {
                Response result = $"adb connect {ip}:{port}".Term(Output.Internal);
                if (result.stdout.Contains($"connected to {ip}:{port}"))
                {
                    connected = true;
                }
                else
                {
                    connected = false;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return connected;
        }

        public static bool CmdDisconnect(string ip, string port)
        {
            bool connected = false;
            try
            {
                $"adb disconnect {ip}:{port}".Term(Output.Internal);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return connected;
        }

        public static void CmdKillServer()
        {
            try
            {
                $"adb kill-server".Term(Output.Internal);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdStartServer()
        {
            try
            {
                $"adb start-server".Term(Output.Internal);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static string CmdList()
        {
            string result = "";
            try
            {
                Response response = $"adb devices -l".Term();
                result = Strings.RemoveWords(response.stdout, $"List of devices attached{Environment.NewLine}");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return result;
        }
    }
}