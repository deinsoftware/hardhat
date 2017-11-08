using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public static partial class Gulp {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Gulp()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="g"   , stt=false, act=Gulp.Select                      });
            opts.Add(new Option{opt="g>i" , stt=false, act=Gulp.InternalPath                });
            opts.Add(new Option{opt="g>d" , stt=false, act=Gulp.Dimension                   });
            opts.Add(new Option{opt="g>f" , stt=false, act=Gulp.Flavor                      });
            opts.Add(new Option{opt="g>n" , stt=false, act=Gulp.Number                      });
            opts.Add(new Option{opt="g>s" , stt=false, act=Gulp.Sync                        });
            opts.Add(new Option{opt="g>p" , stt=false, act=Gulp.Protocol                    });
            opts.Add(new Option{opt="g>o" , stt=false, act=Gulp.Open                        });
            opts.Add(new Option{opt="gu"  , stt=false, act=Gulp.Uglify                      });
            opts.Add(new Option{opt="gr"  , stt=false, act=Gulp.Revert                      });
            opts.Add(new Option{opt="gs"  , stt=false, act=Gulp.Server                      });
        }

        public static void Status(){
            StringBuilder g_cnf = new StringBuilder();
            g_cnf.Append($"{_cp.gbs.ptc}://");
            if (!String.IsNullOrEmpty(_cp.gbs.dmn))
            {
                g_cnf.Append($"{_cp.gbs.dmn}/");
            }
            g_cnf.Append(Selector.Name(Selector.Flavor, _cp.gbs.flv));
            g_cnf.Append(_cp.gbs.srv);
            if (!String.IsNullOrEmpty(_cp.gbs.ipt))
            {
                g_cnf.Append($"/{_cp.gbs.ipt}");
            }
            g_cnf.Append(_cp.gbs.syn ? "+Sync" : "");
            _cp.mnu.g_cnf = g_cnf.ToString();
            _cp.mnu.g_val = !Strings.SomeNullOrEmpty(_cp.spr, _cp.gbs.dmn, _cp.mnu.g_cnf);
            Options.Valid("g"  , Variables.Valid("gp"));
            Options.Valid("g>i", Variables.Valid("gp"));
            Options.Valid("g>d", Variables.Valid("gp"));
            Options.Valid("g>f", Variables.Valid("gp"));
            Options.Valid("g>n", Variables.Valid("gp"));
            Options.Valid("g>s", Variables.Valid("gp"));
            Options.Valid("g>p", Variables.Valid("gp"));
            Options.Valid("g>o", Variables.Valid("gp"));
            Options.Valid("gu" , Variables.Valid("gp") && !Strings.SomeNullOrEmpty(_cp.spr));
            Options.Valid("gr" , Variables.Valid("gp") && !Strings.SomeNullOrEmpty(_cp.spr));
            Options.Valid("gs" , Variables.Valid("gp") && _cp.mnu.g_val);
        }

        public static void Start() {
            if (String.IsNullOrEmpty(_cp.mnu.g_cnf))
            {
                $" [G] Gulp".txtStatus(ct.WriteLine,            Options.Valid("g"));
            } else {
                $"{" [G] Gulp:", -25}".txtStatus(ct.Write,      Options.Valid("g"));
                Section.Configuration(_cp.mnu.g_val, _cp.mnu.g_cnf);
            }
            $"{"   [U] Uglify" , -34}".txtStatus(ct.Write,      Options.Valid("gu"));
            $"{"[R] Revert"    , -34}".txtStatus(ct.Write,      Options.Valid("gr"));
            $"{"[S] Server"    , -17}".txtStatus(ct.WriteLine,  Options.Valid("gs"));
            $"".fmNewLine();
        }
        
        public static void Select() {
            Gulp.Check();
            
            Colorify.Default();
            Console.Clear();
            try
            {
                Section.Header("GULP SERVER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                $"".fmNewLine();
                $"{" [P] Protocol:"     , -25}".txtPrimary();   $"{_cp.gbs.ptc}".txtDefault(ct.WriteLine);
                $"{" [I] Internal Path:", -25}".txtPrimary();   $"{_cp.gbs.ipt}".txtDefault(ct.WriteLine);
                $"{" [D] Dimension:"    , -25}".txtPrimary();   $"{_cp.gbs.dmn}".txtDefault(ct.WriteLine);
                string g_cnf = Selector.Name(Selector.Flavor, _cp.gbs.flv);
                $"{" [F] Flavor:"       , -25}".txtPrimary();   $"{g_cnf}".txtDefault(ct.WriteLine);
                $"{" [N] Number:"       , -25}".txtPrimary();   $"{_cp.gbs.srv}".txtDefault(ct.WriteLine);
                $"{" [S] Sync:"         , -25}".txtPrimary();   $"{(_cp.gbs.syn ? "Yes" : "No")}".txtDefault(ct.WriteLine);
                $"{" [O] Open:"         , -25}".txtPrimary();   $"{(_cp.gbs.opn ? "Yes" : "No")}".txtDefault(ct.WriteLine);

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
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "PROTOCOL");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                Protocols.Start();

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
                Exceptions.General(Ex.Message);
            }
        }
        
        public static void InternalPath() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "INTERNAL PATH");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

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
                Exceptions.General(Ex.Message);
            }
        }

        public static void Dimension() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "DIMENSION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                $"".fmNewLine();
                string dirPath = Paths.Combine(Variables.Value("gp"), _c.gulp.srv); 
                dirPath.Exists("Please review your configuration file.");
                List<string> files = dirPath.Files($"*{_c.gulp.ext}");
                
                if (files.Count < 1)
                {
                    _cp.gbs.dmn = "";
                } else {
                    var i = 1;
                    foreach (var file in files)
                    {
                        string f = file.Slash();
                        $" {i, 2}] {Strings.Remove(f.Substring(f.LastIndexOf("/") + 1), _c.gulp.ext)}".txtPrimary(ct.WriteLine);
                        i++;
                    }
                    if (!String.IsNullOrEmpty(_cp.gbs.dmn))
                    {
                        $"".fmNewLine();
                        $"{"[EMPTY] Current", 82}".txtInfo(ct.WriteLine);
                    }

                    Section.HorizontalRule();
                
                    $"{" Make your choice: ", -25}".txtInfo();
                    string opt_dmn = Console.ReadLine();
                    
                    if (!String.IsNullOrEmpty(opt_dmn))
                    {
                        Validation.Range(opt_dmn, 1, files.Count);
                        var sel = files[Convert.ToInt32(opt_dmn) - 1].Slash();
                        _cp.gbs.dmn = Strings.Remove(sel.Substring(sel.LastIndexOf("/") + 1), _c.gulp.ext);
                    } else {
                        if (String.IsNullOrEmpty(_cp.gbs.dmn)){
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
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "FLAVOR");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

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
                        _cp.gbs.flv = opt_flv;
                        break;
                    case "":
                        _cp.gbs.flv = "a";
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

        public static void Number() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "NUMBER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

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
                    Validation.Number(opt_srv);
                } 
                _cp.gbs.srv = opt_srv;

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Sync() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "SYNC");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                Selector.Start(Selector.Logical, "n");

                string opt_syn = Console.ReadLine();
                opt_syn = opt_syn?.ToLower();
                
                switch (opt_syn)
                {
                    case "y":
                        _cp.gbs.syn = true;
                        break;
                    case "n":
                    case "":
                        _cp.gbs.syn = false;
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
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "OPEN");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                Selector.Start(Selector.Logical, "y");
                
                string opt_opn = Console.ReadLine();
                opt_opn = opt_opn?.ToLower();
                
                switch (opt_opn)
                {
                    case "y":
                    case "":
                        _cp.gbs.opn = true;
                        break;
                    case "n":
                        _cp.gbs.opn = false;
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
        
        public static void Uglify() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GULP UGLIFY");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.cmp); 

                string[] dirs = new string[] {
                    Paths.Combine(Variables.Value("gp"),"www"),
                    Paths.Combine(Variables.Value("gp"),"bld"),
                };

                $"".fmNewLine();
                $" --> Cleaning...".txtInfo(ct.WriteLine);
                foreach (var dir in dirs)
                {
                    Paths.DeleteAll(dir, true, true);
                    Directory.CreateDirectory(dir);
                }

                $"".fmNewLine();
                List<string> filter = new List<string>(_c.android.flt);

                $" --> Copying...".txtInfo(ct.WriteLine);
                $"{" From:", -8}".txtMuted(); $"{dirPath}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{dirs[0]}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirPath, dirs[0], true, true, filter);
                $"{" To:"  , -8}".txtMuted(); $"{dirs[1]}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirPath, dirs[1], true, true, filter);

                $"".fmNewLine();
                $" --> Uglifying...".txtInfo(ct.WriteLine);
                CmdUglify(Paths.Combine(Variables.Value("gp")));

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
            Console.Clear();

            try
            {
                Section.Header("GULP REVERT");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.cmp); 
                string dirSource = Paths.Combine(Variables.Value("gp"),"www");
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
            Console.Clear();

            try
            {
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr);
                CmdServer(
                    dirPath,
                    Paths.Combine(Variables.Value("gp")),
                    _cp.gbs.ipt,
                    _cp.gbs.dmn,
                    _cp.gbs.flv,
                    _cp.gbs.srv,
                    _cp.gbs.syn,
                    _cp.ipl,
                    _cp.gbs.ptc,
                    _cp.gbs.opn
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
                string dirPath = Paths.Combine(Variables.Value("gp"));

                if (Directory.Exists(Paths.Combine(dirPath, ".git"))){
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
            Console.Clear();

            try
            {
                Section.Header("GULP SERVER", "UPDATE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_cp.mnu.g_val, _cp.mnu.g_cnf);

                string dirPath = Paths.Combine(Variables.Value("gp"));

                $"".fmNewLine();
                $" --> Updating...".txtInfo(ct.WriteLine);
                Git.CmdPull(dirPath);
            
                Section.HorizontalRule();
                Section.Pause();

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}