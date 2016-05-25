using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IMessageService _messageService;

        public CreatePackageViewModel(IZipService zipService, IFileService fileService, IMessageService messageService)
        {
            _zipService = zipService;
            _fileService = fileService;
            _messageService = messageService;

            Messenger.Default.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification.Equals(AppHelper.RemoveFileMsg))
                {
                    Files.Remove(m.Sender as FileViewModel);
                }
                else if (m.Notification.Equals(AppHelper.SetPluginFileMsg))
                {
                    RaisePropertyChanged(() => IsValidPackage);
                }
            });
        }

        public ObservableCollection<FileViewModel> Files { get; set; } = new ObservableCollection<FileViewModel>();

        public bool IsValidPackage => Files.Any(x => x.Name.Equals(Manifest.ManifestFileName, StringComparison.InvariantCultureIgnoreCase))
                                      || ManifestViewModel.PluginFileSet;

        public ManifestViewModel ManifestViewModel { get; set; } = new ManifestViewModel(new Manifest());

        public RelayCommand AddFilesCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var filenames = _fileService.GetPackageFiles();
                    AddFiles(filenames);
                });
            }
        }

        public void AddFiles(string[] filenames)
        {
            var files = filenames.Select(x => new FileViewModel(x)).ToList();

            foreach (var file in files)
            {
                Files.Add(file);
            }

            RaisePropertyChanged(() => IsValidPackage);
        }

        public RelayCommand EditMetadataCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await _messageService.ShowMetadataAsync();
                });
            }
        }

        public RelayCommand CreatePackageCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    if (!Files.Any(x => x.IsPluginFile))
                    {
                        // TODO: Display an error message
                        return;
                    }

                    var manifest = ManifestViewModel.GetManifest();
                    var packageFile = _fileService.ChoosePackageLocation();
                    var filePaths = Files.Select(x => x.Path).ToArray();

                    if (await _zipService.ZipFilesAsync(filePaths, packageFile, manifest))
                    {
                        Files.Clear();
                        ManifestViewModel = new ManifestViewModel(new Manifest());
                    }
                });
            }
        }

        public RelayCommand OpenPackageCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var file = _fileService.GetZipFile();
                    await OpenPackage(file);
                });
            }
        }

        public async Task OpenPackage(string file)
        {
            var packageDetails = await _zipService.OpenPackageForEditing(file);
            if (packageDetails.Item1 == null || !packageDetails.Item2.Any())
            {
                return;
            }
            ManifestViewModel = new ManifestViewModel(packageDetails.Item1);

            var files = packageDetails.Item2.Where(x => !x.Contains(Manifest.ManifestFileName)).Select(x => new FileViewModel(x));
            Files.Clear();
            foreach (var f in files)
            {
                Files.Add(f);
            }

            var pluginFile = Files.FirstOrDefault(x => x.Name == ManifestViewModel.PluginFileName);
            if (pluginFile != null)
            {
                pluginFile.IsPluginFile = true;
            }
        }
    }

    public class FileViewModel : ViewModelBase
    {
        public FileViewModel(string file)
        {
            File = new FileInfo(file);

            Messenger.Default.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification.Equals(AppHelper.SetPluginFileMsg))
                {
                    var selectedFile = m.Sender as FileViewModel;
                    IsPluginFile = selectedFile?.Path == this.Path;
                }
            });
        }

        public FileInfo File { get; set; }

        public string Name => File.Name;
        public string Path => File.FullName;
        public bool IsPluginFile { get; set; }
        public string Extension => File.Extension;

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

        public RelayCommand SetPluginFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage(this, AppHelper.SetPluginFileMsg));
                });
            }
        }
    }

    public class ManifestViewModel : ViewModelBase
    {
        private readonly Manifest _manifest;
        public ManifestViewModel(Manifest manifest)
        {
            _manifest = manifest;

            Name = _manifest.Name;
            Author = _manifest.Author;
            ProjectUrl = _manifest.ProjectUrl;
            TermsUrl = _manifest.TermsUrl;
            Version = _manifest.Version;
            TargetWriterVersion = _manifest.TargetWriterVersion;
            PluginFileName = _manifest.PluginFileName;

            Messenger.Default.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification.Equals(AppHelper.SetPluginFileMsg))
                {
                    var file = m.Sender as FileViewModel;
                    _manifest.PluginFileName = file?.Name;
                    _manifest.PluginPath = file?.Path;
                }
            });
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public string ProjectUrl { get; set; }
        public string TermsUrl { get; set; }
        public string Version { get; set; }
        public string TargetWriterVersion { get; set; }
        public string PluginFileName { get; set; }

        public bool PluginFileSet => !string.IsNullOrEmpty(_manifest?.PluginFileName);

        public Manifest GetManifest()
        {
            _manifest.Name = Name;
            _manifest.Author = Author;
            _manifest.ProjectUrl = ProjectUrl;
            _manifest.TermsUrl = TermsUrl;
            _manifest.Version = Version;
            _manifest.TargetWriterVersion = TargetWriterVersion;

            return _manifest;
        }
    }
}