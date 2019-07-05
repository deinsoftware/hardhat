using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Validation = ToolBox.Validations.Strings;
using dein.tools;
using static HardHat.Program;
using static Colorify.Colors;
using ToolBox.Bridge;

namespace HardHat
{

    public static partial class Task
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "t", status = false, action = Task.Select });
            PathList(ref opts);
            opts.Add(new Option { opt = "tw", status = false, action = Task.Watch });
            opts.Add(new Option { opt = "tm", status = false, action = Task.Make });
            opts.Add(new Option { opt = "to", status = false, action = Task.Obfuscate, variant = "" });
            opts.Add(new Option { opt = "to-c", status = false, action = Task.Obfuscate, variant = "c" });
            opts.Add(new Option { opt = "to-l", status = false, action = Task.Obfuscate, variant = "l" });
            opts.Add(new Option { opt = "tr", status = false, action = Task.Revert });
            opts.Add(new Option { opt = "tt", status = false, action = Task.Test });
            ServerList(ref opts);
            LogList(ref opts);
            TestList(ref opts);
        }

        public static void Status()
        {
            Options.IsValid("t", Variables.Valid("task_project"));
            PathStatus();
            Options.IsValid("tw", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("tm", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("to", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("to-c", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("to-l", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("tr", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            ServerStatus();
            TestStatus();
        }

        public static void Start()
        {

            if (String.IsNullOrEmpty(_config.personal.menu.serverConfiguration))
            {
                _colorify.WriteLine($"{"[T] Task",-12}", txtStatus(Options.IsValid("t")));
            }
            else
            {
                _colorify.Write($"{"[T] Task: ",-12}", txtStatus(Options.IsValid("t")));
                Section.Configuration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);
            }
            _colorify.Write($"{"   [W] Watch",-17}", txtStatus(Options.IsValid("tw")));
            _colorify.Write($"{"[O] Obfuscate",-17}", txtStatus(Options.IsValid("to")));
            _colorify.Write($"{"[T] Test",-17}", txtStatus(Options.IsValid("to")));
            _colorify.WriteLine($"{"[S] Server",-12}", txtStatus(Options.IsValid("ts")));
            _colorify.Write($"{"   [M] Make",-17}", txtStatus(Options.IsValid("tm")));
            _colorify.Write($"{"[R] Revert",-34}", txtStatus(Options.IsValid("tr")));
            _colorify.WriteLine($"{"[L] Log",-12}", txtStatus(Options.IsValid("ts")));

            _colorify.BlankLines();
        }

        public static void Select()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "CONFIGURATION");

                _colorify.WriteLine($" [P]", txtMuted);
                _colorify.Write($"{"   [W] Web Server",-25}", txtPrimary); _colorify.WriteLine($"{_config.task.webFolder}");
                _colorify.Write($"{"   [L] Log",-25}", txtPrimary); _colorify.WriteLine($"{_config.task.logFolder}");
                _colorify.Write($"{"   [E] Extension",-25}", txtPrimary); _colorify.WriteLine($"{_config.task.extension}");

                _colorify.BlankLines();
                _colorify.WriteLine($" [S] Server (Web/Log)", txtMuted);
                _colorify.Write($"{"   [I] Internal Path:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.internalPath}");
                _colorify.Write($"{"   [C] Configuration:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.file}");
                string taskConfiguration = Selector.Name(Selector.Flavor, _config.personal.webServer.flavor);
                _colorify.Write($"{"   [F] Flavor:",-25}", txtPrimary); _colorify.WriteLine($"{taskConfiguration}");
                _colorify.Write($"{"   [N] Number:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.webServer.number}");
                _colorify.Write($"{"   [S] Sync:",-25}", txtPrimary); _colorify.WriteLine($"{(_config.personal.webServer.sync ? "Yes" : "No")}");
                _colorify.Write($"{"   [O] Open:",-25}", txtPrimary); _colorify.WriteLine($"{(_config.personal.webServer.open ? "Yes" : "No")}");

                _colorify.BlankLines();
                _colorify.WriteLine($" [T] Test", txtMuted);
                _colorify.Write($"{"   [S] Sync:",-25}", txtPrimary); _colorify.WriteLine($"{(_config.personal.testServer.sync ? "Yes" : "No")}");

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
                    Menu.Route($"t>{opt}", "t");
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
                Section.Header("TASK", "MAKE");
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

        public static void Obfuscate()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "OBFUSCATE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidHybridPath);

                string[] dirs = new string[] {
                    _path.Combine(Variables.Value("task_project"),"service/www"),
                    _path.Combine(Variables.Value("task_project"),"service/bld"),
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
                _colorify.WriteLine($" --> Obfuscate...", txtInfo);
                string type = "lite";
                if (_config.personal.menu.selectedVariant == "c")
                {
                    type = "complete";
                }

                Response result = CmdObfuscate(type);
                if (result.code == 0)
                {
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Replacing...", txtInfo);
                    _colorify.Write($"{" From:",-8}", txtMuted); _colorify.WriteLine($"{dirs[1]}");
                    _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{dirPath}");
                    _disk.CopyAll(dirs[1], dirPath, true);
                }

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
                Section.Header("TASK", "REVERT");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidHybridPath);
                string dirSource = _path.Combine(Variables.Value("task_project"), "service/www");
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
                string dirPath = _path.Combine(Variables.Value("task_project"));

                if (_fileSystem.DirectoryExists(_path.Combine(dirPath, ".git")))
                {
                    Git.CmdFetch(dirPath);
                    bool updated = Git.CmdStatus(dirPath);
                    if (!updated)
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($"There is a new Task project version available.");
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
                Section.Header("TASK", "UPDATE");

                string dirPath = _path.Combine(Variables.Value("task_project"));

                _colorify.WriteLine($" --> Updating...", txtInfo);
                Git.CmdPull(dirPath);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Updating Dependencies...", txtInfo);
                Task.CmdRemove();
                Task.CmdInstall();

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