using System;
using System.Collections.Generic;
using System.Text;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using static Colorify.Colors;

namespace HardHat
{
    public static partial class Build
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "b", status = false, action = Build.Select });
            opts.Add(new Option { opt = "b>t", status = false, action = Build.Type });
            opts.Add(new Option { opt = "b>t:a", status = false, action = Build.Quick, variant = "t:a" });
            opts.Add(new Option { opt = "b>t:b", status = false, action = Build.Quick, variant = "t:b" });
            opts.Add(new Option { opt = "b>d", status = false, action = Build.Dimension });
            opts.Add(new Option { opt = "b>f", status = false, action = Build.Flavor });
            opts.Add(new Option { opt = "b>f:d", status = false, action = Build.Quick, variant = "f:d" });
            opts.Add(new Option { opt = "b>f:q", status = false, action = Build.Quick, variant = "f:q" });
            opts.Add(new Option { opt = "b>f:r", status = false, action = Build.Quick, variant = "f:r" });
            opts.Add(new Option { opt = "b>f:r", status = false, action = Build.Quick, variant = "f:r" });
            opts.Add(new Option { opt = "b>f:m", status = false, action = Build.Quick, variant = "f:m" });
            opts.Add(new Option { opt = "b>f:v", status = false, action = Build.Quick, variant = "f:v" });
            opts.Add(new Option { opt = "b>f:p", status = false, action = Build.Quick, variant = "f:p" });
            opts.Add(new Option { opt = "b>m", status = false, action = Build.Mode });
            opts.Add(new Option { opt = "b>m:d", status = false, action = Build.Quick, variant = "m:d" });
            opts.Add(new Option { opt = "b>m:s", status = false, action = Build.Quick, variant = "m:s" });
            opts.Add(new Option { opt = "b>m:r", status = false, action = Build.Quick, variant = "m:r" });
            opts.Add(new Option { opt = "bi", status = false, action = Build.Init });
            opts.Add(new Option { opt = "bp", status = false, action = Build.Properties });
            opts.Add(new Option { opt = "bc", status = false, action = Build.Clean, variant = "" });
            opts.Add(new Option { opt = "bc-c", status = false, action = Build.Clean, variant = "c" });
            opts.Add(new Option { opt = "bg", status = false, action = Build.Gradle });
        }

        public static void Status()
        {
            StringBuilder buildConfiguration = new StringBuilder();
            string type = Selector.Name(Selector.Type, _config.personal.build.type);
            if (!String.IsNullOrEmpty(type))
            {
                buildConfiguration.Append(type);
            }
            else
            {
                _config.personal.build.type = "";
            }
            buildConfiguration.Append(_config.personal.build.dimension ?? "");
            string flavor = Selector.Name(Selector.Flavor, _config.personal.build.flavor);
            if (!String.IsNullOrEmpty(flavor))
            {
                buildConfiguration.Append(flavor);
            }
            else
            {
                _config.personal.build.flavor = "";
            }
            string mode = Selector.Name(Selector.Mode, _config.personal.build.mode);
            if (!String.IsNullOrEmpty(mode))
            {
                buildConfiguration.Append(mode);
            }
            else
            {
                _config.personal.build.mode = "";
            }
            _config.personal.menu.buildConfiguration = buildConfiguration.ToString();
            _config.personal.menu.buildValidation = !Strings.SomeNullOrEmpty(
                _config.personal.selected.project,
                _config.personal.build.type,
                _config.personal.build.mode,
                _config.personal.build.flavor,
                _config.personal.menu.buildConfiguration);
            Options.IsValid("b", Variables.Valid("git"));
            Options.IsValid("b>t", Variables.Valid("git"));
            Options.IsValid("b>t:a", Variables.Valid("git"));
            Options.IsValid("b>t:b", Variables.Valid("git"));
            Options.IsValid("b>d", Variables.Valid("git"));
            Options.IsValid("b>f", Variables.Valid("git"));
            Options.IsValid("b>f:d", Variables.Valid("git"));
            Options.IsValid("b>f:q", Variables.Valid("git"));
            Options.IsValid("b>f:r", Variables.Valid("git"));
            Options.IsValid("b>f:m", Variables.Valid("git"));
            Options.IsValid("b>f:v", Variables.Valid("git"));
            Options.IsValid("b>f:p", Variables.Valid("git"));
            Options.IsValid("b>m", Variables.Valid("git"));
            Options.IsValid("b>m:d", Variables.Valid("git"));
            Options.IsValid("b>m:s", Variables.Valid("git"));
            Options.IsValid("b>m:r", Variables.Valid("git"));
            Options.IsValid("bi", Variables.Valid("task_project") && !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("bp", Variables.Valid("task_project") && !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("bc", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("bc-c", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("bg", Variables.Valid("git") && _config.personal.menu.buildValidation);
        }

        public static void Start()
        {
            if (String.IsNullOrEmpty(_config.personal.menu.buildConfiguration))
            {
                _colorify.WriteLine($" [B] Build", txtStatus(Options.IsValid("b")));
            }
            else
            {
                _colorify.Write($" [B] Build: ", txtStatus(Options.IsValid("b")));
                Section.Configuration(_config.personal.menu.buildValidation, _config.personal.menu.buildConfiguration);
            }
            _colorify.Write($"{"   [I] Init",-14}", txtStatus(Options.IsValid("bi")));
            _colorify.Write($"{"   [P] Prop",-20}", txtStatus(Options.IsValid("bp")));
            _colorify.Write($"{"[C] Clean",-17}", txtStatus(Options.IsValid("bc")));
            _colorify.WriteLine($"{"[G] Gradle",-17}", txtStatus(Options.IsValid("bg")));
            _colorify.BlankLines();
        }

        public static void Select()
        {
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION");
                Section.SelectedProject();
                Section.CurrentConfiguration(
                    _config.personal.menu.buildValidation,
                    _config.personal.menu.buildConfiguration);

                _colorify.BlankLines();
                string buildType = Selector.Name(Selector.Type, _config.personal.build.type);
                _colorify.Write($"{" [T] Type:",-25}", txtPrimary); _colorify.WriteLine($"{buildType}");
                _colorify.Write($"{" [D] Dimension:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.build.dimension}");
                string buildFlavor = Selector.Name(Selector.Flavor, _config.personal.build.flavor);
                _colorify.Write($"{" [F] Flavor:",-25}", txtPrimary); _colorify.WriteLine($"{buildFlavor}");
                string buildMode = Selector.Name(Selector.Mode, _config.personal.build.mode);
                _colorify.Write($"{" [M] Mode:",-25}", txtPrimary); _colorify.WriteLine($"{buildMode}");

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
                    Menu.Route($"b>{opt}", "b");
                }
                Message.Error();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Type()
        {
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION", "TYPE");
                Section.SelectedProject();
                Section.CurrentConfiguration(
                    _config.personal.menu.buildValidation,
                    _config.personal.menu.buildConfiguration);

                _config.personal.build.type = Selector.Start(Selector.Type, "a");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Dimension()
        {
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION", "DIMENSION");
                Section.SelectedProject();
                Section.CurrentConfiguration(
                    _config.personal.menu.buildValidation,
                    _config.personal.menu.buildConfiguration);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a project dimension:", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Remove",82}", txtWarning);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.personal.build.dimension = $"{opt}";
                }
                else
                {
                    _config.personal.build.dimension = $"";
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
                Section.Header("BUILD CONFIGURATION", "FLAVOR");
                Section.SelectedProject();
                Section.CurrentConfiguration(
                    _config.personal.menu.buildValidation,
                    _config.personal.menu.buildConfiguration);

                _config.personal.build.flavor = Selector.Start(Selector.Flavor, "a");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Mode()
        {
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION", "MODE");
                Section.SelectedProject();
                Section.CurrentConfiguration(
                    _config.personal.menu.buildValidation,
                    _config.personal.menu.buildConfiguration);

                _config.personal.build.mode = Selector.Start(Selector.Mode, "d");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Gradle()
        {
            _colorify.Clear();

            try
            {
                Vpn.Verification();

                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath);
                CmdGradle(dirPath, _config.personal.menu.buildConfiguration);

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Clean()
        {
            _colorify.Clear();

            try
            {
                bool cleanCache = _config.personal.menu.selectedVariant == "c";

                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath);
                CmdClean(dirPath, cleanCache);

                string[] files = new string[] { "mapping", "seeds", "unused" };
                foreach (var file in files)
                {
                    if (_fileSystem.FileExists(_path.Combine(dirPath, file + ".txt")))
                    {
                        _fileSystem.DeleteFile(_path.Combine(dirPath, file + ".txt"));
                    }
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Init()
        {
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION", "INIT");

                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project
                );

                _colorify.WriteLine($" --> Updating Dependencies...", txtInfo);
                Build.CmdRemove(dirPath);
                Build.CmdInstall(dirPath);

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Properties()
        {
            _colorify.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION", "PROPERTIES");
                Section.SelectedProject();
                Section.CurrentConfiguration(
                    _config.personal.menu.buildValidation,
                    _config.personal.menu.buildConfiguration);

                string sourcePath = propertiesSource();
                string destinationPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath);

                _colorify.BlankLines();
                List<string> filter = _disk.FilterCreator(true, ".properties");

                _colorify.WriteLine($" --> Copying...", txtInfo);
                _colorify.BlankLines();
                _colorify.Write($"{" From:",-8}", txtMuted); _colorify.WriteLine($"{sourcePath}");
                _colorify.Write($"{" To:",-8}", txtMuted); _colorify.WriteLine($"{destinationPath}");
                _disk.CopyAll(sourcePath, destinationPath, true, filter);

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        private static string propertiesSource()
        {
            string sourcePath = _path.Combine(Variables.Value("android_properties"));
            string bussinessPath = _path.Combine(sourcePath, _config.path.workspace);
            if (String.IsNullOrEmpty(_config.personal.build.dimension))
            {
                sourcePath = bussinessPath;
            }
            else
            {
                string sourceDimensionPath = _path.Combine(
                    sourcePath,
                    $"{_config.path.workspace}_{_config.personal.build.dimension}"
                );
                if (_fileSystem.DirectoryExists(sourceDimensionPath))
                {
                    sourcePath = sourceDimensionPath;
                }
                else
                {
                    sourcePath = bussinessPath;
                }
            }
            return sourcePath;
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
                    case "t":
                        _config.personal.build.type = value;
                        break;
                    case "f":
                        _config.personal.build.flavor = value;
                        break;
                    case "m":
                        _config.personal.build.mode = value;
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