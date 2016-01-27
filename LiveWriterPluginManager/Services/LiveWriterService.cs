using System.Linq;
using LiveWriterPluginManager.Model;
using Microsoft.Win32;

namespace LiveWriterPluginManager.Services
{
    public interface ILiveWriterService
    {
        void SavePlugin(Plugin plugin);
    }

    public class LiveWriterService : ILiveWriterService
    {
        private const string LiveWriterPluginsKey = @"Software\OpenLiveWriter\PluginAssemblies";
        private readonly RegistryKey _liveWriterPluginsRegistryKey;

        public LiveWriterService()
        {
            _liveWriterPluginsRegistryKey = Registry.CurrentUser.OpenSubKey(LiveWriterPluginsKey, true);
        }

        public void SavePlugin(Plugin plugin)
        {
            _liveWriterPluginsRegistryKey.SetValue(plugin.Name, plugin.Path, RegistryValueKind.String);
        }
    }
}