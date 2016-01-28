using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Services;

namespace LiveWriterPluginManager.ViewModel
{
    public class RemovePluginViewModel : ViewModelBase
    {
        private readonly ILiveWriterService _liveWriterService;
        private readonly IMessageService _messageService;
        private bool _pluginsLoaded;

        public RemovePluginViewModel(ILiveWriterService liveWriterService, IMessageService messageService)
        {
            _liveWriterService = liveWriterService;
            _messageService = messageService;

            Messenger.Default.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification.Equals(AppHelper.RemovePluginMsg))
                {
                    var plugin = m.Sender as PluginViewModel;
                    Plugins.Remove(plugin);
                }
            });
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
                Plugins.Add(new PluginViewModel(plugin, _liveWriterService, _messageService));
            }

            _pluginsLoaded = Plugins.Any();
        }
    }
}
