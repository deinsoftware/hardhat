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
                opts.Add(new Option{opt="m"   , stt=true , act=Menu.Start                       });
                // Project
                opts.Add(new Option{opt="p"   , stt=true , act=Project.Select                   });
                opts.Add(new Option{opt="pf"  , stt=false, act=Project.File                     });
                opts.Add(new Option{opt="pi"  , stt=false, act=Adb.Install                      });
                opts.Add(new Option{opt="pd"  , stt=false, act=Project.Duplicate                });
                opts.Add(new Option{opt="pp"  , stt=false, act=Project.FilePath                 });
                opts.Add(new Option{opt="ps"  , stt=false, act=BuildTools.SignerVerify          });
                opts.Add(new Option{opt="pv"  , stt=false, act=BuildTools.Information           });
                // Version Control System
                opts.Add(new Option{opt="vd"  , stt=false, act=Vcs.Discard                      });
                opts.Add(new Option{opt="vp"  , stt=false, act=Vcs.Pull                         });
                opts.Add(new Option{opt="vr"  , stt=false, act=Vcs.Reset                        });
                opts.Add(new Option{opt="vd+p", stt=false, act=Vcs.DiscardPull                  });
                opts.Add(new Option{opt="vr+p", stt=false, act=Vcs.ResetPull                    });
                // Sonar
                opts.Add(new Option{opt="s"   , stt=true , act=Sonar.Select                     });
                opts.Add(new Option{opt="sq"  , stt=false, act=Sonar.Qube                       });
                opts.Add(new Option{opt="ss"  , stt=false, act=Sonar.Scanner                    });
                opts.Add(new Option{opt="sb"  , stt=false, act=Sonar.Browse                     });
                // Gulp
                opts.Add(new Option{opt="g"   , stt=false, act=Gulp.Select                      });
                opts.Add(new Option{opt="g>i" , stt=false, act=Gulp.InternalPath                });
                opts.Add(new Option{opt="g>d" , stt=false, act=Gulp.Dimension                   });
                opts.Add(new Option{opt="g>f" , stt=false, act=Gulp.Flavor                      });
                opts.Add(new Option{opt="g>n" , stt=false, act=Gulp.Number                      });
                opts.Add(new Option{opt="g>s" , stt=false, act=Gulp.Sync                        });
                opts.Add(new Option{opt="g>p" , stt=false, act=Gulp.Protocol                    });
                opts.Add(new Option{opt="gu"  , stt=false, act=Gulp.Uglify                      });
                opts.Add(new Option{opt="gr"  , stt=false, act=Gulp.Revert                      });
                opts.Add(new Option{opt="gs"  , stt=false, act=Gulp.Server                      });
                // Build
                opts.Add(new Option{opt="b"   , stt=false, act=Build.Select                     });
                opts.Add(new Option{opt="b>d" , stt=false, act=Build.Dimension                  });
                opts.Add(new Option{opt="b>f" , stt=false, act=Build.Flavor                     });
                opts.Add(new Option{opt="b>m" , stt=false, act=Build.Mode                       });
                opts.Add(new Option{opt="bp"  , stt=false, act=Build.Properties                 });
                opts.Add(new Option{opt="bc"  , stt=false, act=Build.Clean                      });
                opts.Add(new Option{opt="bg"  , stt=false, act=Build.Gradle                     });
                // Adb
                opts.Add(new Option{opt="ar"  , stt=true , act=Adb.Restart                      });
                opts.Add(new Option{opt="ad"  , stt=true , act=Adb.Devices                      });
                opts.Add(new Option{opt="aw"  , stt=true , act=Adb.Wireless                     });
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static bool Valid(string opt)
        {
            var response = false;
            try
            {
                var option = list.Where(x => x.opt == opt).FirstOrDefault();
                if (option != null)
                {
                    response = option.stt;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }

        public static void Valid(string opt, bool stt)
        {
            try
            {
                var option = list.Where(x => x.opt == opt).FirstOrDefault();
                if (option != null)
                {
                    option.stt = stt;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static Action Action(string opt, string dfl = "m")
        {
            Action response = Menu.Start;
            try
            {
                var option = list.Where(x => x.opt == opt).FirstOrDefault();
                if (option != null)
                {
                    response = option.act;
                } else {
                    Message.Critical();
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }
    }
}