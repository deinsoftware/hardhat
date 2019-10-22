using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToolBox.Platform;
using static HardHat.Program;

namespace HardHat
{
    static class Settings
    {
        public static void Save(MainConfig config)
        {
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText($"{_path.Combine("~", $".hardhat.config.json")}", json);
        }

        public static MainConfig Read()
        {
            MainConfig config = new MainConfig();

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
            config.project.iosPath = "iOS";
            config.project.androidPath = "android";
            config.project.androidBuildPath = "build/outputs/apk";
            config.project.androidBuildExtension = ".apk";
            config.project.androidMappingSuffix = "-mapping.txt";
            config.project.androidHybridPath = "assets/www";
            config.project.filterFiles = new string[] { ".js", ".css" };

            config.task = new TaskConfiguration();
            config.task.webFolder = "config/server";
            config.task.logFolder = "config/log";
            config.task.extension = ".json";

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
            config.personal.sonar.protocol = "http";
            config.personal.sonar.domain = "localhost";
            config.personal.sonar.port = "9000";
            config.personal.sonar.internalPath = "";
            config.personal.webServer = new WebConfiguration();
            config.personal.webServer.file = "";
            config.personal.webServer.flavor = "";
            config.personal.webServer.number = "";
            config.personal.webServer.sync = false;
            config.personal.webServer.internalPath = "web";
            config.personal.webServer.open = true;
            config.personal.testServer = new TestConfiguration();
            config.personal.testServer.sync = false;
            config.personal.testServer.coveragePath = "coverage/review";
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
            config.personal.menu.buildConfiguration = "";
            config.personal.theme = "";
            config.personal.log = false;

            if (!_fileSystem.FileExists($"{_path.Combine("~", $".hardhat.config.json")}"))
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

                config = JsonConvert.DeserializeObject<MainConfig>(oFile.ToString());
                return config;
            }
        }
    }
}