using System;
using System.IO;
using System.Threading.Tasks;
using Ionic.Zip;
using LiveWriterPluginManager.Helpers;

namespace LiveWriterPluginManager.Services
{
    public interface IZipService
    {
        Task<string> UnzipFileAsync(string filePath);
    }

    public class ZipService : IZipService
    {
        public Task<string> UnzipFileAsync(string filePath)
        {
            var task = Task.FromResult(string.Empty);
            if (string.IsNullOrEmpty(filePath))
            {
                return task;
            }

            try
            {
                using (var zipFile = ZipFile.Read(filePath))
                {
                    zipFile.FlattenFoldersOnExtract = true;
                    var fileName = Path.GetFileNameWithoutExtension(filePath);

                    var extractPath = Path.Combine(AppHelper.PluginsFolder, fileName);
                    zipFile.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);

                    task = Task.FromResult(extractPath);
                }
            }
            catch (Exception ex)
            {
                // TODO: something here
            }

            return task;
        }
    }
}
