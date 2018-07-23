using System;
using dein.tools;
using ToolBox.Validations;

namespace HardHat
{
    public static partial class Browser
    {
        public static void CmdOpen(string url)
        {
            try
            {
                Web.IsUrl(url);
                $"{url}".Browse();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}