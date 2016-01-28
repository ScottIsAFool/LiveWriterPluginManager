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
        private readonly IMessageService _messageService;

        public PluginViewModel(Plugin plugin, ILiveWriterService liveWriterService, IMessageService messageService)
        {
            Plugin = plugin;
            _liveWriterService = liveWriterService;
            _messageService = messageService;
        }

        public Plugin Plugin { get; set; }

        public RelayCommand DeletePluginCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (AppHelper.IsLiveWriterRunning())
                    {
                        // TODO: Display an error message
                        return;
                    }

                    var removePlugin = await _messageService.ShowAsync("Are you sure?", "Are you sure you wish to remove this plugin?");
                    if (removePlugin)
                    {
                        _liveWriterService.DeletePlugin(Plugin);
                    }
                });
            }
        }
    }
}
