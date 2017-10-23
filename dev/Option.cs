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
        public string opt { get; set; }
        public bool stt { get; set; }
        public Action act { get; set; }
    }

    public class Options
    {
        public static IEnumerable<Option> list { get; set; }
        static Options()
        {
            var list = new List<Option>();
            try
            {
                list.Add(new Option{opt="m"   , stt=true , act=Menu.Start});
                list.Add(new Option{opt="p"   , stt=false, act=Project.Select});
                list.Add(new Option{opt="pf"  , stt=false, act=Project.File});
                list.Add(new Option{opt="pi"  , stt=false, act=Adb.Install});
                list.Add(new Option{opt="pd"  , stt=false, act=Project.Duplicate});
                list.Add(new Option{opt="pp"  , stt=false, act=Project.FilePath});
                list.Add(new Option{opt="ps"  , stt=false, act=BuildTools.SignerVerify});
                list.Add(new Option{opt="pv"  , stt=false, act=BuildTools.Information});
                list.Add(new Option{opt="vd"  , stt=false, act=Vcs.Discard});
                list.Add(new Option{opt="vp"  , stt=false, act=Vcs.Pull});
                list.Add(new Option{opt="vr"  , stt=false, act=Vcs.Reset});
                list.Add(new Option{opt="vd+p", stt=false, act=Vcs.DiscardPull});
                list.Add(new Option{opt="vr+p", stt=false, act=Vcs.ResetPull});
                list.Add(new Option{opt="g"   , stt=false, act=Gulp.Select});
                list.Add(new Option{opt="g>i" , stt=false, act=Gulp.InternalPath});
                list.Add(new Option{opt="g>d" , stt=false, act=Gulp.Dimension});
                list.Add(new Option{opt="g>f" , stt=false, act=Gulp.Flavor});
                list.Add(new Option{opt="g>n" , stt=false, act=Gulp.Number});
                list.Add(new Option{opt="g>s" , stt=false, act=Gulp.Sync});
                list.Add(new Option{opt="g>p" , stt=false, act=Gulp.Protocol});
                list.Add(new Option{opt="gu"  , stt=false, act=Gulp.Uglify});
                list.Add(new Option{opt="gr"  , stt=false, act=Gulp.Revert});
                list.Add(new Option{opt="gs"  , stt=false, act=Gulp.Server});
                list.Add(new Option{opt="b"   , stt=false, act=Build.Select});
                list.Add(new Option{opt="b>d" , stt=false, act=Build.Dimension});
                list.Add(new Option{opt="b>f" , stt=false, act=Build.Flavor});
                list.Add(new Option{opt="b>m" , stt=false, act=Build.Mode});
                list.Add(new Option{opt="bp"  , stt=false, act=Build.Properties});
                list.Add(new Option{opt="bc"  , stt=false, act=Build.Clean});
                list.Add(new Option{opt="bg"  , stt=false, act=Build.Gradle});
                list.Add(new Option{opt="c"   , stt=true , act=Configuration.Select});
                list.Add(new Option{opt="c>pd", stt=true , act=Configuration.PathDevelopment});
                list.Add(new Option{opt="c>pb", stt=true , act=Configuration.PathBusiness});
                list.Add(new Option{opt="c>pp", stt=true , act=Configuration.PathProjects});
                list.Add(new Option{opt="c>pf", stt=true , act=Configuration.PathFilter});
                list.Add(new Option{opt="c>ap", stt=true , act=Configuration.AndroidProject});
                list.Add(new Option{opt="c>ab", stt=true , act=Configuration.AndroidBuild});
                list.Add(new Option{opt="c>ae", stt=true , act=Configuration.AndroidExtension});
                list.Add(new Option{opt="c>ac", stt=true , act=Configuration.AndroidCompact});
                list.Add(new Option{opt="c>af", stt=true , act=Configuration.AndroidFilter});
                list.Add(new Option{opt="c>gs", stt=true , act=Configuration.GulpServer});
                list.Add(new Option{opt="c>ge", stt=true , act=Configuration.GulpExtension});
                list.Add(new Option{opt="c>vs", stt=true , act=Configuration.SiteName});
                list.Add(new Option{opt="ar"  , stt=true , act=Adb.Restart});
                list.Add(new Option{opt="ad"  , stt=true , act=Adb.Devices});
                list.Add(new Option{opt="aw"  , stt=true , act=Adb.Wireless});
                list.Add(new Option{opt="i"   , stt=true , act=Information.Versions});
                list.Add(new Option{opt="e"   , stt=true , act=Information.Environment});
                list.Add(new Option{opt="x"   , stt=true , act=Program.Exit});

                Options.list = list;
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