using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using dein.tools;

namespace dein.tools
{

    public static class Exceptions
    {

        public static void General(string msg = null)
        {
            if (!String.IsNullOrEmpty(msg))
            {
                msg = $" {msg}";
            }
            Message.Critical(msg);
        }

    }
}