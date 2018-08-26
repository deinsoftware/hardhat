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
using ToolBox.Bridge;

namespace HardHat
{

    public static partial class Adb
    {
        public static void WirelessList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "aw", status = true, action = Adb.Wireless });
            opts.Add(new Option { opt = "aw>i", status = true, action = Adb.Base });
            opts.Add(new Option { opt = "aw>p", status = true, action = Adb.Port });
            opts.Add(new Option { opt = "aw>c", status = true, action = Adb.Connect });
        }


        public static void Wireless()
        {
            if (!_config.personal.adb.wifiStatus)
            {
                Adb.SelectWireless();
            }
            else
            {
                Adb.Disconnect();
            }
        }

        public static void SelectWireless()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE");

                _config.personal.ipAddress = Network.GetLocalIPv4();
                Section.CurrentIP();

                _colorify.BlankLines();
                _colorify.Write($"{" [I] IP Address:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.adb.wifiIpAddress}");
                _colorify.Write($"{" [P] Port:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.adb.wifiPort}");

                _colorify.BlankLines();
                _colorify.Write($"{" [C] Connect",-68}", txtStatus(!String.IsNullOrEmpty(_config.personal.adb.wifiIpAddress)));
                _colorify.WriteLine($"{"[EMPTY] Cancel",-17}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:",-25}", txtInfo);
                string opt = Console.ReadLine()?.ToLower();

                if (String.IsNullOrEmpty(opt))
                {
                    Menu.Start();
                }
                else
                {
                    Menu.Route($"aw>{opt}", "aw");
                }
                Message.Error();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Base()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE", "IP ADDRESS");

                _colorify.BlankLines();
                _colorify.WriteLine($" Write last mobile device IP octet.", txtPrimary);
                _colorify.WriteLine($" PC and Mobile device needs to be in same WiFi Network.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _config.personal.ipAddressBase = Network.GetOctetsIPv4(_config.personal.ipAddress, 3);
                _colorify.Write($"{$" {_config.personal.ipAddressBase} ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), 255);
                    _config.personal.adb.wifiIpAddress = $"{_config.personal.ipAddressBase}{opt}";
                }

                Menu.Status();
                SelectWireless();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Port()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE", "PORT");

                _colorify.WriteLine($" Write mobile device port.", txtPrimary);
                _colorify.Write($" Between 5555", txtPrimary); _colorify.Write($" (Default)", txtInfo); _colorify.WriteLine($" and 5585", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(5555, Convert.ToInt32(opt), 5585);
                    _config.personal.adb.wifiPort = opt;
                }
                else
                {
                    _config.personal.adb.wifiPort = "5555";
                }

                Menu.Status();

                StringBuilder msg = new StringBuilder();
                msg.Append($" Do you want to change device port to {_config.personal.adb.wifiPort}?");
                bool update = Message.Confirmation(msg.ToString());
                if (update)
                {
                    Listening();
                }

                SelectWireless();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Listening()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE", "LISTENING PORT");

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Checking devices...", txtInfo);
                if (CmdDevices())
                {
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Changing port...", txtInfo);
                    Response result = CmdTcpIp(_config.personal.adb.wifiPort, _config.personal.adb.deviceName);
                    if (result.code == 0)
                    {
                        _colorify.WriteLine($" --> Restarting...", txtInfo);
                        _colorify.BlankLines();
                        _colorify.WriteLine($" TCP mode port: {_config.personal.adb.wifiPort}");
                    }

                    Section.HorizontalRule();
                    Section.Pause();
                }
                else
                {
                    Message.Alert(" No device/emulators found");
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Connect()
        {
            if (!String.IsNullOrEmpty(_config.personal.adb.wifiIpAddress))
            {
                _colorify.Clear();

                try
                {
                    Section.Header("CONNECT DEVICE", "CONNECT");

                    _colorify.WriteLine($" --> Connecting...", txtInfo);
                    bool connected = CmdConnect(_config.personal.adb.wifiIpAddress, _config.personal.adb.wifiPort);
                    _config.personal.adb.wifiStatus = connected;

                    Section.HorizontalRule();
                    Section.Pause();

                    Menu.Start();
                }
                catch (Exception Ex)
                {
                    Exceptions.General(Ex);
                }
            }
        }
        public static void Disconnect()
        {
            _colorify.Clear();

            try
            {
                Section.Header("DISCONNECT DEVICE");

                _colorify.WriteLine($" --> Disconnecting...", txtInfo);
                bool connected = CmdDisconnect(_config.personal.adb.wifiIpAddress, _config.personal.adb.wifiPort);
                _config.personal.adb.wifiStatus = connected;
                if (_config.personal.adb.deviceName == $"{_config.personal.adb.wifiIpAddress}:{_config.personal.adb.wifiPort}")
                {
                    _config.personal.adb.deviceName = "";
                }

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}