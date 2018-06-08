using System;
using System.Net;
using System.Net.Sockets;
using Colorify;
using static Colorify.Colors;
using HardHat;
using static HardHat.Program;

namespace dein.tools
{
    public static class Message
    {
        public static void Critical(string msg = null)
        {
            Error(msg, !String.IsNullOrEmpty(msg));
        }

        public static void Error(string msg = null, bool replace = false, bool exit = false)
        {
            _colorify.Clear();
            try
            {
                _colorify.DivisionLine('=', bgDanger);
                _colorify.AlignLeft(" ERROR", bgDanger);
                _colorify.DivisionLine('=', bgDanger);
                _colorify.BlankLines();

                if (!replace)
                {
                    _colorify.WriteLine(" Invalid option, please try again.");
                }
                if (!String.IsNullOrEmpty(msg))
                {
                    _colorify.Wrap($" {msg}", txtDefault);
                }

                _colorify.BlankLines();
                _colorify.DivisionLine('=', bgDanger);
                _colorify.BlankLines();

                _colorify.Write(" Press [Any] key to continue...", txtDanger);
                Console.ReadKey();

                if (!exit)
                {
                    Menu.Route();
                }
                else
                {
                    Program.Exit();
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static void Alert(string msg, bool exit = false)
        {
            _colorify.Clear();
            try
            {
                _colorify.DivisionLine('=', bgWarning);
                _colorify.AlignLeft(" WARNING", bgWarning);
                _colorify.DivisionLine('=', bgWarning);
                _colorify.BlankLines();

                _colorify.Wrap($" {msg}", txtDefault);

                _colorify.BlankLines();
                _colorify.DivisionLine('=', bgWarning);
                _colorify.BlankLines();

                _colorify.Write($" Press [Any] key to continue...", txtWarning);
                Console.ReadKey();

                if (exit)
                {
                    Program.Exit();
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static bool Confirmation(string msg)
        {
            _colorify.Clear();

            bool sel = false;
            try
            {
                _colorify.DivisionLine('=', bgWarning);
                _colorify.AlignLeft(" WARNING", bgWarning);
                _colorify.DivisionLine('=', bgWarning);
                _colorify.BlankLines();

                _colorify.Wrap($" {msg}", txtDefault);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                _colorify.BlankLines();
                _colorify.DivisionLine('=', bgWarning);
                _colorify.BlankLines();

                _colorify.Write($" [Y] Yes or [N] No: ", txtWarning);

                string opt = Console.ReadLine()?.ToLower();

                switch (opt)
                {
                    case "y":
                    case "n":
                        sel = (opt == "y");
                        break;
                    case "":
                        break;
                    default:
                        Message.Error();
                        break;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
            return sel;
        }
    }
}