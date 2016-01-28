using GalaSoft.MvvmLight.Ioc;
using LiveWriterPluginManager.Services;

namespace LiveWriterPluginManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                var message = SimpleIoc.Default.GetInstance<IMessageService>();
                message.SetWindow(this);
            };
        }
    }
}
