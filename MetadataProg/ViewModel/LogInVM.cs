using MetadataProg.Data;
using MetadataProg.ViewModel.Commands;

namespace MetadataProg.ViewModel
{
    public class LogInVM : BasicVM
    {
        IFileParser fileParser;

        string login = string.Empty;
        string password = string.Empty;

        public Action? LoginSucces;
        public Action<string>? LoginDenied;

        public LogInVM(IFileParser fileParser)
        {
            this.fileParser = fileParser;
        }

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

        /// <summary>
        /// Создание команды для привязки View к обработке нажатия
        /// </summary>
        public Command GoToMain => Command.Create(LogIn);

        /// <summary>
        /// Если регистрация прошла успешно, пользователь входит в главное окно, иначе - выскакиевает окно об ошибке
        /// </summary>
        private void LogIn()
        {
            if (fileParser.ParseMenu(Login, password))
                LoginSucces?.Invoke();
            else LoginDenied?.Invoke("Неправильный логин или пароль или пользователь отсутствует!");
        }
    }
}
