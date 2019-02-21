using System;
using System.Collections.Generic;
using static Colorify.Colors;
using dein.tools;
using static HardHat.Program;

namespace HardHat
{

    public static class Selector
    {
        public static Dictionary<string, string> Flavor = new Dictionary<string, string>
        {
            {"a", "Alfa"},
            {"b", "Beta"},
            {"s", "Stag"},
            {"p", "Prod"}
        };

        public static readonly IReadOnlyDictionary<string, string> Logical = new Dictionary<string, string>
        {
            {"y", "Yes"},
            {"n", "No"}
        };

        public static readonly IReadOnlyDictionary<string, string> Mode = new Dictionary<string, string>
        {
            {"d", "Debug"},
            {"r", "Release"}
        };

        public static readonly IReadOnlyDictionary<string, string> Editor = new Dictionary<string, string>
        {
            {"a", "Android Studio"},
            {"c", "Visual Studio Code"},
            {"s", "Sublime Text"},
            {"w", "Web Storm"},
            {"x", "Xcode"}
        };

        public static readonly IReadOnlyDictionary<string, string> Priority = new Dictionary<string, string>
        {
            {"v", "Verbouse"},
            {"d", "Debug"},
            {"i", "Info"},
            {"w", "Warning"},
            {"e", "Error"},
            {"f", "Fatal"},
            {"s", "Silence"},
        };

        public static readonly IReadOnlyDictionary<string, string> Protocol = new Dictionary<string, string>
        {
            {"1", "http"},
            {"2", "https"}
        };

        public static readonly IReadOnlyDictionary<string, string> Status = new Dictionary<string, string>
        {
            {"e", "Enable"},
            {"d", "Disable"}
        };

        public static readonly IReadOnlyDictionary<string, string> Theme = new Dictionary<string, string>
        {
            {"l", "Light"},
            {"d", "Dark"},
        };

        public static string Name(IReadOnlyDictionary<string, string> sel, string opt)
        {
            try
            {
                if (String.IsNullOrEmpty(opt))
                {
                    opt = "";
                }
                else
                {
                    opt = opt.ToLower();
                    opt = sel[opt];
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return opt;
        }

        public static string Start(IReadOnlyDictionary<string, string> sel, string dfl)
        {
            string opt = "";

            try
            {
                dfl = dfl.ToLower();

                _colorify.BlankLines();
                foreach (var item in sel)
                {
                    if (item.Key == dfl)
                    {
                        _colorify.Write($" {item.Key.ToUpper(),2}] {item.Value}", txtPrimary);
                        _colorify.WriteLine($" (Default)", txtInfo);
                    }
                    else
                    {
                        _colorify.WriteLine($" {item.Key.ToUpper(),2}] {item.Value}", txtPrimary);
                    }
                }
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ",-25}", txtInfo);

                opt = Console.ReadLine()?.ToLower();

                if (String.IsNullOrEmpty(opt))
                {
                    opt = dfl;
                }
                else
                {
                    if (!sel.ContainsKey(opt))
                    {
                        Message.Error();
                    }
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return opt;
        }
    }
}