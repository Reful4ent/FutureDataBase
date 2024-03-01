using MetadataProg.Data;
using MetadataProg.ViewModel.Commands;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataProg.ViewModel
{
    public class LogInVM : BasicVM
    {
        IFileParser fileParser;
        string login = string.Empty;
        string password = string.Empty;

        public Action? LoginSucces;
        public Action<string>? LoginDenied;
        public string Login
        {
            get => login;
            set => Set(ref login, value);
        }

        public string Password
        {
            get => password;
            set => Set(ref password, value);
        }

        public LogInVM(IFileParser fileParser)
        {
            this.fileParser = fileParser;
        }


        public Command GoToMain => Command.Create(LogIn);


        private void LogIn()
        {
            if (fileParser.ParseMenu(Login, password))
                LoginSucces?.Invoke();
            else LoginDenied?.Invoke("Неправильный логин или пароль или пользователь отсутствует!");
        }
    }
}
