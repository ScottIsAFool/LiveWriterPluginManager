using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace LiveWriterPluginManager.Services
{
    public interface IMessageService
    {
        Task<bool> ShowAsync(string title, string message);
        void SetWindow(MetroWindow window);
    }

    public class MessageService : IMessageService
    {
        private MetroWindow _metroWindow;
        public async Task<bool> ShowAsync(string title, string message)
        {
            var result = await _metroWindow.ShowMessageAsync(title, message, settings: new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "Cancel"
            });

            return result == MessageDialogResult.Affirmative;
        }

        public void SetWindow(MetroWindow window)
        {
            _metroWindow = window;
        }
    }
}