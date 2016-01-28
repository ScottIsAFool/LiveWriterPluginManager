using System.Windows;

namespace LiveWriterPluginManager.Controls.Messages
{
    /// <summary>
    /// Interaction logic for QuestionControl.xaml
    /// </summary>
    public partial class QuestionControl
    {
        public QuestionControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        public static readonly DependencyProperty PositiveStringProperty = DependencyProperty.Register(
            "PositiveString", typeof (string), typeof (QuestionControl), new PropertyMetadata(default(string)));

        public string PositiveString
        {
            get { return (string) GetValue(PositiveStringProperty); }
            set { SetValue(PositiveStringProperty, value); }
        }

        public static readonly DependencyProperty NegativeStringProperty = DependencyProperty.Register(
            "NegativeString", typeof (string), typeof (QuestionControl), new PropertyMetadata(default(string)));

        public string NegativeString
        {
            get { return (string) GetValue(NegativeStringProperty); }
            set { SetValue(NegativeStringProperty, value); }
        }

        public static readonly DependencyProperty QuestionStringProperty = DependencyProperty.Register(
            "QuestionString", typeof (string), typeof (QuestionControl), new PropertyMetadata(default(string)));

        public string QuestionString
        {
            get { return (string) GetValue(QuestionStringProperty); }
            set { SetValue(QuestionStringProperty, value); }
        }
    }
}
