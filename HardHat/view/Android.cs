using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Colorify;
using static Colorify.Colors;
using dein.tools;
using ToolBox.Platform;
using ToolBox.System;
using ToolBox.Validations;
using static HardHat.Program;

namespace HardHat {

    public static partial class Adb {

        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="ar"  , stt=true , act=Adb.Restart                      });
            opts.Add(new Option{opt="ad"  , stt=true , act=Adb.Devices                      });
            opts.Add(new Option{opt="aw"  , stt=true , act=Adb.Wireless                     });
        }

        public static void Start(){
            if (String.IsNullOrEmpty(_config.personal.adb.dvc))
            {
                _colorify.WriteLine($" [A] ADB", txtMuted);
            } else {
                _colorify.Write($"[A] ADB: ", txtMuted);
                _colorify.WriteLine($"{_config.personal.adb.dvc}");
            }
            _colorify.Write($"{"   [D] Devices"         , -34}", txtPrimary);
            if (!_config.personal.adb.wst)
            {
                _colorify.Write($"{"[W] WiFi Connect"   , -34}", txtPrimary);
            } else {
                _colorify.Write($"{"[W] WiFi Disconnect", -34}", txtPrimary);
            }
            _colorify.WriteLine($"{"[R] Restart"        , -17}", txtPrimary);
            _colorify.BlankLines();
        }

        public static void Install() {
            _colorify.Clear();

            try
            {
                Section.Header("INSTALL FILE");
                Section.SelectedFile();

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld, _config.personal.sfl); 

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Checking devices...", txtInfo);
                if (CmdDevices()){
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Installing...", txtInfo);
                    Response result = CmdInstall(dirPath, _config.personal.adb.dvc);
                    if (result.code == 0) {
                        _colorify.BlankLines();
                        _colorify.WriteLine($" --> Launching...", txtInfo);
                        CmdLaunch(dirPath, _config.personal.adb.dvc);
                    }

                    Section.HorizontalRule();
                    Section.Pause();
                } else {
                    Message.Alert(" No device/emulators found");
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Restart() {
            _colorify.Clear();

            try
            {
                Section.Header("ADB KILL/RESTART");
                
                if (_config.personal.adb.wst)
                {
                    _colorify.WriteLine($" --> Disconnecting device...", txtInfo);
                    CmdDisconnect(_config.personal.adb.wip, _config.personal.adb.wpr);
                    _config.personal.adb.wst = false;
                    _colorify.BlankLines();
                }
                
                _colorify.WriteLine($" --> Kill Server...", txtInfo);
                CmdKillServer();

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Start Server...", txtInfo);
                CmdStartServer();

                _config.personal.adb.dvc = "";

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Devices() {
            _colorify.Clear();

            try
            {
                Section.Header("DEVICE LIST");
                
                if (CmdDevices()){
                    string list = CmdList();
                    string[] lines = Shell.SplitLines(list);

                    if (lines.Length >= 1) {
                        var i = 1;
                        foreach (string l in lines)
                        {
                            if (!String.IsNullOrEmpty(l))
                            {
                                _colorify.WriteLine($" {i, 2}] {Shell.GetWord(l, 0)}", txtPrimary);
                                i++;
                            }
                        }
                    }

                    _colorify.BlankLines();
                    _colorify.WriteLine($"{"[EMPTY] None", 82}", txtDanger);
                    
                    Section.HorizontalRule();

                    _colorify.Write($"{" Make your choice:", -25}", txtInfo);
                    string opt = Console.ReadLine();

                    if (!String.IsNullOrEmpty(opt))
                    {
                        Number.IsOnRange(1, Convert.ToInt32(opt), list.Length);
                        var sel = Shell.GetWord(lines[Convert.ToInt32(opt) - 1], 0);
                        _config.personal.adb.dvc = sel;
                    } else {
                        _config.personal.adb.dvc = "";
                    }
                } else {
                    _config.personal.adb.dvc = "";
                    Message.Alert(" No device found.");
                }
                
                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Wireless(){
            if (!_config.personal.adb.wst) { 
                Adb.Configuration(); 
            } else { 
                Adb.Disconnect(); 
            }
        }

        public static void Configuration() {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE");
                
                _config.personal.ipl = Network.GetLocalIPv4();
                _colorify.Write($"{" Current IP:", -25}", txtMuted);
                _colorify.WriteLine($"{_config.personal.ipl}");

                _colorify.BlankLines();
                _colorify.Write($"{" [I] IP Address:" , -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.adb.wip}");
                _colorify.Write($"{" [P] Port:"       , -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.adb.wpr}");
                
                _colorify.BlankLines();
                _colorify.Write($"{" [C] Connect"     , -68}", txtStatus(!String.IsNullOrEmpty(_config.personal.adb.wip)));
                _colorify.WriteLine($"{"[EMPTY] Cancel"   , -17}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
                string opt = Console.ReadLine();

                switch (opt?.ToLower())
                {
                    case "i":
                        Base();
                        break;
                    case "p":
                        Port();
                        break;
                    case "c":
                        if (!String.IsNullOrEmpty(_config.personal.adb.wip))
                        {
                            Connect();
                            Message.Error();
                        }
                        break;
                    case "":
                        Menu.Start();
                        break;
                    default:
                        _config.personal.mnu.sel = "aw";
                        break;
                }

                Message.Error();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Base() {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE", "IP ADDRESS");
                
                _colorify.BlankLines();
                _colorify.WriteLine($" Write last mobile device IP octet.", txtPrimary);
                _colorify.WriteLine($" PC and Mobile device needs to be in same WiFi Network.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();

                _config.personal.ipb = Network.RemoveLastOctetIPv4(_config.personal.ipl);
                _colorify.Write($"{$" {_config.personal.ipb} ", -25}", txtInfo);
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    Number.IsOnRange(1, Convert.ToInt32(opt), 255);
                    _config.personal.adb.wip = $"{_config.personal.ipb}{opt}";
                }

                Menu.Status();
                Configuration();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Port() {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE", "PORT");
                
                _colorify.WriteLine($" Write mobile device port.", txtPrimary);
                _colorify.Write($" Between 5555", txtPrimary); _colorify.Write($" (Default)", txtInfo); _colorify.WriteLine($" and 5585", txtPrimary); 
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default", 82}", txtInfo);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    Number.IsOnRange(5555, Convert.ToInt32(opt), 5585);
                    _config.personal.adb.wpr = opt;
                } else {
                    _config.personal.adb.wpr = "5555";
                }

                Menu.Status();

                StringBuilder msg = new StringBuilder();
                msg.Append($" Do you want to change device port to {_config.personal.adb.wpr}?");
                bool update = Message.Confirmation(msg.ToString());
                if (update){
                    Listening();
                }

                Configuration();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Listening() {
            _colorify.Clear();

            try
            {
                Section.Header("LISTENING PORT");

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Checking devices...", txtInfo);
                if (CmdDevices()){
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Changing port...", txtInfo);
                    Response result = CmdTcpIp(_config.personal.adb.wpr, _config.personal.adb.dvc);
                    if (result.code == 0) {
                        _colorify.BlankLines();
                        _colorify.WriteLine($" --> Restarting...", txtInfo);
                        _colorify.BlankLines();
                        _colorify.WriteLine($" TCP mode port: {_config.personal.adb.wpr}");                        
                    }

                    Section.HorizontalRule();
                    Section.Pause();
                } else {
                    Message.Alert(" No device/emulators found");
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Connect() {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE");
                
                _colorify.WriteLine($" --> Connecting...", txtInfo);
                bool connected = CmdConnect(_config.personal.adb.wip, _config.personal.adb.wpr);
                _config.personal.adb.wst = connected;

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        public static void Disconnect() {
            _colorify.Clear();

            try
            {
                Section.Header("DISCONNECT DEVICE");
                
                _colorify.WriteLine($" --> Disconnecting...", txtInfo);
                bool connected = CmdDisconnect(_config.personal.adb.wip, _config.personal.adb.wpr);
                _config.personal.adb.wst = connected;
                if (_config.personal.adb.dvc == $"{_config.personal.adb.wip}:{_config.personal.adb.wpr}")
                {
                    _config.personal.adb.dvc = "";
                }

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }

    public static partial class BuildTools {
        
        public static void SignerVerify() {
            _colorify.Clear();

            try
            {
                Section.Header("SIGNER VERIFY");
                Section.SelectedFile();
                
                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld, _config.personal.sfl); 

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Verifying...", txtInfo);
                CmdSignerVerify(dirPath);

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Information() {
            _colorify.Clear();

            try
            {
                Section.Header("INFORMATION VALUES");
                Section.SelectedFile();
                
                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld, _config.personal.sfl); 

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Dump Badging...", txtInfo);
                CmdInformation(dirPath);

                if ((OS.IsWin() && Variables.Valid("sh")) || OS.IsMac()){
                    Response result = CmdSha(dirPath);
                    if (result.code == 0) {
                        _colorify.BlankLines();
                        _colorify.WriteLine($" --> File Hash...", txtInfo);
                
                        _colorify.BlankLines();
                        _colorify.Write($" SHA256: ", txtMuted);
                        _colorify.WriteLine($"{result.stdout}");
                    }
                }

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Upgrade(){
            try
            {
                string currentVersion = Variables.Value("ab");
                string lastVersion = "";
                string dirPath = _path.Combine(Variables.Value("ah"), "build-tools");

                if (Directory.Exists(dirPath)){
                    string dir = Directory.EnumerateDirectories(dirPath).OrderByDescending(name => name).Take(1).FirstOrDefault();
                    string d = dir;
                    lastVersion = _path.GetFileName(d);
                    if (currentVersion != lastVersion){
                        StringBuilder msg = new StringBuilder();
                        msg.Append($"There is a new Android Build Tools version installed.");
                        msg.Append(Environment.NewLine);
                        msg.Append($" Please verify your Environment Variables and change ANDROID_BT_VERSION from {currentVersion} to {lastVersion}.");
                        Message.Alert(msg.ToString());
                    }
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}