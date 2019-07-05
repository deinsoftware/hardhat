using System;
using System.Collections.Generic;
using dein.tools;
using static HardHat.Program;
using static Colorify.Colors;

namespace HardHat
{

    public static partial class Task
    {
        public static void PathList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "t>pw", status = true, action = Task.PathServer });
            opts.Add(new Option { opt = "t>pl", status = true, action = Task.PathLog });
            opts.Add(new Option { opt = "t>pe", status = true, action = Task.PathExtension });
        }

        public static void PathStatus()
        {
            Options.IsValid("t>pw", Variables.Valid("tp"));
            Options.IsValid("t>pl", Variables.Valid("tp"));
            Options.IsValid("t>pe", Variables.Valid("tp"));
        }

        public static void PathServer()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "PATH", "WEB SERVER");

                _colorify.WriteLine($" Web Server configuration path inside path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.task.webFolder = $"{opt}";
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
                Section.Header("TASK", "PATH", "LOG");

                _colorify.WriteLine($" Log configuration path inside path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.task.logFolder = $"{opt}";
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
                Section.Header("TASK", "PATH", "EXTENSION");

                _colorify.WriteLine($" File extension inside Server folder.", txtPrimary);
                _colorify.WriteLine($" Don't use . (dot character) at start.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.task.extension = $".{opt}";
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