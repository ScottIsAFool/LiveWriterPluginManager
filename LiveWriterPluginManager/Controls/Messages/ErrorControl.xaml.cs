using System.Windows;

namespace LiveWriterPluginManager.Controls.Messages
{
    /// <summary>
    /// Interaction logic for ErrorControl.xaml
    /// </summary>
    public partial class ErrorControl
    {
        public ErrorControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register(
            "ErrorMessage", typeof (string), typeof (ErrorControl), new PropertyMetadata(default(string)));

        public string ErrorMessage
        {
            get { return (string) GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }
    }
}
