using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static partial class Build {
        
        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="b"   , stt=false, act=Build.Select                     });
            opts.Add(new Option{opt="b>d" , stt=false, act=Build.Dimension                  });
            opts.Add(new Option{opt="b>f" , stt=false, act=Build.Flavor                     });
            opts.Add(new Option{opt="b>m" , stt=false, act=Build.Mode                       });
            opts.Add(new Option{opt="bp"  , stt=false, act=Build.Properties                 });
            opts.Add(new Option{opt="bc"  , stt=false, act=Build.Clean                      });
            opts.Add(new Option{opt="bg"  , stt=false, act=Build.Gradle                     });
        }

        public static void Status(){
            StringBuilder b_cnf = new StringBuilder();
            b_cnf.Append(_config.personal.gdl.dmn ?? "");
            b_cnf.Append(Selector.Name(Selector.Flavor, _config.personal.gdl.flv));
            b_cnf.Append(Modes.Name(_config.personal.gdl.mde));
            _config.personal.mnu.b_cnf = b_cnf.ToString();
            _config.personal.mnu.b_val = !Strings.SomeNullOrEmpty(_config.personal.spr, _config.personal.gdl.mde, _config.personal.gdl.flv, _config.personal.mnu.b_cnf);
            Options.Valid("b"  , Variables.Valid("gh"));
            Options.Valid("b>d", Variables.Valid("gh"));
            Options.Valid("b>f", Variables.Valid("gh"));
            Options.Valid("b>m", Variables.Valid("gh"));
            Options.Valid("bp" , Variables.Valid("gp") && !Strings.SomeNullOrEmpty(_config.personal.spr));
            Options.Valid("bc" , Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.spr));
            Options.Valid("bg" , Variables.Valid("gh") && _config.personal.mnu.b_val);
        }

        public static void Start(){
            if (String.IsNullOrEmpty(_config.personal.mnu.b_cnf))
            {
                $" [B] Build".txtStatus(ct.WriteLine,                Options.Valid("b"));
            } else {
                $" [B] Build: ".txtStatus(ct.Write,                  Options.Valid("b"));
                Section.Configuration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);
            }
            $"{"   [P] Properties"  , -34}".txtStatus(ct.Write,      Options.Valid("bp"));
            $"{"[C] Clean"          , -34}".txtStatus(ct.Write,      Options.Valid("bc"));
            $"{"[G] Gradle"         , -17}".txtStatus(ct.WriteLine,  Options.Valid("bg"));
            $"".fmNewLine();
        }
        
        public static void Select() {
            Colorify.Default();

            try
            {
                Section.Header("BUILD CONFIGURATION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                $"".fmNewLine();
                $"{" [D] Dimension:"    , -25}".txtPrimary();   $"{_config.personal.gdl.dmn}".txtDefault(ct.WriteLine);
                string b_flv = Selector.Name(Selector.Flavor, _config.personal.gdl.flv);
                $"{" [F] Flavor:"       , -25}".txtPrimary();   $"{b_flv}".txtDefault(ct.WriteLine);
                string b_mde = Modes.Name(_config.personal.gdl.mde);
                $"{" [M] Mode:"         , -25}".txtPrimary();   $"{b_mde}".txtDefault(ct.WriteLine);

                $"{"[EMPTY] Exit", 82}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();

                if(String.IsNullOrEmpty(opt?.ToLower()))
                {
                    Menu.Start();
                } else {
                    Menu.Route($"b>{opt?.ToLower()}", "b");
                }
                Message.Error();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Dimension() {
            Colorify.Default();

            try
            {
                Section.Header("BUILD CONFIGURATION > DIMENSION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                $"".fmNewLine();
                $" Write a project dimension:".txtPrimary(ct.WriteLine);
                $" EMPTY".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_dmn = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt_dmn))
                {
                    _config.personal.gdl.dmn = $"{opt_dmn}";
                } else {
                    _config.personal.gdl.dmn = $"";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Flavor() {
            Colorify.Default();

            try
            {
                Section.Header("BUILD CONFIGURATION > FLAVOR");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                Selector.Start(Selector.Flavor, "a");
                
                string opt_flv = Console.ReadLine();
                opt_flv = opt_flv.ToLower();
                
                switch (opt_flv)
                {
                    case "a":
                    case "b":
                    case "s":
                    case "p":
                    case "d":
                        _config.personal.gdl.flv = opt_flv;
                        break;
                    case "":
                        _config.personal.gdl.flv = "a";
                        break;
                    default:
                        Message.Error();
                        break;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Mode() {
            Colorify.Default();

            try
            {
                Section.Header("BUILD CONFIGURATION > MODE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                $"".fmNewLine();
                $" {"D", 2}] Debug".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                $" {"R", 2}] Release".txtPrimary(ct.WriteLine);
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_mde = Console.ReadLine();
                opt_mde = opt_mde.ToLower();
                
                switch (opt_mde)
                {
                    case "d":
                    case "r":
                        _config.personal.gdl.mde = opt_mde;
                        break;
                    case "":
                        _config.personal.gdl.mde = "d";
                        break;
                    default:
                        Message.Error();
                        break;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Gradle() {
            Colorify.Default();

            try
            {
                Vpn.Verification();

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj); 
                CmdGradle(dirPath, _config.personal.mnu.b_cnf);

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Clean() {
            Colorify.Default();

            try
            {
                Vpn.Verification();

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj); 
                CmdClean(dirPath);

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Properties()
        {
            Colorify.Default();

            try
            {
                Section.Header("BUILD CONFIGURATION > PROPERTIES");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                string sourcePath = _path.Combine(Variables.Value("bp"), _config.path.bsn);
                string destinationPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj); 
                
                $"".fmNewLine();
                List<string> filter = new List<string>() { 
                    ".properties"
                };

                $" --> Copying...".txtInfo(ct.WriteLine);
                $"".fmNewLine();
                $"{" From:", -8}".txtMuted(); $"{sourcePath}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{destinationPath}".txtDefault(ct.WriteLine);
                Paths.CopyAll(sourcePath, destinationPath, true, true, filter);     
            
                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }
    }
}