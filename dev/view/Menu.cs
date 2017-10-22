using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public class Menu {

        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Menu()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Status(string sel = null){
            try
            {
                _cp.ipl = Network.GetLocalIPAddress();

                if (!String.IsNullOrEmpty(sel))
                {
                    _cp.mnu.sel = sel;
                }

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr ?? "");
                // Project
                if (!Directory.Exists(dirPath))
                {
                    _cp.spr = "";
                }
                _cp.mnu.p_sel = String.IsNullOrEmpty(_cp.spr);
                string filePath = Paths.Combine(dirPath, _c.android.prj, _c.android.bld, _cp.sfl ?? "");
                if (!File.Exists(filePath))
                {
                    _cp.sfl = "";
                }
                _cp.mnu.f_sel = String.IsNullOrEmpty(_cp.sfl);
                // Version Control System
                if (!_cp.mnu.p_sel){
                    _cp.mnu.v_bnc = $"git:{Git.CmdBranch(dirPath)}";
                } else {
                    _cp.mnu.v_bnc = "";
                }
                _cp.mnu.v_sel = String.IsNullOrEmpty(_cp.mnu.v_bnc);
                // Gradle
                StringBuilder g_cnf = new StringBuilder();
                g_cnf.Append($"{_cp.gbs.ptc}:");
                if (!String.IsNullOrEmpty(_cp.gbs.dmn))
                {
                    g_cnf.Append(_cp.gbs.dmn);
                }
                switch (_cp.gbs.flv?.ToLower())
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
                g_cnf.Append(_cp.gbs.srv);
                g_cnf.Append(_cp.gbs.syn ? "+Sync" : "");
                _cp.mnu.g_cnf = g_cnf.ToString();
                _cp.mnu.g_env = Env.Check("GULP_PROJECT");
                _cp.mnu.g_sel = String.IsNullOrEmpty(_cp.gbs.dmn) || String.IsNullOrEmpty(_cp.mnu.g_cnf);
                // Build
                StringBuilder b_cnf = new StringBuilder();
                b_cnf.Append(_cp.gdl.dmn ?? "");
                switch (_cp.gdl.flv?.ToLower())
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
                switch (_cp.gdl.mde?.ToLower())
                {
                    case "d":
                        b_cnf.Append("Debug");
                        break;
                    case "r":
                        b_cnf.Append("Release");
                        break;
                }
                _cp.mnu.b_cnf = b_cnf.ToString();
                _cp.mnu.b_env = Env.Check("GRADLE_HOME");
                _cp.mnu.t_env = Env.Check("ANDROID_PROPERTIES");
                _cp.mnu.b_sel = String.IsNullOrEmpty(_cp.gdl.mde) && String.IsNullOrEmpty(_cp.gdl.flv) && String.IsNullOrEmpty(_cp.mnu.b_cnf);
                //VPN
                _cp.mnu.v_env = Env.Check("VPN_HOME");
                //Sigcheck
                _cp.mnu.s_env = Env.Check("SIGCHECK_HOME");
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

            $"=".bgInfo(ct.Repeat);
            $" {name} # {version}|{_cp.hst} : {_cp.ipl} ".bgInfo(ct.Justify);
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            Status("m");

            #region Project
            if (_cp.mnu.p_sel)
            {
                $" [P] Select Project".txtPrimary(ct.WriteLine);
            } else {
                $"{" [P] Selected Project:", -25}".txtPrimary();
                $"{_cp.spr}".txtDefault(ct.WriteLine);
            }
            
            if (_cp.mnu.f_sel)
            {
                $"   [F] Select File".txtStatus(ct.WriteLine, !_cp.mnu.p_sel);
            } else {
                $"{"   [F] Selected File:", -25}".txtPrimary();
                $"{_cp.sfl}".txtDefault(ct.WriteLine);
            }

            $"{"   [I] Install" , -17}".txtStatus(ct.Write,     !_cp.mnu.p_sel && !_cp.mnu.f_sel);
            $"{"[D] Duplicate"  , -17}".txtStatus(ct.Write,     !_cp.mnu.p_sel && !_cp.mnu.f_sel);
            $"{"[P] Path"       , -17}".txtStatus(ct.Write,     !_cp.mnu.p_sel && !_cp.mnu.f_sel);
            $"{"[S] Signer"     , -17}".txtStatus(ct.Write,     !_cp.mnu.p_sel && !_cp.mnu.f_sel);
            $"{"[V] Values"     , -17}".txtStatus(ct.WriteLine, !_cp.mnu.p_sel && !_cp.mnu.f_sel);

            $"".fmNewLine();
            #endregion

            #region VCS
            if (_cp.mnu.v_sel)
            {
                $" [V] VCS".txtMuted(ct.WriteLine);
            } else {
                $"{" [V] VCS:", -25}".txtMuted(ct.Write);
                $"{_cp.mnu.v_bnc}".txtDefault(ct.WriteLine);
            }
            $"{"   [D] Discard" , -34}".txtStatus(ct.Write,     !_cp.mnu.p_sel && !_cp.mnu.v_sel);
            $"{"[P] Pull"       , -34}".txtStatus(ct.Write,     !_cp.mnu.p_sel && !_cp.mnu.v_sel);
            $"{"[R] Reset"      , -17}".txtStatus(ct.WriteLine, !_cp.mnu.p_sel && !_cp.mnu.v_sel);
            $"".fmNewLine();
            #endregion

            #region Sonar
            if (_cp.mnu.g_sel)
            {
                $" [S] Sonar".txtStatus(ct.WriteLine, !_cp.mnu.s_sel && _cp.mnu.g_env);
            } else {
                $"{" [G] Sonar:", -25}".txtStatus(ct.Write, !_cp.mnu.p_sel && !_cp.mnu.g_sel && _cp.mnu.g_env);
                $"{_cp.mnu.s_cnf}".txtDefault(ct.WriteLine);    
            }
            $"{"   [Q] Qube"   , -34}".txtStatus(ct.Write,     !_cp.mnu.s_sel && _cp.mnu.sq_env);
            $"{"[S] Scanner"   , -34}".txtStatus(ct.Write,     !_cp.mnu.s_sel && _cp.mnu.ss_env);
            $"{"[B] Browse"    , -17}".txtStatus(ct.WriteLine, !_cp.mnu.s_sel && _cp.mnu.sq_env);
            $"".fmNewLine();
            #endregion
            
            #region Gulp
            if (_cp.mnu.g_sel)
            {
                $" [G] Gulp".txtStatus(ct.WriteLine, !_cp.mnu.p_sel && _cp.mnu.g_env);
            } else {
                $"{" [G] Gulp:", -25}".txtStatus(ct.Write, !_cp.mnu.p_sel && !_cp.mnu.g_sel && _cp.mnu.g_env);
                $"{_cp.mnu.g_cnf}".txtDefault(ct.WriteLine);    
            }
            $"{"   [U] Uglify" , -34}".txtStatus(ct.Write,     !_cp.mnu.p_sel && _cp.mnu.g_env);
            $"{"[R] Revert"    , -34}".txtStatus(ct.Write,     !_cp.mnu.p_sel && _cp.mnu.g_env);
            $"{"[S] Server"    , -17}".txtStatus(ct.WriteLine, !_cp.mnu.p_sel && !_cp.mnu.g_sel && _cp.mnu.g_env);
            $"".fmNewLine();
            #endregion

            #region Build
            if (_cp.mnu.b_sel)
            {
                $" [B] Build".txtStatus(ct.WriteLine, !_cp.mnu.p_sel && _cp.mnu.b_env);
            } else {
                $"{" [B] Build:", -25}".txtStatus(ct.Write, !_cp.mnu.p_sel && !_cp.mnu.b_sel && _cp.mnu.b_env);
                $"{_cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
            }
            $"{"   [P] Properties" , -34}".txtStatus(ct.Write,     !_cp.mnu.p_sel && _cp.mnu.t_env);
            $"{"[C] Clean"         , -34}".txtStatus(ct.Write,     !_cp.mnu.p_sel && !_cp.mnu.b_sel && _cp.mnu.b_env);
            $"{"[G] Gradle"        , -17}".txtStatus(ct.WriteLine, !_cp.mnu.p_sel && !_cp.mnu.b_sel && _cp.mnu.b_env);
            $"".fmNewLine();
            #endregion

            #region ADB
            if (String.IsNullOrEmpty(_cp.adb.dvc))
            {
                $" [A] ADB".txtMuted(ct.WriteLine);
            } else {
                $"{" [A] ADB:", -25}".txtMuted();
                $"{_cp.adb.dvc}".txtDefault(ct.WriteLine);
            }
            $"{"   [D] Devices"     , -34}".txtPrimary(ct.Write);
            if (!_cp.adb.wst)
            {
                $"{"[W] WiFi Connect"   , -34}".txtPrimary(ct.Write);
            } else {
                $"{"[W] WiFi Disconnect", -34}".txtPrimary(ct.Write);
            }
            $"{"[R] Restart"        , -17}".txtPrimary(ct.WriteLine);
            $"".fmNewLine();
            #endregion

            #region Footer
            $"".fmNewLine();
            $"{" [C] Config", -17}".txtInfo();
            $"{"[I] Info", -17}".txtInfo();
            $"{"[E] Environment", -34}".txtInfo();
            $"{"[X] Exit", -17}".txtDanger(ct.WriteLine);
            #endregion

            Section.HorizontalRule();

            $"{" Make your choice:", -25}".txtInfo();
            string opt = Console.ReadLine();
            Route(opt);
        }

        public static void Route(string sel = "m", string dfl = "m") {
            _cp.mnu.sel = sel?.ToLower();
            switch (_cp.mnu.sel)
            {
                case "m":
                    Menu.Start();
                    break;
                // Project
                case "p":
                    Project.Select();
                    break;
                case "pf":
                    if (!_cp.mnu.p_sel) Project.File();
                    break;
                case "pi":
                    if (!_cp.mnu.p_sel && !_cp.mnu.f_sel) Adb.Install();
                    break;
                case "pd":
                    if (!_cp.mnu.p_sel && !_cp.mnu.f_sel) Project.Duplicate();
                    break;
                case "pp":
                    if (!_cp.mnu.p_sel && !_cp.mnu.f_sel) Project.FilePath();
                    break;
                case "ps":
                    if (!_cp.mnu.p_sel && !_cp.mnu.f_sel) BuildTools.SignerVerify();
                    break;
                case "pv":
                    if (!_cp.mnu.p_sel && !_cp.mnu.f_sel) BuildTools.Information();
                    break;
                // Version Control System
                case "vd":
                    if (!_cp.mnu.p_sel && !_cp.mnu.v_sel) Vcs.Actions(true, false, false);
                    break;
                case "vp":
                    if (!_cp.mnu.p_sel && !_cp.mnu.v_sel) Vcs.Actions(false, true, false);
                    break;
                case "vr":
                    if (!_cp.mnu.p_sel && !_cp.mnu.v_sel) Vcs.Actions(false, false, true);
                    break;
                case "vd+p":
                    if (!_cp.mnu.p_sel && !_cp.mnu.v_sel) Vcs.Actions(true, true, false);
                    break;
                case "vr+p":
                    if (!_cp.mnu.p_sel && !_cp.mnu.v_sel) Vcs.Actions(false, true, true);
                    break;
                //Gulp
                case "g":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Select();
                    break;
                case "g>i":      
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.InternalPath();
                    break;
                case "g>d":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Dimension();
                    break;
                case "g>f":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Flavor();
                    break;
                case "g>n":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Number();
                    break;
                case "g>s":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Sync();
                    break;
                case "g>p":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Protocol();
                    break;
                case "gu":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Uglify();
                    break;
                case "gr":
                    if (!_cp.mnu.p_sel && _cp.mnu.g_env) Gulp.Revert();
                    break;
                case "gs":
                    if (!_cp.mnu.p_sel && !_cp.mnu.g_sel && _cp.mnu.g_env) Gulp.Server();
                    break;
                // Build
                case "b":
                    if (!_cp.mnu.p_sel && _cp.mnu.b_env) Build.Select();
                    break;
                case "b>d":
                    if (!_cp.mnu.p_sel && _cp.mnu.b_env) Build.Dimension();
                    break;
                case "b>f":
                    if (!_cp.mnu.p_sel && _cp.mnu.b_env) Build.Flavor();
                    break;
                case "b>m":
                    if (!_cp.mnu.p_sel && _cp.mnu.b_env) Build.Mode();
                    break;
                case "bp":
                    if (!_cp.mnu.p_sel && _cp.mnu.t_env) Build.Properties();
                    break;
                case "bc":
                    if (!_cp.mnu.p_sel && !_cp.mnu.b_sel && _cp.mnu.t_env) Build.Clean();
                    break;
                case "bg":
                    if (!_cp.mnu.p_sel && !_cp.mnu.b_sel && _cp.mnu.b_env) Build.Gradle();
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
                    if (!_cp.adb.wst)
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
                    _cp.mnu.sel = dfl;
                    break;
            }
            
            Message.Critical();
        }
    }
}