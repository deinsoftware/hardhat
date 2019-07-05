using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dein.tools;
using ToolBox.System;

namespace HardHat
{

    public class Variable
    {
        public string opt { get; set; }
        public string name { get; set; }
        public bool verified { get; set; }
        public bool status { get; set; }
        public string value { get; set; }
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
                opts.Add(new Variable { opt = "java", name = "JAVA_HOME", verified = false, status = false, value = "" });
                // Android
                opts.Add(new Variable { opt = "android_root", name = "ANDROID_SDK_ROOT", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "android_home", name = "ANDROID_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "android_ndk", name = "ANDROID_NDK_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "android_buildtools", name = "ANDROID_BT_VERSION", verified = false, status = false, value = "" });
                // Project
                opts.Add(new Variable { opt = "signcheck", name = "SIGCHECK_HOME", verified = false, status = false, value = "" });
                // Git
                opts.Add(new Variable { opt = "git", name = "GIT_HOME", verified = false, status = false, value = "" });
                // Task
                opts.Add(new Variable { opt = "task_project", name = "TASK_PROJECT", verified = false, status = false, value = "" });
                // Build
                opts.Add(new Variable { opt = "gradle", name = "GRADLE_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "android_properties", name = "ANDROID_PROPERTIES", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "vpn", name = "VPN_HOME", verified = false, status = false, value = "" });
                // Sonar
                opts.Add(new Variable { opt = "sonar_qube", name = "SONAR_QUBE_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "sonar_scanner", name = "SONAR_SCANNER_HOME", verified = false, status = false, value = "" });

                list = opts;
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Update()
        {
            try
            {
                Parallel.ForEach(list, v =>
                {
                    Valid(v.opt, true);
                });
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static bool Valid(string opt, bool verified = false)
        {
            var response = false;
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    if (verified || !option.verified)
                    {
                        Check(option);
                    }
                    response = option.status;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return response;
        }

        public static void Check(Variable opt)
        {
            try
            {
                opt.status = !Env.IsNullOrEmpty(opt.name);
                if (opt.status)
                {
                    opt.value = Env.GetValue(opt.name);
                }
                else
                {
                    opt.value = "";
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static string Value(string opt)
        {
            var response = "";
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    if (!option.verified)
                    {
                        Check(option);
                    }
                    response = option.value;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return response;
        }

        public static void Value(string opt, string value)
        {
            try
            {
                var option = list.FirstOrDefault(x => x.opt == opt);
                if (option != null)
                {
                    Env.SetValue(option.name, value);
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Upgrade()
        {
            try
            {
                BuildTools.Upgrade();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}