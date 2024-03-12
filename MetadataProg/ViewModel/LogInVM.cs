using MetadataProg.Data;
using MetadataProg.ViewModel.Commands;

namespace MetadataProg.ViewModel
{
    /// <summary>
    /// Реализация MVVM (к нему привязано окно LogIn).
    /// </summary>
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

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login
        {
            get => login;
            set => Set(ref login, value);
        }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password
        {
            get => password;
            set => Set(ref password, value);
        }

        /// <summary>
        /// Создание команды для привязки View к обработке нажатия.
        /// </summary>
        public Command GoToMain => Command.Create(LogIn);

        /// <summary>
        /// Если регистрация прошла успешно, то пользователь входит в главное окно, иначе - выскакивает окно об ошибке.
        /// </summary>
        private void LogIn()
        {
            if (fileParser.ParseMenu(Login, password))
                LoginSucces?.Invoke();
            else LoginDenied?.Invoke("Неправильный логин или пароль или пользователь отсутствует!");
        }
    }
}
