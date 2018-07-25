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

        public static void LogList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "gl", status = false, action = Gulp.Log });
        }

        public static void LogStatus()
        {
            StringBuilder logConfiguration = new StringBuilder();
            logConfiguration.Append($"ssh://");
            if (!String.IsNullOrEmpty(_config.personal.webServer.file))
            {
                logConfiguration.Append($"{_config.personal.webServer.file}/");
            }
            logConfiguration.Append(Selector.Name(Selector.Flavor, _config.personal.webServer.flavor));
            logConfiguration.Append(_config.personal.webServer.number);
            _config.personal.menu.logConfiguration = logConfiguration.ToString();
            _config.personal.menu.logValidation = !Validation.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.webServer.file, _config.personal.menu.logConfiguration);

            Options.IsValid("gl", Variables.Valid("gp") && _config.personal.menu.logValidation);
        }

        public static void Log()
        {
            _colorify.Clear();

            try
            {
                Vpn.Verification();

                CmdLog(_config.personal.webServer);
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}