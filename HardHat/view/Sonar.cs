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
            opts.Add(new Option{opt="s>sd", stt=true , act=Sonar.Domain                     });
            opts.Add(new Option{opt="s>sp", stt=true , act=Sonar.Port                       });
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
                $" [S] Sonar: ".txtStatus(ct.Write,             Options.Valid("s"));
                Section.Configuration(_cp.mnu.s_val, _cp.mnu.s_cnf);
            }
            $"{"   [Q] Qube"   , -34}".txtStatus(ct.Write,      Options.Valid("sq"));
            if (String.IsNullOrEmpty(_cp.snr.ipt))
            {
                $"{"[S] Scanner" , -34}".txtStatus(ct.Write,    Options.Valid("ss"));
            } else {
                $"{"[S] Scanner: ", -13}".txtStatus(ct.Write,    Options.Valid("ss"));
                $"{_cp.snr.ipt    , -21}".txtDefault(ct.Write);
            }
            $"{"[B] Browse"    , -17}".txtStatus(ct.WriteLine,  Options.Valid("sb"));
            $"".fmNewLine();
        }

        public static void Select() {
            Colorify.Default();

            try
            {
                Section.Header("SONAR CONFIGURATION");
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
                Exceptions.General(Ex.Message);
            }
        }

        public static void Protocol() {
            Colorify.Default();

            try
            {
                Section.Header("SONAR CONFIGURATION", "PROTOCOL");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);

                if (!String.IsNullOrEmpty(_cp.mnu.s_cnf))
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.s_cnf}".txtDefault(ct.WriteLine);
                }

                Selector.Start(Selector.Protocol, "1");

                string opt_ptc = Console.ReadLine();
                opt_ptc = opt_ptc?.ToLower();

                if (!String.IsNullOrEmpty(opt_ptc)){
                    Validation.Range(opt_ptc, 1, 2);
                    switch (opt_ptc)
                    {
                        case "1":
                            _cp.snr.ptc = "http";
                            break;
                        case "2":
                            _cp.snr.ptc = "https";
                            break;
                        default:
                            Message.Error();
                            break;
                    }
                } else {
                    _cp.snr.ptc = "http";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Server() {
            Colorify.Default();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);

                $"".fmNewLine();
                $"{" [D] Domain:"     , -25}".txtPrimary();   $"{_cp.snr.dmn}".txtDefault(ct.WriteLine);
                $"{" [P] Port:"       , -25}".txtPrimary();   $"{_cp.snr.prt}".txtDefault(ct.WriteLine);

                $"{"[EMPTY] Exit", 82}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if(String.IsNullOrEmpty(opt?.ToLower()))
                {
                    Menu.Start();
                } else {
                    Menu.Route($"s>s{opt?.ToLower()}", "s");
                }
                Message.Error();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Domain() {
            Colorify.Default();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER", "DOMAIN");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);
                
                $"".fmNewLine();
                $" Write server domain.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();

                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    _cp.snr.dmn = opt;
                }

                Menu.Status();
                Server();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Port() {
            Colorify.Default();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER", "PORT");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);
                
                $"".fmNewLine();
                $" Write server port.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    Validation.Range(opt, 0, 65536);
                    _cp.snr.prt = opt;
                } else {
                    _cp.snr.prt = "9000";
                }

                Menu.Status();
                Server();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void InternalPath() {
            Colorify.Default();

            try
            {
                Section.Header("SONAR CONFIGURATION", "INTERNAL PATH");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);

                $"".fmNewLine();
                $" Write an internal path inside your project.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_ipt = Console.ReadLine();
                _cp.snr.ipt = $"{opt_ipt}";
                
                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        
        public static void Qube() {
            Colorify.Default();

            try
            {
                CmdQube();
                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Scanner() {
            Colorify.Default();

            try
            {
                Section.Header("SONAR SCANNER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.s_val, _cp.mnu.s_cnf);

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _cp.snr.ipt);

                $"".fmNewLine();
                $" --> Scanning...".txtInfo(ct.WriteLine);
                CmdScanner(dirPath);

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Browse() {
            Colorify.Default();

            try
            {
                CmdBrowse(_cp.mnu.s_cnf);
                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}