using System;
using static HardHat.Program;
using ToolBox.Log;

namespace dein.tools
{

    public static class Exceptions
    {
        public static void General(Exception Ex)
        {
            string message = Ex.Message;

            if (_config.personal.log)
            {
                _logSystem.Save(Ex, LogLevel.Error);
            }

            if (!String.IsNullOrEmpty(message))
            {
                message = $" {message}";
            }
            Message.Critical(message);
        }
    }
}