using Newtonsoft.Json;

namespace LiveWriterPluginManager.Model
{
    public class Manifest
    {
        internal const string ManifestFileName = "manifest.json";
        public string Name { get; set; }
        public string Author { get; set; }
        public string ProjectUrl { get; set; }
        public string TermsUrl { get; set; }
        public string Version { get; set; }
        public string TargetWriterVersion { get; set; }
        public string PluginFileName { get; set; }
        [JsonIgnore]
        public string PluginPath { get; set; }
    }
}
