using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveWriterPluginManager.Model;
using LiveWriterPluginManager.ViewModel;

namespace LiveWriterPluginManager.Controls
{
    /// <summary>
    /// Interaction logic for CreatePluginControl.xaml
    /// </summary>
    public partial class CreatePackageControl : UserControl
    {
        public CreatePackageControl()
        {
            InitializeComponent();
        }

        private async void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var fileVms = files.Select(x => new FileViewModel(x));
                var packageFile = fileVms.FirstOrDefault(x => x.Extension == ".olwpkg");
                var vm = DataContext as CreatePackageViewModel;

                if (packageFile != null)
                {
                    await vm?.OpenPackage(packageFile.Path);
                }
                else
                {
                    vm?.AddFiles(files);
                }
            }
        }
    }
}
