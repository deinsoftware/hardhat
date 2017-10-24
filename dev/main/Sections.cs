using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static class Section {

        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Section()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Header(string title, params string[] sections){
            $"=".bgInfo(ct.Repeat);
            StringBuilder text = new StringBuilder();
            text.Append(title);
            foreach (var s in sections)
            {
                text.Append($" > {s}");
            } 
            $" {text.ToString()}".bgInfo((title.Contains("|") ? ct.Justify : ct.PadLeft));
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();
        }

        public static void SelectedProject(){
            $"{" Selected Project:" , -25}".txtMuted();
            $"{_cp.spr}".txtDefault(ct.WriteLine);
        }

        public static void SelectedFile(){
            $"{" Selected File:"    , -25}".txtMuted();
            $"{_cp.sfl}".txtDefault(ct.WriteLine);
        }

        public static void HorizontalRule() {
            $"".fmNewLine();
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();
        }

        public static string FlavorName(string flv){
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return flv;
        }

        public static string ModeName(string mde){
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return mde;
        }
    }
}