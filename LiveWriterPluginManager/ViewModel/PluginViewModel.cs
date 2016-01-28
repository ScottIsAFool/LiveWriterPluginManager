using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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

        public bool CanRemove => AppHelper.IsRunningAsAdmin();

        public RelayCommand DeletePluginCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (AppHelper.IsLiveWriterRunning())
                    {
                        await _messageService.ShowErrorAsync("Live Writer is currently running, please close it before trying to remove any plugins");
                        return;
                    }

                    var removePlugin = await _messageService.ShowQuestionAsync("Are you sure you wish to remove this plugin?", "Yes", "No, ignore me");
                    if (removePlugin)
                    {
                        try
                        {
                            _liveWriterService.DeletePlugin(Plugin);
                            Messenger.Default.Send(new NotificationMessage(this, AppHelper.RemovePluginMsg));

                            await _messageService.ShowMessageAsync("Plugin deleted.");
                        }
                        catch
                        {
                            
                        }
                    }
                });
            }
        }
    }
}
