using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public class Section {

        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Section()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Header(string title){
            $"=".bgInfo(ct.Repeat);
            $" {title}".bgInfo(ct.PadLeft);
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
    }
}