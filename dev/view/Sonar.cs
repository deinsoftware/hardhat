using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public static partial class Sonar {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Sonar()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="s"   , stt=true , act=Sonar.Select                     });
            opts.Add(new Option{opt="s>p" , stt=true , act=Sonar.Protocol                   });
            opts.Add(new Option{opt="s>s" , stt=true , act=Sonar.Server                     });
            opts.Add(new Option{opt="s>i" , stt=true , act=Sonar.InternalPath               });
            opts.Add(new Option{opt="sq"  , stt=false, act=Sonar.Qube                       });
            opts.Add(new Option{opt="ss"  , stt=false, act=Sonar.Scanner                    });
            opts.Add(new Option{opt="sb"  , stt=false, act=Sonar.Browse                     });
        }

        public static void Status(){
            StringBuilder s_cnf = new StringBuilder();
            s_cnf.Append($"{_cp.snr.ptc}://");
            if (!String.IsNullOrEmpty(_cp.snr.dmn))
            {
                s_cnf.Append($"{_cp.snr.dmn}");
            }
            if (!String.IsNullOrEmpty(_cp.snr.prt))
            {
                s_cnf.Append($":{_cp.snr.prt}");
            }
            _cp.mnu.s_url = s_cnf.ToString();
            if (!String.IsNullOrEmpty(_cp.snr.ipt))
            {
                s_cnf.Append($"/{_cp.snr.ipt}");
            }
            _cp.mnu.s_cnf = s_cnf.ToString();

            _cp.mnu.s_val = !Strings.SomeNullOrEmpty(_cp.snr.ptc, _cp.snr.dmn, _cp.mnu.s_cnf);
            Options.Valid("s" , Variables.Valid("sq"));
            Options.Valid("sq", Variables.Valid("sq"));
            Options.Valid("ss", Variables.Valid("ss") && !Strings.SomeNullOrEmpty(_cp.spr));
            Options.Valid("sb", _cp.mnu.s_val);
        }
        
        public static void Start() {
            if (String.IsNullOrEmpty(_cp.mnu.s_cnf))
            {
                $" [S] Sonar".txtStatus(ct.WriteLine,           Options.Valid("s"));
            } else {
                $"{" [S] Sonar:", -25}".txtStatus(ct.Write,     Options.Valid("s"));
                StringBuilder s_cnf = new StringBuilder();
                Section.Configuration(_cp.mnu.s_val, _cp.mnu.s_cnf);
            }
            $"{"   [Q] Qube"   , -34}".txtStatus(ct.Write,      Options.Valid("sq"));
            $"{"[S] Scanner"   , -34}".txtStatus(ct.Write,      Options.Valid("ss"));
            $"{"[B] Browse"    , -17}".txtStatus(ct.WriteLine,  Options.Valid("sb"));
            $"".fmNewLine();
        }

        public static void Select() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("SONAR SERVER CONFIGURATION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);

                $"".fmNewLine();
                $"{" [P] Protocol:"     , -25}".txtPrimary();   $"{_cp.snr.ptc}".txtDefault(ct.WriteLine);
                $"{" [S] Server:"       , -25}".txtPrimary();   $"{_cp.snr.dmn}{(!String.IsNullOrEmpty(_cp.snr.prt) ? ":"+_cp.snr.prt : "")}".txtDefault(ct.WriteLine);
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

        public static void Server() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("SONAR SERVER CONFIGURATION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);

                $"".fmNewLine();
                $"{" [P] Protocol:"     , -25}".txtPrimary();   $"{_cp.snr.ptc}".txtDefault(ct.WriteLine);
                $"{" [S] Server:"       , -25}".txtPrimary();   $"{_cp.snr.dmn}{(!String.IsNullOrEmpty(_cp.snr.prt) ? ":"+_cp.snr.prt : "")}".txtDefault(ct.WriteLine);
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

                if (!String.IsNullOrEmpty(_cp.mnu.s_cnf))
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.s_cnf}".txtDefault(ct.WriteLine);
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

        public static void Domain() {
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

        public static void InternalPath() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER CONFIGURATION", "INTERNAL PATH");
                Section.SelectedProject();

                if (!String.IsNullOrEmpty(_cp.mnu.s_cnf))
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.s_cnf}".txtDefault(ct.WriteLine);
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
                CmdQube();
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
                Section.Header("SONAR SCANNER");
                Section.SelectedProject();

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _cp.snr.ipt);

                $"".fmNewLine();
                $" --> Scanning...".txtInfo(ct.WriteLine);
                CmdScanner(dirPath);

                Section.HorizontalRule();
                Sections.Pause();

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
                CmdBrowse(_cp.mnu.s_url);
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