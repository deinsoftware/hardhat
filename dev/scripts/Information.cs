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
            $"{" Sonar Scanner", -25}".txtPrimary();    Version.CmdSonarScanner();
            
            $"".fmNewLine();
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            $" Press [Any] key to continue...".txtInfo();
            Console.ReadKey();

            Menu.Start();
        }

        private static readonly Dictionary<string, string> Variables = new Dictionary<string, string>
        {
            {"ANDROID_SDK"          , "ANDROID_HOME"        },
            {"ANDROID_NDK"          , "ANDROID_NDK_HOME"    },
            {"ANDROID_BUILDTOOL"    , "ANDROID_BT_VERSION"  },
            {"ANDROID_TEMPLATE"     , "ANDROID_TEMPLATE"    },
            {"JAVA"                 , "JAVA_HOME"           },
            {"GIT"                  , "GIT_HOME"            },
            {"GRADLE"               , "GRADLE_HOME"         },
            {"GULP"                 , "GULP_PROJECT"        },
            {"SONAR_LINT"           , "SONAR_LINT_HOME"     },
            {"SONAR_QUBE"           , "SONAR_QUBE_HOME"     },
            {"SONAR_SCANNER"        , "SONAR_SCANNER_HOME"  },
            {"VPN"                  , "VPN_HOME"            }
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