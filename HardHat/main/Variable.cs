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
                opts.Add(new Variable { opt = "jh", name = "JAVA_HOME", verified = false, status = false, value = "" });
                // Android
                opts.Add(new Variable { opt = "ah", name = "ANDROID_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "an", name = "ANDROID_NDK_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "ab", name = "ANDROID_BT_VERSION", verified = false, status = false, value = "" });
                // Project
                opts.Add(new Variable { opt = "sh", name = "SIGCHECK_HOME", verified = false, status = false, value = "" });
                // Sonar
                opts.Add(new Variable { opt = "sq", name = "SONAR_QUBE_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "ss", name = "SONAR_SCANNER_HOME", verified = false, status = false, value = "" });
                // VCS
                opts.Add(new Variable { opt = "gh", name = "GIT_HOME", verified = false, status = false, value = "" });
                // Gulp
                opts.Add(new Variable { opt = "gp", name = "GULP_PROJECT", verified = false, status = false, value = "" });
                // Build
                opts.Add(new Variable { opt = "bh", name = "GRADLE_HOME", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "bp", name = "ANDROID_PROPERTIES", verified = false, status = false, value = "" });
                opts.Add(new Variable { opt = "bv", name = "VPN_HOME", verified = false, status = false, value = "" });

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