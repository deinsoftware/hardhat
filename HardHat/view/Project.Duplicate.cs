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
        public static void Duplicate()
        {
            _colorify.Clear();

            try
            {
                Section.Header("PROJECT", "DUPLICATE FILE");
                Section.SelectedFile();

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.android.projectPath, _config.android.buildPath);

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a new name, without include his extension.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    System.IO.File.Copy(_path.Combine(dirPath, _config.personal.selected.path, _config.personal.selected.file), _path.Combine(dirPath, $"{opt}{_config.android.buildExtension}"));
                    if (_config.personal.selected.mappingStatus)
                    {
                        System.IO.File.Copy(_path.Combine(dirPath, _config.personal.selected.path, _config.personal.selected.mapping), _path.Combine(dirPath, $"{opt}{_config.android.mappingSuffix}"));
                    }
                    _config.personal.selected.path = "";
                    _config.personal.selected.file = $"{opt}{_config.android.buildExtension}";
                    _config.personal.selected.mapping = $"{opt}{_config.android.mappingSuffix}";
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