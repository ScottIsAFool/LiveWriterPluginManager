using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiveWriterPluginManager.Model;
using Microsoft.Win32;

namespace LiveWriterPluginManager.Services
{
    public interface ILiveWriterService
    {
        void SavePlugin(Plugin plugin);
        Task<List<Plugin>> GetReferencedPlugins();
        void DeletePlugin(Plugin plugin);
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

        public async Task<List<Plugin>> GetReferencedPlugins()
        {
            return await Task.Run(() =>
            {
                var result = new List<Plugin>();
                if (_liveWriterPluginsRegistryKey == null)
                {
                    return result;
                }

                var pluginKeys = _liveWriterPluginsRegistryKey.GetValueNames();
                foreach (var key in pluginKeys)
                {
                    var path = _liveWriterPluginsRegistryKey.GetValue(key, string.Empty)?.ToString();
                    if (File.Exists(path))
                    {
                        result.Add(new Plugin { Name = key, Path = path });
                    }
                }

                return result;
            });
        }

        public void DeletePlugin(Plugin plugin)
        {
            var pluginKeys = _liveWriterPluginsRegistryKey.GetValueNames();
            if (!pluginKeys.Contains(plugin.Name))
            {
                return;
            }

            _liveWriterPluginsRegistryKey.DeleteValue(plugin.Name, false);

            var file = new FileInfo(plugin.Path);
            var path = file.Directory;
            if (path?.Exists ?? false)
            {
                path.Delete(true);
            }
        }
    }
}