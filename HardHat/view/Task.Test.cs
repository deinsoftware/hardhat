using System;
using System.Collections.Generic;
using dein.tools;
using static HardHat.Program;

namespace HardHat
{

    public static partial class Task
    {
        public static void TestList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "tt", status = false, action = Task.Test });
            opts.Add(new Option { opt = "t>ts", status = false, action = Task.TestSync });
        }

        public static void TestStatus()
        {
            Options.IsValid("tt", Variables.Valid("task_project"));
            Options.IsValid("t>ts", Variables.Valid("task_project"));
        }

        public static void TestSync()
        {
            _colorify.Clear();

            try
            {
                Section.Header("TASK", "TEST", "SYNC");
                Section.SelectedProject();
                Section.CurrentConfiguration(_config.personal.menu.serverValidation, _config.personal.menu.serverConfiguration);

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

        public static void Test()
        {
            _colorify.Clear();

            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
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
    }
}