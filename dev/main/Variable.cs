using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using dein.tools;

namespace HardHat {
    
    public class Variable {
        public string   opt { get; set; }       //Option
        public string   nme { get; set; }       //Name
        public bool     chk { get; set; }       //Checked
        public bool     stt { get; set; }       //Status
        public string   vlu { get; set; }       //Value
    }

    public static class Variables
    {
        public static IEnumerable<Variable> list { get; set; }
        
        static Variables()
        {
            var opts = new List<Variable>();
            try
            {
                // Java
                opts.Add(new Variable{opt="jh", nme="JAVA_HOME",          chk=false, stt=false, vlu=""});
                // Android
                opts.Add(new Variable{opt="ah", nme="ANDROID_HOME",       chk=false, stt=false, vlu=""});
                opts.Add(new Variable{opt="an", nme="ANDROID_NDK_HOME",   chk=false, stt=false, vlu=""});
                opts.Add(new Variable{opt="ab", nme="ANDROID_BT_VERSION", chk=false, stt=false, vlu=""});
                // Project
                opts.Add(new Variable{opt="sh", nme="SIGCHECK_HOME",      chk=false, stt=false, vlu=""});
                // Sonar
                opts.Add(new Variable{opt="sl", nme="SONAR_LINT_HOME",    chk=false, stt=false, vlu=""});
                opts.Add(new Variable{opt="sq", nme="SONAR_QUBE_HOME",    chk=false, stt=false, vlu=""});
                opts.Add(new Variable{opt="ss", nme="SONAR_SCANNER_HOME", chk=false, stt=false, vlu=""});
                // VCS
                opts.Add(new Variable{opt="gh", nme="GIT_HOME",           chk=false, stt=false, vlu=""});
                // Gulp
                opts.Add(new Variable{opt="gp", nme="GULP_PROJECT",       chk=false, stt=false, vlu=""});
                // Build
                opts.Add(new Variable{opt="bh", nme="GRADLE_HOME",        chk=false, stt=false, vlu=""});
                opts.Add(new Variable{opt="bp", nme="ANDROID_PROPERTIES", chk=false, stt=false, vlu=""});
                opts.Add(new Variable{opt="bv", nme="VPN_HOME",           chk=false, stt=false, vlu=""});

                list = opts;
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Update(){
            try
            {
                Parallel.ForEach(list, v =>
                {
                    Valid(v.opt, true);
                });
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static bool Valid(string opt, bool agn = false)
        {
            var response = false;
            try
            {
                var option = list.Where(x => x.opt == opt).FirstOrDefault();
                if (option != null)
                {
                    if (agn || !option.chk){
                        Check(option);
                    }
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

        public static void Check(Variable opt)
        {
            try
            {
                opt.stt = Env.Check(opt.nme);
                if (opt.stt)
                {
                    opt.vlu = Env.Get(opt.nme);
                } else {
                    opt.vlu = "";
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static string Value(string opt)
        {
            var response = "";
            try
            {
                var option = list.Where(x => x.opt == opt).FirstOrDefault();
                if (option != null)
                {
                    if (!option.chk)
                    {
                        Check(option);
                    }
                    response = option.vlu;
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }
    }
}