using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace dein.tools
{
    public static class Colorify
    {
        public struct Color
        {
            public ConsoleColor background { get; private set; }
            public ConsoleColor foreground { get; private set; }

            public Color(ConsoleColor? _background, ConsoleColor? _foreground) : this()
            {
                background = _background ?? ConsoleColor.Black;
                foreground = _foreground ?? ConsoleColor.White;
            }
        }

        public static readonly Dictionary<string, Color> Theme = new Dictionary<string, Color>
        {
            {"text-default", new Color(null                  , null                  )},
            {"text-muted"  , new Color(null                  , ConsoleColor.DarkGray )},
            {"text-primary", new Color(null                  , ConsoleColor.Gray     )},
            {"text-success", new Color(null                  , ConsoleColor.DarkGreen)},
            {"text-info"   , new Color(null                  , ConsoleColor.DarkCyan )},
            {"text-warning", new Color(null                  , ConsoleColor.Yellow   )},
            {"text-danger" , new Color(null                  , ConsoleColor.Red      )},
            {"bg-default"  , new Color(null                  , null                  )},
            {"bg-muted"    , new Color(ConsoleColor.DarkGray , ConsoleColor.Black    )},
            {"bg-primary"  , new Color(ConsoleColor.Gray     , ConsoleColor.White    )},
            {"bg-success"  , new Color(ConsoleColor.DarkGreen, ConsoleColor.White    )},
            {"bg-info"     , new Color(ConsoleColor.DarkCyan , ConsoleColor.White    )},
            {"bg-warning"  , new Color(ConsoleColor.Yellow   , ConsoleColor.Black    )},
            {"bg-danger"   , new Color(ConsoleColor.Red      , ConsoleColor.White    )},
        };

        public static void txtStatus   (this string s, Type? type = Type.Write, bool status = false) { 
            if (status)
            {
                s.txtPrimary(type);
            } else {
                s.txtMuted(type);
            }
        }
        
        public static void txtDefault   (this string s, Type? type = Type.Write) { Write (s, "text-default", type); }
        public static void txtMuted     (this string s, Type? type = Type.Write) { Write (s, "text-muted"  , type); }
        public static void txtPrimary   (this string s, Type? type = Type.Write) { Write (s, "text-primary", type); }
        public static void txtSuccess   (this string s, Type? type = Type.Write) { Write (s, "text-success", type); }
        public static void txtInfo      (this string s, Type? type = Type.Write) { Write (s, "text-info"   , type); }
        public static void txtWarning   (this string s, Type? type = Type.Write) { Write (s, "text-warning", type); }
        public static void txtDanger    (this string s, Type? type = Type.Write) { Write (s, "text-danger" , type); }
        public static void bgDefault    (this string s, Type? type = Type.Write) { Write (s, "bg-default"  , type); }
        public static void bgMuted      (this string s, Type? type = Type.Write) { Write (s, "bg-muted"    , type); }
        public static void bgPrimary    (this string s, Type? type = Type.Write) { Write (s, "bg-primary"  , type); }
        public static void bgSuccess    (this string s, Type? type = Type.Write) { Write (s, "bg-success"  , type); }
        public static void bgInfo       (this string s, Type? type = Type.Write) { Write (s, "bg-info"     , type); }
        public static void bgWarning    (this string s, Type? type = Type.Write) { Write (s, "bg-warning"  , type); }
        public static void bgDanger     (this string s, Type? type = Type.Write) { Write (s, "bg-danger"   , type); }
        public static void fmNewLine    (this string s, int? lines = 1) { BlankLine(lines); }

        public enum Type {
            Write,
            WriteLine,
            PadLeft,
            PadRight,
            Justify,
            Repeat,
            Shell
        }

        private static void Write(string s, string color, Type? type = Type.Write)
        {
            var t = Theme[color];
            Console.BackgroundColor = t.background;
            Console.ForegroundColor = t.foreground;

            switch (type)
            {
                case Type.WriteLine:
                    Console.WriteLine(s);
                    break;
                case Type.PadLeft:
                    Console.WriteLine(s.PadRight(Console.WindowWidth - 1));
                    break;
                case Type.PadRight:
                    Console.WriteLine(s.PadLeft(Console.WindowWidth - 1));
                    break;
                case Type.Justify:
                    int r = Console.WindowWidth / 2;
                    int l = Console.WindowWidth - r;

                    Console.Write(s.Split('|')[0].PadRight(r - 1));
                    Console.WriteLine(s.Split('|')[1].PadLeft(l - 0));
                    break;
                case Type.Repeat:
                    char c = ( String.IsNullOrEmpty(s) ? ' ' : s.ToCharArray()[0] );
                    s = new String(c, Console.WindowWidth - 1);
                    Console.WriteLine(s);
                    break;
                case Type.Shell:
                    string line = "";
                    string[] words = s.Split(' ');
                    foreach (var item in words)
                    {
                        int chunkSize = (Console.WindowWidth - 3);
                        if ( (line.Length + item.Length) >= chunkSize )
                        {
                            Console.WriteLine($" {line.Trim()}");
                            line = "";
                        }
                        if ( item.Length >= chunkSize )
                        {
                            if (line.Length > 0){
                                Console.WriteLine($" {line.Trim()}");
                            }
                            line = "";
                            for (int i = 0; i < item.Length ; i += chunkSize)
                            {
                                if (i + chunkSize > item.Length) chunkSize = item.Length  - i;
                                Console.WriteLine($" {item.Substring(i, chunkSize).Trim()}");
                            }
                            line = "";
                        } else {
                            line += $"{item} ";
                        }
                    }
                    if (!String.IsNullOrEmpty(line.Trim()))
                    {
                        Console.WriteLine($" {line.Trim()}");
                    }
                    break;
                case Type.Write:
                default:
                    Console.Write(s);
                    break;
            }
            
            Console.ResetColor();
        }

        private static void BlankLine(int? lines = 1)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.WriteLine("");
            }
        }

        public static void Default(string color = "bg-default")
        {
            var t = Theme[color];
            Console.BackgroundColor = t.background;
            Console.ForegroundColor = t.foreground;
            Console.Clear();
        }
    }
}