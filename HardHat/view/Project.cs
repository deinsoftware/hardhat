using System;
using System.Collections.Generic;
using System.IO;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using static dein.tools.Paths;
using static Colorify.Colors;

namespace HardHat
{

    public static class Project
    {


        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "p", status = true, action = Project.Select });
            opts.Add(new Option { opt = "pf", status = false, action = Project.SelectFile });
            opts.Add(new Option { opt = "pi", status = false, action = Adb.Install });
            opts.Add(new Option { opt = "pd", status = false, action = Project.Duplicate });
            opts.Add(new Option { opt = "pp", status = false, action = Project.FilePath });
            opts.Add(new Option { opt = "pp>p", status = false, action = Project.CopyFilePath });
            opts.Add(new Option { opt = "pp>f", status = false, action = Project.CopyFullPath });
            opts.Add(new Option { opt = "ps", status = false, action = BuildTools.SignerVerify });
            opts.Add(new Option { opt = "pv", status = false, action = BuildTools.Information });
        }

        public static void Status(string dirPath)
        {
            if (!_fileSystem.DirectoryExists(dirPath))
            {
                _config.personal.selectedProject = "";
            }
            Options.Valid("p", true);
            string filePath = _path.Combine(dirPath, _config.android.projectPath, _config.android.buildPath, _config.personal.selectedPath, _config.personal.selectedFile);
            if (!File.Exists(filePath))
            {
                _config.personal.selectedPath = "";
                _config.personal.selectedFile = "";
            }
            Options.Valid("pf", !Strings.SomeNullOrEmpty(_config.personal.selectedProject));
            Options.Valid("pi", !Strings.SomeNullOrEmpty(_config.personal.selectedProject, _config.personal.selectedFile));
            Options.Valid("pd", !Strings.SomeNullOrEmpty(_config.personal.selectedProject, _config.personal.selectedFile));
            Options.Valid("pp", !Strings.SomeNullOrEmpty(_config.personal.selectedProject, _config.personal.selectedFile));
            Options.Valid("pp>p", !Strings.SomeNullOrEmpty(_config.personal.selectedProject, _config.personal.selectedFile));
            Options.Valid("pp>f", !Strings.SomeNullOrEmpty(_config.personal.selectedProject, _config.personal.selectedFile));
            Options.Valid("ps", !Strings.SomeNullOrEmpty(_config.personal.selectedProject, _config.personal.selectedFile));
            Options.Valid("pv", !Strings.SomeNullOrEmpty(_config.personal.selectedProject, _config.personal.selectedFile));
        }

        public static void Start()
        {
            if (String.IsNullOrEmpty(_config.personal.selectedProject))
            {
                _colorify.WriteLine($" [P] Select Project", txtPrimary);
            }
            else
            {
                _colorify.Write($" [P] Selected Project: ", txtPrimary);
                _colorify.WriteLine($"{_config.personal.selectedProject}");
            }

            if (String.IsNullOrEmpty(_config.personal.selectedProject))
            {
                _colorify.WriteLine($"   [F] Select File", txtStatus(Options.Valid("pf")));
            }
            else
            {
                _colorify.Write($"   [F] Selected File:  ", txtPrimary);
                _colorify.WriteLine($"{_config.personal.selectedFile}");
            }

            _colorify.Write($"{"   [I] Install",-17}", txtStatus(Options.Valid("pi")));
            _colorify.Write($"{"[D] Duplicate",-17}", txtStatus(Options.Valid("pd")));
            _colorify.Write($"{"[P] Path",-17}", txtStatus(Options.Valid("pp")));
            _colorify.Write($"{"[S] Signer",-17}", txtStatus(Options.Valid("ps")));
            _colorify.WriteLine($"{"[V] Values",-17}", txtStatus(Options.Valid("pv")));

            _colorify.BlankLines();
        }

        public static void Select()
        {
            _colorify.Clear();

            try
            {
                Section.Header("SELECT PROJECT");

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project);
                dirPath.Exists("Please review your configuration file.");
                List<string> dirs = dirPath.Directories(_config.path.filter, "projects");

                if (dirs.Count < 1)
                {
                    _config.personal.selectedProject = "";
                }
                else
                {
                    var i = 1;
                    foreach (var dir in dirs)
                    {
                        string d = dir;
                        _colorify.WriteLine($" {i,2}] {_path.GetFileName(d)}", txtPrimary);
                        i++;
                    }
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
                    _config.personal.selectedProject = _path.GetFileName(sel);
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void SelectFile()
        {
            _colorify.Clear();

            try
            {
                Section.Header("SELECT FILE");

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selectedProject, _config.android.projectPath, _config.android.buildPath);
                dirPath.Exists("Please review your configuration file or make a build first.");
                List<string> files = dirPath.Files($"*{_config.android.buildExtension}", "Please make a build first.", SearchOption.AllDirectories);

                if (files.Count < 1)
                {
                    _config.personal.selectedPath = "";
                    _config.personal.selectedFile = "";
                }
                else
                {
                    var i = 1;
                    foreach (var file in files)
                    {
                        string f = file;
                        _colorify.WriteLine($" {i,2}] {_path.GetFileName(f)}", txtPrimary);
                        i++;
                    }
                }

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:",-25}", txtInfo);
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), files.Count);
                    var sel = files[Convert.ToInt32(opt) - 1];
                    _config.personal.selectedPath = _path.Split(_path.GetDirectoryName(sel), dirPath);
                    _config.personal.selectedFile = _path.GetFileName(sel);
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CopyFilePath()
        {
            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selectedProject, _config.android.projectPath, _config.android.buildPath, _config.personal.selectedPath);
                Clipboard.Copy(dirPath);
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CopyFullPath()
        {
            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selectedProject, _config.android.projectPath, _config.android.buildPath);
                Clipboard.Copy(_path.Combine(dirPath, _config.personal.selectedPath, _config.personal.selectedFile));
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }


        public static void Duplicate()
        {
            _colorify.Clear();

            try
            {
                Section.Header("DUPLICATE FILE");
                Section.SelectedFile();

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selectedProject, _config.android.projectPath, _config.android.buildPath);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a new name, without include his extension.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    System.IO.File.Copy(_path.Combine(dirPath, _config.personal.selectedPath, _config.personal.selectedFile), _path.Combine(dirPath, $"{opt}{_config.android.buildExtension}"));
                    _config.personal.selectedPath = "";
                    _config.personal.selectedFile = $"{opt}{_config.android.buildExtension}";
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void FilePath()
        {
            _colorify.Clear();

            try
            {
                Section.Header("FILE PATH");

                string developmentPath = _path.Combine(_config.path.development);
                string workspacePath = _path.Combine(_config.path.workspace, _config.path.project, _config.personal.selectedProject, _config.android.projectPath, _config.android.buildPath, _config.personal.selectedPath);

                _colorify.Write($"{" Path:",-15}", txtMuted);
                _colorify.WriteLine($"{developmentPath}");

                _colorify.Write($"{" Project:",-15}", txtMuted);
                _colorify.WriteLine($"{workspacePath}");

                _colorify.Write($"{" File:",-15}", txtMuted);
                _colorify.WriteLine($"{_config.personal.selectedFile}");

                _colorify.BlankLines();
                _colorify.Write($"{" [P] Copy Path",-34}", txtInfo);
                _colorify.Write($"{"[F] Copy Full Path",-34}", txtInfo);
                _colorify.WriteLine($"{"[EMPTY] Cancel",-17}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:",-25}", txtInfo);
                string opt = Console.ReadLine()?.ToLower();

                if (String.IsNullOrEmpty(opt))
                {
                    Menu.Start();
                }
                else
                {
                    Menu.Route($"pp>{opt}", "pp");
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}