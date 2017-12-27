using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static partial class Modes {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Modes()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static string Name(string mde){
            try {
                switch (mde?.ToLower())
                {
                    case "d":
                        mde = "Debug";
                        break;
                    case "r":
                        mde = "Release";
                        break;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return mde;
        }
    }
}