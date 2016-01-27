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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AddPluginViewModel(IZipService zipService, IFileService fileService)
        {
            _zipService = zipService;
            _fileService = fileService;
            CanAdd = AppHelper.LiveWriterInstalled;
        }

        public bool CanAdd { get; set; } = true;

        public RelayCommand BrowseCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    CanAdd = false;

                    try
                    {
                        var file = _fileService.GetZipFile();
                        var unzippedFolder = await _zipService.UnzipFileAsync(file);
                    }
                    finally
                    {
                        CanAdd = true;
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