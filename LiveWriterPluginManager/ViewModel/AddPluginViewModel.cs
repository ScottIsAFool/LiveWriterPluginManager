using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Services;

namespace LiveWriterPluginManager.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddPluginViewModel : ViewModelBase
    {
        private readonly IZipService _zipService;
        private readonly IFileService _fileService;
        private readonly ILiveWriterService _liveWriterService;
        private readonly IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AddPluginViewModel(IZipService zipService, IFileService fileService, ILiveWriterService liveWriterService, IMessageService messageService)
        {
            _zipService = zipService;
            _fileService = fileService;
            _liveWriterService = liveWriterService;
            _messageService = messageService;
            CanAdd = AppHelper.LiveWriterInstalled && AppHelper.IsRunningAsAdmin();
        }

        public bool CanAdd { get; set; }

        public bool IsAdding { get; set; }

        public RelayCommand BrowseCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    CanAdd = false;
                    IsAdding = true;

                    try
                    {
                        var file = _fileService.GetZipFile();
                        if (!string.IsNullOrEmpty(file))
                        {
                            var plugin = await _zipService.UnzipFileAsync(file);
                            if (!string.IsNullOrEmpty(plugin?.Path))
                            {
                                _liveWriterService.SavePlugin(plugin);
                                await _messageService.ShowMessageAsync("Plugin has been installed, please restart Live Writer in order to start using it.");
                            }
                        }
                    }
                    finally
                    {
                        CanAdd = true;
                        IsAdding = false;
                    }
                });
            }
        }

        public RelayCommand ZipInstructionsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }
    }
}