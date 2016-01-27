using Microsoft.Win32;

namespace LiveWriterPluginManager.Services
{
    public interface IFileService
    {
        string GetZipFile();
    }

    public class FileService : IFileService
    {
        public string GetZipFile()
        {
            var openFile = new OpenFileDialog
            {
                Filter = "Zip files (*.zip)|*.zip",
                Multiselect = false,
                Title = "Please choose your plugin zip file"
            };

            var response = openFile.ShowDialog();
            var file = string.Empty;
            if (response ?? false)
            {
                file = openFile.FileName;
            }

            return file;
        }
    }
}