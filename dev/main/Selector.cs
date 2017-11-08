using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public static class Selector {

        public static readonly Dictionary<string, string> Flavor = new Dictionary<string, string>
        {
            {"a", "Alfa"},
            {"b", "Beta"},
            {"s", "Stag"},
            {"p", "Prog"},
            {"d", "Desk"}
        };

        public static readonly Dictionary<string, string> Logical = new Dictionary<string, string>
        {
            {"y", "Yes"},
            {"n", "No"}
        };

        public static string Name(Dictionary<string, string> sel, string opt){
            try {
                opt = opt.ToLower();
                opt = sel[opt];
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return opt;
        }

        public static void Start(Dictionary<string, string> sel, string dfl){
            try {
                dfl = dfl.ToLower();
                $"".fmNewLine();
                foreach (var item in sel)
                {
                    if (item.Key == dfl) {
                        $" {item.Key, 2}] {item.Value}".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                    } else {
                        $" {item.Key, 2}] {item.Value}".txtPrimary(ct.WriteLine);
                    }
                }
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                Section.HorizontalRule();
            
                $"{" Make your choice: ", -25}".txtInfo();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}