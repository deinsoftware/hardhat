using System;
using dein.tools;
using static HardHat.Program;
using static Colorify.Colors;
using ToolBox.Validations;
using System.Collections.Generic;

namespace HardHat
{
    public static partial class Project
    {

        public static void PathList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "pp>p", status = false, action = Project.CopyPath });
            opts.Add(new Option { opt = "pp>f", status = false, action = Project.CopyFilePath });
            opts.Add(new Option { opt = "pp>m", status = false, action = Project.CopyMappingPath });
            opts.Add(new Option { opt = "pp>d", status = false, action = Project.Duplicate });
            opts.Add(new Option { opt = "pp>c", status = false, action = Project.Compress });

        }

        public static void PathStatus()
        {
            Options.IsValid("pp>p", !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.selected.file));
            Options.IsValid("pp>f", !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.selected.file));
            Options.IsValid("pp>m", _config.personal.selected.mappingStatus);
            Options.IsValid("pp>d", !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.selected.file));
            Options.IsValid("pp>c", !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.selected.file));
        }

        public static void Path()
        {
            _colorify.Clear();

            try
            {
                Section.Header("PROJECT", "PATH");

                string developmentPath = _path.Combine(_config.path.development);
                string workspacePath = _path.Combine(
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath,
                    _config.project.androidBuildPath,
                    _config.personal.selected.path
                );

                _colorify.Write($"{" Path:",-15}", txtMuted);
                _colorify.WriteLine($"{developmentPath}");

                _colorify.Write($"{" Project:",-15}", txtMuted);
                _colorify.WriteLine($"{workspacePath}");

                _colorify.Write($"{" File:",-15}", txtMuted);
                _colorify.WriteLine($"{_config.personal.selected.file}");

                _colorify.Write($"{" Mapping:",-15}", txtMuted);
                if (_config.personal.selected.mappingStatus)
                {
                    _colorify.WriteLine($"{_config.personal.selected.mapping}");
                }

                _colorify.BlankLines();
                _colorify.Write($"{" [P] Project",-17}", txtPrimary);
                _colorify.Write($"{"[F] File",-17}", txtPrimary);
                _colorify.Write($"{"[M] Mapping",-17}", txtStatus(_config.personal.selected.mappingStatus));
                _colorify.Write($"{"[D] Duplicate",-17}", txtPrimary);
                _colorify.Write($"{"[C] Compress",-17}", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

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

        public static void CopyPath()
        {
            try
            {
                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath,
                    _config.project.androidBuildPath,
                    _config.personal.selected.path
                );
                Clipboard.Copy(dirPath);
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CopyFilePath()
        {
            CopyFullPath(_config.personal.selected.file);
        }

        public static void CopyMappingPath()
        {
            CopyFullPath(_config.personal.selected.mapping);
        }

        private static void CopyFullPath(string fileName)
        {
            try
            {
                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath,
                    _config.project.androidBuildPath,
                    _config.personal.selected.path
                );
                Clipboard.Copy(_path.Combine(dirPath, fileName));
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
                Section.Header("PROJECT", "DUPLICATE FILE");
                Section.SelectedFile();

                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath,
                    _config.project.androidBuildPath
                );

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a new name, without include his extension.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    string sourceFileName = _path.Combine(
                        dirPath,
                        _config.personal.selected.path,
                        _config.personal.selected.file
                    );
                    string destFileName = _path.Combine(
                        dirPath,
                        $"{opt}{_config.project.androidBuildExtension}"
                    );
                    System.IO.File.Copy(sourceFileName, destFileName);
                    if (_config.personal.selected.mappingStatus)
                    {
                        sourceFileName = _path.Combine(
                            dirPath,
                            _config.personal.selected.path,
                            _config.personal.selected.mapping
                        );
                        destFileName = _path.Combine(
                            dirPath,
                            $"{opt}{_config.project.androidMappingSuffix}"
                        );
                        System.IO.File.Copy(sourceFileName, destFileName);
                    }
                    _config.personal.selected.path = "";
                    _config.personal.selected.file = $"{opt}{_config.project.androidBuildExtension}";
                    _config.personal.selected.mapping = $"{opt}{_config.project.androidMappingSuffix}";
                }

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Compress()
        {
            _colorify.Clear();

            try
            {
                Section.Header("PROJECT", "COMPRESS FILE");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.project.androidPath,
                    _config.project.androidBuildPath
                );

                string fileName = _path.GetFileNameWithoutExtension(
                    _path.Combine(
                        dirPath,
                        _config.personal.selected.file
                    )
                );

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Compressing...", txtInfo);

                CmdCompress(dirPath, fileName);

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