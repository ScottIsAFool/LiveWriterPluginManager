using GalaSoft.MvvmLight.Ioc;
using LiveWriterPluginManager.Services;
using Microsoft.Practices.ServiceLocation;
using ScottIsAFool.Windows.MvvmLight.Extensions;

namespace LiveWriterPluginManager.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.RegisterIf<IZipService, ZipService>();
            SimpleIoc.Default.RegisterIf<IFileService, FileService>();
            SimpleIoc.Default.RegisterIf<ILiveWriterService, LiveWriterService>();
            SimpleIoc.Default.RegisterIf<IMessageService, MessageService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddPluginViewModel>();
            SimpleIoc.Default.Register<RemovePluginViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public AddPluginViewModel AddPlugin => ServiceLocator.Current.GetInstance<AddPluginViewModel>();
        public RemovePluginViewModel RemovePlugin => ServiceLocator.Current.GetInstance<RemovePluginViewModel>();
        public AboutViewModel About => ServiceLocator.Current.GetInstance<AboutViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}