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
        public static void ServerList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "gs", status = false, action = Gulp.Server });
            opts.Add(new Option { opt = "g>sp", status = false, action = Gulp.Protocol });
            opts.Add(new Option { opt = "g>si", status = false, action = Gulp.InternalPath });
            opts.Add(new Option { opt = "g>sc", status = false, action = Gulp.ConfigurationFile });
            opts.Add(new Option { opt = "g>sf", status = false, action = Gulp.Flavor });
            opts.Add(new Option { opt = "g>sn", status = false, action = Gulp.ServerNumber });
            opts.Add(new Option { opt = "g>ss", status = false, action = Gulp.Sync });
            opts.Add(new Option { opt = "g>so", status = false, action = Gulp.Open });
        }

        public static void ServerStatus()
        {
            StringBuilder serverConfiguration = new StringBuilder();
            serverConfiguration.Append($"{_config.personal.webServer.protocol}://");
            if (!String.IsNullOrEmpty(_config.personal.webServer.file))
            {
                serverConfiguration.Append($"{_config.personal.webServer.file}/");
            }
            serverConfiguration.Append(Selector.Name(Selector.Flavor, _config.personal.webServer.flavor));
            serverConfiguration.Append(_config.personal.webServer.number);
            if (!String.IsNullOrEmpty(_config.personal.webServer.internalPath))
            {
                serverConfiguration.Append($"/{_config.personal.webServer.internalPath}");
            }
            serverConfiguration.Append(_config.personal.webServer.sync ? "+Sync" : "");
            _config.personal.menu.serverConfiguration = serverConfiguration.ToString();
            _config.personal.menu.serverValidation = !Validation.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.webServer.file, _config.personal.menu.serverConfiguration);

            Options.Valid("gs", Variables.Valid("gp") && _config.personal.menu.serverValidation);
            Options.Valid("g>sp", Variables.Valid("gp"));
            Options.Valid("g>si", Variables.Valid("gp"));
            Options.Valid("g>sc", Variables.Valid("gp"));
            Options.Valid("g>sf", Variables.Valid("gp"));
            Options.Valid("g>sn", Variables.Valid("gp"));
            Options.Valid("g>ss", Variables.Valid("gp"));
            Options.Valid("g>so", Variables.Valid("gp"));
        }

        public static void Protocol()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "PROTOCOL");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

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
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write an internal path inside your project.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Remove",82}", txtWarning);

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

        public static void ConfigurationFile()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "SERVER", "FILE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

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
                Section.Header("GULP", "SERVER", "NUMBER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

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
                Section.Header("GULP", "SERVER", "OPEN");
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
    }
}