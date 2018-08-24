using System;
using dein.tools;
using ToolBox.Validations;
using static HardHat.Program;

namespace HardHat
{
    public static partial class Browser
    {
        public static void CmdOpen(string url)
        {
            try
            {
                Web.IsUrl(url);
                _shell.Browse($"{url}");
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}