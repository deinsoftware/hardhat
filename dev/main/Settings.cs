using System.Collections.Generic;
using System.IO;
using dein.tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardHat
{
    static class Settings{
        public static void Save(Config config){
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText($"{Paths.Combine("~", $".hardhat.config.json")}", json);
        }

        public static Config Read(){
            Config config = new Config();

            config.window = new WindowConfiguration();
            config.window.height = 30;
            config.window.width = 86;

            config.path = new PathConfiguration();
            switch (Os.Platform())
            {
                case "win":
                    config.path.dir = "D:/Developer";
                    break;
                case "mac":
                    config.path.dir = "~/Developer";
                    break;
            }
            config.path.bsn = "";
            config.path.prj = "Projects";
            config.path.flt = "_d*";
    
            config.android = new AndroidConfiguration();
            config.android.prj = "android";
            config.android.bld = "build/outputs/apk";
            config.android.ext = ".apk";
            config.android.cmp = "assets/www";
            config.android.flt = new string[] {".js",".css"};

            config.gulp = new GulpConfiguration();
            config.gulp.srv = "server";
            config.gulp.ext = ".json";

            config.vpn = new VpnConfiguration();
            config.vpn.snm = "";

            config.personal = new PersonalConfiguration();
            config.personal.hst = "";
            config.personal.ipl = "";
            config.personal.ipb = "";
            config.personal.spr = "";
            config.personal.sfl = "";
            config.personal.snr = new SonarConfiguration();
            config.personal.snr.ptc = "http";
            config.personal.snr.dmn = "localhost";
            config.personal.snr.prt = "9000";
            config.personal.snr.ipt = "";
            config.personal.gbs = new ServerConfiguration();
            config.personal.gbs.dmn = "";
            config.personal.gbs.flv = "";
            config.personal.gbs.srv = "";
            config.personal.gbs.syn = false;
            config.personal.gbs.ptc = "http";
            config.personal.gbs.ipt = "";
            config.personal.gdl = new BuildConfiguration();
            config.personal.gdl.mde = "";
            config.personal.gdl.dmn = "";
            config.personal.gdl.flv = "";
            config.personal.adb = new AdbConfiguration();
            config.personal.adb.dvc = "";
            config.personal.adb.wip = "";
            config.personal.adb.wpr = "";
            config.personal.adb.wst = false;
            config.personal.mnu = new MenuConfiguration();
            config.personal.mnu.sel = "";
            config.personal.mnu.v_bnc = "";
            config.personal.mnu.s_cnf = "";
            config.personal.mnu.g_cnf = "";
            config.personal.mnu.b_cnf = "";
            
            if (!File.Exists($"{Paths.Combine("~", $".hardhat.config.json")}")) {
                return config;
            } else {
                string file = JsonConvert.SerializeObject(config);
                string json = File.ReadAllText($"{Paths.Combine("~", $".hardhat.config.json")}");

                JObject oFile = JObject.Parse(file);
                JObject oJson = JObject.Parse(json);

                oFile.Merge(oJson, new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Union
                });

                config = JsonConvert.DeserializeObject<Config>(oFile.ToString());
                return config;
            }
        }
    }

    class Config
    {
        public WindowConfiguration      window      { get; set; }
        public PathConfiguration        path        { get; set; }
        public AndroidConfiguration     android     { get; set; }
        public GulpConfiguration        gulp        { get; set; }
        public VpnConfiguration         vpn         { get; set; }
        public PersonalConfiguration    personal    { get; set; }
    }

    class WindowConfiguration 
    {
        public int      height  { get; set; }
        public int      width   { get; set; }
    }

    class PathConfiguration
    {
        public string   dir     { get; set; }               //Development
        public string   bsn     { get; set; }               //Bussiness Name
        public string   prj     { get; set; }               //Projects
        public string   flt     { get; set; }               //Filter
    }

    class AndroidConfiguration {
        public string   prj     { get; set; }               //Android Project Folder
        public string   bld     { get; set; }               //Build Path
        public string   ext     { get; set; }               //Build Extension
        public string   cmp     { get; set; }               //Path to process with Gulp
        public string[] flt     { get; set; }               //Filter files to Process
    }

    class GulpConfiguration {
        public string   srv     { get; set; }               //Server Folder
        public string   ext     { get; set; }               //Server Extension
    }

    class VpnConfiguration {
        public string   snm     { get; set; }               //Sitename
    }
    
    class PersonalConfiguration {
        public string               hst     { get; set; }   //Hostname
        public string               ipl     { get; set; }   //Local IP Address
        public string               ipb     { get; set; }   //Local IP Address base
        public string               spr     { get; set; }   //Selected Project
        public string               sfl     { get; set; }   //Selected File
        public SonarConfiguration   snr     { get; set; }   //Sonar
        public ServerConfiguration  gbs     { get; set; }   //Gradle Server
        public BuildConfiguration   gdl     { get; set; }   //Gradle Configuration
        public AdbConfiguration     adb     { get; set; }   //ADB Configuration
        public MenuConfiguration    mnu     { get; set; }   //Menu Configuration
    }

    class SonarConfiguration {
        public string   ptc     { get; set; }               //Protocol
        public string   dmn     { get; set; }               //Domain
        public string   prt     { get; set; }               //Port
        public string   ipt     { get; set; }               //Internal Path
    }

    class ServerConfiguration {
        public string   ptc     { get; set; }               //Protocol
        public string   ipt     { get; set; }               //Internal Path
        public string   dmn     { get; set; }               //Dimension
        public string   flv     { get; set; }               //Flavor
        public string   srv     { get; set; }               //Server
        public bool     syn     { get; set; }               //Sync
    }

    class BuildConfiguration {
        public string   mde     { get; set; }               //Mode
        public string   dmn     { get; set; }               //Dimension
        public string   flv     { get; set; }               //Flavor
    }

    class AdbConfiguration {
        public string   dvc     { get; set; }               //Device Name
        public string   wip     { get; set; }               //WiFi IP
        public string   wpr     { get; set; }               //WiFi Port
        public bool     wst     { get; set; }               //WiFi Status
    }

    public class MenuConfiguration
    {
        public string   sel { get; set; }                   //Option
        public string   v_bnc   { get; set; }               //Current Branch
        public string   s_cnf   { get; set; }               //Sonar Configuration
        public bool     s_val   { get; set; }               //Sonar Validation
        public string   g_cnf   { get; set; }               //Gulp Configuration
        public bool     g_val   { get; set; }               //Gulp Validation
        public string   b_cnf   { get; set; }               //Build Configuration
        public bool     b_val   { get; set; }               //Build Validation
    }
}