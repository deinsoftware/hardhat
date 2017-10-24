using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    
    public static class Menu {

        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }
        private static IEnumerable<Option> _m { get; set; }

        static Menu()
        {
            _c  = Program.config;
            _cp = Program.config.personal;
            _m  = Options.list;
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
                Options.Valid("p" , true);
                string filePath = Paths.Combine(dirPath, _c.android.prj, _c.android.bld, _cp.sfl ?? "");
                if (!File.Exists(filePath))
                {
                    _cp.sfl = "";
                }
                Options.Valid("pf", !Strings.SomeNullOrEmpty(_cp.spr));
                Options.Valid("pi", !Strings.SomeNullOrEmpty(_cp.spr, _cp.sfl));
                Options.Valid("pd", !Strings.SomeNullOrEmpty(_cp.spr, _cp.sfl));
                Options.Valid("pp", !Strings.SomeNullOrEmpty(_cp.spr, _cp.sfl));
                Options.Valid("ps", !Strings.SomeNullOrEmpty(_cp.spr, _cp.sfl));
                Options.Valid("pv", !Strings.SomeNullOrEmpty(_cp.spr, _cp.sfl));
                _cp.mnu.ps_env = Env.Check("SIGCHECK_HOME");
                // Version Control System
                _cp.mnu.v_env = Env.Check("GIT_HOME");
                _cp.mnu.v_bnc = "";
                if (!String.IsNullOrEmpty(_cp.spr)){
                    string bnc = Git.CmdBranch(dirPath);
                    if (!String.IsNullOrEmpty(bnc))
                    {
                        _cp.mnu.v_bnc = $"git:{Git.CmdBranch(dirPath)}";
                    } 
                }
                Options.Valid("v" , !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
                Options.Valid("vd", !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
                Options.Valid("vp", !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
                Options.Valid("vr", !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
                // Sonar
                StringBuilder s_cnf = new StringBuilder();
                s_cnf.Append($"{_cp.snr.ptc}/");
                if (!String.IsNullOrEmpty(_cp.snr.dmn))
                {
                    s_cnf.Append($"{_cp.snr.dmn}");
                }
                if (!String.IsNullOrEmpty(_cp.snr.prt))
                {
                    s_cnf.Append($":{_cp.snr.prt}");
                }
                if (!String.IsNullOrEmpty(_cp.snr.ipt))
                {
                    s_cnf.Append($"/{_cp.snr.ipt}");
                }
                _cp.mnu.s_cnf = s_cnf.ToString();
                _cp.mnu.sl_env = Env.Check("SONAR_LINT_HOME");
                _cp.mnu.sq_env = Env.Check("SONAR_QUBE_HOME");
                _cp.mnu.ss_env = Env.Check("SONAR_SCANNER_HOME");
                Options.Valid("s" , true);
                Options.Valid("sq", _cp.mnu.sq_env);
                Options.Valid("ss", _cp.mnu.ss_env && !Strings.SomeNullOrEmpty(_cp.spr));
                Options.Valid("sb", _cp.mnu.sq_env && !Strings.SomeNullOrEmpty(_cp.mnu.s_cnf));
                // Gulp
                StringBuilder g_cnf = new StringBuilder();
                g_cnf.Append($"{_cp.gbs.ptc}/");
                if (!String.IsNullOrEmpty(_cp.gbs.dmn))
                {
                    g_cnf.Append(_cp.gbs.dmn);
                }
                g_cnf.Append(Section.FlavorName(_cp.gbs.flv));
                g_cnf.Append(_cp.gbs.srv);
                g_cnf.Append(_cp.gbs.syn ? "+Sync" : "");
                _cp.mnu.g_cnf = g_cnf.ToString();
                _cp.mnu.g_env = Env.Check("GULP_PROJECT");
                _cp.mnu.g_opt = !Strings.SomeNullOrEmpty(_cp.gbs.dmn, _cp.mnu.g_cnf);
                // Build
                StringBuilder b_cnf = new StringBuilder();
                b_cnf.Append(_cp.gdl.dmn ?? "");
                b_cnf.Append(Section.FlavorName(_cp.gdl.flv));
                b_cnf.Append(Section.ModeName(_cp.gdl.mde));
                _cp.mnu.b_cnf = b_cnf.ToString();
                _cp.mnu.b_env = Env.Check("GRADLE_HOME");
                _cp.mnu.t_env = Env.Check("ANDROID_PROPERTIES");
                _cp.mnu.b_opt = !Strings.SomeNullOrEmpty(_cp.gdl.mde, _cp.gdl.flv, _cp.mnu.b_cnf);
                //VPN
                _cp.mnu.cv_env = Env.Check("VPN_HOME");
                
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

            Section.Header($" {name} # {version}|{_cp.hst} : {_cp.ipl} ");

            Status("m");

            Project();
            Vcs();
            Sonar();
            Gulp();
            Build();
            Adb();
            Footer();

            Section.HorizontalRule();

            $"{" Make your choice:", -25}".txtInfo();
            string opt = Console.ReadLine();
            Route(opt);
        }

        public static void Project() {
            if (!Options.Valid("p"))
            {
                $" [P] Select Project".txtPrimary(ct.WriteLine);
            } else {
                $"{" [P] Selected Project:", -25}".txtPrimary();
                $"{_cp.spr}".txtDefault(ct.WriteLine);
            }
            
            if (!Options.Valid("pf"))
            {
                $"   [F] Select File".txtStatus(ct.WriteLine,   Options.Valid("pf"));
            } else {
                $"{"   [F] Selected File:", -25}".txtPrimary();
                $"{_cp.sfl}".txtDefault(ct.WriteLine);
            }

            $"{"   [I] Install" , -17}".txtStatus(ct.Write,     Options.Valid("pi"));
            $"{"[D] Duplicate"  , -17}".txtStatus(ct.Write,     Options.Valid("pd"));
            $"{"[P] Path"       , -17}".txtStatus(ct.Write,     Options.Valid("pp"));
            $"{"[S] Signer"     , -17}".txtStatus(ct.Write,     Options.Valid("ps"));
            $"{"[V] Values"     , -17}".txtStatus(ct.WriteLine, Options.Valid("pv"));

            $"".fmNewLine();
        }

        public static void Vcs() {
            if (Options.Valid("v"))
            {
                $" [V] VCS".txtMuted(ct.WriteLine);
            } else {
                $"{" [V] VCS:", -25}".txtMuted(ct.Write);
                $"{_cp.mnu.v_bnc}".txtDefault(ct.WriteLine);
            }
            $"{"   [D] Discard" , -34}".txtStatus(ct.Write,     Options.Valid("vd"));
            $"{"[P] Pull"       , -34}".txtStatus(ct.Write,     Options.Valid("vp"));
            $"{"[R] Reset"      , -17}".txtStatus(ct.WriteLine, Options.Valid("vr"));
            $"".fmNewLine();
        }

        public static void Sonar() {
            if (!Options.Valid("s"))
            {
                $" [S] Sonar".txtStatus(ct.WriteLine,           Options.Valid("s"));
            } else {
                $"{" [G] Sonar:", -25}".txtStatus(ct.Write,     Options.Valid("s"));
                $"{_cp.mnu.s_cnf}".txtDefault(ct.WriteLine);    
            }
            $"{"   [Q] Qube"   , -34}".txtStatus(ct.Write,      Options.Valid("sq"));
            $"{"[S] Scanner"   , -34}".txtStatus(ct.Write,      Options.Valid("ss"));
            $"{"[B] Browse"    , -17}".txtStatus(ct.WriteLine,  Options.Valid("sb"));
            $"".fmNewLine();
        }

        public static void Gulp() {
            if (!_cp.mnu.g_opt)
            {
                $" [G] Gulp".txtStatus(ct.WriteLine,            Options.Valid("g"));
            } else {
                $"{" [G] Gulp:", -25}".txtStatus(ct.Write,      Options.Valid("g"));
                $"{_cp.mnu.g_cnf}".txtDefault(ct.WriteLine);    
            }
            $"{"   [U] Uglify" , -34}".txtStatus(ct.Write,      Options.Valid("gu"));
            $"{"[R] Revert"    , -34}".txtStatus(ct.Write,      Options.Valid("gr"));
            $"{"[S] Server"    , -17}".txtStatus(ct.WriteLine,  Options.Valid("gs"));
            $"".fmNewLine();
        }

        public static void Build(){
            if (!_cp.mnu.b_opt)
            {
                $" [B] Build".txtStatus(ct.WriteLine,               Options.Valid("b"));
            } else {
                $"{" [B] Build:", -25}".txtStatus(ct.Write,         Options.Valid("b"));
                $"{_cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
            }
            $"{"   [P] Properties" , -34}".txtStatus(ct.Write,      Options.Valid("bp"));
            $"{"[C] Clean"         , -34}".txtStatus(ct.Write,      Options.Valid("bc"));
            $"{"[G] Gradle"        , -17}".txtStatus(ct.WriteLine,  Options.Valid("bg"));
            $"".fmNewLine();
        }

        public static void Adb(){
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
        }

        public static void Footer(){
            $"".fmNewLine();
            $"{" [C] Config", -17}".txtInfo();
            $"{"[I] Info", -17}".txtInfo();
            $"{"[E] Environment", -34}".txtInfo();
            $"{"[X] Exit", -17}".txtDanger(ct.WriteLine);
        }

        public static void Route(string sel = "m", string dfl = "m") {
            _cp.mnu.sel = sel?.ToLower();
            if (Options.Valid(_cp.mnu.sel))
            {
                Action act = Options.Action(_cp.mnu.sel, dfl);
                _cp.mnu.sel = dfl;
                act.Invoke();
            } else {
                Message.Critical();
            }
        }
    }
}