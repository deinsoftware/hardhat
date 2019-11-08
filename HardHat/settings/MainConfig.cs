namespace HardHat
{
    class MainConfig
    {
        public About about { get; set; }
        public PathConfiguration path { get; set; }
        public ProjectConfiguration project { get; set; }
        public TaskConfiguration task { get; set; }
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

    class TaskConfiguration
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
        public TestConfiguration testServer { get; set; }
        public BuildConfiguration build { get; set; }
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
        public string internalPath { get; set; }
        public string file { get; set; }
        public string flavor { get; set; }
        public string number { get; set; }
        public bool sync { get; set; }
        public bool open { get; set; }
    }

    public class TestConfiguration
    {
        public bool sync { get; set; }
        public string coveragePath { get; set; }
    }

    class BuildConfiguration
    {
        public string type { get; set; }
        public string dimension { get; set; }
        public string flavor { get; set; }
        public string mode { get; set; }
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
        public string buildConfiguration { get; set; }
        public bool buildValidation { get; set; }
    }
}