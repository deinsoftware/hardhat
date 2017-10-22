using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public partial class Adb {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Adb()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Install() {
            Colorify.Default();
            Console.Clear();

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

                    $"".fmNewLine();
                    $"=".bgInfo(ct.Repeat);
                    $"".fmNewLine();

                    $" Press [Any] key to continue...".txtInfo();
                    Console.ReadKey();
                } else {
                    Message.Alert(" No device/emulators found");
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Restart() {
            Colorify.Default();
            Console.Clear();

            try
            {
                $"=".bgInfo(ct.Repeat);
                $" ADB KILL/RESTART".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

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

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Press [Any] key to continue...".txtInfo();
                Console.ReadKey();

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Devices() {
            Colorify.Default();
            Console.Clear();

            try
            {
                $"=".bgInfo(ct.Repeat);
                $" DEVICE LIST".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                if (CmdDevices()){
                    string list = CmdList();
                    string[] lines = Shell.SplitLines(list);

                    if (lines.Length < 1) {
                        _cp.adb.dvc = "";
                    } else {
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
                    
                    $"".fmNewLine();
                    $"=".bgInfo(ct.Repeat);
                    $"".fmNewLine();

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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Configuration() {
            Colorify.Default();
            Console.Clear();

            try
            {
                $"=".bgInfo(ct.Repeat);
                $" CONNECT DEVICE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                _cp.ipl = Network.GetLocalIPAddress();
                $"{" Current IP:", -25}".txtMuted();
                $"{_cp.ipl}".txtDefault(ct.WriteLine);

                $"".fmNewLine();
                $"{" [I] IP Address:" , -25}".txtPrimary();   $"{_cp.adb.wip}".txtDefault(ct.WriteLine);
                $"{" [P] Port:"       , -25}".txtPrimary();   $"{_cp.adb.wpr}".txtDefault(ct.WriteLine);
                
                $"".fmNewLine();
                $"{" [C] Connect", -68}".txtStatus(ct.Write, !String.IsNullOrEmpty(_cp.adb.wip));
                $"{"[EMPTY] Cancel", -17}".txtDanger(ct.WriteLine);

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Base() {
            Colorify.Default();
            Console.Clear();

            try
            {
                $"=".bgInfo(ct.Repeat);
                $" CONNECT DEVICE > IP ADDRESS".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                
                $"".fmNewLine();
                $" Write last mobile device IP octet.".txtPrimary(ct.WriteLine);
                $" PC and Mobile device needs to be in same WiFi Network.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Port() {
            Colorify.Default();
            Console.Clear();

            try
            {
                $"=".bgInfo(ct.Repeat);
                $" CONNECT DEVICE > PORT".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Write mobile device port.".txtPrimary(ct.WriteLine);
                $" Between 5555".txtPrimary(); $" (Default)".txtInfo(); $" and 5585".txtPrimary(ct.WriteLine); 
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Connect() {
            Colorify.Default();
            Console.Clear();

            try
            {
                $"=".bgInfo(ct.Repeat);
                $" CONNECT DEVICE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" --> Connecting...".txtInfo(ct.WriteLine);
                bool connected = CmdConnect(_cp.adb.wip, _cp.adb.wpr);
                _cp.adb.wst = connected;

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Press [Any] key to continue...".txtInfo();
                Console.ReadKey();

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
        public static void Disconnect() {
            Colorify.Default();
            Console.Clear();

            try
            {
                $"=".bgInfo(ct.Repeat);
                $" DISCONNECT DEVICE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
                
                $" --> Disconnecting...".txtInfo(ct.WriteLine);
                bool connected = CmdDisconnect(_cp.adb.wip, _cp.adb.wpr);
                _cp.adb.wst = connected;
                if (_cp.adb.dvc == $"{_cp.adb.wip}:{_cp.adb.wpr}")
                {
                    _cp.adb.dvc = "";
                }

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Press [Any] key to continue...".txtInfo();
                Console.ReadKey();

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
    partial class BuildTools {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static BuildTools()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }
        
        public static void SignerVerify() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("SIGNER VERIFY");
                Section.SelectedFile();
                
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld, _cp.sfl); 

                $"".fmNewLine();
                $" --> Verifying...".txtInfo(ct.WriteLine);
                CmdSignerVerify(dirPath);

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Press [Any] key to continue...".txtInfo();
                Console.ReadKey();

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Information() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("INFORMATION VALUES");
                Section.SelectedFile();
                
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld, _cp.sfl); 

                $"".fmNewLine();
                $" --> Dump Badging...".txtInfo(ct.WriteLine);
                CmdInformation(dirPath);

                if ((Os.IsWindows() && _cp.mnu.s_env) || Os.IsMacOS()){
                    Response result = CmdSha(dirPath);
                    if (result.code == 0) {
                        $"".fmNewLine();
                        $" --> File Hash...".txtInfo(ct.WriteLine);
                
                        $"".fmNewLine();
                        $" SHA256: ".txtMuted();
                        $"{result.stdout}".txtDefault(ct.WriteLine);    
                    }
                }

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Press [Any] key to continue...".txtInfo();
                Console.ReadKey();

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}