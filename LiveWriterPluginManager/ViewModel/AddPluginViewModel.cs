using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

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
        private readonly OpenFileDialog _openFile;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AddPluginViewModel()
        {
            _openFile = new OpenFileDialog
            {
                Filter = "Zip files (*.zip)|*.zip",
                Multiselect = false,
                Title = "Please choose your plugin zip file"
            };
        }

        public RelayCommand BrowseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var response = _openFile.ShowDialog();
                    if (response ?? false)
                    {
                        using (var file = _openFile.OpenFile())
                        {
                            var i = file.Length;
                        }
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