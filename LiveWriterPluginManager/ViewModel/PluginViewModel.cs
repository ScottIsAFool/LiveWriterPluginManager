using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Model;
using LiveWriterPluginManager.Services;

namespace LiveWriterPluginManager.ViewModel
{
    public class PluginViewModel : ViewModelBase
    {
        private readonly ILiveWriterService _liveWriterService;
        public PluginViewModel(Plugin plugin, ILiveWriterService liveWriterService)
        {
            Plugin = plugin;
            _liveWriterService = liveWriterService;
        }

        public Plugin Plugin { get; set; }

        public RelayCommand DeletePluginCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (AppHelper.IsLiveWriterRunning())
                    {
                        // TODO: Display an error message
                        return;
                    }
                    _liveWriterService.DeletePlugin(Plugin);
                });
            }
        }
    }
}
