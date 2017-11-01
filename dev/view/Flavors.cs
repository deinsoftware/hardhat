using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static partial class Flavors {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Flavors()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static string Name(string flv){
            try {
                switch (flv?.ToLower())
                {
                    case "a":
                        flv = "Alfa";
                        break;
                    case "b":
                        flv = "Beta";
                        break;
                    case "s":
                        flv = "Stag";
                        break;
                    case "p":
                        flv = "Prod";
                        break;
                    case "d":
                        flv = "Desk";
                        break;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return flv;
        }

        public static void Start(){
            $"".fmNewLine();
            $" {"A", 2}] Alfa".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
            $" {"B", 2}] Beta".txtPrimary(ct.WriteLine);
            $" {"S", 2}] Stag".txtPrimary(ct.WriteLine);
            $" {"P", 2}] Prod".txtPrimary(ct.WriteLine);
            $" {"D", 2}] Desk".txtPrimary(ct.WriteLine);
            $"".fmNewLine();
            $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
            
            Section.HorizontalRule();
        
            $"{" Make your choice: ", -25}".txtInfo();
        }
    }
}