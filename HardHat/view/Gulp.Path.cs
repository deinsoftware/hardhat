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
        public static void PathList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "g>pw", status = true, action = Gulp.PathServer });
            opts.Add(new Option { opt = "g>pl", status = true, action = Gulp.PathLog });
            opts.Add(new Option { opt = "g>pe", status = true, action = Gulp.PathExtension });
        }

        public static void PathStatus()
        {
            Options.IsValid("g>pw", Variables.Valid("gp"));
            Options.IsValid("g>pl", Variables.Valid("gp"));
            Options.IsValid("g>pe", Variables.Valid("gp"));
        }

        public static void PathServer()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "PATH", "WEB SERVER");

                _colorify.WriteLine($" Web Server configuration path inside Gulp path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.webFolder = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void PathLog()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "PATH", "LOG");

                _colorify.WriteLine($" Log configuration path inside Gulp path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.logFolder = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void PathExtension()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "PATH", "EXTENSION");

                _colorify.WriteLine($" File extension inside Server folder.", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.gulp.extension = $".{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}