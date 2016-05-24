using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zip;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Model;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace LiveWriterPluginManager.Services
{
    public interface IZipService
    {
        Task<Plugin> UnzipFileAsync(string filePath);
        Task<bool> ZipFilesAsync(string[] files, string outputFile, Manifest manifest);
    }

    public class ZipService : IZipService
    {
        private readonly IMessageService _messageService;

        public ZipService(IMessageService messageService)
        {
            _messageService = messageService;
        }

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

                        var manifest = await ReadManifest(extractPath);

                        result = new Plugin
                        {
                            Name = manifest?.Name,
                            Path = manifest?.PluginPath
                        };

                        tcs.SetResult(true);
                    }
                });

                await tcs.Task;
            }
            catch (Exception ex)
            {
                await _messageService.ShowErrorAsync("There was an error unpacking this zip file: " + ex.Message);
            }

            return result;
        }

        public async Task<bool> ZipFilesAsync(string[] files, string outputFile, Manifest manifest)
        {
            var manifestJson = JsonConvert.SerializeObject(manifest);
            var tempManifestFile = $"{Path.GetTempPath()}{Manifest.ManifestFileName}";
            if (File.Exists(tempManifestFile))
            {
                File.Delete(tempManifestFile);
            }

            File.WriteAllText(tempManifestFile, manifestJson);

            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(() =>
            {
                try
                {
                    using (var zipFile = new ZipFile())
                    {
                        zipFile.AddFile(tempManifestFile, "");
                        zipFile.AddFiles(files, false, "");
                        zipFile.Save(outputFile);
                    }

                    tcs.SetResult(true);
                }
                catch
                {
                    tcs.SetResult(false);
                }
            });

            return await tcs.Task;
        }

        private static Task<Manifest> ReadManifest(string extractPath)
        {
            var directory = new DirectoryInfo(extractPath);
            var file = directory.EnumerateFiles(Manifest.ManifestFileName).FirstOrDefault();
            if (file == null)
            {
                return null;
            }

            var content = File.ReadAllText(file.FullName);
            var manifest = JsonConvert.DeserializeObject<Manifest>(content);
            manifest.PluginPath = string.Concat(extractPath, manifest.PluginFileName);
            return Task.FromResult(manifest);
        }
    }
}
