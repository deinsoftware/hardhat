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
        public static void Path()
        {
            _colorify.Clear();

            try
            {
                Section.Header("PROJECT", "PATH");

                string developmentPath = _path.Combine(_config.path.development);
                string workspacePath = _path.Combine(_config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidBuildPath, _config.personal.selected.path);

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

        public static void CopyPath()
        {
            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidBuildPath, _config.personal.selected.path);
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
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.project.androidPath, _config.project.androidBuildPath, _config.personal.selected.path);
                Clipboard.Copy(_path.Combine(dirPath, fileName));
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}