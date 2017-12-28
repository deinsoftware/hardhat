using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Colorify;
using static Colorify.Colors;
using dein.tools;
using static HardHat.Program;

namespace HardHat {

    public static class Selector {

        public static readonly IReadOnlyDictionary<string, string> Flavor = new Dictionary<string, string>
        {
            {"a", "Alfa"},
            {"b", "Beta"},
            {"s", "Stag"},
            {"p", "Prod"},
            {"d", "Desk"}
        };

        public static readonly IReadOnlyDictionary<string, string> Logical = new Dictionary<string, string>
        {
            {"y", "Yes"},
            {"n", "No"}
        };

        public static readonly IReadOnlyDictionary<string, string> Protocol = new Dictionary<string, string>
        {
            {"1", "http"},
            {"2", "https"}
        };

        public static string Name(IReadOnlyDictionary<string, string> sel, string opt){
            try {
                if (String.IsNullOrEmpty(opt)){
                    opt = "";
                } else {
                    opt = opt.ToLower();
                    opt = sel[opt];
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return opt;
        }

        public static void Start(IReadOnlyDictionary<string, string> sel, string dfl){
            try {
                dfl = dfl.ToLower();
                _colorify.BlankLines();
                foreach (var item in sel)
                {
                    if (item.Key == dfl) {
                        _colorify.Write($" {item.Key, 2}] {item.Value}", txtPrimary);
                        _colorify.WriteLine($" (Default)", txtInfo);
                    } else {
                        _colorify.WriteLine($" {item.Key, 2}] {item.Value}", txtPrimary);
                    }
                }
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default", 82}" , txtInfo);
                
                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}