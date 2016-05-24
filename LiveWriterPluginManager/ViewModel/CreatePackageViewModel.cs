using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LiveWriterPluginManager.Helpers;
using LiveWriterPluginManager.Model;
using LiveWriterPluginManager.Services;

namespace LiveWriterPluginManager.ViewModel
{
    public class CreatePackageViewModel : ViewModelBase
    {
        private readonly IZipService _zipService;
        private readonly IFileService _fileService;

        public CreatePackageViewModel(IZipService zipService, IFileService fileService)
        {
            _zipService = zipService;
            _fileService = fileService;

            Messenger.Default.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification.Equals(AppHelper.RemoveFileMsg))
                {
                    Files.Remove(m.Sender as FileViewModel);
                }
            });
        }

        public ObservableCollection<FileViewModel> Files { get; set; } = new ObservableCollection<FileViewModel>();
        public bool IsValidPackage { get; set; }

        public RelayCommand AddFilesCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var filenames = _fileService.GetPackageFiles();
                    var files = filenames.Select(x => new FileViewModel(x)).ToList();
                    if (!files.Any(x => x.Name.Equals(Manifest.ManifestFileName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        IsValidPackage = false;
                        return;
                    }

                    IsValidPackage = true;

                    foreach (var file in files)
                    {
                        Files.Add(file);
                    }
                });
            }
        }

        public RelayCommand CreatePackageCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var packageFile = _fileService.ChoosePackageLocation();
                    var filePaths = Files.Select(x => x.Path).ToArray();

                    await _zipService.ZipFilesAsync(filePaths, packageFile);
                });
            }
        }
    }

    public class FileViewModel : ViewModelBase
    {
        public FileViewModel(string file)
        {
            File = new FileInfo(file);
        }

        public FileInfo File { get; set; }

        public string Name => File.Name;
        public string Path => File.FullName;

        public RelayCommand RemoveFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage(this, AppHelper.RemoveFileMsg));
                });
            }
        }
    }
}