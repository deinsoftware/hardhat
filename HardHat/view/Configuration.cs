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
using ToolBox.Platform;
using Colorify.UI;

namespace HardHat
{

    public static class Configuration
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "c", status = true, action = Configuration.Select });
            opts.Add(new Option { opt = "c>md", status = true, action = Configuration.PathDevelopment });
            opts.Add(new Option { opt = "c>mw", status = true, action = Configuration.PathWorkspace });
            opts.Add(new Option { opt = "c>mp", status = true, action = Configuration.PathProjects });
            opts.Add(new Option { opt = "c>mf", status = true, action = Configuration.PathFilter });
            opts.Add(new Option { opt = "c>ap", status = true, action = Configuration.AndroidProject });
            opts.Add(new Option { opt = "c>ab", status = true, action = Configuration.AndroidBuild });
            opts.Add(new Option { opt = "c>ae", status = true, action = Configuration.AndroidExtension });
            opts.Add(new Option { opt = "c>ac", status = true, action = Configuration.AndroidCompact });
            opts.Add(new Option { opt = "c>af", status = true, action = Configuration.AndroidFilter });
            opts.Add(new Option { opt = "c>gw", status = true, action = Configuration.GulpServer });
            opts.Add(new Option { opt = "c>gl", status = true, action = Configuration.GulpLog });
            opts.Add(new Option { opt = "c>ge", status = true, action = Configuration.GulpExtension });
            opts.Add(new Option { opt = "c>v", status = true, action = Configuration.SiteName });
            opts.Add(new Option { opt = "c>t", status = true, action = Configuration.ThemeSelector });
        }

        public static void Select()
        {
            _colorify.Clear();

            Section.Header("CONFIGURATION");

            _colorify.WriteLine($" [M] Main Path", txtMuted);
            _colorify.Write($"{"   [D] Development",-25}", txtPrimary); _colorify.WriteLine($"{_config.path.development}");
            _colorify.Write($"{"   [W] Workspace",-25}", txtPrimary); _colorify.WriteLine($"{_config.path.workspace}");
            _colorify.Write($"{"   [P] Projects",-25}", txtPrimary); _colorify.WriteLine($"{_config.path.project}");
            _colorify.Write($"{"   [F] Filter",-25}", txtPrimary); _colorify.WriteLine($"{_config.path.filter}");
            _colorify.WriteLine($" [A] Android Path", txtMuted);
            _colorify.Write($"{"   [P] Project",-25}", txtPrimary); _colorify.WriteLine($"{_config.android.projectPath}");
            _colorify.Write($"{"   [B] Build",-25}", txtPrimary); _colorify.WriteLine($"{_config.android.buildPath}");
            _colorify.Write($"{"   [E] Extension",-25}", txtPrimary); _colorify.WriteLine($"{_config.android.buildExtension}");
            _colorify.Write($"{"   [C] Compact",-25}", txtPrimary); _colorify.WriteLine($"{_config.android.hybridFiles}");
            _colorify.Write($"{"   [F] Filter",-25}", txtPrimary); _colorify.WriteLine($"{string.Join(",", _config.android.filterFiles)}");
            _colorify.WriteLine($" [G] Gulp Path", txtMuted);
            _colorify.Write($"{"   [W] Web Server",-25}", txtPrimary); _colorify.WriteLine($"{_config.gulp.webFolder}");
            _colorify.Write($"{"   [L] Log",-25}", txtPrimary); _colorify.WriteLine($"{_config.gulp.logFolder}");
            _colorify.Write($"{"   [E] Extension",-25}", txtPrimary); _colorify.WriteLine($"{_config.gulp.extension}");

            _colorify.BlankLines();
            _colorify.Write($"{" [V] VPN",-25}", txtPrimary); _colorify.WriteLine($"{_config.vpn.siteName}");
            string selectedTheme = Selector.Name(Selector.Theme, _config.personal.theme);
            _colorify.Write($"{" [T] Theme",-25}", txtPrimary); _colorify.WriteLine($"{selectedTheme}");

            _colorify.BlankLines();
            _colorify.WriteLine($"{"[EMPTY] Save",82}", txtSuccess);

            Section.HorizontalRule();

            _colorify.Write($"{" Make your choice:",-25}", txtInfo);
            string opt = Console.ReadLine()?.ToLower();

            if (String.IsNullOrEmpty(opt))
            {
                Settings.Save(_config);
                Menu.Start();
            }
            else
            {
                Menu.Route($"c>{opt}", "c");
            }
            Message.Error();
        }

        public static void ThemeSelector()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "THEME");

                string defaultColor = String.Empty;
                switch (OS.GetCurrent())
                {
                    case "win":
                        defaultColor = "d";
                        break;
                    case "mac":
                        defaultColor = "l";
                        break;
                }

                _config.personal.theme = Selector.Start(Selector.Theme, defaultColor);
                switch (_config.personal.theme)
                {
                    case "d":
                        _colorify = new Format(Theme.Dark);
                        break;
                    case "l":
                        _colorify = new Format(Theme.Light);
                        break;
                    default:
                        Message.Error();
                        break;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        #region Paths
        public static void PathDevelopment()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH DEVELOPMENT");

                _colorify.WriteLine($" Write main Development path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = _path.Combine(opt);
                    if (!_fileSystem.DirectoryExists(dirPath))
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($" Path not found: {Environment.NewLine}");
                        msg.Append($" '{dirPath}'{Environment.NewLine}");

                        Message.Error(
                            msg: msg.ToString(),
                            replace: true
                        );
                    }
                    else
                    {
                        _config.path.development = $"{opt}";
                        _config.path.workspace = "";
                    }
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void PathWorkspace()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH WORKSPACE");

                string dirPath = _path.Combine(_config.path.development);

                if (!_fileSystem.DirectoryExists(dirPath))
                {
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
                    msg.Append($" There is no workspace in current location:{Environment.NewLine}");
                    msg.Append($" '{dirPath}'");

                    Message.Alert(msg.ToString());
                }
                var i = 1;
                foreach (var dir in dirs)
                {
                    string d = dir;
                    _colorify.WriteLine($" {i,2}] {_path.GetFileName(d)}", txtPrimary);
                    i++;
                }

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:",-25}", txtInfo);
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), dirs.Count);

                    var sel = dirs[Convert.ToInt32(opt) - 1];
                    _config.path.workspace = _path.GetFileName(sel);
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
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void PathProjects()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH PROJECTS");

                _colorify.WriteLine($" Projects folder inside Workspace path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = _path.Combine(_config.path.development, _config.path.workspace, opt);
                    if (!_fileSystem.DirectoryExists(dirPath))
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($" Path not found: {Environment.NewLine}");
                        msg.Append($" '{dirPath}'{Environment.NewLine}");

                        Message.Error(
                            msg: msg.ToString(),
                            replace: true
                        );
                    }
                    else
                    {
                        _config.path.project = $"{opt}";
                    }
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void PathFilter()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH FILTER");

                _colorify.WriteLine($" Filter for folders inside Projects path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                _colorify.WriteLine($" Can use wildcard.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.path.filter = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }
        #endregion

        #region Android
        public static void AndroidProject()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID PROJECT");

                _colorify.WriteLine($" Android folder inside selected project path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    string dirPath = _path.Combine(_config.path.development, _config.path.workspace, opt);
                    if (!_fileSystem.DirectoryExists(dirPath))
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($" Path not found: {Environment.NewLine}");
                        msg.Append($" '{dirPath}'{Environment.NewLine}");

                        Message.Error(
                            msg: msg.ToString(),
                            replace: true
                        );
                    }
                    else
                    {
                        _config.android.projectPath = $"{opt}";
                    }
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidBuild()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID BUILD");

                _colorify.WriteLine($" Build path inside Android Project folder.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.android.buildPath = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidExtension()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID EXTENSION");

                _colorify.WriteLine($" File extension inside Build folder.", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.android.buildExtension = $".{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidCompact()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID COMPACT");

                _colorify.WriteLine($" Files path inside Selected Project to be compacted with gulp.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.android.hybridFiles = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void AndroidFilter()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID FILTER");

                _colorify.WriteLine($" Filter extension name to be proccessed with gulp.", txtPrimary);
                _colorify.WriteLine($" List separated by , (comma character).", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    var list = opt.Split(',');
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = $".{list[i]}";
                    }
                    _config.android.filterFiles = list;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }
        #endregion

        #region Gulp
        public static void GulpServer()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "GULP WEB SERVER");

                _colorify.WriteLine($" Web Server configuration path inside Gulp path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.webFolder = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void GulpLog()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "GULP LOG");

                _colorify.WriteLine($" Log configuration path inside Gulp path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.logFolder = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void GulpExtension()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "GULP EXTENSION");

                _colorify.WriteLine($" File extension inside Server folder.", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.extension = $".{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }
        #endregion

        #region VPN
        public static void SiteName()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "VPN SITE NAME");

                _colorify.WriteLine($" Site name for VPN connection.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.vpn.siteName = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }
        #endregion
    }
}