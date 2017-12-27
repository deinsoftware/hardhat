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

        public static void Footer(){
            $"".fmNewLine();
            $"{" [C] Config"        , -17}".txtInfo();
            $"{"[I] Info"           , -17}".txtInfo();
            $"{"[E] Environment"    , -34}".txtInfo();
            $"{"[X] Exit"           , -17}".txtDanger(ct.WriteLine);
        }

        public static void SelectedProject(){
            $"{" Selected Project:" , -25}".txtMuted();
            $"{_cp.spr}".txtDefault(ct.WriteLine);
        }

        public static void CurrentConfiguration(bool val, string cnf){
            if (!String.IsNullOrEmpty(cnf))
            {
                $"{" Current Configuration:", -25}".txtMuted();
                Configuration(val, cnf);
            }
        }

        public static void Configuration(bool val, string cnf){
            if (val)
            {
                $"{cnf}".txtDefault(ct.WriteLine);
            } else {
                $"{cnf}".txtWarning(ct.WriteLine);
            }
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

        public static void Pause() {
            $" Press [Any] key to continue...".txtInfo();
            Console.ReadKey();
        }
    }
}