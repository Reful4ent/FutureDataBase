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

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public static User Instance(string name, string password) => new(name, password);
    }
}
