using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataProg.Data
{
    public interface IFileParser
    {
        public string[][]? MenuItems { get; }
        public bool ParseMenu(string name, string password);
    }
}
