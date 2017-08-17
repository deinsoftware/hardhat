using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    static partial class ADB {
        public static void Install() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" INSTALL FILE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj, c.android.bld, cp.sfl); 

                $"{" Selected File:", -25}".txtMuted();
                $"{cp.sfl}".txtDefault(ct.WriteLine);

                $"".fmNewLine();
                $" --> Cheking devices...".txtInfo(ct.WriteLine);
                if (CmdDevices()){
                    $"".fmNewLine();
                    $" --> Installing...".txtInfo(ct.WriteLine);
                    Response result = CmdInstall(dirPath, cp.adb.dvc);
                    if (result.code == 0) {
                        $"".fmNewLine();
                        $" --> Launching...".txtInfo(ct.WriteLine);
                        CmdLaunch(dirPath, cp.adb.dvc);
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

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" ADB KILL/RESTART".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                if (cp.adb.wst)
                {
                    $" --> Disconnecting device...".txtInfo(ct.WriteLine);
                    CmdDisconnect(cp.adb.wip, cp.adb.wpr);
                    cp.adb.wst = false;
                    $"".fmNewLine();
                }
                
                $" --> Kill Server...".txtInfo(ct.WriteLine);
                CmdKillServer();

                $"".fmNewLine();
                $" --> Start Server...".txtInfo(ct.WriteLine);
                CmdStartServer();

                cp.adb.dvc = "";

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

            var c =  Program.config;
            var cp =  Program.config.personal;
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
                        cp.adb.dvc = "";
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
                    $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                    
                    $"".fmNewLine();
                    $"=".bgInfo(ct.Repeat);
                    $"".fmNewLine();

                    $"{" Make your choice:", -25}".txtInfo();
                    string opt = Console.ReadLine();

                    if (!String.IsNullOrEmpty(opt))
                    {
                        Validation.Range(opt, 1, list.Length);
                        var sel = Shell.GetWord(lines[Convert.ToInt32(opt) - 1], 0);
                        cp.adb.dvc = sel;
                    }
                } else {
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

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" CONNECT DEVICE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                cp.ipl = Network.GetLocalIPAddress();
                $"{" Current IP:", -25}".txtMuted();
                $"{cp.ipl}".txtDefault(ct.WriteLine);

                $"".fmNewLine();
                $"{" [I] IP Address:" , -25}".txtPrimary();   $"{cp.adb.wip}".txtDefault(ct.WriteLine);
                $"{" [P] Port:"       , -25}".txtPrimary();   $"{cp.adb.wpr}".txtDefault(ct.WriteLine);
                
                $"".fmNewLine();
                $"{" [C] Connect", -68}".txtStatus(ct.Write, !String.IsNullOrEmpty(cp.adb.wip));
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
                        if (!String.IsNullOrEmpty(cp.adb.wip))
                        {
                            Connect();
                            Message.Error();
                        }
                        break;
                    case "":
                        Menu.Start();
                        break;
                    default:
                        cp.mnu.sel = "aw";
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

            var c =  Program.config;
            var cp =  Program.config.personal;
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

                cp.ipb = Network.GetLocalIPBase(cp.ipl);
                $"{$" {cp.ipb} ", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    Validation.Range(opt, 1, 255);
                    cp.adb.wip = $"{cp.ipb}{opt}";
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

            var c =  Program.config;
            var cp =  Program.config.personal;
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
                    cp.adb.wpr = opt;
                } else {
                    cp.adb.wpr = "5555";
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

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" CONNECT DEVICE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" --> Connecting...".txtInfo(ct.WriteLine);
                bool connected = CmdConnect(cp.adb.wip, cp.adb.wpr);
                cp.adb.wst = connected;

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

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" DISCONNECT DEVICE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
                
                $" --> Disconnecting...".txtInfo(ct.WriteLine);
                bool connected = CmdDisconnect(cp.adb.wip, cp.adb.wpr);
                cp.adb.wst = connected;
                if (cp.adb.dvc == $"{cp.adb.wip}:{cp.adb.wpr}")
                {
                    cp.adb.dvc = "";
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
    static partial class BuildTools {
        public static void SignerVerify() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" SIGNER VERIFY".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj, c.android.bld, cp.sfl); 

                $"{" Selected File:", -25}".txtMuted();
                $"{cp.sfl}".txtDefault(ct.WriteLine);

                $"".fmNewLine();
                $" --> Verifing...".txtInfo(ct.WriteLine);
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

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" INFORMATION VALUES".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj, c.android.bld, cp.sfl); 

                $"{" Selected File:", -25}".txtMuted();
                $"{cp.sfl}".txtDefault(ct.WriteLine);

                $"".fmNewLine();
                $" --> Dump Badging...".txtInfo(ct.WriteLine);
                CmdInformation(dirPath);

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