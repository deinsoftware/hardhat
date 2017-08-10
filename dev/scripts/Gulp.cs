using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public partial class Gulp {
        public static void Select() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP SERVER CONFIGURATION".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.g_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $"{" [P] Protocol:"     , -25}".txtPrimary();   $"{cp.gbs.ptc}".txtDefault(ct.WriteLine);
                $"{" [I] Internal Path:", -25}".txtPrimary();   $"{cp.gbs.ipt}".txtDefault(ct.WriteLine);
                $"{" [D] Dimension:"    , -25}".txtPrimary();   $"{cp.gbs.dmn}".txtDefault(ct.WriteLine);
                string g_cnf = "";
                switch (cp.gbs.flv?.ToLower())
                {
                    case "a":
                        g_cnf = "Alfa";
                        break;
                    case "b":
                        g_cnf = "Beta";
                        break;
                    case "s":
                        g_cnf = "Stag";
                        break;
                    case "p":
                        g_cnf = "Prod";
                        break;
                }
                $"{" [F] Flavor:"       , -25}".txtPrimary();   $"{g_cnf}".txtDefault(ct.WriteLine);
                $"{" [N] Number:"       , -25}".txtPrimary();   $"{cp.gbs.srv}".txtDefault(ct.WriteLine);
                $"{" [S] Sync:"         , -25}".txtPrimary();   $"{(cp.gbs.syn ? "Yes" : "No")}".txtDefault(ct.WriteLine);

                $"{"[EMPTY] Exit", 82}".txtDanger(ct.WriteLine);

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();
                cp.mnu.sel = $"g>{opt.ToLower()}";

                switch (opt?.ToLower())
                {
                    case "i":      
                        InternalPath();
                        break;
                    case "d":
                        Dimension();
                        break;
                    case "f":
                        Flavor();
                        break;
                    case "n":
                        Number();
                        break;
                    case "s":
                        Sync();
                        break;
                    case "p":
                        Protocol();
                        break;
                    case "":
                        Menu.Start();
                        break;
                    default:
                        cp.mnu.sel = "g";
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

        public static void Protocol() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP SERVER CONFIGURATION > PROTOCOL".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.g_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" {"1", 2}] http".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                $" {"2", 2}] https".txtPrimary(ct.WriteLine);
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_ptc = Console.ReadLine();
                opt_ptc = opt_ptc?.ToLower();

                if (!String.IsNullOrEmpty(opt_ptc)){
                    Validation.Range(opt_ptc, 1, 2);
                    switch (opt_ptc)
                    {
                        case "1":
                            cp.gbs.ptc = "http";
                            break;
                        case "2":
                            cp.gbs.ptc = "https";
                            break;
                        default:
                            Message.Error();
                            break;
                    }
                } else {
                    cp.gbs.ptc = "http";
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
        
        public static void InternalPath() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP SERVER CONFIGURATION > INTERNAL PATH".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.g_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" Write an internal path inside your project.".txtPrimary();
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_ipt = Console.ReadLine();
                cp.gbs.ipt = $"{opt_ipt}";
                
                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Dimension() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP SERVER CONFIGURATION > DIMENSION".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.g_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                string dirPath = Paths.Combine(Env.Get("GULP_PROJECT"), c.gulp.srv); 
                dirPath.Exists("Please review your configuration file.");
                List<string> files = dirPath.Files($"*{c.gulp.ext}");
                
                if (files.Count < 1)
                {
                    cp.gbs.dmn = "";
                } else {
                    var i = 1;
                    foreach (var file in files)
                    {
                        string f = file.Slash();
                        $" {i, 2}] {Strings.Remove(f.Substring(f.LastIndexOf("/") + 1), c.gulp.ext)}".txtPrimary(ct.WriteLine);
                        i++;
                    }
                    if (!String.IsNullOrEmpty(cp.gbs.dmn))
                    {
                        $"".fmNewLine();
                        $"{"[EMPTY] Current", 82}".txtInfo(ct.WriteLine);
                    }

                    $"".fmNewLine();
                    $"=".bgInfo(ct.Repeat);
                    $"".fmNewLine();
                
                    $"{" Make your choice: ", -25}".txtInfo();
                    string opt_dmn = Console.ReadLine();
                    
                    if (!String.IsNullOrEmpty(opt_dmn))
                    {
                        Validation.Range(opt_dmn, 1, files.Count);
                        var sel = files[Convert.ToInt32(opt_dmn) - 1].Slash();
                        cp.gbs.dmn = Strings.Remove(sel.Substring(sel.LastIndexOf("/") + 1), c.gulp.ext);
                    } else {
                        if (String.IsNullOrEmpty(cp.gbs.dmn)){
                            Message.Error();
                        }
                    }
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

        public static void Flavor() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP SERVER CONFIGURATION > FLAVOR".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.g_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" {"A", 2}] Alfa".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                $" {"B", 2}] Beta".txtPrimary(ct.WriteLine);
                $" {"S", 2}] Stag".txtPrimary(ct.WriteLine);
                $" {"P", 2}] Prod".txtPrimary(ct.WriteLine);
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_flv = Console.ReadLine();
                opt_flv = opt_flv?.ToLower();
                
                switch (opt_flv)
                {
                    case "a":
                    case "b":
                    case "s":
                    case "p":
                        cp.gbs.flv = opt_flv;
                        break;
                    case "":
                        cp.gbs.flv = "a";
                        break;
                    default:
                        Message.Error();
                        break;
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

        public static void Number() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP SERVER CONFIGURATION > NUMBER".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.g_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" Write a server number:".txtPrimary(ct.WriteLine);
                $" 1".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_srv = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt_srv))
                {
                    Validation.Number(opt_srv);
                } 
                cp.gbs.srv = opt_srv;

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Sync() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP SERVER CONFIGURATION > SYNC".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.g_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" {"Y", 2}] Yes".txtPrimary(ct.WriteLine);
                $" {"N", 2}] No".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Make your choice: ", -25}".txtInfo();
                string opt_syn = Console.ReadLine();
                opt_syn = opt_syn?.ToLower();
                
                switch (opt_syn)
                {
                    case "y":
                        cp.gbs.syn = true;
                        break;
                    case "n":
                    case "":
                        cp.gbs.syn = false;
                        break;
                    default:
                        Message.Error();
                        break;
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
        
        public static void Uglify() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP UGLIFY".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj, c.android.cmp); 

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);
                
                string[] dirs = new string[] { 
                    Paths.Combine(Env.Get("GULP_PROJECT"),"www"),
                    Paths.Combine(Env.Get("GULP_PROJECT"),"bkp"),
                    Paths.Combine(Env.Get("GULP_PROJECT"),"bld"),
                };

                $"".fmNewLine();
                $" --> Cleaning...".txtInfo(ct.WriteLine);
                foreach (var dir in dirs)
                {
                    Paths.DeleteAll(dir, true, true);
                    Directory.CreateDirectory(dir);
                }

                $"".fmNewLine();
                List<string> filter = new List<string>(c.android.flt);

                $" --> Copying...".txtInfo(ct.WriteLine);
                $"{" From:", -8}".txtMuted(); $"{dirPath}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{dirs[0]}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirPath, dirs[0], true, true, filter);
                $"{" To:"  , -8}".txtMuted(); $"{dirs[1]}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirPath, dirs[1], true, true, filter);

                $"".fmNewLine();
                $" --> Uglifying...".txtInfo(ct.WriteLine);
                CmdUglify(Paths.Combine(Env.Get("GULP_PROJECT")));

                //TODO: Ask process done correctly

                $"".fmNewLine();
                $" --> Replacing...".txtInfo(ct.WriteLine);
                $"{" From:", -8}".txtMuted(); $"{dirs[2]}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{dirPath}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirs[2], dirPath, true, true); 

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

        public static void Revert() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" GULP REVERT".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj, c.android.cmp); 
                string dirSource = Paths.Combine(Env.Get("GULP_PROJECT"),"bkp");
                $"".fmNewLine();
                $" --> Reverting...".txtInfo(ct.WriteLine);
                List<string> exclude = new List<string>() {};
                $"{" From:", -8}".txtMuted(); $"{dirSource}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{dirPath}".txtDefault(ct.WriteLine);
                Paths.CopyAll(dirSource, dirPath, true, true); 

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
        public static void Server() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr); 
                CmdServer(
                    dirPath, 
                    Paths.Combine(Env.Get("GULP_PROJECT")),
                    cp.gbs.ipt,
                    cp.gbs.dmn,
                    cp.gbs.flv,
                    cp.gbs.srv,
                    cp.gbs.syn,
                    cp.ipl,
                    cp.gbs.ptc
                );
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