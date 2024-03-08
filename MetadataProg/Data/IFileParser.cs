using MetadataProg.Model;

namespace MetadataProg.Data
{
    public interface IFileParser
    {
        public string[][]? MenuItems { get; }
        public IUser ConcreteUser { get; }
        public bool ParseMenu(string name, string password);
    }
}
