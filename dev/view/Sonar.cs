using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public partial class Sonar {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Sonar()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }
        
        public static void Select() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("SONAR SERVER CONFIGURATION");
                Section.SelectedProject();

                if (Options.Valid("s"))
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.s_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $"{" [P] Protocol:"     , -25}".txtPrimary();   $"{_cp.snr.ptc}".txtDefault(ct.WriteLine);
                $"{" [D] Domain:"       , -25}".txtPrimary();   $"{_cp.snr.dmn}".txtDefault(ct.WriteLine);
                $"{" [P] Port:"         , -25}".txtPrimary();   $"{_cp.snr.prt}".txtDefault(ct.WriteLine);
                $"{" [I] Internal Path:", -25}".txtPrimary();   $"{_cp.snr.ipt}".txtDefault(ct.WriteLine);

                $"{"[EMPTY] Exit", 82}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if(String.IsNullOrEmpty(opt?.ToLower()))
                {
                    Menu.Start();
                } else {
                    Menu.Route($"s>{opt?.ToLower()}", "s");
                }
                Message.Error();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Protocol() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("SONAR SERVER CONFIGURATION", "PROTOCOL");
                Section.SelectedProject();

                if (!_cp.mnu.g_opt)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" {"1", 2}] http".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                $" {"2", 2}] https".txtPrimary(ct.WriteLine);
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_ptc = Console.ReadLine();
                opt_ptc = opt_ptc?.ToLower();

                if (!String.IsNullOrEmpty(opt_ptc)){
                    Validation.Range(opt_ptc, 1, 2);
                    switch (opt_ptc)
                    {
                        case "1":
                            _cp.gbs.ptc = "http";
                            break;
                        case "2":
                            _cp.gbs.ptc = "https";
                            break;
                        default:
                            Message.Error();
                            break;
                    }
                } else {
                    _cp.gbs.ptc = "http";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Server() {
            Colorify.Default();
            Console.Clear();

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
                $"{" [C] Connect", -68}".txtStatus(ct.Write, !String.IsNullOrEmpty(_cp.adb.wip));
                $"{"[EMPTY] Cancel", -17}".txtDanger(ct.WriteLine);

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
                //Configuration();
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
                Section.Header("CONNECT DEVICE", "PORT");
                
                $"".fmNewLine();
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
                //Configuration();
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
                Section.Header("CONNECT DEVICE");
                
                $" --> Connecting...".txtInfo(ct.WriteLine);
                //bool connected = CmdConnect(_cp.adb.wip, _cp.adb.wpr);
                //_cp.adb.wst = connected;

                Section.HorizontalRule();

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
        
        public static void InternalPath() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER CONFIGURATION", "INTERNAL PATH");
                Section.SelectedProject();

                if (!_cp.mnu.g_opt)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" Write an internal path inside your project.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_ipt = Console.ReadLine();
                _cp.gbs.ipt = $"{opt_ipt}";
                
                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        
        
        
        public static void Qube() {
            Colorify.Default();
            Console.Clear();

            try
            {
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr);
                // CmdServer(
                //     dirPath,
                //     Paths.Combine(Env.Get("GULP_PROJECT")),
                //     _cp.gbs.ipt,
                //     _cp.gbs.dmn,
                //     _cp.gbs.flv,
                //     _cp.gbs.srv,
                //     _cp.gbs.syn,
                //     _cp.ipl,
                //     _cp.gbs.ptc
                // );
                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Scanner() {
            Colorify.Default();
            Console.Clear();

            try
            {
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr);
                // CmdServer(
                //     dirPath,
                //     Paths.Combine(Env.Get("GULP_PROJECT")),
                //     _cp.gbs.ipt,
                //     _cp.gbs.dmn,
                //     _cp.gbs.flv,
                //     _cp.gbs.srv,
                //     _cp.gbs.syn,
                //     _cp.ipl,
                //     _cp.gbs.ptc
                // );
                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Browse() {
            Colorify.Default();
            Console.Clear();

            try
            {
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr);
                // CmdServer(
                //     dirPath,
                //     Paths.Combine(Env.Get("GULP_PROJECT")),
                //     _cp.gbs.ipt,
                //     _cp.gbs.dmn,
                //     _cp.gbs.flv,
                //     _cp.gbs.srv,
                //     _cp.gbs.syn,
                //     _cp.ipl,
                //     _cp.gbs.ptc
                // );
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