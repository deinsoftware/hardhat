using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public static partial class Adb {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Adb()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="ar"  , stt=true , act=Adb.Restart                      });
            opts.Add(new Option{opt="ad"  , stt=true , act=Adb.Devices                      });
            opts.Add(new Option{opt="aw"  , stt=true , act=Adb.Wireless                     });
        }

        public static void Start(){
            if (String.IsNullOrEmpty(_cp.adb.dvc))
            {
                $" [A] ADB".txtMuted(ct.WriteLine);
            } else {
                $"{" [A] ADB:"          , -25}".txtMuted();
                $"{_cp.adb.dvc}".txtDefault(ct.WriteLine);
            }
            $"{"   [D] Devices"         , -34}".txtPrimary(ct.Write);
            if (!_cp.adb.wst)
            {
                $"{"[W] WiFi Connect"   , -34}".txtPrimary(ct.Write);
            } else {
                $"{"[W] WiFi Disconnect", -34}".txtPrimary(ct.Write);
            }
            $"{"[R] Restart"            , -17}".txtPrimary(ct.WriteLine);
            $"".fmNewLine();
        }

        public static void Install() {
            Colorify.Default();

            try
            {
                Section.Header("INSTALL FILE");
                Section.SelectedFile();

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld, _cp.sfl); 

                $"".fmNewLine();
                $" --> Checking devices...".txtInfo(ct.WriteLine);
                if (CmdDevices()){
                    $"".fmNewLine();
                    $" --> Installing...".txtInfo(ct.WriteLine);
                    Response result = CmdInstall(dirPath, _cp.adb.dvc);
                    if (result.code == 0) {
                        $"".fmNewLine();
                        $" --> Launching...".txtInfo(ct.WriteLine);
                        CmdLaunch(dirPath, _cp.adb.dvc);
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
            Colorify.Default();

            try
            {
                Section.Header("ADB KILL/RESTART");
                
                if (_cp.adb.wst)
                {
                    $" --> Disconnecting device...".txtInfo(ct.WriteLine);
                    CmdDisconnect(_cp.adb.wip, _cp.adb.wpr);
                    _cp.adb.wst = false;
                    $"".fmNewLine();
                }
                
                $" --> Kill Server...".txtInfo(ct.WriteLine);
                CmdKillServer();

                $"".fmNewLine();
                $" --> Start Server...".txtInfo(ct.WriteLine);
                CmdStartServer();

                _cp.adb.dvc = "";

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Devices() {
            Colorify.Default();

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
                                $" {i, 2}] {Shell.GetWord(l, 0)}".txtPrimary(ct.WriteLine);
                                i++;
                            }
                        }
                    }

                    $"".fmNewLine();
                    $"{"[EMPTY] None", 82}".txtDanger(ct.WriteLine);
                    
                    Section.HorizontalRule();

                    $"{" Make your choice:", -25}".txtInfo();
                    string opt = Console.ReadLine();

                    if (!String.IsNullOrEmpty(opt))
                    {
                        Validation.Range(opt, 1, list.Length);
                        var sel = Shell.GetWord(lines[Convert.ToInt32(opt) - 1], 0);
                        _cp.adb.dvc = sel;
                    } else {
                        _cp.adb.dvc = "";
                    }
                } else {
                    _cp.adb.dvc = "";
                    Message.Alert(" No device found.");
                }
                
                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Wireless(){
            if (!_cp.adb.wst) { 
                Adb.Configuration(); 
            } else { 
                Adb.Disconnect(); 
            }
        }

        public static void Configuration() {
            Colorify.Default();

            try
            {
                Section.Header("CONNECT DEVICE");
                
                _cp.ipl = Network.GetLocalIPAddress();
                $"{" Current IP:", -25}".txtMuted();
                $"{_cp.ipl}".txtDefault(ct.WriteLine);

                $"".fmNewLine();
                $"{" [I] IP Address:" , -25}".txtPrimary();   $"{_cp.adb.wip}".txtDefault(ct.WriteLine);
                $"{" [P] Port:"       , -25}".txtPrimary();   $"{_cp.adb.wpr}".txtDefault(ct.WriteLine);
                
                $"".fmNewLine();
                $"{" [C] Connect"     , -68}".txtStatus(ct.Write, !String.IsNullOrEmpty(_cp.adb.wip));
                $"{"[EMPTY] Cancel"   , -17}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
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
                        if (!String.IsNullOrEmpty(_cp.adb.wip))
                        {
                            Connect();
                            Message.Error();
                        }
                        break;
                    case "":
                        Menu.Start();
                        break;
                    default:
                        _cp.mnu.sel = "aw";
                        break;
                }

                Message.Error();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Base() {
            Colorify.Default();

            try
            {
                Section.Header("CONNECT DEVICE", "IP ADDRESS");
                
                $"".fmNewLine();
                $" Write last mobile device IP octet.".txtPrimary(ct.WriteLine);
                $" PC and Mobile device needs to be in same WiFi Network.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();

                _cp.ipb = Network.GetLocalIPBase(_cp.ipl);
                $"{$" {_cp.ipb} ", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    Validation.Range(opt, 1, 255);
                    _cp.adb.wip = $"{_cp.ipb}{opt}";
                }

                Menu.Status();
                Configuration();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Port() {
            Colorify.Default();

            try
            {
                Section.Header("CONNECT DEVICE", "PORT");
                
                $" Write mobile device port.".txtPrimary(ct.WriteLine);
                $" Between 5555".txtPrimary(); $" (Default)".txtInfo(); $" and 5585".txtPrimary(ct.WriteLine); 
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    Validation.Range(opt, 5555, 5585);
                    _cp.adb.wpr = opt;
                } else {
                    _cp.adb.wpr = "5555";
                }

                Menu.Status();
                Configuration();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Connect() {
            Colorify.Default();

            try
            {
                Section.Header("CONNECT DEVICE");
                
                $" --> Connecting...".txtInfo(ct.WriteLine);
                bool connected = CmdConnect(_cp.adb.wip, _cp.adb.wpr);
                _cp.adb.wst = connected;

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        public static void Disconnect() {
            Colorify.Default();

            try
            {
                Section.Header("DISCONNECT DEVICE");
                
                $" --> Disconnecting...".txtInfo(ct.WriteLine);
                bool connected = CmdDisconnect(_cp.adb.wip, _cp.adb.wpr);
                _cp.adb.wst = connected;
                if (_cp.adb.dvc == $"{_cp.adb.wip}:{_cp.adb.wpr}")
                {
                    _cp.adb.dvc = "";
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
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static BuildTools()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }
        
        public static void SignerVerify() {
            Colorify.Default();

            try
            {
                Section.Header("SIGNER VERIFY");
                Section.SelectedFile();
                
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld, _cp.sfl); 

                $"".fmNewLine();
                $" --> Verifying...".txtInfo(ct.WriteLine);
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
            Colorify.Default();

            try
            {
                Section.Header("INFORMATION VALUES");
                Section.SelectedFile();
                
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld, _cp.sfl); 

                $"".fmNewLine();
                $" --> Dump Badging...".txtInfo(ct.WriteLine);
                CmdInformation(dirPath);

                if ((Os.IsWindows() && Variables.Valid("sh")) || Os.IsMacOS()){
                    Response result = CmdSha(dirPath);
                    if (result.code == 0) {
                        $"".fmNewLine();
                        $" --> File Hash...".txtInfo(ct.WriteLine);
                
                        $"".fmNewLine();
                        $" SHA256: ".txtMuted();
                        $"{result.stdout}".txtDefault(ct.WriteLine);    
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
                string dirPath = Paths.Combine(Variables.Value("ah"), "build-tools");

                if (Directory.Exists(dirPath)){
                    string dir = Directory.EnumerateDirectories(dirPath).OrderByDescending(name => name).Take(1).FirstOrDefault();
                    string d = dir.Slash();
                    lastVersion = d.Substring(d.LastIndexOf("/") + 1);
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