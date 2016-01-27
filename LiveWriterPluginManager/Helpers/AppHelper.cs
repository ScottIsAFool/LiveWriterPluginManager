using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LiveWriterPluginManager.Helpers
{
    public static class AppHelper
    {
        static AppHelper()
        {
            LocalAppDataFolder = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "OpenLiveWriter");
            PluginsFolder = Path.Combine(LocalAppDataFolder, "Plugins");
        }

        public static string LocalAppDataFolder { get; }
        public static string PluginsFolder { get; }

        public static bool LiveWriterInstalled => Directory.Exists(LocalAppDataFolder);

        public static void CreatePluginDirectory()
        {
            if (!LiveWriterInstalled)
            {
                return;
            }

            if (!Directory.Exists(PluginsFolder))
            {
                Directory.CreateDirectory(PluginsFolder);
            }
        }

        public static bool IsLiveWriterRunning()
        {
            if (!LiveWriterInstalled)
            {
                return false;
            }

            var process = Process.GetProcessesByName("OpenLiveWriter");
            return process.Any();
        }
    }
}
