using System;
using System.Text;
using dein.tools;
using ToolBox.Bridge;
using ToolBox.Platform;
using ToolBox.Transform;
using static HardHat.Program;

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
                Response result = _shell.Term($"adb devices -l");
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
            Response result = _shell.Term($"adb -s {device} get-state");
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
                result = _shell.Term(cmd.ToString(), Output.Internal);
                string status = Strings.ExtractLine(result.stdout, "Success");
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
                Response pid = _shell.Term($"adb shell pidof -s {packagename}");
                pid.stdout = Strings.CleanSpecialCharacters(pid.stdout);
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
                string packagename = _config.personal.selected.packageName;
                if (!String.IsNullOrEmpty(packagename))
                {
                    StringBuilder cmd = new StringBuilder();
                    cmd.Append("adb");
                    if (!String.IsNullOrEmpty(device))
                    {
                        Response result = CmdState(device);
                        if (result.code == 0)
                        {
                            cmd.Append($" -s {device}");
                        }
                    }
                    cmd.Append($" shell monkey -p {packagename} 1");
                    _shell.Term(cmd.ToString());
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CmdLogcat(string device, LogcatConfiguration logcat, string pid = "")
        {
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    Response result = CmdState(device);
                    if (result.code == 0)
                    {
                        cmd.Append($" -s {device}");
                    }
                }
                cmd.Append(" logcat");
                cmd.Append($" *:");
                if (!String.IsNullOrEmpty(logcat.priority))
                {
                    cmd.Append($"{logcat.priority.ToUpper()}");
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

                if (!String.IsNullOrEmpty(logcat.filter))
                {
                    cmd.Append($" | ");
                    switch (OS.GetCurrent())
                    {
                        case "win":
                            cmd.Append($"findstr ");
                            break;
                        case "mac":
                            cmd.Append($"egrep -i ");
                            break;
                    }
                    cmd.Append($"{logcat.filter}");
                }

                _shell.Term(cmd.ToString(), Output.External);
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
                result = _shell.Term(cmd.ToString(), Output.Internal);
                string status = Strings.ExtractLine(result.stdout, $"{port}");
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
                Response result = _shell.Term($"adb connect {ip}:{port}", Output.Internal);
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
                _shell.Term($"adb disconnect {ip}:{port}", Output.Internal);
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
                _shell.Term($"adb kill-server", Output.Internal);
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
                _shell.Term($"adb start-server", Output.Internal);
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
                Response response = _shell.Term($"adb devices -l");
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