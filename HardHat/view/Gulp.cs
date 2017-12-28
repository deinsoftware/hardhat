using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Validation = ToolBox.Validations.Strings;
using Transform = ToolBox.Transform.Strings;
using dein.tools;
using static HardHat.Program;

using ct = dein.tools.Colorify.Type;
using ToolBox.Validations;

namespace HardHat {

    public static partial class Gulp {

        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="g"   , stt=false, act=Gulp.Select                      });
            opts.Add(new Option{opt="g>i" , stt=false, act=Gulp.InternalPath                });
            opts.Add(new Option{opt="g>d" , stt=false, act=Gulp.Dimension                   });
            opts.Add(new Option{opt="g>f" , stt=false, act=Gulp.Flavor                      });
            opts.Add(new Option{opt="g>n" , stt=false, act=Gulp.ServerNumber                });
            opts.Add(new Option{opt="g>s" , stt=false, act=Gulp.Sync                        });
            opts.Add(new Option{opt="g>p" , stt=false, act=Gulp.Protocol                    });
            opts.Add(new Option{opt="g>o" , stt=false, act=Gulp.Open                        });
            opts.Add(new Option{opt="gm"  , stt=false, act=Gulp.Make                        });
            opts.Add(new Option{opt="gu"  , stt=false, act=Gulp.Uglify                      });
            opts.Add(new Option{opt="gr"  , stt=false, act=Gulp.Revert                      });
            opts.Add(new Option{opt="gs"  , stt=false, act=Gulp.Server                      });
            opts.Add(new Option{opt="gl"  , stt=false, act=Gulp.Log                         });
        }

        public static void Status(){
            StringBuilder g_cnf = new StringBuilder();
            g_cnf.Append($"{_config.personal.gbs.ptc}://");
            if (!String.IsNullOrEmpty(_config.personal.gbs.dmn))
            {
                g_cnf.Append($"{_config.personal.gbs.dmn}/");
            }
            g_cnf.Append(Selector.Name(Selector.Flavor, _config.personal.gbs.flv));
            g_cnf.Append(_config.personal.gbs.srv);
            if (!String.IsNullOrEmpty(_config.personal.gbs.ipt))
            {
                g_cnf.Append($"/{_config.personal.gbs.ipt}");
            }
            g_cnf.Append(_config.personal.gbs.syn ? "+Sync" : "");
            _config.personal.mnu.g_cnf = g_cnf.ToString();
            _config.personal.mnu.g_val = !Validation.SomeNullOrEmpty(_config.personal.spr, _config.personal.gbs.dmn, _config.personal.mnu.g_cnf);
            Options.Valid("g"  , Variables.Valid("gp"));
            Options.Valid("g>i", Variables.Valid("gp"));
            Options.Valid("g>d", Variables.Valid("gp"));
            Options.Valid("g>f", Variables.Valid("gp"));
            Options.Valid("g>n", Variables.Valid("gp"));
            Options.Valid("g>s", Variables.Valid("gp"));
            Options.Valid("g>p", Variables.Valid("gp"));
            Options.Valid("g>o", Variables.Valid("gp"));
            Options.Valid("gm" , Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.spr));
            Options.Valid("gu" , Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.spr));
            Options.Valid("gr" , Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.spr));
            Options.Valid("gs" , Variables.Valid("gp") && _config.personal.mnu.g_val);
            Options.Valid("gl" , Variables.Valid("gp") && _config.personal.mnu.g_val);
        }

        public static void Start() {
            if (String.IsNullOrEmpty(_config.personal.mnu.g_cnf))
            {
                $" [G] Gulp".txtStatus(ct.WriteLine,            Options.Valid("g"));
            } else {
                $" [G] Gulp: ".txtStatus(ct.Write,              Options.Valid("g"));
                Section.Configuration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);
            }
            $"{"   [M] Make"   , -17}".txtStatus(ct.Write,      Options.Valid("gm"));
            $"{"[U] Uglify"    , -17}".txtStatus(ct.Write,      Options.Valid("gu"));
            $"{"[R] Revert"    , -17}".txtStatus(ct.Write,      Options.Valid("gr"));
            $"{"[S] Server"    , -17}".txtStatus(ct.Write,      Options.Valid("gs"));
            $"{"[L] Log"       , -17}".txtStatus(ct.WriteLine,  Options.Valid("gl"));
            $"".fmNewLine();
        }
        
        public static void Select() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "SERVER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                $"".fmNewLine();
                $"{" [P] Protocol:"     , -25}".txtPrimary();   $"{_config.personal.gbs.ptc}".txtDefault(ct.WriteLine);
                $"{" [I] Internal Path:", -25}".txtPrimary();   $"{_config.personal.gbs.ipt}".txtDefault(ct.WriteLine);
                $"{" [D] Dimension:"    , -25}".txtPrimary();   $"{_config.personal.gbs.dmn}".txtDefault(ct.WriteLine);
                string g_cnf = Selector.Name(Selector.Flavor, _config.personal.gbs.flv);
                $"{" [F] Flavor:"       , -25}".txtPrimary();   $"{g_cnf}".txtDefault(ct.WriteLine);
                $"{" [N] Number:"       , -25}".txtPrimary();   $"{_config.personal.gbs.srv}".txtDefault(ct.WriteLine);
                $"{" [S] Sync:"         , -25}".txtPrimary();   $"{(_config.personal.gbs.syn ? "Yes" : "No")}".txtDefault(ct.WriteLine);
                $"{" [O] Open:"         , -25}".txtPrimary();   $"{(_config.personal.gbs.opn ? "Yes" : "No")}".txtDefault(ct.WriteLine);

                $"{"[EMPTY] Exit", 82}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if(String.IsNullOrEmpty(opt?.ToLower()))
                {
                    Menu.Start();
                } else {
                    Menu.Route($"g>{opt?.ToLower()}", "g");
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
                Section.Header("GULP", "SERVER", "PROTOCOL");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                Selector.Start(Selector.Protocol, "1");

                string opt_ptc = Console.ReadLine();
                opt_ptc = opt_ptc?.ToLower();

                if (!String.IsNullOrEmpty(opt_ptc)){
                    Number.IsOnRange(1, Convert.ToInt32(opt_ptc), 2);
                    switch (opt_ptc)
                    {
                        case "1":
                            _config.personal.gbs.ptc = "http";
                            break;
                        case "2":
                            _config.personal.gbs.ptc = "https";
                            break;
                        default:
                            Message.Error();
                            break;
                    }
                } else {
                    _config.personal.gbs.ptc = "http";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        
        public static void InternalPath() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "SERVER", "INTERNAL PATH");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                $"".fmNewLine();
                $" Write an internal path inside your project.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_ipt = Console.ReadLine();
                _config.personal.gbs.ipt = $"{opt_ipt}";

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Dimension() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "SERVER", "DIMENSION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                $"".fmNewLine();
                string dirPath = _path.Combine(Variables.Value("gp"), _config.gulp.srv); 
                dirPath.Exists("Please review your configuration file.");
                List<string> files = dirPath.Files($"*{_config.gulp.ext}");
                
                if (files.Count < 1)
                {
                    _config.personal.gbs.dmn = "";
                } else {
                    var i = 1;
                    foreach (var file in files)
                    {
                        string f = file;
                        $" {i, 2}] {Transform.RemoveWords(f.Substring(f.LastIndexOf("/") + 1), _config.gulp.ext)}".txtPrimary(ct.WriteLine);
                        i++;
                    }
                    if (!String.IsNullOrEmpty(_config.personal.gbs.dmn))
                    {
                        $"".fmNewLine();
                        $"{"[EMPTY] Current", 82}".txtInfo(ct.WriteLine);
                    }

                    Section.HorizontalRule();
                
                    $"{" Make your choice: ", -25}".txtInfo();
                    string opt_dmn = Console.ReadLine();
                    
                    if (!String.IsNullOrEmpty(opt_dmn))
                    {
                        Number.IsOnRange(1, Convert.ToInt32(opt_dmn), files.Count);
                        var sel = files[Convert.ToInt32(opt_dmn) - 1];
                        _config.personal.gbs.dmn = Transform.RemoveWords(sel.Substring(sel.LastIndexOf("/") + 1), _config.gulp.ext);
                    } else {
                        if (String.IsNullOrEmpty(_config.personal.gbs.dmn)){
                            Message.Error();
                        }
                    }
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
                Section.Header("GULP", "SERVER", "FLAVOR");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                Selector.Start(Selector.Flavor, "a");
                
                string opt_flv = Console.ReadLine();
                opt_flv = opt_flv?.ToLower();
                
                switch (opt_flv)
                {
                    case "a":
                    case "b":
                    case "s":
                    case "p":
                    case "d":
                        _config.personal.gbs.flv = opt_flv;
                        break;
                    case "":
                        _config.personal.gbs.flv = "a";
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

        public static void ServerNumber() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "SERVER", "NUMBER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                $"".fmNewLine();
                $" Write a server number:".txtPrimary(ct.WriteLine);
                $" 1".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_srv = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt_srv))
                {
                    Number.IsNumber(opt_srv);
                } 
                _config.personal.gbs.srv = opt_srv;

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Sync() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "SERVER", "SYNC");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                Selector.Start(Selector.Logical, "n");

                string opt_syn = Console.ReadLine();
                opt_syn = opt_syn?.ToLower();
                
                switch (opt_syn)
                {
                    case "y":
                        _config.personal.gbs.syn = true;
                        break;
                    case "n":
                    case "":
                        _config.personal.gbs.syn = false;
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

        public static void Open() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "SERVER", "OPEN");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                Selector.Start(Selector.Logical, "y");
                
                string opt_opn = Console.ReadLine();
                opt_opn = opt_opn?.ToLower();
                
                switch (opt_opn)
                {
                    case "y":
                    case "":
                        _config.personal.gbs.opn = true;
                        break;
                    case "n":
                        _config.personal.gbs.opn = false;
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

        public static void Make() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "MAKE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr); 

                $"".fmNewLine();
                $" --> Making...".txtInfo(ct.WriteLine);
                CmdMake(dirPath, _path.Combine(Variables.Value("gp")));

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        
        public static void Uglify() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "UGLIFY");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.cmp); 

                string[] dirs = new string[] {
                    _path.Combine(Variables.Value("gp"),"www"),
                    _path.Combine(Variables.Value("gp"),"bld"),
                };

                $"".fmNewLine();
                $" --> Cleaning...".txtInfo(ct.WriteLine);
                foreach (var dir in dirs)
                {
                    Paths.DeleteAll(dir, true, true);
                    Directory.CreateDirectory(dir);
                }

                $"".fmNewLine();
                List<string> filter = new List<string>(_config.android.flt);

                $" --> Copying...".txtInfo(ct.WriteLine);
                $"{" From:", -8}".txtMuted(); $"{dirPath}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{dirs[0]}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirPath, dirs[0], true, true, filter);
                $"{" To:"  , -8}".txtMuted(); $"{dirs[1]}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirPath, dirs[1], true, true, filter);

                $"".fmNewLine();
                $" --> Uglifying...".txtInfo(ct.WriteLine);
                CmdUglify(_path.Combine(Variables.Value("gp")));

                $"".fmNewLine();
                $" --> Replacing...".txtInfo(ct.WriteLine);
                $"{" From:", -8}".txtMuted(); $"{dirs[1]}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{dirPath}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirs[1], dirPath, true, true); 

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Revert() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "REVERT");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.mnu.g_val, _config.personal.mnu.g_cnf);

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.cmp); 
                string dirSource = _path.Combine(Variables.Value("gp"),"www");
                $"".fmNewLine();
                $" --> Reverting...".txtInfo(ct.WriteLine);
                $"{" From:", -8}".txtMuted(); $"{dirSource}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{dirPath}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirSource, dirPath, true, true); 

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Server() {
            Colorify.Default();

            try
            {
                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr);
                CmdServer(
                    dirPath,
                    _path.Combine(Variables.Value("gp")),
                    _config.personal.gbs,
                    _config.personal.ipl
                );
                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Log() {
            Colorify.Default();

            try
            {
                Vpn.Verification();

                CmdLog(
                    _path.Combine(Variables.Value("gp")),
                    _config.personal.gbs
                );
                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Check(){
            try
            {
                string dirPath = _path.Combine(Variables.Value("gp"));

                if (Directory.Exists(_path.Combine(dirPath, ".git"))){
                    Git.CmdFetch(dirPath);
                    bool updated = Git.CmdStatus(dirPath);
                    if (!updated){
                        StringBuilder msg = new StringBuilder();
                        msg.Append($"There is a new Gulp project version available.");
                        msg.Append(Environment.NewLine);
                        msg.Append($" Do you want update it?");
                        bool update = Message.Confirmation(msg.ToString());
                        if (update){
                            Upgrade();
                        }
                    }
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Upgrade() {
            Colorify.Default();

            try
            {
                Section.Header("GULP", "UPDATE");
                
                string dirPath = _path.Combine(Variables.Value("gp"));

                $" --> Updating...".txtInfo(ct.WriteLine);
                Git.CmdPull(dirPath);

                $"".fmNewLine();
                $" --> Updating Dependencies...".txtInfo(ct.WriteLine);
                Gulp.CmdInstall(dirPath);
            
                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}