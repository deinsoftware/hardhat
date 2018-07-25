using System;
using System.Collections.Generic;
using System.IO;
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
            opts.Add(new Option { opt = "c>pi", status = true, action = Configuration.IosProject });
            opts.Add(new Option { opt = "c>pa", status = true, action = Configuration.AndroidProject });
            opts.Add(new Option { opt = "c>pb", status = true, action = Configuration.AndroidBuild });
            opts.Add(new Option { opt = "c>pe", status = true, action = Configuration.AndroidExtension });
            opts.Add(new Option { opt = "c>pm", status = true, action = Configuration.AndroidMappingSuffix });
            opts.Add(new Option { opt = "c>pc", status = true, action = Configuration.AndroidCompact });
            opts.Add(new Option { opt = "c>pf", status = true, action = Configuration.AndroidFilter });
            opts.Add(new Option { opt = "c>e", status = true, action = Configuration.EditorOpenCommand });
            opts.Add(new Option { opt = "c>v", status = true, action = Configuration.VpnSiteName });
            opts.Add(new Option { opt = "c>t", status = true, action = Configuration.ThemeSelector });
            opts.Add(new Option { opt = "c>l", status = true, action = Configuration.Log });
        }

        public static void Status()
        {
            Options.IsValid("c>v", OS.IsWin());
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

            _colorify.BlankLines();
            _colorify.WriteLine($" [P] Project Path", txtMuted);
            _colorify.Write($"{"   [I] iOS",-25}", txtPrimary); _colorify.WriteLine($"{_config.project.iosPath}");
            _colorify.Write($"{"   [A] Android",-25}", txtPrimary); _colorify.WriteLine($"{_config.project.androidPath}");
            _colorify.Write($"{"   [B] Build",-25}", txtPrimary); _colorify.WriteLine($"{_config.project.androidBuildPath}");
            _colorify.Write($"{"   [E] Extension",-25}", txtPrimary); _colorify.WriteLine($"{_config.project.androidBuildExtension}");
            _colorify.Write($"{"   [M] Mapping",-25}", txtPrimary); _colorify.WriteLine($"{_config.project.androidMappingSuffix}");
            _colorify.Write($"{"   [C] Compact",-25}", txtPrimary); _colorify.WriteLine($"{_config.project.androidHybridPath}");
            _colorify.Write($"{"   [F] Filter",-25}", txtPrimary); _colorify.WriteLine($"{string.Join(",", _config.project.filterFiles)}");

            _colorify.BlankLines();
            string selectedEditor = Selector.Name(Selector.Editor, _config.editor.selected);
            _colorify.Write($"{" [E] Editor",-25}", txtPrimary); _colorify.WriteLine($"{selectedEditor}");
            _colorify.Write($"{" [V] VPN",-25}", txtStatus(OS.IsWin())); _colorify.WriteLine($"{_config.vpn.siteName}");
            string selectedTheme = Selector.Name(Selector.Theme, _config.personal.theme);
            _colorify.Write($"{" [T] Theme",-25}", txtPrimary); _colorify.WriteLine($"{selectedTheme}");
            string statusLog = Selector.Name(Selector.Status, (_config.personal.log ? "e" : "d"));
            _colorify.Write($"{" [L] Log",-25}", txtPrimary); _colorify.WriteLine($"{statusLog}");

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

        #region Paths
        public static void PathDevelopment()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH", "DEVELOPMENT");

                _colorify.WriteLine($" Write main Development path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
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
                Exceptions.General(Ex);
            }
        }

        public static void PathWorkspace()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH", "WORKSPACE");

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
                string opt = Console.ReadLine().Trim();

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
                Exceptions.General(UAEx);
            }
            catch (PathTooLongException PathEx)
            {
                Exceptions.General(PathEx);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void PathProjects()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH", "PROJECTS");

                _colorify.WriteLine($" Projects folder inside Workspace path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
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
                Exceptions.General(Ex);
            }
        }

        public static void PathFilter()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PATH", "FILTER");

                _colorify.WriteLine($" Filter for folders inside Projects path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);
                _colorify.WriteLine($" Can use * as wildcard.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.path.filter = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
        #endregion

        #region Project


        public static void IosProject()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PROJECT", "IOS");

                _colorify.WriteLine($" iOS folder inside selected project path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.project.iosPath = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
        public static void AndroidProject()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "PROJECT", "ANDROID");

                _colorify.WriteLine($" Android folder inside selected project path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.project.androidPath = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void AndroidBuild()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID", "BUILD");

                _colorify.WriteLine($" Build path inside Android Project folder.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.project.androidBuildPath = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void AndroidExtension()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID", "EXTENSION");

                _colorify.WriteLine($" File extension inside Build folder.", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.project.androidBuildExtension = $".{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void AndroidMappingSuffix()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID", "MAPPING SUFFIX");

                _colorify.WriteLine($" Suffix name and extension inside Build folder.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.project.androidMappingSuffix = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void AndroidCompact()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID", "COMPACT");

                _colorify.WriteLine($" Files path inside Selected Project to be compacted with gulp.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.project.androidHybridPath = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void AndroidFilter()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "ANDROID", "FILTER");

                _colorify.WriteLine($" Filter extension name to be proccessed with gulp.", txtPrimary);
                _colorify.WriteLine($" List separated by , (comma character).", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    var list = opt.Split(',');
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = $".{list[i]}";
                    }
                    _config.project.filterFiles = list;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
        #endregion

        #region Others

        public static void EditorOpenCommand()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "EDITOR");

                string opt = Selector.Start(Selector.Editor, "c");
                _config.editor.selected = $"{opt}";

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
        public static void VpnSiteName()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "VPN");

                _colorify.WriteLine($" Site name for VPN connection.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.vpn.siteName = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void ThemeSelector()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "THEME");

                string defaultColor = "";
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
                Exceptions.General(Ex);
            }
        }

        public static void Log()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONFIGURATION", "LOG");

                string opt = Selector.Start(Selector.Status, "d");
                _config.personal.log = (opt == "e");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        #endregion
    }
}