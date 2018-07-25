using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using Colorify;
using static Colorify.Colors;

namespace HardHat
{

    public static partial class Sonar
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "s", status = true, action = Sonar.Select });
            opts.Add(new Option { opt = "s>p", status = true, action = Sonar.Protocol });
            opts.Add(new Option { opt = "s>s", status = true, action = Sonar.Server });
            opts.Add(new Option { opt = "s>sd", status = true, action = Sonar.Domain });
            opts.Add(new Option { opt = "s>sp", status = true, action = Sonar.Port });
            opts.Add(new Option { opt = "s>i", status = true, action = Sonar.InternalPath });
            opts.Add(new Option { opt = "sq", status = false, action = Sonar.Qube });
            opts.Add(new Option { opt = "ss", status = false, action = Sonar.Scanner });
            opts.Add(new Option { opt = "sb", status = false, action = Sonar.Browse });
        }

        public static void Status()
        {
            StringBuilder sonarConfiguration = new StringBuilder();
            sonarConfiguration.Append($"{_config.personal.sonar.protocol}://");
            if (!String.IsNullOrEmpty(_config.personal.sonar.domain))
            {
                sonarConfiguration.Append($"{_config.personal.sonar.domain}");
            }
            if (!String.IsNullOrEmpty(_config.personal.sonar.port))
            {
                sonarConfiguration.Append($":{_config.personal.sonar.port}");
            }
            _config.personal.menu.sonarConfiguration = sonarConfiguration.ToString();

            _config.personal.menu.sonarValidation = !Strings.SomeNullOrEmpty(_config.personal.sonar.protocol, _config.personal.sonar.domain, _config.personal.menu.sonarConfiguration);
            Options.IsValid("s", Variables.Valid("sq"));
            Options.IsValid("sq", Variables.Valid("sq"));
            Options.IsValid("ss", Variables.Valid("ss") && !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("sb", _config.personal.menu.sonarValidation);
        }

        public static void Start()
        {
            if (String.IsNullOrEmpty(_config.personal.menu.sonarConfiguration))
            {
                _colorify.WriteLine($" [S] Sonar", txtStatus(Options.IsValid("s")));
            }
            else
            {
                _colorify.Write($" [S] Sonar: ", txtStatus(Options.IsValid("s")));
                Section.Configuration(_config.personal.menu.sonarValidation, _config.personal.menu.sonarConfiguration);
            }
            _colorify.Write($"{"   [Q] Qube",-17}", txtStatus(Options.IsValid("sq")));
            if (String.IsNullOrEmpty(_config.personal.sonar.internalPath))
            {
                _colorify.Write($"{"[S] Scanner",-17}", txtStatus(Options.IsValid("ss")));
            }
            else
            {
                _colorify.Write($"{"[S] Scanner: ",-13}", txtStatus(Options.IsValid("ss")));
                _colorify.Write($"{_config.personal.sonar.internalPath,-21}", txtDefault);
            }
            _colorify.WriteLine($"{"[B] Browse",-17}", txtStatus(Options.IsValid("sb")));
            _colorify.BlankLines();
        }

        public static void Select()
        {
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.sonarValidation, _config.personal.menu.sonarConfiguration);

                _colorify.BlankLines();
                _colorify.Write($"{" [P] Protocol:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.sonar.protocol}");
                _colorify.Write($"{" [S] Server:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.sonar.domain}{(!String.IsNullOrEmpty(_config.personal.sonar.port) ? ":" + _config.personal.sonar.port : "")}");
                _colorify.Write($"{" [I] Internal Path:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.sonar.internalPath}");

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
                    Menu.Route($"s>{opt}", "s");
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
                Section.Header("SONAR CONFIGURATION", "PROTOCOL");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.sonarValidation, _config.personal.menu.sonarConfiguration);

                if (!String.IsNullOrEmpty(_config.personal.menu.sonarConfiguration))
                {
                    _colorify.Write($"{" Current Configuration:",-25}", txtMuted);
                    _colorify.WriteLine($"{_config.personal.menu.sonarConfiguration}");
                }

                string opt = Selector.Start(Selector.Protocol, "2");
                Number.IsOnRange(1, Convert.ToInt32(opt), 2);
                _config.personal.sonar.protocol = Selector.Name(Selector.Protocol, opt);

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
                Section.Header("SONAR CONFIGURATION", "SERVER");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.sonarValidation, _config.personal.menu.sonarConfiguration);

                _colorify.BlankLines();
                _colorify.Write($"{" [D] Domain:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.sonar.domain}");
                _colorify.Write($"{" [P] Port:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.sonar.port}");

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
                    Menu.Route($"s>s{opt}", "s");
                }
                Message.Error();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Domain()
        {
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER", "DOMAIN");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.sonarValidation, _config.personal.menu.sonarConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write server domain.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    _config.personal.sonar.domain = opt;
                }

                Menu.Status();
                Server();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Port()
        {
            _colorify.Clear();

            try
            {
                Section.Header("SONAR CONFIGURATION", "SERVER", "PORT");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.sonarValidation, _config.personal.menu.sonarConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write server port.", txtPrimary);
                _colorify.Write($" Between 0 and 65536.", txtPrimary); _colorify.Write($" 9000 (Default)", txtInfo);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(0, Convert.ToInt32(opt), 65536);
                    _config.personal.sonar.port = opt;
                }
                else
                {
                    _config.personal.sonar.port = "9000";
                }

                Menu.Status();
                Server();
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
                Section.Header("SONAR CONFIGURATION", "INTERNAL PATH");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.sonarValidation, _config.personal.menu.sonarConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write an internal path inside your project.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Remove",82}", txtWarning);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                _config.personal.sonar.internalPath = $"{opt}";

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Qube()
        {
            _colorify.Clear();

            try
            {
                CmdQube();
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Scanner()
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.personal.sonar.internalPath);
                CmdScanner(dirPath);
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Browse()
        {
            _colorify.Clear();

            try
            {
                Browser.CmdOpen(_config.personal.menu.sonarConfiguration);
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}