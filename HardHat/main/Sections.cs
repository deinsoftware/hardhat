using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Colorify;
using static Colorify.Colors;
using dein.tools;
using static HardHat.Program;


namespace HardHat {
    public static class Section {

        public static void Header(string title, params string[] sections){
            _colorify.DivisionLine('=', bgInfo);
            StringBuilder text = new StringBuilder();
            text.Append(title);
            foreach (var s in sections)
            {
                text.Append($" > {s}");
            } 
            if (title.Contains("|")){
                _colorify.AlignSplit($" {text.ToString()}", bgInfo);
            } else {
                _colorify.AlignLeft($" {text.ToString()}", bgInfo);
            }
            _colorify.DivisionLine('=', bgInfo);
            _colorify.BlankLines();
        }

        public static void Footer(){
            _colorify.BlankLines();
            _colorify.Write($"{" [C] Config"        , -17}", txtInfo);
            _colorify.Write($"{"[I] Info"           , -17}", txtInfo);
            _colorify.Write($"{"[E] Environment"    , -34}", txtInfo);
            _colorify.WriteLine($"{"[X] Exit"           , -17}", txtDanger);
        }

        public static void SelectedProject(){
            _colorify.Write($"{" Selected Project:" , -25}", txtMuted);
            _colorify.WriteLine($"{_config.personal.spr}");
        }

        public static void CurrentConfiguration(bool val, string cnf){
            if (!String.IsNullOrEmpty(cnf))
            {
                _colorify.Write($"{" Current Configuration:", -25}", txtMuted);
                Configuration(val, cnf);
            }
        }

        public static void Configuration(bool val, string cnf){
            if (val)
            {
                _colorify.WriteLine($"{cnf}");
            } else {
                _colorify.WriteLine($"{cnf}", txtWarning);
            }
        }

        public static void SelectedFile(){
            _colorify.Write($"{" Selected File:"    , -25}", txtMuted);
            _colorify.WriteLine($"{_config.personal.sfl}");
        }

        public static void HorizontalRule() {
            _colorify.BlankLines();
            _colorify.DivisionLine('=', bgInfo);
            _colorify.BlankLines();
        }

        public static void Pause() {
            _colorify.Write($" Press [Any] key to continue...", txtInfo);
            Console.ReadKey();
        }
    }
}