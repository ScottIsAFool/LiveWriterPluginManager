using PropertyChanged;

namespace LiveWriterPluginManager.Model
{
    [ImplementPropertyChanged]
    public class Plugin
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    [ImplementPropertyChanged]
    public class Library
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
