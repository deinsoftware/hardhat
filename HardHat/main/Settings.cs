using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToolBox.Platform;
using static HardHat.Program;

namespace HardHat
{
    static class Settings
    {
        public static void Save(Config config)
        {
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText($"{_path.Combine("~", $".hardhat.config.json")}", json);
        }

        public static Config Read()
        {
            Config config = new Config();

            config.path = new PathConfiguration();
            switch (OS.GetCurrent())
            {
                case "win":
                    config.path.development = "D:/Developer";
                    break;
                case "mac":
                    config.path.development = "~/Developer";
                    break;
            }
            config.path.workspace = "";
            config.path.project = "Projects";
            config.path.filter = "_d*";

            config.android = new AndroidConfiguration();
            config.android.projectPath = "android";
            config.android.buildPath = "build/outputs/apk";
            config.android.buildExtension = ".apk";
            config.android.hybridFiles = "assets/www";
            config.android.filterFiles = new string[] { ".js", ".css" };

            config.gulp = new GulpConfiguration();
            config.gulp.webFolder = "server";
            config.gulp.logFolder = "ssh";
            config.gulp.extension = ".json";

            config.vpn = new VpnConfiguration();
            config.vpn.siteName = "";

            config.personal = new PersonalConfiguration();
            config.personal.hostName = "";
            config.personal.ipAddress = "";
            config.personal.ipAddressBase = "";
            config.personal.selectedProject = "";
            config.personal.selectedFile = "";
            config.personal.sonar = new SonarConfiguration();
            config.personal.sonar.protocol = "https";
            config.personal.sonar.domain = "localhost";
            config.personal.sonar.port = "9000";
            config.personal.sonar.internalPath = "";
            config.personal.webServer = new WebConfiguration();
            config.personal.webServer.dimension = "";
            config.personal.webServer.flavor = "";
            config.personal.webServer.number = "";
            config.personal.webServer.sync = false;
            config.personal.webServer.protocol = "http";
            config.personal.webServer.internalPath = "";
            config.personal.webServer.open = true;
            config.personal.gradle = new BuildConfiguration();
            config.personal.gradle.mode = "";
            config.personal.gradle.dimension = "";
            config.personal.gradle.flavor = "";
            config.personal.adb = new AdbConfiguration();
            config.personal.adb.deviceName = "";
            config.personal.adb.wifiIpAddress = "";
            config.personal.adb.wifiPort = "";
            config.personal.adb.wifiStatus = false;
            config.personal.menu = new MenuConfiguration();
            config.personal.menu.selectedOption = "";
            config.personal.menu.currentBranch = "";
            config.personal.menu.sonarConfiguration = "";
            config.personal.menu.gulpConfiguration = "";
            config.personal.menu.buildConfiguration = "";
            config.personal.theme = "";
            config.personal.log = false;

            if (!File.Exists($"{_path.Combine("~", $".hardhat.config.json")}"))
            {
                return config;
            }
            else
            {
                string file = JsonConvert.SerializeObject(config);
                string json = File.ReadAllText($"{_path.Combine("~", $".hardhat.config.json")}");

                if (string.IsNullOrEmpty(json))
                {
                    return config;
                }

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
        public PathConfiguration path { get; set; }
        public AndroidConfiguration android { get; set; }
        public GulpConfiguration gulp { get; set; }
        public VpnConfiguration vpn { get; set; }
        public PersonalConfiguration personal { get; set; }
    }

    class PathConfiguration
    {
        public string development { get; set; }
        public string workspace { get; set; }
        public string project { get; set; }
        public string filter { get; set; }
    }

    class AndroidConfiguration
    {
        public string projectPath { get; set; }
        public string buildPath { get; set; }
        public string buildExtension { get; set; }
        public string hybridFiles { get; set; }
        public string[] filterFiles { get; set; }
    }

    class GulpConfiguration
    {
        public string webFolder { get; set; }
        public string logFolder { get; set; }
        public string extension { get; set; }
    }

    class VpnConfiguration
    {
        public string siteName { get; set; }
    }

    class PersonalConfiguration
    {
        public string hostName { get; set; }
        public string ipAddress { get; set; }
        public string ipAddressBase { get; set; }
        public string selectedProject { get; set; }
        public string selectedPath { get; set; }
        public string selectedFile { get; set; }
        public SonarConfiguration sonar { get; set; }
        public WebConfiguration webServer { get; set; }
        public BuildConfiguration gradle { get; set; }
        public AdbConfiguration adb { get; set; }
        public MenuConfiguration menu { get; set; }
        public string theme { get; set; }
        public bool log { get; set; }
    }

    class SonarConfiguration
    {
        public string protocol { get; set; }
        public string domain { get; set; }
        public string port { get; set; }
        public string internalPath { get; set; }
    }

    public class WebConfiguration
    {
        public string protocol { get; set; }
        public string internalPath { get; set; }
        public string dimension { get; set; }
        public string flavor { get; set; }
        public string number { get; set; }
        public bool sync { get; set; }
        public bool open { get; set; }
    }

    class BuildConfiguration
    {
        public string mode { get; set; }
        public string dimension { get; set; }
        public string flavor { get; set; }
    }

    class AdbConfiguration
    {
        public string deviceName { get; set; }
        public string wifiIpAddress { get; set; }
        public string wifiPort { get; set; }
        public bool wifiStatus { get; set; }
    }

    public class MenuConfiguration
    {
        public string selectedOption { get; set; }
        public string currentBranch { get; set; }
        public string sonarConfiguration { get; set; }
        public bool sonarValidation { get; set; }
        public string gulpConfiguration { get; set; }
        public bool gulpValidation { get; set; }
        public string buildConfiguration { get; set; }
        public bool buildValidation { get; set; }
    }
}