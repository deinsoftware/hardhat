using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using static dein.tools.Paths;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public static class Configuration {

        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Configuration()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Select() {
            Colorify.Default();

            Section.Header("CONFIGURATION");
            
            $" [P] Paths".txtMuted(ct.WriteLine);
            $"{"   [D] Development", -25}".txtPrimary();   $"{_c.path.dir}".txtDefault(ct.WriteLine);
            $"{"   [B] Business"   , -25}".txtPrimary();   $"{_c.path.bsn}".txtDefault(ct.WriteLine);
            $"{"   [P] Projects"   , -25}".txtPrimary();   $"{_c.path.prj}".txtDefault(ct.WriteLine);
            $"{"   [F] Filter"     , -25}".txtPrimary();   $"{_c.path.flt}".txtDefault(ct.WriteLine);

            $"".fmNewLine();
            $" [A] Android Path".txtMuted(ct.WriteLine);
            $"{"   [P] Project"    , -25}".txtPrimary();   $"{_c.android.prj}".txtDefault(ct.WriteLine);
            $"{"   [B] Build"      , -25}".txtPrimary();   $"{_c.android.bld}".txtDefault(ct.WriteLine);
            $"{"   [E] Extension"  , -25}".txtPrimary();   $"{_c.android.ext}".txtDefault(ct.WriteLine);
            $"{"   [C] Compact"    , -25}".txtPrimary();   $"{_c.android.cmp}".txtDefault(ct.WriteLine);
            $"{"   [F] Filter"     , -25}".txtPrimary();   $"{string.Join(",", _c.android.flt)}".txtDefault(ct.WriteLine);

            $"".fmNewLine();
            $" [G] Gulp Path".txtMuted(ct.WriteLine);
            $"{"   [S] Server"     , -25}".txtPrimary();   $"{_c.gulp.srv}".txtDefault(ct.WriteLine);
            $"{"   [E] Extension"  , -25}".txtPrimary();   $"{_c.gulp.ext}".txtDefault(ct.WriteLine);

            $"".fmNewLine();
            $" [V] VPN".txtMuted(ct.WriteLine);
            $"{"   [S] Sitename"   , -25}".txtPrimary();   $"{_c.vpn.snm}".txtDefault(ct.WriteLine);

            $"".fmNewLine();
            $"{"[EMPTY] Save", 82}".txtSuccess(ct.WriteLine);
            
            Section.HorizontalRule();

            $"{" Make your choice:", -25}".txtInfo();
            string opt = Console.ReadLine();

            if(String.IsNullOrEmpty(opt?.ToLower()))
            {
                Settings.Save(_c);
                Menu.Start();
            } else {
                Menu.Route($"c>{opt?.ToLower()}", "c");
            }
            Message.Error();
        }

        #region Paths
        public static void PathDevelopment() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "PATH DEVELOPMENT");
                
                $" Write main Development path.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = Paths.Combine(opt);
                    if (!Directory.Exists(dirPath))
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($" Path not found: {Environment.NewLine}");
                        msg.Append($" '{dirPath}'{Environment.NewLine}");

                        Message.Error(
                            msg: msg.ToString(), 
                            replace: true
                        );
                    } else {
                        _c.path.dir = $"{opt}";
                        _c.path.bsn = "";
                    }
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void PathBusiness() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "PATH BUSINESS");
                
                string dirPath = Paths.Combine(_c.path.dir);

                if (!Directory.Exists(dirPath)){
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" Path not found:{Environment.NewLine}");
                    msg.Append($" '{dirPath}'{Environment.NewLine}");
                    msg.Append(Environment.NewLine);
                    msg.Append(" Please review your configuration file.");

                    Message.Error(
                        msg: msg.ToString()
                    );
                }

                List<string> dirs = new List<string>(Directory.EnumerateDirectories(dirPath));
                if (dirs.Count < 1)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" There is no business in current location:{Environment.NewLine}");
                    msg.Append($" '{dirPath}'");
                    
                    Message.Alert(msg.ToString());
                }
                var i = 1;
                foreach (var dir in dirs)
                {
                    string d = dir.Slash();
                    $" {i, 2}] {d.Substring(d.LastIndexOf("/") + 1)}".txtPrimary(ct.WriteLine);
                    i++;
                }

                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    Validation.Range(opt, 1, dirs.Count);
                    
                    var sel = dirs[Convert.ToInt32(opt) - 1].Slash();
                    _c.path.bsn = sel.Substring(sel.LastIndexOf("/") + 1);
                }

                Select();
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Exceptions.General(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Exceptions.General(PathEx.Message);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void PathProjects() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "PATH PROJECTS");
                
                $" Projects folder inside Business path.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, opt);
                    if (!Directory.Exists(dirPath))
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($" Path not found: {Environment.NewLine}");
                        msg.Append($" '{dirPath}'{Environment.NewLine}");

                        Message.Error(
                            msg: msg.ToString(), 
                            replace: true
                        );
                    } else {
                        _c.path.prj = $"{opt}";
                    }
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void PathFilter() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "PATH FILTER");
                
                $" Filter for folders inside Projects path.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                $" Can use wildcard.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _c.path.flt = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        #endregion

        #region Android
        public static void AndroidProject() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID PROJECT");
                
                $" Android folder inside selected project path.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, opt);
                    if (!Directory.Exists(dirPath))
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($" Path not found: {Environment.NewLine}");
                        msg.Append($" '{dirPath}'{Environment.NewLine}");

                        Message.Error(
                            msg: msg.ToString(), 
                            replace: true
                        );
                    } else {
                        _c.android.prj = $"{opt}";
                    }
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidBuild() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID BUILD");
                
                $" Build path inside Android Project folder.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _c.android.bld = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidExtension() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID EXTENSION");
                
                $" File extension inside Build folder.".txtPrimary(ct.WriteLine);
                $" Don't use . (dot character) at start.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _c.android.ext = $".{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidCompact() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION > ANDROID COMPACT");
                
                $" Files path inside Selected Project to be compacted with gulp.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _c.android.cmp = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidFilter() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID FILTER");
                
                $" Filter extension name to be proccessed with gulp.".txtPrimary(ct.WriteLine);
                $" List separated by , (comma character).".txtPrimary(ct.WriteLine);
                $" Don't use . (dot character) at start.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    var list = opt.Split(',');
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = $".{list[i]}";
                    }
                    _c.android.flt = list;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        #endregion

        #region Gulp
        public static void GulpServer() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "GULP SERVER");
                
                $" Server path inside Gulp path.".txtPrimary(ct.WriteLine);
                $" Don't use / (slash character) at start or end.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _c.gulp.srv = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void GulpExtension() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "GULP EXTENSION");
                
                $" File extension inside Server folder.".txtPrimary(ct.WriteLine);
                $" Don't use . (dot character) at start.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _c.gulp.ext = $".{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        #endregion

        #region VPN
        public static void SiteName() {
            Colorify.Default();

            try
            {
                Section.Header("CONFIGURATION", "VPN SITE NAME");
                
                $" Site name for VPN connection.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _c.vpn.snm = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        #endregion
    }
}