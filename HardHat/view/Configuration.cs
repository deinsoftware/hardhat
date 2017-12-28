using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Validations;
using static HardHat.Program;
using Colorify;
using static Colorify.Colors;

namespace HardHat {

    public static class Configuration {

        public static void Select() {
            _colorify.Clear();
            
            Section.Header("CONFIGURATION");
            
            _colorify.WriteLine($" [P] Paths", txtMuted);
            _colorify.Write($"{"   [D] Development", -25}", txtPrimary);   _colorify.WriteLine($"{_config.path.dir}");
            _colorify.Write($"{"   [B] Business"   , -25}", txtPrimary);   _colorify.WriteLine($"{_config.path.bsn}");
            _colorify.Write($"{"   [P] Projects"   , -25}", txtPrimary);   _colorify.WriteLine($"{_config.path.prj}");
            _colorify.Write($"{"   [F] Filter"     , -25}", txtPrimary);   _colorify.WriteLine($"{_config.path.flt}");

            _colorify.BlankLines();
            _colorify.WriteLine($" [A] Android Path", txtMuted);
            _colorify.Write($"{"   [P] Project"    , -25}", txtPrimary);   _colorify.WriteLine($"{_config.android.prj}");
            _colorify.Write($"{"   [B] Build"      , -25}", txtPrimary);   _colorify.WriteLine($"{_config.android.bld}");
            _colorify.Write($"{"   [E] Extension"  , -25}", txtPrimary);   _colorify.WriteLine($"{_config.android.ext}");
            _colorify.Write($"{"   [C] Compact"    , -25}", txtPrimary);   _colorify.WriteLine($"{_config.android.cmp}");
            _colorify.Write($"{"   [F] Filter"     , -25}", txtPrimary);   _colorify.WriteLine($"{string.Join(",", _config.android.flt)}");

            _colorify.BlankLines();
            _colorify.WriteLine($" [G] Gulp Path", txtMuted);
            _colorify.Write($"{"   [S] Server"     , -25}", txtPrimary);   _colorify.WriteLine($"{_config.gulp.srv}");
            _colorify.Write($"{"   [E] Extension"  , -25}", txtPrimary);   _colorify.WriteLine($"{_config.gulp.ext}");

            _colorify.BlankLines();
            _colorify.WriteLine($" [V] VPN", txtMuted);
            _colorify.Write($"{"   [S] Sitename"   , -25}", txtPrimary);   _colorify.WriteLine($"{_config.vpn.snm}");

            _colorify.BlankLines();
            _colorify.WriteLine($"{"[EMPTY] Save", 82}", txtSuccess);
            
            Section.HorizontalRule();

            _colorify.Write($"{" Make your choice:", -25}", txtInfo);
            string opt = Console.ReadLine();

            if(String.IsNullOrEmpty(opt?.ToLower()))
            {
                Settings.Save(_config);
                Menu.Start();
            } else {
                Menu.Route($"c>{opt?.ToLower()}", "c");
            }
            Message.Error();
        }

        #region Paths
        public static void PathDevelopment() {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH DEVELOPMENT");
                
                _colorify.WriteLine($" Write main Development path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at end.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = _path.Combine(opt);
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
                        _config.path.dir = $"{opt}";
                        _config.path.bsn = "";
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
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH BUSINESS");
                
                string dirPath = _path.Combine(_config.path.dir);

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
                    string d = dir;
                    _colorify.WriteLine($" {i, 2}] {d.Substring(d.LastIndexOf("/") + 1)}", txtPrimary);
                    i++;
                }

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), dirs.Count);
                    
                    var sel = dirs[Convert.ToInt32(opt) - 1];
                    _config.path.bsn = sel.Substring(sel.LastIndexOf("/") + 1);
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
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH PROJECTS");
                
                _colorify.WriteLine($" Projects folder inside Business path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, opt);
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
                        _config.path.prj = $"{opt}";
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
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH FILTER");
                
                _colorify.WriteLine($" Filter for folders inside Projects path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                _colorify.WriteLine($" Can use wildcard.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.path.flt = $"{opt}";
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
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID PROJECT");
                
                _colorify.WriteLine($" Android folder inside selected project path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, opt);
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
                        _config.android.prj = $"{opt}";
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
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID BUILD");
                
                _colorify.WriteLine($" Build path inside Android Project folder.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.android.bld = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidExtension() {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID EXTENSION");
                
                _colorify.WriteLine($" File extension inside Build folder.", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.android.ext = $".{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidCompact() {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION > ANDROID COMPACT");
                
                _colorify.WriteLine($" Files path inside Selected Project to be compacted with gulp.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.android.cmp = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidFilter() {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID FILTER");
                
                _colorify.WriteLine($" Filter extension name to be proccessed with gulp.", txtPrimary);
                _colorify.WriteLine($" List separated by , (comma character).", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    var list = opt.Split(',');
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = $".{list[i]}";
                    }
                    _config.android.flt = list;
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
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "GULP SERVER");
                
                _colorify.WriteLine($" Server path inside Gulp path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.srv = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void GulpExtension() {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "GULP EXTENSION");
                
                _colorify.WriteLine($" File extension inside Server folder.", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.ext = $".{opt}";
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
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "VPN SITE NAME");
                
                _colorify.WriteLine($" Site name for VPN connection.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();
            
                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.vpn.snm = $"{opt}";
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