using System;
using System.Collections.Generic;
using System.Linq;
using dein.tools;

namespace HardHat
{

    public class Option
    {
        public string opt { get; set; }
        public bool status { get; set; }
        public Action action { get; set; }
        public string variant { get; set; }
    }

    public static class Options
    {
        public static IEnumerable<Option> list { get; set; }

        static Options()
        {
            var opts = new List<Option>();
            try
            {
                // Main
                opts.Add(new Option { opt = "m", status = true, action = Menu.Start });

                // Views
                Project.List(ref opts);
                Vcs.List(ref opts);
                Sonar.List(ref opts);
                Gulp.List(ref opts);
                Build.List(ref opts);
                Adb.List(ref opts);
                Configuration.List(ref opts);
                Information.List(ref opts);

                // Extras
                opts.Add(new Option { opt = "x", status = true, action = Program.Exit });

                list = opts;
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static bool IsValid(string opt)
        {
            var response = false;
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    response = option.status;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return response;
        }

        public static void IsValid(string opt, bool status)
        {
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    option.status = status;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static Action GetAction(string opt, string defaultValue = "m")
        {
            Action response = Menu.Start;
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    response = option.action;
                }
                else
                {
                    Message.Error();
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return response;
        }

        public static string GetVariant(string opt)
        {
            string variant = "";
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    variant = option.variant;
                }
                else
                {
                    Message.Error();
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return variant;
        }
    }
}