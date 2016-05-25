using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Services;
using OpenLiveWriter.Api;
using Task = System.Threading.Tasks.Task;

namespace LiveWriterPluginManager.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        public MainViewModel(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public RelayCommand LoadedCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    WriterPlugin plugin = null;
                    if (!AppHelper.LiveWriterInstalled)
                    {
                        await _messageService.ShowErrorAsync("It doesn't look like you have Live Writer installed, please go to http://openlivewriter.org to download and install it.");
                        Application.Current.Shutdown();
                        return;
                    }

                    await Task.Delay(500);

                    if (!AppHelper.IsRunningAsAdmin())
                    {
                        await _messageService.ShowErrorAsync("In order for this app to add/remove plugins, it needs to run as Administrator, please restart the app with higher privileges");
                    }

                    AppHelper.CheckForUpdates();
                });
            }
        }
    }
}
