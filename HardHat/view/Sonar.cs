using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using Colorify;
using static Colorify.Colors;

namespace HardHat {

    public static partial class Sonar {

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
            s_cnf.Append($"{_config.personal.snr.ptc}://");
            if (!String.IsNullOrEmpty(_config.personal.snr.dmn))
            {
                s_cnf.Append($"{_config.personal.snr.dmn}");
            }
            if (!String.IsNullOrEmpty(_config.personal.snr.prt))
            {
                s_cnf.Append($":{_config.personal.snr.prt}");
            }
            _config.personal.mnu.s_cnf = s_cnf.ToString();

            _config.personal.mnu.s_val = !Strings.SomeNullOrEmpty(_config.personal.snr.ptc, _config.personal.snr.dmn, _config.personal.mnu.s_cnf);
            Options.Valid("s" , Variables.Valid("sq"));
            Options.Valid("sq", Variables.Valid("sq"));
            Options.Valid("ss", Variables.Valid("ss") && !Strings.SomeNullOrEmpty(_config.personal.spr));
            Options.Valid("sb", _config.personal.mnu.s_val);
        }
        
        public static void Start() {
            if (String.IsNullOrEmpty(_config.personal.mnu.s_cnf))
            {
                _colorify.WriteLine($" [S] Sonar", txtStatus(Options.Valid("s")));
            } else {
                _colorify.Write($" [S] Sonar: ", txtStatus(Options.Valid("s")));
                Section.Configuration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);
            }
            _colorify.Write($"{"   [Q] Qube"      , -34}", txtStatus(Options.Valid("sq")));
            if (String.IsNullOrEmpty(_config.personal.snr.ipt))
            {
                _colorify.Write($"{"[S] Scanner"  , -34}", txtStatus(Options.Valid("ss")));
            } else {
                _colorify.Write($"{"[S] Scanner: ", -13}", txtStatus(Options.Valid("ss")));
                _colorify.Write($"{_config.personal.snr.ipt    , -21}", txtDefault);
            }
            _colorify.WriteLine($"{"[B] Browse"   , -17}", txtStatus(Options.Valid("sb")));
            _colorify.BlankLines();
        }

        public static void Select() {
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);

                _colorify.BlankLines();
                _colorify.Write($"{" [P] Protocol:"     , -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.snr.ptc}");
                _colorify.Write($"{" [S] Server:"       , -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.snr.dmn}{(!String.IsNullOrEmpty(_config.personal.snr.prt) ? ":"+_config.personal.snr.prt : "")}");
                _colorify.Write($"{" [I] Internal Path:", -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.snr.ipt}");

                _colorify.WriteLine($"{"[EMPTY] Exit", 82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
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
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION", "PROTOCOL");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);

                if (!String.IsNullOrEmpty(_config.personal.mnu.s_cnf))
                {
                    _colorify.Write($"{" Current Configuration:", -25}", txtMuted);
                    _colorify.WriteLine($"{_config.personal.mnu.s_cnf}");
                }

                Selector.Start(Selector.Protocol, "1");

                string opt_ptc = Console.ReadLine();
                opt_ptc = opt_ptc?.ToLower();

                if (!String.IsNullOrEmpty(opt_ptc)){
                    Number.IsOnRange(1, Convert.ToInt32(opt_ptc), 2);
                    switch (opt_ptc)
                    {
                        case "1":
                            _config.personal.snr.ptc = "http";
                            break;
                        case "2":
                            _config.personal.snr.ptc = "https";
                            break;
                        default:
                            Message.Error();
                            break;
                    }
                } else {
                    _config.personal.snr.ptc = "http";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Server() {
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);

                _colorify.BlankLines();
                _colorify.Write($"{" [D] Domain:"     , -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.snr.dmn}");
                _colorify.Write($"{" [P] Port:"       , -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.snr.prt}");

                _colorify.WriteLine($"{"[EMPTY] Exit", 82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
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
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER", "DOMAIN");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);
                
                _colorify.BlankLines();
                _colorify.WriteLine($" Write server domain.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    _config.personal.snr.dmn = opt;
                }

                Menu.Status();
                Server();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Port() {
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER", "PORT");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);
                
                _colorify.BlankLines();
                _colorify.WriteLine($" Write server port.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default", 82}", txtInfo);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt)){
                    Number.IsOnRange(0, Convert.ToInt32(opt), 65536);
                    _config.personal.snr.prt = opt;
                } else {
                    _config.personal.snr.prt = "9000";
                }

                Menu.Status();
                Server();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void InternalPath() {
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION", "INTERNAL PATH");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write an internal path inside your project.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default", 82}", txtInfo);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt_ipt = Console.ReadLine();
                _config.personal.snr.ipt = $"{opt_ipt}";
                
                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        
        public static void Qube() {
            _colorify.Clear();

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
            _colorify.Clear();

            try
            {
                Section.Header("SONAR SCANNER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.s_val, _config.personal.mnu.s_cnf);

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.personal.snr.ipt);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Scanning...", txtInfo);
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
            _colorify.Clear();

            try
            {
                CmdBrowse(_config.personal.mnu.s_cnf);
                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}