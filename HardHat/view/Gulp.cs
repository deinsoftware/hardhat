using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Validation = ToolBox.Validations.Strings;
using Transform = ToolBox.Transform.Strings;
using dein.tools;
using static HardHat.Program;
using ToolBox.Validations;
using static Colorify.Colors;

namespace HardHat
{

    public static partial class Gulp
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "g", status = false, action = Gulp.Select });
            opts.Add(new Option { opt = "g>i", status = false, action = Gulp.InternalPath });
            opts.Add(new Option { opt = "g>s", status = false, action = Gulp.ServerFile });
            opts.Add(new Option { opt = "g>f", status = false, action = Gulp.Flavor });
            opts.Add(new Option { opt = "g>n", status = false, action = Gulp.ServerNumber });
            opts.Add(new Option { opt = "g>s", status = false, action = Gulp.Sync });
            opts.Add(new Option { opt = "g>p", status = false, action = Gulp.Protocol });
            opts.Add(new Option { opt = "g>o", status = false, action = Gulp.Open });
            opts.Add(new Option { opt = "gw", status = false, action = Gulp.Watch });
            opts.Add(new Option { opt = "gm", status = false, action = Gulp.Make });
            opts.Add(new Option { opt = "gu", status = false, action = Gulp.Uglify });
            opts.Add(new Option { opt = "gr", status = false, action = Gulp.Revert });
            opts.Add(new Option { opt = "gs", status = false, action = Gulp.Server });
            opts.Add(new Option { opt = "gl", status = false, action = Gulp.Log });
        }

        public static void Status()
        {
            StringBuilder gulpConfiguration = new StringBuilder();
            gulpConfiguration.Append($"{_config.personal.webServer.protocol}://");
            if (!String.IsNullOrEmpty(_config.personal.webServer.file))
            {
                gulpConfiguration.Append($"{_config.personal.webServer.file}/");
            }
            gulpConfiguration.Append(Selector.Name(Selector.Flavor, _config.personal.webServer.flavor));
            gulpConfiguration.Append(_config.personal.webServer.number);
            if (!String.IsNullOrEmpty(_config.personal.webServer.internalPath))
            {
                gulpConfiguration.Append($"/{_config.personal.webServer.internalPath}");
            }
            gulpConfiguration.Append(_config.personal.webServer.sync ? "+Sync" : "");
            _config.personal.menu.gulpConfiguration = gulpConfiguration.ToString();
            _config.personal.menu.gulpValidation = !Validation.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.webServer.file, _config.personal.menu.gulpConfiguration);
            Options.Valid("g", Variables.Valid("gp"));
            Options.Valid("g>i", Variables.Valid("gp"));
            Options.Valid("g>s", Variables.Valid("gp"));
            Options.Valid("g>f", Variables.Valid("gp"));
            Options.Valid("g>n", Variables.Valid("gp"));
            Options.Valid("g>s", Variables.Valid("gp"));
            Options.Valid("g>p", Variables.Valid("gp"));
            Options.Valid("g>o", Variables.Valid("gp"));
            Options.Valid("gw", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.Valid("gm", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.Valid("gu", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.Valid("gr", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.Valid("gs", Variables.Valid("gp") && _config.personal.menu.gulpValidation);
            Options.Valid("gl", Variables.Valid("gp") && _config.personal.menu.gulpValidation);
        }

        public static void Start()
        {
            if (String.IsNullOrEmpty(_config.personal.menu.gulpConfiguration))
            {
                _colorify.WriteLine($" [G] Gulp", txtStatus(Options.Valid("g")));
            }
            else
            {
                _colorify.Write($" [G] Gulp: ", txtStatus(Options.Valid("g")));
                Section.Configuration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);
            }
            _colorify.Write($"{"   [W] Watch",-17}", txtStatus(Options.Valid("gw")));
            _colorify.Write($"{"[U] Uglify",-34}", txtStatus(Options.Valid("gu")));
            _colorify.Write($"{"[S] Server",-17}", txtStatus(Options.Valid("gs")));
            _colorify.WriteLine($"{"[F] FTP",-17}", txtStatus(Options.Valid("gf")));

            _colorify.Write($"{"   [M] Make",-17}", txtStatus(Options.Valid("gm")));
            _colorify.Write($"{"[R] Revert",-34}", txtStatus(Options.Valid("gr")));
            _colorify.WriteLine($"{"[L] Log",-17}", txtStatus(Options.Valid("gl")));

            _colorify.BlankLines();
        }

        public static void Select()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                _colorify.BlankLines();
                _colorify.Write($"{" [P] Protocol:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.protocol}");
                _colorify.Write($"{" [I] Internal Path:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.internalPath}");
                _colorify.Write($"{" [S] Server:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.file}");
                string gulpConfiguration = Selector.Name(Selector.Flavor, _config.personal.webServer.flavor);
                _colorify.Write($"{" [F] Flavor:",-25}", txtPrimary); _colorify.WriteLine($"{gulpConfiguration}");
                _colorify.Write($"{" [N] Number:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.number}");
                _colorify.Write($"{" [S] Sync:",-25}", txtPrimary); _colorify.WriteLine($"{(_config.personal.webServer.sync ? "Yes" : "No")}");
                _colorify.Write($"{" [O] Open:",-25}", txtPrimary); _colorify.WriteLine($"{(_config.personal.webServer.open ? "Yes" : "No")}");

                _colorify.WriteLine($"{"[EMPTY] Exit",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:",-25}", txtInfo);
                string opt = Console.ReadLine()?.ToLower();

                if (String.IsNullOrEmpty(opt))
                {
                    Menu.Start();
                }
                else
                {
                    Menu.Route($"g>{opt}", "g");
                }
                Message.Error();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Protocol()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "PROTOCOL");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                string opt = Selector.Start(Selector.Protocol, "2");
                Number.IsOnRange(1, Convert.ToInt32(opt), 2);
                _config.personal.webServer.protocol = Selector.Name(Selector.Protocol, opt);

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void InternalPath()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "INTERNAL PATH");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write an internal path inside your project.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                _config.personal.webServer.internalPath = $"{opt}";

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void ServerFile()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "FILE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                _colorify.BlankLines();
                string dirPath = _path.Combine(Variables.Value("gp"), _config.gulp.webFolder);
                dirPath.Exists("Please review your configuration file.");
                List<string> files = dirPath.Files($"*{_config.gulp.extension}");

                if (files.Count < 1)
                {
                    _config.personal.webServer.file = "";
                }
                else
                {
                    var i = 1;
                    foreach (var file in files)
                    {
                        string f = file;
                        _colorify.WriteLine($" {i,2}] {Transform.RemoveWords(_path.GetFileName(f), _config.gulp.extension)}", txtPrimary);
                        i++;
                    }
                    if (!String.IsNullOrEmpty(_config.personal.webServer.file))
                    {
                        _colorify.BlankLines();
                        _colorify.WriteLine($"{"[EMPTY] Current",82}", txtInfo);
                    }

                    Section.HorizontalRule();

                    _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                    string opt = Console.ReadLine().Trim();

                    if (!String.IsNullOrEmpty(opt))
                    {
                        Number.IsOnRange(1, Convert.ToInt32(opt), files.Count);
                        var sel = files[Convert.ToInt32(opt) - 1];
                        _config.personal.webServer.file = Transform.RemoveWords(_path.GetFileName(sel), _config.gulp.extension);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(_config.personal.webServer.file))
                        {
                            Message.Error();
                        }
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

        public static void Flavor()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "FLAVOR");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                _config.personal.webServer.flavor = Selector.Start(Selector.Flavor, "a");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void ServerNumber()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "NUMBER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a server number:", txtPrimary);
                _colorify.Write($" 1", txtPrimary); _colorify.WriteLine($" (Default)", txtInfo);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsNumber(opt);
                }
                _config.personal.webServer.number = opt;

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Sync()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "SYNC");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                string opt = Selector.Start(Selector.Logical, "n");
                _config.personal.webServer.sync = (opt == "y");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Open()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "OPEN");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                string opt = Selector.Start(Selector.Logical, "y");
                _config.personal.webServer.open = (opt == "y");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Watch()
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
                CmdWatch(dirPath, _path.Combine(Variables.Value("gp")));

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Make()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "MAKE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Making...", txtInfo);
                CmdMake(dirPath, _path.Combine(Variables.Value("gp")));

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Uglify()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "UGLIFY");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.android.projectPath, _config.android.hybridFiles);

                string[] dirs = new string[] {
                    _path.Combine(Variables.Value("gp"),"www"),
                    _path.Combine(Variables.Value("gp"),"bld"),
                };

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Cleaning...", txtInfo);
                foreach (var dir in dirs)
                {
                    _disk.DeleteAll(dir, true);
                    Directory.CreateDirectory(dir);
                }

                _colorify.BlankLines();
                List<string> filter = _disk.FilterCreator(true, _config.android.filterFiles);

                _colorify.WriteLine($" --> Copying...", txtInfo);
                _colorify.Write($"{" From:",-8}", txtMuted); _colorify.WriteLine($"{dirPath}");
                _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{dirs[0]}");
                _disk.CopyAll(dirPath, dirs[0], true, filter);
                _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{dirs[1]}");
                _disk.CopyAll(dirPath, dirs[1], true, filter);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Uglifying...", txtInfo);
                CmdUglify(_path.Combine(Variables.Value("gp")));

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Replacing...", txtInfo);
                _colorify.Write($"{" From:",-8}", txtMuted); _colorify.WriteLine($"{dirs[1]}");
                _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{dirPath}");
                _disk.CopyAll(dirs[1], dirPath, true);

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Revert()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "REVERT");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.gulpValidation, _config.personal.menu.gulpConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.android.projectPath, _config.android.hybridFiles);
                string dirSource = _path.Combine(Variables.Value("gp"), "www");
                _colorify.BlankLines();
                _colorify.WriteLine($" --> Reverting...", txtInfo);
                _colorify.Write($"{" From:",-8}", txtMuted); _colorify.WriteLine($"{dirSource}");
                _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{dirPath}");
                _disk.CopyAll(dirSource, dirPath, true);

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Server()
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
                CmdServer(
                    dirPath,
                    _path.Combine(Variables.Value("gp")),
                    _config.personal.webServer,
                    _config.personal.ipAddress
                );
                Menu.Start();
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
                Vpn.Verification();

                CmdLog(
                    _path.Combine(Variables.Value("gp")),
                    _config.personal.webServer
                );
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Check()
        {
            try
            {
                string dirPath = _path.Combine(Variables.Value("gp"));

                if (_fileSystem.DirectoryExists(_path.Combine(dirPath, ".git")))
                {
                    Git.CmdFetch(dirPath);
                    bool updated = Git.CmdStatus(dirPath);
                    if (!updated)
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($"There is a new Gulp project version available.");
                        msg.Append(Environment.NewLine);
                        msg.Append($" Do you want update it?");
                        bool update = Message.Confirmation(msg.ToString());
                        if (update)
                        {
                            Upgrade();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Upgrade()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "UPDATE");

                string dirPath = _path.Combine(Variables.Value("gp"));

                _colorify.WriteLine($" --> Updating...", txtInfo);
                Git.CmdPull(dirPath);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Updating Dependencies...", txtInfo);
                Gulp.CmdInstall(dirPath);

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}