using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using ToolBox.Platform;

namespace dein.tools
{
    public static class Colorify
    {
        public static Dictionary<string, Color> Theme {get; private set;}

        static Colorify(){
            Theme = new Dictionary<string, Color>();

            Theme.Add("text-default"   , new Color(null                   , null                   ));
            Theme.Add("bg-default"     , new Color(null                   , null                   ));
            switch (OS.GetCurrent())
            {
                case "win":
                    Theme.Add("text-muted"     , new Color(null                   , ConsoleColor.DarkGray  ));
                    Theme.Add("text-primary"   , new Color(null                   , ConsoleColor.Gray      ));
                    Theme.Add("text-warning"   , new Color(null                   , ConsoleColor.Yellow    ));
                    Theme.Add("text-danger"    , new Color(null                   , ConsoleColor.Red       ));
                    Theme.Add("bg-muted"       , new Color(ConsoleColor.DarkGray  , ConsoleColor.Black     ));
                    Theme.Add("bg-primary"     , new Color(ConsoleColor.Gray      , ConsoleColor.White     ));
                    Theme.Add("bg-warning"     , new Color(ConsoleColor.Yellow    , ConsoleColor.Black     ));
                    Theme.Add("bg-danger"      , new Color(ConsoleColor.Red       , ConsoleColor.White     ));
                    break;
                case "mac":
                    Theme.Add("text-muted"     , new Color(null                   , ConsoleColor.Gray      ));
                    Theme.Add("text-primary"   , new Color(null                   , ConsoleColor.DarkGray  ));
                    Theme.Add("text-warning"   , new Color(null                   , ConsoleColor.DarkYellow));
                    Theme.Add("text-danger"    , new Color(null                   , ConsoleColor.DarkRed   ));
                    Theme.Add("bg-muted"       , new Color(ConsoleColor.Gray      , ConsoleColor.Black     ));
                    Theme.Add("bg-primary"     , new Color(ConsoleColor.DarkGray  , ConsoleColor.White     ));
                    Theme.Add("bg-warning"     , new Color(ConsoleColor.DarkYellow, ConsoleColor.White     ));
                    Theme.Add("bg-danger"      , new Color(ConsoleColor.DarkRed   , ConsoleColor.White     ));
                    break;
            }

            Theme.Add("text-success"   , new Color(null                   , ConsoleColor.DarkGreen ));
            Theme.Add("text-info"      , new Color(null                   , ConsoleColor.DarkCyan  ));
            Theme.Add("bg-success"     , new Color(ConsoleColor.DarkGreen , ConsoleColor.White     ));
            Theme.Add("bg-info"        , new Color(ConsoleColor.DarkCyan  , ConsoleColor.White     ));
        }

        public struct Color
        {
            public ConsoleColor background { get; private set; }
            public ConsoleColor foreground { get; private set; }

            public Color(ConsoleColor? _background, ConsoleColor? _foreground) : this()
            {

                switch (OS.GetCurrent())
                {
                    case "win":
                        background = _background ?? ConsoleColor.Black;
                        foreground = _foreground ?? ConsoleColor.White;
                        break;
                    case "mac":
                        background = _background ?? ConsoleColor.White;
                        foreground = _foreground ?? ConsoleColor.Black;
                        break;
                }
            }
        }

        public static void txtStatus   (this string s, Type? type = Type.Write, params bool[] values) { 
            bool status = true;
            foreach (var v in values)
            {
                status = status && v;
            }
            
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
                    WriteJustify(s);
                    break;
                case Type.Repeat:
                    WriteRepeat(s);
                    break;
                case Type.Shell:
                    WriteShell(s);
                    break;
                default:
                    Console.Write(s);
                    break;
            }
            
            Console.ResetColor();
        }

        private static void WriteJustify(string s)
        {
            int r = Console.WindowWidth / 2;
            int l = Console.WindowWidth - r;

            Console.Write(s.Split('|')[0].PadRight(r - 1));
            Console.WriteLine(s.Split('|')[1].PadLeft(l - 0));
        }

        private static void WriteRepeat(string s)
        {
            
            char c = ( String.IsNullOrEmpty(s) ? ' ' : s[0] );
            s = new String(c, Console.WindowWidth - 1);
            Console.WriteLine(s);
        }

        private static void WriteShell(string s)
        {
            StringBuilder line = new StringBuilder();
            string[] words = s.Split(' ');
            int chunkSize = (Console.WindowWidth - 3);
            foreach (var item in words)
            {
                WriteShellLine(ref line, item, chunkSize);
                WriteShellItem(ref line, item, chunkSize);
            }
            if (!String.IsNullOrEmpty(line.ToString().Trim()))
            {
                Console.WriteLine($" {line.ToString().Trim()}");
            }
        }

        private static void WriteShellLine(ref StringBuilder line, string item, int chunkSize)
        {
            if (
                ((line.Length + item.Length) >= chunkSize) || 
                (line.ToString().Contains(Environment.NewLine))
            )
            {
                Console.WriteLine($" {line.ToString().Trim()}");
                line.Clear();
            }
        }

        private static void WriteShellItem(ref StringBuilder line, string item, int chunkSize)
        {
            if ( item.Length >= chunkSize )
            {
                if (line.Length > 0){
                    Console.WriteLine($" {line.ToString().Trim()}");
                    line.Clear();
                }
                for (int i = 0; i < item.Length ; i += chunkSize)
                {
                    if (i + chunkSize > item.Length) chunkSize = item.Length  - i;
                    Console.WriteLine($" {item.Substring(i, chunkSize).Trim()}");
                    line.Clear();
                }
            } else {
                line.Append($"{item} ");
            }
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