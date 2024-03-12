using MetadataProg.Model;

namespace MetadataProg.Data
{
    /// <summary>
    /// Интерфейс парсера меню.
    /// </summary>
    public interface IFileParser
    {
        /// <summary>
        /// Элементы меню.
        /// </summary>
        public string[][]? MenuItems { get; }

        /// <summary>
        /// Конкретный пользователь.
        /// </summary>
        public IUser ConcreteUser { get; }

        /// <summary>
        /// Парсер меню.
        /// </summary>
        /// <param name="name"> Введённый логин. </param>
        /// <param name="password"> Введённый пароль. </param>
        /// <returns></returns>
        public bool ParseMenu(string name, string password);
    }
}
