using System.Threading.Tasks;
using LiveWriterPluginManager.Controls.Messages;
using MaterialDesignThemes.Wpf;

namespace LiveWriterPluginManager.Services
{
    public interface IMessageService
    {
        Task<bool> ShowQuestionAsync(string question, string positive, string negative);
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
    }
}