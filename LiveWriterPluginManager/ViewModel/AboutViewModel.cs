using System.Collections.Generic;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Model;

namespace LiveWriterPluginManager.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            SetVersion();
        }

        private void SetVersion()
        {
            Version = AppHelper.GetAppVersion().ToString();
        }

        public List<Library> Libraries => new List<Library>
        {
            new Library {Name = "MVVM Light", Url = "http://mvvmlight.codeplex.com"},
            new Library {Name = "MaterialDesignInXamlToolkit", Url = "https://github.com/ButchersBoy/MaterialDesignInXamlToolkit"},
            new Library {Name = "MahApps", Url = "http://mahapps.com/" },
            new Library {Name = "Fody", Url = "https://github.com/fody" },
            new Library {Name = "Squirrel", Url = "https://github.com/Squirrel/Squirrel.Windows/" }
        };

        public string Version { get; set; }

        public RelayCommand<Library> GoToCommand
        {
            get
            {
                return new RelayCommand<Library>(library =>
                {
                    Process.Start(library.Url);
                });
            }
        }

        public RelayCommand SayHiToScottCommand
        {
            get
            {
                return new RelayCommand(() => Process.Start("https://twitter.com/scottisafool"));
            }
        }
    }
}
