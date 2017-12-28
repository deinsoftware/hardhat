using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using static HardHat.Program;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static partial class Modes {

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