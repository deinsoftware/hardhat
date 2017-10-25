using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static partial class Protocols {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Protocols()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Start(){
            $"".fmNewLine();
            $" {"1", 2}] http".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
            $" {"2", 2}] https".txtPrimary(ct.WriteLine);
            $"".fmNewLine();
            $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
            
            Section.HorizontalRule();
        
            $"{" Make your choice: ", -25}".txtInfo();
        }
    }
}