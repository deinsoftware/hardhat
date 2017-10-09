using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static class Menu {
        public static void Status(string sel = null){
            var c = Program.config;
            var cp =  Program.config.personal;
            try
            {
                cp.ipl = Network.GetLocalIPAddress();

                if (!String.IsNullOrEmpty(sel))
                {
                    cp.mnu.sel = sel;
                }

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr ?? "");
                // Project
                if (!Directory.Exists(dirPath))
                {
                    cp.spr = "";
                }
                cp.mnu.p_sel = String.IsNullOrEmpty(cp.spr);
                string filePath = Paths.Combine(dirPath, c.android.prj, c.android.bld, cp.sfl ?? "");
                if (!File.Exists(filePath))
                {
                    cp.sfl = "";
                }
                cp.mnu.f_sel = String.IsNullOrEmpty(cp.sfl);
                // Version Control System
                if (!cp.mnu.p_sel){
                    cp.mnu.v_bnc = $"git:{Git.CmdBranch(dirPath)}";
                } else {
                    cp.mnu.v_bnc = "";
                }
                cp.mnu.v_sel = String.IsNullOrEmpty(cp.mnu.v_bnc);
                // Gradle
                StringBuilder g_cnf = new StringBuilder();
                g_cnf.Append($"{cp.gbs.ptc}:");
                if (!String.IsNullOrEmpty(cp.gbs.dmn))
                {
                    g_cnf.Append(cp.gbs.dmn);
                }
                switch (cp.gbs.flv?.ToLower())
                {
                    case "a":
                        g_cnf.Append("Alfa");
                        break;
                    case "b":
                        g_cnf.Append("Beta");
                        break;
                    case "s":
                        g_cnf.Append("Stag");
                        break;
                    case "p":
                        g_cnf.Append("Prod");
                        break;
                    case "d":
                        g_cnf.Append("Desk");
                        break;
                }
                g_cnf.Append(cp.gbs.srv);
                g_cnf.Append(cp.gbs.syn ? "+Sync" : "");
                cp.mnu.g_cnf = g_cnf.ToString();
                cp.mnu.g_env = dein.tools.Env.Check("GULP_PROJECT");
                cp.mnu.g_sel = String.IsNullOrEmpty(cp.gbs.dmn) || String.IsNullOrEmpty(cp.mnu.g_cnf);
                // Build
                StringBuilder b_cnf = new StringBuilder();
                b_cnf.Append(cp.gdl.dmn ?? "");
                switch (cp.gdl.flv?.ToLower())
                {
                    case "a":
                        b_cnf.Append("Alfa");
                        break;
                    case "b":
                        b_cnf.Append("Beta");
                        break;
                    case "s":
                        b_cnf.Append("Stag");
                        break;
                    case "p":
                        b_cnf.Append("Prod");
                        break;
                    case "d":
                        b_cnf.Append("Desk");
                        break;
                }
                switch (cp.gdl.mde?.ToLower())
                {
                    case "d":
                        b_cnf.Append("Debug");
                        break;
                    case "r":
                        b_cnf.Append("Release");
                        break;
                }
                cp.mnu.b_cnf = b_cnf.ToString();
                cp.mnu.b_env = dein.tools.Env.Check("GRADLE_HOME");
                cp.mnu.t_env = dein.tools.Env.Check("ANDROID_TEMPLATE");
                cp.mnu.b_sel = String.IsNullOrEmpty(cp.gdl.mde) && String.IsNullOrEmpty(cp.gdl.flv) && String.IsNullOrEmpty(cp.mnu.b_cnf);
                //VPN
                cp.mnu.v_env = dein.tools.Env.Check("VPN_HOME");
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Start() {
            Colorify.Default();
            Console.Clear();

            string name = Assembly.GetEntryAssembly().GetName().Name.ToUpper().ToString();
            string version = Assembly.GetEntryAssembly().GetName().Version.ToString();

            var cp =  Program.config.personal;

            $"=".bgInfo(ct.Repeat);
            $" {name} # {version}|{cp.hst} : {cp.ipl} ".bgInfo(ct.Justify);
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            Status("m");

            #region Project
            if (cp.mnu.p_sel)
            {
                $" [P] Select Project".txtPrimary(ct.WriteLine);
            } else {
                $"{" [P] Selected Project:", -25}".txtPrimary();
                $"{cp.spr}".txtDefault(ct.WriteLine);
            }
            
            if (cp.mnu.f_sel)
            {
                $"   [F] Select File".txtStatus(ct.WriteLine, !cp.mnu.p_sel);
            } else {
                $"{"   [F] Selected File:", -25}".txtPrimary();
                $"{cp.sfl}".txtDefault(ct.WriteLine);
            }

            $"{"   [I] Install" , -17}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.f_sel);
            $"{"[D] Duplicate"  , -17}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.f_sel);
            $"{"[P] Path"       , -17}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.f_sel);
            $"{"[S] Signer"     , -17}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.f_sel);
            $"{"[V] Values"     , -17}".txtStatus(ct.WriteLine, !cp.mnu.p_sel && !cp.mnu.f_sel);

            $"".fmNewLine();
            #endregion

            #region VCS
            if (cp.mnu.v_sel)
            {
                $" [V] VCS".txtMuted(ct.WriteLine);
            } else {
                $"{" [V] VCS:", -25}".txtMuted(ct.Write);
                $"{cp.mnu.v_bnc}".txtDefault(ct.WriteLine);
            }
            $"{"   [D] Discard" , -34}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.v_sel);
            $"{"[P] Pull"       , -34}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.v_sel);
            $"{"[R] Reset"      , -17}".txtStatus(ct.WriteLine, !cp.mnu.p_sel && !cp.mnu.v_sel);
            $"".fmNewLine();
            #endregion
            
            #region Gulp
            if (cp.mnu.g_sel)
            {
                $" [G] Gulp".txtStatus(ct.WriteLine, !cp.mnu.p_sel && cp.mnu.g_env);
            } else {
                $"{" [G] Gulp:", -25}".txtStatus(ct.Write, !cp.mnu.p_sel && !cp.mnu.g_sel && cp.mnu.g_env);
                $"{cp.mnu.g_cnf}".txtDefault(ct.WriteLine);    
            }
            $"{"   [U] Uglify" , -34}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.g_sel && cp.mnu.g_env);
            $"{"[R] Revert"    , -34}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.g_sel && cp.mnu.g_env);
            $"{"[S] Server"    , -17}".txtStatus(ct.WriteLine, !cp.mnu.p_sel && !cp.mnu.g_sel && cp.mnu.g_env);
            $"".fmNewLine();
            #endregion

            #region Build
            if (cp.mnu.b_sel)
            {
                $" [B] Build".txtStatus(ct.WriteLine, !cp.mnu.p_sel && cp.mnu.b_env);
            } else {
                $"{" [B] Build:", -25}".txtStatus(ct.Write, !cp.mnu.p_sel && !cp.mnu.b_sel && cp.mnu.b_env);
                $"{cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
            }
            $"{"   [P] Properties" , -34}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.b_sel && cp.mnu.t_env);
            $"{"[C] Clean"         , -34}".txtStatus(ct.Write,     !cp.mnu.p_sel && !cp.mnu.b_sel && cp.mnu.b_env);
            $"{"[G] Gradle"        , -17}".txtStatus(ct.WriteLine, !cp.mnu.p_sel && !cp.mnu.b_sel && cp.mnu.b_env);
            $"".fmNewLine();
            #endregion

            #region ADB
            $" [A] Android Debug Bridge".txtMuted(ct.WriteLine);
            $"{"   [R] Restart"}".txtPrimary(ct.WriteLine);

            if (String.IsNullOrEmpty(cp.adb.dvc))
            {
                $"{"   [D] Devices"}".txtPrimary(ct.WriteLine);
            } else {
                $"{"   [D] Device:", -25}".txtPrimary();
                $"{cp.adb.dvc}".txtDefault(ct.WriteLine);
            }

            if (!cp.adb.wst)
            {
                $"{"   [W] WiFi Connect"}".txtPrimary(ct.WriteLine);
            } else {
                $"{"   [W] WiFi Disconnect:", -25}".txtPrimary();
                $"{cp.adb.wip + (!String.IsNullOrEmpty(cp.adb.wpr) ? ":" + cp.adb.wpr : "")}".txtDefault(ct.WriteLine);
            }

            $"".fmNewLine();
            #endregion

            #region Footer
            $"{" [C] Config", -17}".txtInfo();
            $"{"[I] Info", -17}".txtInfo();
            $"{"[E] Environment", -34}".txtInfo();
            $"{"[X] Exit", -17}".txtDanger(ct.WriteLine);
            #endregion

            $"".fmNewLine();
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();
            
            $"{" Make your choice:", -25}".txtInfo();
            string opt = Console.ReadLine();
            cp.mnu.sel = opt?.ToLower();
            Route();
        }

        public static void Route() {
            var cp =  Program.config.personal;

            switch (cp.mnu.sel)
            {
                case "m":
                    Menu.Start();
                    break;
                // Project
                case "p":
                    Project.Select();
                    break;
                case "pf":
                    if (!cp.mnu.p_sel) Project.File();
                    break;
                case "pi":
                    if (!cp.mnu.p_sel && !cp.mnu.f_sel) Adb.Install();
                    break;
                case "pd":
                    if (!cp.mnu.p_sel && !cp.mnu.f_sel) Project.Duplicate();
                    break;
                case "pp":
                    if (!cp.mnu.p_sel && !cp.mnu.f_sel) Project.FilePath();
                    break;
                case "ps":
                    if (!cp.mnu.p_sel && !cp.mnu.f_sel) BuildTools.SignerVerify();
                    break;
                case "pv":
                    if (!cp.mnu.p_sel && !cp.mnu.f_sel) BuildTools.Information();
                    break;
                // Version Control System
                case "vd":
                    if (!cp.mnu.p_sel && !cp.mnu.v_sel) Vcs.Actions(true, false, false);
                    break;
                case "vp":
                    if (!cp.mnu.p_sel && !cp.mnu.v_sel) Vcs.Actions(false, true, false);
                    break;
                case "vr":
                    if (!cp.mnu.p_sel && !cp.mnu.v_sel) Vcs.Actions(false, false, true);
                    break;
                case "vd+p":
                    if (!cp.mnu.p_sel && !cp.mnu.v_sel) Vcs.Actions(true, true, false);
                    break;
                case "vr+p":
                    if (!cp.mnu.p_sel && !cp.mnu.v_sel) Vcs.Actions(false, true, true);
                    break;
                //Gulp
                case "g":
                    if (!cp.mnu.p_sel && cp.mnu.g_env) Gulp.Select();
                    break;
                case "g>i":      
                    if (!cp.mnu.p_sel && cp.mnu.g_env) Gulp.InternalPath();
                    break;
                case "g>d":
                    if (!cp.mnu.p_sel && cp.mnu.g_env) Gulp.Dimension();
                    break;
                case "g>f":
                    if (!cp.mnu.p_sel && cp.mnu.g_env) Gulp.Flavor();
                    break;
                case "g>n":
                    if (!cp.mnu.p_sel && cp.mnu.g_env) Gulp.Number();
                    break;
                case "g>s":
                    if (!cp.mnu.p_sel && cp.mnu.g_env) Gulp.Sync();
                    break;
                case "g>p":
                    if (!cp.mnu.p_sel && cp.mnu.g_env) Gulp.Protocol();
                    break;
                case "gu":
                    if (!cp.mnu.p_sel && !cp.mnu.g_sel && cp.mnu.g_env) Gulp.Uglify();
                    break;
                case "gr":
                    if (!cp.mnu.p_sel && !cp.mnu.g_sel && cp.mnu.g_env) Gulp.Revert();
                    break;
                case "gs":
                    if (!cp.mnu.p_sel && !cp.mnu.g_sel && cp.mnu.g_env) Gulp.Server();
                    break;
                // Build
                case "b":
                    if (!cp.mnu.p_sel && cp.mnu.b_env) Build.Select();
                    break;
                case "b>d":
                    if (!cp.mnu.p_sel && cp.mnu.b_env) Build.Dimension();
                    break;
                case "b>f":
                    if (!cp.mnu.p_sel && cp.mnu.b_env) Build.Flavor();
                    break;
                case "b>m":
                    if (!cp.mnu.p_sel && cp.mnu.b_env) Build.Mode();
                    break;
                case "bg":
                    if (!cp.mnu.p_sel && !cp.mnu.b_sel && cp.mnu.b_env) Build.Gradle();
                    break;
                case "bc":
                    if (!cp.mnu.p_sel && !cp.mnu.b_sel && cp.mnu.t_env) Build.Clean();
                    break;
                case "bp":
                    if (!cp.mnu.p_sel && !cp.mnu.b_sel && cp.mnu.t_env) Build.Properties();
                    break;
                //Configuration
                case "c":
                    Configuration.Select();
                    break;
                case "c>pd":
                    Configuration.PathDevelopment();
                    break;
                case "c>pb":
                    Configuration.PathBusiness();
                    break;
                case "c>pp":
                    Configuration.PathProjects();
                    break;
                case "c>pf":
                    Configuration.PathFilter();
                    break;
                case "c>ap":
                    Configuration.AndroidProject();
                    break;
                case "c>ab":
                    Configuration.AndroidBuild();
                    break;
                case "c>ae":
                    Configuration.AndroidExtension();
                    break;
                case "c>ac":
                    Configuration.AndroidCompact();
                    break;
                case "c>af":
                    Configuration.AndroidFilter();
                    break;
                case "c>gs":
                    Configuration.GulpServer();
                    break;
                case "c>ge":
                    Configuration.GulpExtension();
                    break;
                case "c>vs":
                    Configuration.SiteName();
                    break;
                // ADB
                case "ar":
                    Adb.Restart();
                    break;
                case "ad":
                    Adb.Devices();
                    break;
                case "aw":
                    if (!cp.adb.wst)
                    {
                        Adb.Configuration();
                    } else {
                        Adb.Disconnect();
                    }
                    break;
                // Information
                case "i":
                    Information.Versions();
                    break;
                case "e":
                    Information.Environment();
                    break;
                // Exit
                case "x":
                    Program.Exit();
                    break;
                case "":
                    Menu.Start();
                    break;
                default:
                    cp.mnu.sel = "m";
                    break;
            }
            
            Message.Critical();
        }
    }
}