using System;
using System.Collections.Generic;
using dein.tools;
using Validation = ToolBox.Validations.Strings;
using static HardHat.Program;
using static Colorify.Colors;

namespace HardHat
{

    public static partial class Task
    {
        public static void TestList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "tt", status = false, action = Task.Test });
            opts.Add(new Option { opt = "tc", status = false, action = Task.Coverage });
            opts.Add(new Option { opt = "t>ts", status = false, action = Task.TestSync });
            opts.Add(new Option { opt = "t>tc", status = false, action = Task.CoveragePath });
        }

        public static void TestStatus()
        {
            Options.IsValid("tt", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project));
            Options.IsValid("tc", Variables.Valid("task_project") && !Validation.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.testServer.coveragePath));
            Options.IsValid("t>ts", Variables.Valid("task_project"));
            Options.IsValid("t>tc", Variables.Valid("task_project"));
        }

        public static void TestSync()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "TEST", "SYNC");

                string opt = Selector.Start(Selector.Logical, "n");
                _config.personal.testServer.sync = (opt == "y");

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void CoveragePath()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "TEST", "COVERAGE");

                _colorify.WriteLine($" Coverage configuration path inside path.", txtPrimary);
                _colorify.WriteLine($" Don't use / (slash character) at start or end.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(opt))
                {
                    _config.personal.testServer.coveragePath = $"{opt}";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Test()
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
                CmdTest(
                    dirPath,
                    _config.personal.testServer
                );
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Coverage()
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(
                    _config.path.development,
                    _config.path.workspace,
                    _config.path.project,
                    _config.personal.selected.project,
                    _config.personal.testServer.coveragePath
                );
                Browser.CmdOpen($"{dirPath}/index.html");
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}