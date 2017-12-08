using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using dein.tools;
using static dein.tools.Machine;

using ct = dein.tools.Colorify.Type;

namespace dein.tools {

    public static class Exceptions {

        public static void General(string msg = null) {
            if (!String.IsNullOrEmpty(msg)){
                msg = $" {msg}";
            }
            Message.Critical(msg);
        }

    }
}