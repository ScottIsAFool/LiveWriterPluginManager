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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AddPluginViewModel(IZipService zipService, IFileService fileService, ILiveWriterService liveWriterService)
        {
            _zipService = zipService;
            _fileService = fileService;
            _liveWriterService = liveWriterService;
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