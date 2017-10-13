using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using dein.tools;
using static dein.tools.Machine;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public static class Information {
        public static void Versions() {
            Colorify.Default();
            Console.Clear();

            $"=".bgInfo(ct.Repeat);
            $" COMMANDS".bgInfo(ct.PadLeft);
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();
            
            $" Required".txtInfo(ct.WriteLine);
            $"{" Gradle", -25}".txtPrimary();           Version.CmdGradle();
            $"{" Gulp", -25}".txtPrimary();             Version.CmdGulp();
            $"{" Java", -25}".txtPrimary();             Version.CmdJava();
            $"{" Node", -25}".txtPrimary();             Version.CmdNode();
            $"{" NPM", -25}".txtPrimary();              Version.CmdNPM();

            $"".fmNewLine();
            $" Optional".txtInfo(ct.WriteLine);
            $"{" Cordova", -25}".txtPrimary();          Version.CmdCordova();
            $"{" GIT", -25}".txtPrimary();              Version.CmdGit();
            $"{" NativeScript", -25}".txtPrimary();     Version.CmdNativescript();
            $"{" TypeScript", -25}".txtPrimary();       Version.CmdTypescript();
            $"{" SonarLint", -25}".txtPrimary();        Version.CmdSonarLint();
            $"{" SonarScanner", -25}".txtPrimary();     Version.CmdSonarScanner();
            
            $"".fmNewLine();
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            $" Press [Any] key to continue...".txtInfo();
            Console.ReadKey();

            Menu.Start();
        }

        private static readonly Dictionary<string, string> Variables = new Dictionary<string, string>
        {
            {"AndroidSDK"       , "ANDROID_HOME"        },
            {"AndroidNDK"       , "ANDROID_NDK_HOME"    },
            {"AndroidBuildTool" , "ANDROID_BT_VERSION"  },
            {"AndroidTemplate"  , "ANDROID_PROPERTIES"  },
            {"Java"             , "JAVA_HOME"           },
            {"Git"              , "GIT_HOME"            },
            {"Gradle"           , "GRADLE_HOME"         },
            {"Gulp"             , "GULP_PROJECT"        },
            {"Sigcheck"         , "SIGCHECK_HOME"       },
            {"SonarLint"        , "SONAR_LINT_HOME"     },
            {"SonarQube"        , "SONAR_QUBE_HOME"     },
            {"SonarPort"        , "SONAR_QUBE_PORT"     },
            {"SonarScanner"     , "SONAR_SCANNER_HOME"  },
            {"Vpn"              , "VPN_HOME"            }
        };

        public static void Environment() {
            Colorify.Default();
            Console.Clear();

            $"=".bgInfo(ct.Repeat);
            $" ENVIRONMENT VARIABLES".bgInfo(ct.PadLeft);
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            foreach (var variable in Variables)
            {
                $"{$" {variable.Value}:", -25}".txtPrimary();
                Env.Status(variable.Value);
            }

            $"".fmNewLine();
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            $" Press [Any] key to continue...".txtInfo();
            Console.ReadKey();

            Menu.Start();
        }
    }
}