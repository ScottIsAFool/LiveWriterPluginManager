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
            else
            {
                // TODO: Display some kind of message prompt
            }

            AppDomain myDomain = Thread.GetDomain();

            myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
            if (!myPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                // TODO: Show a message that it's not in administrator
                MessageBox.Show("Not running as adming");
                var i = 0;
            }
        }
    }
}
