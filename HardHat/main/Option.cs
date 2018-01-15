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
                
                // Config
                opts.Add(new Option{opt="c"   , stt=true , act=Configuration.Select             });
                opts.Add(new Option{opt="c>pd", stt=true , act=Configuration.PathDevelopment    });
                opts.Add(new Option{opt="c>pb", stt=true , act=Configuration.PathBusiness       });
                opts.Add(new Option{opt="c>pp", stt=true , act=Configuration.PathProjects       });
                opts.Add(new Option{opt="c>pf", stt=true , act=Configuration.PathFilter         });
                opts.Add(new Option{opt="c>ap", stt=true , act=Configuration.AndroidProject     });
                opts.Add(new Option{opt="c>ab", stt=true , act=Configuration.AndroidBuild       });
                opts.Add(new Option{opt="c>ae", stt=true , act=Configuration.AndroidExtension   });
                opts.Add(new Option{opt="c>ac", stt=true , act=Configuration.AndroidCompact     });
                opts.Add(new Option{opt="c>af", stt=true , act=Configuration.AndroidFilter      });
                opts.Add(new Option{opt="c>gs", stt=true , act=Configuration.GulpServer         });
                opts.Add(new Option{opt="c>ge", stt=true , act=Configuration.GulpExtension      });
                opts.Add(new Option{opt="c>vs", stt=true , act=Configuration.SiteName           });
                
                // Extras
                opts.Add(new Option{opt="i"   , stt=true , act=Information.Versions             });
                opts.Add(new Option{opt="e"   , stt=true , act=Information.Environment          });
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