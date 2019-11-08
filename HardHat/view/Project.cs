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
    public static partial class Project
    {
        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "p", status = true, action = Project.Select });
            opts.Add(new Option { opt = "pf", status = false, action = Project.SelectFile });
            opts.Add(new Option { opt = "po", status = false, action = Project.Open });
            opts.Add(new Option { opt = "po-b", status = false, action = Project.Open, variant = "b" });
            opts.Add(new Option { opt = "pe", status = false, action = Project.Editor });
            opts.Add(new Option { opt = "pe>a", status = false, action = Project.Editor, variant = "a" });
            opts.Add(new Option { opt = "pe>c", status = false, action = Project.Editor, variant = "c" });
            opts.Add(new Option { opt = "pe>s", status = false, action = Project.Editor, variant = "s" });
            opts.Add(new Option { opt = "pe>w", status = false, action = Project.Editor, variant = "w" });
            opts.Add(new Option { opt = "pe>x", status = false, action = Project.Editor, variant = "x" });
            opts.Add(new Option { opt = "pi", status = false, action = Adb.Install });
            opts.Add(new Option { opt = "pp", status = false, action = Project.Path });
            Project.PathList(ref opts);
            opts.Add(new Option { opt = "pv", status = false, action = BuildTools.Values });
        }

        public static void Status(string dirPath)
        {
            if (!_fileSystem.DirectoryExists(dirPath))
            {
                _config.personal.selected.project = "";
                _config.personal.selected.file = "";
            }
            Options.IsValid("p", true);
            string selectedFile = _path.Combine(dirPath, _config.project.androidPath, _config.project.androidBuildPath, _config.personal.selected.path, _config.personal.selected.file);
            if (!File.Exists(selectedFile))
            {
                _config.personal.selected.path = "";
                _config.personal.selected.file = "";
                _config.personal.selected.packageName = "";
                _config.personal.selected.versionCode = "";
                _config.personal.selected.versionName = "";
                _config.personal.selected.mapping = "";
                _config.personal.selected.mappingStatus = false;
            }
            string selectedFileMapping = _path.Combine(dirPath, _config.project.androidPath, _config.project.androidBuildPath, _config.personal.selected.path, _config.personal.selected.mapping);
            _config.personal.selected.mappingStatus = File.Exists(selectedFileMapping);
            Options.IsValid("pf", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("po", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("po-b", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("pe", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("pe>a", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("pe>c", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("pe>s", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("pe>w", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("pe>x", !Strings.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("pi", !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.selected.file));
            Options.IsValid("pp", !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.selected.file));
            Project.PathStatus();
            Options.IsValid("pv", !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.selected.file));
        }

        public static void Start()
        {
            if (String.IsNullOrEmpty(_config.personal.selected.project))
            {
                _colorify.WriteLine($" [P] Select Project", txtPrimary);
            }
            else
            {
                _colorify.Write($" [P] Selected Project: ", txtPrimary);
                _colorify.WriteLine($"{_config.personal.selected.project}");
            }

            if (String.IsNullOrEmpty(_config.personal.selected.file))
            {
                _colorify.WriteLine($"   [F] Select File", txtStatus(Options.IsValid("pf")));
            }
            else
            {
                _colorify.Write($"   [F] Selected File:  ", txtPrimary);
                _colorify.Write($"{_config.personal.selected.file}");
                string mappingStatus = (_config.personal.selected.mappingStatus ? "(M)" : "");
                _colorify.WriteLine($" {mappingStatus}", txtWarning);
            }

            _colorify.Write($"{"   [O] Open",-17}", txtPrimary);
            _colorify.Write($"{"[E] Editor",-17}", txtPrimary);
            _colorify.Write($"{"[I] Install",-17}", txtStatus(Options.IsValid("pi")));
            _colorify.Write($"{"[P] Path",-17}", txtStatus(Options.IsValid("pp")));
            _colorify.WriteLine($"{"[V] Values",-17}", txtStatus(Options.IsValid("pv")));

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
                    _config.personal.selected.project = "";
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
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), dirs.Count);

                    var sel = dirs[Convert.ToInt32(opt) - 1];
                    _config.personal.selected.project = _path.GetFileName(sel);
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
                Section.Header("PROJECT", "SELECT FILE");

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidBuildPath);
                dirPath.Exists("Please review your configuration file or make a build first.");
                List<string> files = dirPath.Files($"*{_config.project.androidBuildExtension}", "Please make a build first.", SearchOption.AllDirectories);

                if (files.Count < 1)
                {
                    _config.personal.selected.path = "";
                    _config.personal.selected.file = "";
                    _config.personal.selected.packageName = "";
                    _config.personal.selected.versionCode = "";
                    _config.personal.selected.versionName = "";
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
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), files.Count);
                    var sel = files[Convert.ToInt32(opt) - 1];
                    _config.personal.selected.path = _path.Split(_path.GetDirectoryName(sel), dirPath);
                    _config.personal.selected.file = _path.GetFileName(sel);
                    _config.personal.selected.mapping = _config.personal.selected.file.Replace(_config.project.androidBuildExtension, _config.project.androidMappingSuffix);
                    _config.personal.selected.packageName = BuildTools.CmdGetPackage(sel, "name", 1);
                    _config.personal.selected.versionCode = BuildTools.CmdGetPackage(sel, "versionCode", 2);
                    _config.personal.selected.versionName = BuildTools.CmdGetPackage(sel, "versionName", 3);
                }

                Menu.Start();
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
                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project
                );
                if (_config.personal.menu.selectedVariant == "b")
                {
                    dirPath = _path.Combine(
                        dirPath,
                        _config.project.androidPath,
                        _config.project.androidBuildPath
                    );
                }
                CmdOpen(dirPath);

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Editor()
        {
            _colorify.Clear();

            try
            {
                EditorLauncher(_config.personal.menu.selectedVariant);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }


        private static void EditorLauncher(string editor)
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
                CmdEditor(editor, dirPath);

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

    }
}