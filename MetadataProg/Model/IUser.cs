namespace MetadataProg.Model
{
    /// <summary>
    /// Интерфейс пользователя (для использования в будущем).
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        string Password { get; }
    }
}
