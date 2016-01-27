using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace LiveWriterPluginManager.ViewModel
{
    public class RemovePluginViewModel : ViewModelBase
    {
        private bool _pluginsLoaded;

        public RemovePluginViewModel()
        {
            
        }

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

        private Task LoadPlugins(bool isRefresh = false)
        {
            if (_pluginsLoaded && !isRefresh)
            {
                return Task.FromResult(0);
            }

            return Task.FromResult(0);
        }
    }
}
