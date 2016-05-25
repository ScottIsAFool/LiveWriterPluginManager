using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Squirrel;

namespace LiveWriterPluginManager.Helpers
{
    public static class AppHelper
    {
        public const string SetPluginFileMsg = "SetPluginFileMsg";
        public const string RemovePluginMsg = "RemovePluginMsg";
        public const string RemoveFileMsg = "RemoveFileMsg";

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

        public static bool IsRunningAsAdmin()
        {
            var myDomain = Thread.GetDomain();

            myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            var myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
            return myPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void CheckForUpdates()
        {
            Task.Run(async () =>
            {
                const string updatePath = "https://github.com/ScottIsAFool/LiveWriterPluginManager";
                if (!string.IsNullOrEmpty(updatePath))
                {
                    using (var mgr = UpdateManager.GitHubUpdateManager(updatePath))
                    {
                        await mgr.Result.UpdateApp();
                    }
                }
            });
        }

        public static Version GetAppVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            return version;
        }
    }
}
