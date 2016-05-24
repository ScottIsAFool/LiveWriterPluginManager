using Microsoft.Win32;

namespace LiveWriterPluginManager.Services
{
    public interface IFileService
    {
        string GetZipFile();
        string[] GetPackageFiles();
        string ChoosePackageLocation();
    }

    public class FileService : IFileService
    {
        public string GetZipFile()
        {
            var openFile = new OpenFileDialog
            {
                Filter = "Package files (*.wlwpkg)|*.wlwpkg",
                Multiselect = false,
                Title = "Please choose your plugin package file"
            };

            var response = openFile.ShowDialog();
            var file = string.Empty;
            if (response ?? false)
            {
                file = openFile.FileName;
            }

            return file;
        }

        public string[] GetPackageFiles()
        {
            var openFile = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Please choose your package files"
            };

            var response = openFile.ShowDialog();
            if (response ?? false)
            {
                return openFile.FileNames;
            }

            return new string[0];
        }

        public string ChoosePackageLocation()
        {
            var saveFile = new SaveFileDialog
            {
                Filter = "Package files (*.wlwpkg)|*.wlwpkg",
                AddExtension = true,
                Title = "Please choose where to save the package",
                CreatePrompt = true,
                OverwritePrompt = true
            };

            var response = saveFile.ShowDialog();
            var file = string.Empty;
            if (response ?? false)
            {
                file = saveFile.FileName;
            }

            return file;
        }
    }
}