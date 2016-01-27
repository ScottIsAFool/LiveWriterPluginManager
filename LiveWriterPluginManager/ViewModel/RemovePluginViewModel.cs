using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveWriterPluginManager.Services;

namespace LiveWriterPluginManager.ViewModel
{
    public class RemovePluginViewModel : ViewModelBase
    {
        private readonly ILiveWriterService _liveWriterService;
        private bool _pluginsLoaded;

        public RemovePluginViewModel(ILiveWriterService liveWriterService)
        {
            _liveWriterService = liveWriterService;
        }

        public ObservableCollection<PluginViewModel> Plugins { get; set; } = new ObservableCollection<PluginViewModel>();

        public RelayCommand LoadedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadPlugins();
                });
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadPlugins(true);
                });
            }
        }

        private async Task LoadPlugins(bool isRefresh = false)
        {
            if (_pluginsLoaded && !isRefresh)
            {
                return;
            }

            if (isRefresh)
            {
                Plugins.Clear();
            }

            var plugins = await _liveWriterService.GetReferencedPlugins();
            foreach (var plugin in plugins)
            {
                Plugins.Add(new PluginViewModel(plugin, _liveWriterService));
            }

            _pluginsLoaded = Plugins.Any();
        }
    }
}
