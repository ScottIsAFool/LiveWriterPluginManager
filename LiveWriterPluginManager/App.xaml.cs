using System;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using LiveWriterPluginManager.Helpers;

namespace LiveWriterPluginManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += OnStartup;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            if (AppHelper.LiveWriterInstalled)
            {
                AppHelper.CreatePluginDirectory();
            }
        }
    }
}
