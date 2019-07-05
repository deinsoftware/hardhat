using System;
using System.Collections.Generic;
using System.Text;
using Validation = ToolBox.Validations.Strings;
using Transform = ToolBox.Transform.Strings;
using dein.tools;
using static HardHat.Program;
using ToolBox.Validations;
using static Colorify.Colors;

namespace HardHat
{

    public static partial class Task
    {
        public static void ServerList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "ts", status = false, action = Task.Server });
            opts.Add(new Option { opt = "t>si", status = false, action = Task.InternalPath });
            opts.Add(new Option { opt = "t>sc", status = false, action = Task.ConfigurationFile });
            opts.Add(new Option { opt = "t>sf", status = false, action = Task.Flavor });
            opts.Add(new Option { opt = "t>f:d", status = false, action = Task.Quick, variant = "f:d" });
            opts.Add(new Option { opt = "t>f:q", status = false, action = Task.Quick, variant = "f:q" });
            opts.Add(new Option { opt = "t>f:r", status = false, action = Task.Quick, variant = "f:r" });
            opts.Add(new Option { opt = "t>f:m", status = false, action = Task.Quick, variant = "f:m" });
            opts.Add(new Option { opt = "t>f:v", status = false, action = Task.Quick, variant = "f:v" });
            opts.Add(new Option { opt = "t>f:p", status = false, action = Task.Quick, variant = "f:p" });
            opts.Add(new Option { opt = "t>sn", status = false, action = Task.ServerNumber });
            opts.Add(new Option { opt = "t>n:1", status = false, action = Task.Quick, variant = "n:1" });
            opts.Add(new Option { opt = "t>n:2", status = false, action = Task.Quick, variant = "n:2" });
            opts.Add(new Option { opt = "t>n:3", status = false, action = Task.Quick, variant = "n:3" });
            opts.Add(new Option { opt = "t>n:4", status = false, action = Task.Quick, variant = "n:4" });
            opts.Add(new Option { opt = "t>n:5", status = false, action = Task.Quick, variant = "n:5" });
            opts.Add(new Option { opt = "t>ss", status = false, action = Task.ServerSync });
            opts.Add(new Option { opt = "t>so", status = false, action = Task.Open });
        }

        public static void ServerStatus()
        {
            StringBuilder serverConfiguration = new StringBuilder();
            if (!String.IsNullOrEmpty(_config.personal.webServer.file))
            {
                serverConfiguration.Append($"{_config.personal.webServer.file}/");
            }
            string flavor = Selector.Name(Selector.Flavor, _config.personal.webServer.flavor);
            if (!String.IsNullOrEmpty(flavor))
            {
                serverConfiguration.Append(flavor);
            }
            else
            {
                _config.personal.webServer.flavor = "";
            }
            serverConfiguration.Append(_config.personal.webServer.number);
            if (!String.IsNullOrEmpty(_config.personal.webServer.internalPath))
            {
                serverConfiguration.Append($"/{_config.personal.webServer.internalPath}");
            }
            serverConfiguration.Append(_config.personal.webServer.sync ? "+Sync" : "");
            _config.personal.menu.serverConfiguration = serverConfiguration.ToString();
            _config.personal.menu.serverValidation = !Validation.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.webServer.file, _config.personal.menu.serverConfiguration);

            Options.IsValid("ts", Variables.Valid("task_project") && _config.personal.menu.serverValidation);
            Options.IsValid("t>sp", Variables.Valid("task_project"));
            Options.IsValid("t>si", Variables.Valid("task_project"));
            Options.IsValid("t>sc", Variables.Valid("task_project"));
            Options.IsValid("t>sf", Variables.Valid("task_project"));
            Options.IsValid("t>f:d", Variables.Valid("task_project"));
            Options.IsValid("t>f:q", Variables.Valid("task_project"));
            Options.IsValid("t>f:r", Variables.Valid("task_project"));
            Options.IsValid("t>f:m", Variables.Valid("task_project"));
            Options.IsValid("t>f:v", Variables.Valid("task_project"));
            Options.IsValid("t>f:p", Variables.Valid("task_project"));
            Options.IsValid("t>sn", Variables.Valid("task_project"));
            Options.IsValid("t>n:1", Variables.Valid("task_project"));
            Options.IsValid("t>n:2", Variables.Valid("task_project"));
            Options.IsValid("t>n:3", Variables.Valid("task_project"));
            Options.IsValid("t>n:4", Variables.Valid("task_project"));
            Options.IsValid("t>n:5", Variables.Valid("task_project"));
            Options.IsValid("t>ss", Variables.Valid("task_project"));
            Options.IsValid("t>so", Variables.Valid("task_project"));
        }

        public static void InternalPath()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "SERVER", "INTERNAL PATH");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write an internal path inside your project.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Remove",82}", txtWarning);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
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

        public static void ConfigurationFile()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "SERVER", "FILE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                _colorify.BlankLines();
                string dirPath = _path.Combine(Variables.Value("task_project"), _config.task.webFolder);
                dirPath.Exists("Please review your configuration file.");
                List<string> files = dirPath.Files($"*{_config.task.extension}");

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
                        _colorify.WriteLine($" {i,2}] {Transform.RemoveWords(_path.GetFileName(f), _config.task.extension)}", txtPrimary);
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
                        _config.personal.webServer.file = Transform.RemoveWords(_path.GetFileName(sel), _config.task.extension);
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
                Section.Header("TASK", "SERVER", "FLAVOR");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

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
                Section.Header("TASK", "SERVER", "NUMBER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a server number:", txtPrimary);
                _colorify.Write($" 1", txtPrimary); _colorify.WriteLine($" (Default)", txtInfo);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
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

        public static void ServerSync()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "SERVER", "SYNC");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

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
                Section.Header("TASK", "SERVER", "OPEN");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

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

        public static void Server()
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
                CmdServer(
                    dirPath,
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

        public static void Quick()
        {
            try
            {
                string[] variant = _config.personal.menu.selectedVariant.Split(':');
                string option = variant[0];
                string value = variant[1];

                switch (option)
                {
                    case "f":
                        _config.personal.webServer.flavor = value;
                        break;
                    case "n":
                        _config.personal.webServer.number = value;
                        break;
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}