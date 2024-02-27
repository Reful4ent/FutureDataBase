using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataProg.Model
{
    public interface IUser
    {
        string Name { get; }
        string Password { get; }
        Dictionary<string, string> Properties { get; }
    }
}
