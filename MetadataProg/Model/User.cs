namespace MetadataProg.Model
{
    /// <summary>
    /// Класс пользователя (для использования в будущем).
    /// </summary>
    public class User : IUser
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; private set; }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        /// <summary>
        /// Создание объекта класса при вызове метода.
        /// </summary>
        /// <param name="name"> Введённый логин. </param>
        /// <param name="password"> Введённый пароль</param>
        /// <returns> Объект класса. </returns>
        public static User Instance(string name, string password) => new(name, password);
    }
}
