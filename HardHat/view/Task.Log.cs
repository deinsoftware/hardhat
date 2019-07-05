using System;
using System.Collections.Generic;
using dein.tools;
using static HardHat.Program;

namespace HardHat
{

    public static partial class Task
    {

        public static void LogList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "tl", status = false, action = Task.Log });
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