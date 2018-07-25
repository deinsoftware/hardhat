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
            PathList(ref opts);
            opts.Add(new Option { opt = "gw", status = false, action = Gulp.Watch });
            opts.Add(new Option { opt = "gm", status = false, action = Gulp.Make });
            opts.Add(new Option { opt = "gu", status = false, action = Gulp.Uglify });
            opts.Add(new Option { opt = "gr", status = false, action = Gulp.Revert });
            ServerList(ref opts);
            FtpList(ref opts);
            LogList(ref opts);
        }

        public static void Status()
        {
            Options.IsValid("g", Variables.Valid("gp"));
            PathStatus();
            Options.IsValid("gw", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("gm", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("gu", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("gr", Variables.Valid("gp") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            ServerStatus();
            FtpStatus();
            LogStatus();
        }

        public static void Start()
        {

            _colorify.WriteLine($" [G] Gulp", txtStatus(Options.IsValid("g")));
            _colorify.Write($"{"   [W] Watch",-17}", txtStatus(Options.IsValid("gw")));
            _colorify.Write($"{"[U] Uglify",-17}", txtStatus(Options.IsValid("gu")));
            if (String.IsNullOrEmpty(_config.personal.menu.serverConfiguration))
            {
                _colorify.WriteLine($"{"[S] Server",-12}", txtStatus(Options.IsValid("gs")));
            }
            else
            {
                _colorify.Write($"{"[S] Server: ",-12}", txtStatus(Options.IsValid("gs")));
                Section.Configuration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);
            }
            _colorify.Write($"{"   [M] Make",-17}", txtStatus(Options.IsValid("gm")));
            _colorify.Write($"{"[R] Revert",-17}", txtStatus(Options.IsValid("gr")));
            if (String.IsNullOrEmpty(_config.personal.menu.ftpConfiguration))
            {
                _colorify.WriteLine($"{"[F] FTP",-12}", txtStatus(Options.IsValid("gs")));
            }
            else
            {
                _colorify.Write($"{"[F] FTP: ",-12}", txtStatus(Options.IsValid("gs")));
                Section.Configuration(_config.personal.menu.ftpValidation, _config.personal.menu.ftpConfiguration);
            }
            if (String.IsNullOrEmpty(_config.personal.menu.logConfiguration))
            {
                _colorify.WriteLine($"{"[L] Log",41}", txtStatus(Options.IsValid("gs")));
            }
            else
            {
                _colorify.Write($"{" ",34}", txtStatus(Options.IsValid("gs")));
                _colorify.Write($"{"[L] Log: ",-12}", txtStatus(Options.IsValid("gs")));
                Section.Configuration(_config.personal.menu.logValidation, _config.personal.menu.logConfiguration);
            }

            _colorify.BlankLines();
        }

        public static void Select()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "CONFIGURATION");

                _colorify.WriteLine($" [P]", txtMuted);
                _colorify.Write($"{"   [W] Web Server",-25}", txtPrimary); _colorify.WriteLine($"{_config.gulp.webFolder}");
                _colorify.Write($"{"   [L] Log",-25}", txtPrimary); _colorify.WriteLine($"{_config.gulp.logFolder}");
                _colorify.Write($"{"   [E] Extension",-25}", txtPrimary); _colorify.WriteLine($"{_config.gulp.extension}");

                _colorify.BlankLines();
                _colorify.WriteLine($" [S] Server (Web/Log)", txtMuted);
                _colorify.Write($"{"   [P] Protocol:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.protocol}");
                _colorify.Write($"{"   [I] Internal Path:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.internalPath}");
                _colorify.Write($"{"   [C] Configuration:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.file}");
                string gulpConfiguration = Selector.Name(Selector.Flavor, _config.personal.webServer.flavor);
                _colorify.Write($"{"   [F] Flavor:",-25}", txtPrimary); _colorify.WriteLine($"{gulpConfiguration}");
                _colorify.Write($"{"   [N] Number:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.number}");
                _colorify.Write($"{"   [S] Sync:",-25}", txtPrimary); _colorify.WriteLine($"{(_config.personal.webServer.sync ? "Yes" : "No")}");
                _colorify.Write($"{"   [O] Open:",-25}", txtPrimary); _colorify.WriteLine($"{(_config.personal.webServer.open ? "Yes" : "No")}");

                _colorify.BlankLines();
                _colorify.WriteLine($" [F] FTP", txtMuted);
                _colorify.Write($"{"   [H] Host:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.ftpServer.host}");
                _colorify.Write($"{"   [P] Port:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.ftpServer.port}");
                _colorify.Write($"{"   [A] Authentication:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.ftpServer.authenticationPath}");
                _colorify.Write($"{"   [K] Key:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.ftpServer.authenticationKey}");
                _colorify.Write($"{"   [R] Remote Path:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.ftpServer.remotePath}");
                _colorify.Write($"{"   [D] Dimension:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.ftpServer.dimension}");

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

        public static void Watch()
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
                CmdWatch(dirPath);

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
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Making...", txtInfo);
                CmdMake(dirPath);

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
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidHybridPath);

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
                List<string> filter = _disk.FilterCreator(true, _config.project.filterFiles);

                _colorify.WriteLine($" --> Copying...", txtInfo);
                _colorify.Write($"{" From:",-8}", txtMuted); _colorify.WriteLine($"{dirPath}");
                _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{dirs[0]}");
                _disk.CopyAll(dirPath, dirs[0], true, filter);
                _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{dirs[1]}");
                _disk.CopyAll(dirPath, dirs[1], true, filter);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Uglifying...", txtInfo);
                CmdUglify();

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
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidHybridPath);
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
                            Update();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Update()
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
                Gulp.CmdInstall();

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