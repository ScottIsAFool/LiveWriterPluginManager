using System.Threading.Tasks;
using LiveWriterPluginManager.Controls;
using LiveWriterPluginManager.Controls.Messages;
using MaterialDesignThemes.Wpf;

namespace LiveWriterPluginManager.Services
{
    public interface IMessageService
    {
        Task<bool> ShowQuestionAsync(string question, string positive, string negative);
        Task ShowErrorAsync(string errorMessage);
        Task ShowMessageAsync(string message);
        Task ShowMetadataAsync();
    }

    public class MessageService : IMessageService
    {
        public async Task<bool> ShowQuestionAsync(string question, string positive, string negative)
        {
            var questionControl = new QuestionControl
            {
                NegativeString = negative,
                PositiveString = positive,
                QuestionString = question
            };
            var result = await DialogHost.Show(questionControl);
            return (bool)result;
        }

        public Task ShowErrorAsync(string errorMessage)
        {
            var errorControl = new ErrorControl
            {
                ErrorMessage = errorMessage
            };

            return DialogHost.Show(errorControl);
        }

        public Task ShowMessageAsync(string message)
        {
            var infoControl = new InformationControl
            {
                Message = message
            };

            return DialogHost.Show(infoControl);
        }

        public Task ShowMetadataAsync()
        {
            var metadataControl = new MetadataControl();

            return DialogHost.Show(metadataControl);
        }
    }
}