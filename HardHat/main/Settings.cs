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

            config.about = new About();
            config.about.url = "github.com";
            config.about.user = "deinsoftware";
            config.about.name = "hardhat";
            config.about.content = "blob/master";
            config.about.readme = "README.md";
            config.about.changelog = "CHANGELOG.md";

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

            config.project = new ProjectConfiguration();
            config.project.iosPath = "ios";
            config.project.androidPath = "android";
            config.project.androidBuildPath = "build/outputs/apk";
            config.project.androidBuildExtension = ".apk";
            config.project.androidMappingSuffix = "-mapping.txt";
            config.project.androidHybridPath = "assets/www";
            config.project.filterFiles = new string[] { ".js", ".css" };

            config.gulp = new GulpConfiguration();
            config.gulp.webFolder = "server";
            config.gulp.logFolder = "ssh";
            config.gulp.extension = ".json";

            config.editor = new EditorConfiguration();
            config.editor.selected = "c";

            config.vpn = new VpnConfiguration();
            config.vpn.siteName = "";

            config.personal = new PersonalConfiguration();
            config.personal.hostName = "";
            config.personal.ipAddress = "";
            config.personal.ipAddressBase = "";
            config.personal.selected = new SelectedConfiguration();
            config.personal.selected.project = "";
            config.personal.selected.packageName = "";
            config.personal.selected.versionCode = "";
            config.personal.selected.versionName = "";
            config.personal.selected.path = "";
            config.personal.selected.file = "";
            config.personal.selected.mapping = "";
            config.personal.selected.mappingStatus = false;
            config.personal.sonar = new SonarConfiguration();
            config.personal.sonar.protocol = "https";
            config.personal.sonar.domain = "localhost";
            config.personal.sonar.port = "9000";
            config.personal.sonar.internalPath = "";
            config.personal.webServer = new WebConfiguration();
            config.personal.webServer.file = "";
            config.personal.webServer.flavor = "";
            config.personal.webServer.number = "";
            config.personal.webServer.sync = false;
            config.personal.webServer.protocol = "http";
            config.personal.webServer.internalPath = "";
            config.personal.webServer.open = true;
            config.personal.ftpServer = new FtpConfiguration();
            config.personal.ftpServer.host = "";
            config.personal.ftpServer.port = 22;
            config.personal.ftpServer.authenticationPath = "../FTP/.ftppass";
            config.personal.ftpServer.authenticationKey = "keyMain";
            config.personal.ftpServer.remotePath = "/";
            config.personal.ftpServer.dimension = "";
            config.personal.ftpServer.resourcePath = "";
            config.personal.gradle = new BuildConfiguration();
            config.personal.gradle.mode = "";
            config.personal.gradle.dimension = "";
            config.personal.gradle.flavor = "";
            config.personal.adb = new AdbConfiguration();
            config.personal.adb.deviceName = "";
            config.personal.adb.wifiIpAddress = "";
            config.personal.adb.wifiPort = "";
            config.personal.adb.wifiStatus = false;
            config.personal.logcat = new LogcatConfiguration();
            config.personal.logcat.priority = "V";
            config.personal.logcat.filter = "";
            config.personal.menu = new MenuConfiguration();
            config.personal.menu.selectedOption = "";
            config.personal.menu.selectedVariant = "";
            config.personal.menu.currentBranch = "";
            config.personal.menu.sonarConfiguration = "";
            config.personal.menu.serverConfiguration = "";
            config.personal.menu.serverValidation = false;
            config.personal.menu.ftpConfiguration = "";
            config.personal.menu.ftpValidation = false;
            config.personal.menu.logConfiguration = "";
            config.personal.menu.logValidation = false;
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
        public About about { get; set; }
        public PathConfiguration path { get; set; }
        public ProjectConfiguration project { get; set; }
        public GulpConfiguration gulp { get; set; }
        public EditorConfiguration editor { get; set; }
        public VpnConfiguration vpn { get; set; }
        public PersonalConfiguration personal { get; set; }
    }

    class About
    {
        public string url { get; set; }
        public string user { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string readme { get; set; }
        public string changelog { get; set; }
    }

    class PathConfiguration
    {
        public string development { get; set; }
        public string workspace { get; set; }
        public string project { get; set; }
        public string filter { get; set; }
    }

    class ProjectConfiguration
    {
        public string iosPath { get; set; }
        public string androidPath { get; set; }
        public string androidBuildPath { get; set; }
        public string androidBuildExtension { get; set; }
        public string androidMappingSuffix { get; set; }
        public string androidHybridPath { get; set; }
        public string[] filterFiles { get; set; }
    }

    class GulpConfiguration
    {
        public string webFolder { get; set; }
        public string logFolder { get; set; }
        public string extension { get; set; }
    }

    class EditorConfiguration
    {
        public string selected { get; set; }
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
        public SelectedConfiguration selected { get; set; }
        public SonarConfiguration sonar { get; set; }
        public WebConfiguration webServer { get; set; }
        public FtpConfiguration ftpServer { get; set; }
        public BuildConfiguration gradle { get; set; }
        public AdbConfiguration adb { get; set; }
        public LogcatConfiguration logcat { get; set; }
        public MenuConfiguration menu { get; set; }
        public string theme { get; set; }
        public bool log { get; set; }
    }

    class SelectedConfiguration
    {
        public string project { get; set; }
        public string path { get; set; }
        public string file { get; set; }
        public string packageName { get; set; }
        public string versionCode { get; set; }
        public string versionName { get; set; }
        public string mapping { get; set; }
        public bool mappingStatus { get; set; }
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
        public string file { get; set; }
        public string flavor { get; set; }
        public string number { get; set; }
        public bool sync { get; set; }
        public bool open { get; set; }
    }

    public class FtpConfiguration
    {
        public string host { get; set; }
        public int port { get; set; }
        public string authenticationPath { get; set; }
        public string authenticationKey { get; set; }
        public string remotePath { get; set; }
        public string dimension { get; set; }
        public string resourcePath { get; set; }
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

    public class LogcatConfiguration
    {
        public string priority { get; set; }
        public string filter { get; set; }
    }

    public class MenuConfiguration
    {
        public string selectedOption { get; set; }
        public string selectedVariant { get; set; }
        public string currentBranch { get; set; }
        public string sonarConfiguration { get; set; }
        public bool sonarValidation { get; set; }
        public string serverConfiguration { get; set; }
        public bool serverValidation { get; set; }
        public string ftpConfiguration { get; set; }
        public bool ftpValidation { get; set; }
        public string logConfiguration { get; set; }
        public bool logValidation { get; set; }
        public string buildConfiguration { get; set; }
        public bool buildValidation { get; set; }
    }
}