using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

namespace HardHat {
    
    public class Option {
        public string   opt { get; set; }       //Key Combination
        public bool     stt { get; set; }       //Status (Dis/Enable)
        public Action   act { get; set; }       //Class.Method
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
                opts.Add(new Option{opt="m"   , stt=true , act=Menu.Start                       });

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
                opts.Add(new Option{opt="x"   , stt=true , act=Program.Exit                     });

                list = opts;
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static bool Valid(string opt)
        {
            var response = false;
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    response = option.stt;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }

        public static void Valid(string opt, bool stt)
        {
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    option.stt = stt;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static Action Action(string opt, string dfl = "m")
        {
            Action response = Menu.Start;
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    response = option.act;
                } else {
                    Message.Error();
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }
    }
}