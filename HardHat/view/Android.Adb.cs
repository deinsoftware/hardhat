using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Colorify.Colors;
using dein.tools;
using ToolBox.Platform;
using ToolBox.System;
using ToolBox.Validations;
using static HardHat.Program;

namespace HardHat
{

    public static partial class Adb
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "ar", status = true, action = Adb.Restart });
            opts.Add(new Option { opt = "ad", status = true, action = Adb.Devices });
            opts.Add(new Option { opt = "aw", status = true, action = Adb.Wireless });
            opts.Add(new Option { opt = "aw>i", status = true, action = Adb.Base });
            opts.Add(new Option { opt = "aw>p", status = true, action = Adb.Port });
            opts.Add(new Option { opt = "aw>c", status = true, action = Adb.Connect });
            opts.Add(new Option { opt = "al", status = true, action = Adb.Logcat });
            opts.Add(new Option { opt = "al>a", status = true, action = Adb.Application });
            opts.Add(new Option { opt = "al>p", status = true, action = Adb.Priority });
            opts.Add(new Option { opt = "al>s", status = true, action = Adb.Show });
        }

        public static void Start()
        {
            if (String.IsNullOrEmpty(_config.personal.adb.deviceName))
            {
                _colorify.WriteLine($" [A] ADB", txtMuted);
            }
            else
            {
                _colorify.Write($"[A] ADB: ", txtMuted);
                _colorify.WriteLine($"{_config.personal.adb.deviceName}");
            }
            _colorify.Write($"{"   [D] Devices",-17}", txtPrimary);
            _colorify.Write($"{"[L] Logcat",-17}", txtPrimary);
            if (!_config.personal.adb.wifiStatus)
            {
                _colorify.Write($"{"[W] WiFi Connect",-34}", txtPrimary);
            }
            else
            {
                _colorify.Write($"{"[W] WiFi Disconnect",-34}", txtPrimary);
            }
            _colorify.WriteLine($"{"[R] Restart",-17}", txtPrimary);
            _colorify.BlankLines();
        }

        public static void Install()
        {
            _colorify.Clear();

            try
            {
                Section.Header("INSTALL FILE");
                Section.SelectedFile();

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.android.projectPath, _config.android.buildPath, _config.personal.selected.path, _config.personal.selected.file);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Checking devices...", txtInfo);
                if (CmdDevices())
                {
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Installing...", txtInfo);
                    Response result = CmdInstall(dirPath, _config.personal.adb.deviceName);
                    if (result.code == 0)
                    {
                        _colorify.BlankLines();
                        _colorify.WriteLine($" --> Launching...", txtInfo);
                        CmdLaunch(dirPath, _config.personal.adb.deviceName);
                    }

                    Section.HorizontalRule();
                    Section.Pause();
                }
                else
                {
                    Message.Alert(" No device/emulators found");
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Restart()
        {
            _colorify.Clear();

            try
            {
                Section.Header("ADB KILL/RESTART");

                if (_config.personal.adb.wifiStatus)
                {
                    _colorify.WriteLine($" --> Disconnecting device...", txtInfo);
                    CmdDisconnect(_config.personal.adb.wifiIpAddress, _config.personal.adb.wifiPort);
                    _config.personal.adb.wifiStatus = false;
                    _colorify.BlankLines();
                }

                _colorify.WriteLine($" --> Kill Server...", txtInfo);
                CmdKillServer();

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Start Server...", txtInfo);
                CmdStartServer();

                _config.personal.adb.deviceName = "";

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Devices()
        {
            _colorify.Clear();

            try
            {
                Section.Header("DEVICE LIST");

                if (CmdDevices())
                {
                    string list = CmdList();
                    string[] lines = Shell.SplitLines(list);

                    if (lines.Length >= 1)
                    {
                        var i = 1;
                        foreach (string l in lines)
                        {
                            if (!String.IsNullOrEmpty(l))
                            {
                                _colorify.WriteLine($" {i,2}] {Shell.GetWord(l, 0)}", txtPrimary);
                                i++;
                            }
                        }
                    }

                    _colorify.BlankLines();
                    _colorify.WriteLine($"{"[EMPTY] None",82}", txtDanger);

                    Section.HorizontalRule();

                    _colorify.Write($"{" Make your choice:",-25}", txtInfo);
                    string opt = Console.ReadLine().Trim();

                    if (!String.IsNullOrEmpty(opt))
                    {
                        Number.IsOnRange(1, Convert.ToInt32(opt), list.Length);
                        var sel = Shell.GetWord(lines[Convert.ToInt32(opt) - 1], 0);
                        _config.personal.adb.deviceName = sel;
                    }
                    else
                    {
                        _config.personal.adb.deviceName = "";
                    }
                }
                else
                {
                    _config.personal.adb.deviceName = "";
                    Message.Alert(" No device found.");
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}