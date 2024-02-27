using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataProg.Model
{
    public class User : IUser
    {
        public string Name { get; private set; }
        public string Password { get; private set; }
        public Dictionary<string, string> Properties { get; private set; }

        public User(string name, string password, Dictionary<string,string> properties)
        {
            Name = name;
            Password = password;
            Properties = properties;
        }
    }
}
