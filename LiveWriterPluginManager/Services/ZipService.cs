using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ionic.Zip;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Model;
using OpenLiveWriter.Api;
using Task = System.Threading.Tasks.Task;

namespace LiveWriterPluginManager.Services
{
    public interface IZipService
    {
        Task<Plugin> UnzipFileAsync(string filePath);
    }

    public class ZipService : IZipService
    {
        public async Task<Plugin> UnzipFileAsync(string filePath)
        {
            Plugin result = null;
            if (string.IsNullOrEmpty(filePath))
            {
                return result;
            }

            try
            {
            var tcs = new TaskCompletionSource<bool>();
                await Task.Run(async () =>
                {
                    using (var zipFile = ZipFile.Read(filePath))
                    {
                        zipFile.FlattenFoldersOnExtract = true;
                        var fileName = Path.GetFileNameWithoutExtension(filePath);

                        var extractPath = Path.Combine(AppHelper.PluginsFolder, fileName);
                        zipFile.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);

                        var pluginFile = await CheckExtractedFilesForPlugin(extractPath);

                        result = new Plugin
                        {
                            Name = fileName,
                            Path = pluginFile?.FullName
                        };

                        tcs.SetResult(true);
                    }
                });

                await tcs.Task;
            }
            catch (Exception ex)
            {
                // TODO: something here
            }

            return result;
        }

        private async Task<FileInfo> CheckExtractedFilesForPlugin(string extractedPath)
        {
            var directory = new DirectoryInfo(extractedPath);
            var fileTasks = directory.EnumerateFiles().Select(CheckFile).ToList();

            var files = await Task.WhenAll(fileTasks);
            var pluginFile = files.FirstOrDefault(x => x.IsLiveWriterFile);
            return pluginFile?.File;
        }

        private static async Task<LiveWriterFile> CheckFile(FileInfo file)
        {
            return await Task.Run(() =>
            {
                var asm = Assembly.LoadFile(file.FullName);
                var types = asm.GetTypes().ToList();
                var writerTypes = types.Where(x => typeof(WriterPlugin).IsAssignableFrom(x)).ToList();
                return new LiveWriterFile(file, writerTypes.Any());
            });
        }

        private class LiveWriterFile
        {
            public LiveWriterFile(FileInfo file, bool isLiveWriterFile)
            {
                File = file;
                IsLiveWriterFile = isLiveWriterFile;
            }

            public bool IsLiveWriterFile { get; }
            public FileInfo File { get; }
        }
    }
}
