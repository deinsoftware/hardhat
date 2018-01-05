using System;
using System.Collections.Generic;
using System.Text;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using static Colorify.Colors;

namespace HardHat
{
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
                _colorify.WriteLine($" [B] Build", txtStatus(Options.Valid("b")));
            } else {
                _colorify.Write($" [B] Build: ", txtStatus(Options.Valid("b")));
                Section.Configuration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);
            }
            _colorify.Write($"{"   [P] Properties"  , -34}", txtStatus(Options.Valid("bp")));
            _colorify.Write($"{"[C] Clean"          , -34}", txtStatus(Options.Valid("bc")));
            _colorify.WriteLine($"{"[G] Gradle"    , -17}", txtStatus(Options.Valid("bg")));
            _colorify.BlankLines();
        }
        
        public static void Select() {
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                _colorify.BlankLines();
                _colorify.Write($"{" [D] Dimension:"    , -25}", txtPrimary);   _colorify.WriteLine($"{_config.personal.gdl.dmn}");
                string b_flv = Selector.Name(Selector.Flavor, _config.personal.gdl.flv);
                _colorify.Write($"{" [F] Flavor:"       , -25}", txtPrimary);   _colorify.WriteLine($"{b_flv}");
                string b_mde = Modes.Name(_config.personal.gdl.mde);
                _colorify.Write($"{" [M] Mode:"         , -25}", txtPrimary);   _colorify.WriteLine($"{b_mde}");

                _colorify.WriteLine($"{"[EMPTY] Exit", 82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
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
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION > DIMENSION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a project dimension:", txtPrimary);
                _colorify.Write($" EMPTY", txtPrimary); _colorify.WriteLine($" (Default)", txtInfo);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default", 82}", txtInfo);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
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
            _colorify.Clear();

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
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION > MODE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                _colorify.BlankLines();
                _colorify.Write($" {"D", 2}] Debug", txtPrimary); _colorify.WriteLine($" (Default)", txtInfo);
                _colorify.WriteLine($" {"R", 2}] Release", txtPrimary);
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default", 82}", txtInfo);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
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
            _colorify.Clear();

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
            _colorify.Clear();

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
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION > PROPERTIES");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.b_val, _config.personal.mnu.b_cnf);

                string sourcePath = _path.Combine(Variables.Value("bp"), _config.path.bsn);
                string destinationPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj); 
                
                _colorify.BlankLines();
                List<string> filter = _disk.FilterCreator(".properties");

                _colorify.WriteLine($" --> Copying...", txtInfo);
                _colorify.BlankLines();
                _colorify.Write($"{" From:", -8}", txtMuted); _colorify.WriteLine($"{sourcePath}");
                _colorify.Write($"{" To:"  , -8}", txtMuted); _colorify.WriteLine($"{destinationPath}");
                _disk.CopyAll(sourcePath, destinationPath, true, filter);
            
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