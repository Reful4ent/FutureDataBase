using MetadataProg.Model;
using System.IO;

namespace MetadataProg.Data
{
    /// <summary>
    /// Класс считывающий и содержащий в себе конфигурацию меню пользователя.
    /// </summary>
    public class FileParser : IFileParser
    {
        string pathMenu = string.Empty;
        string pathUser = string.Empty;

        /// <summary>
        /// Конфигурация пользователя.
        /// </summary>
        string[][]? UserConfig { get; set; }

        /// <summary>
        /// Поток ввода.
        /// </summary>
        FileStream FileStream { get; set; }

        /// <summary>
        /// Элементы меню.
        /// </summary>
        public string[][]? MenuItems { get; private set; }

        /// <summary>
        /// Конкретный пользователь.
        /// </summary>
        public IUser ConcreteUser { get; private set; }

        public FileParser(string pathMenu, string pathUser)
        {
            // Проверки на нулевой путь к файлам
            if (pathMenu == null)
                throw new ArgumentNullException("Нулевая ссылка!");
            if (pathUser == null)
                throw new ArgumentNullException("Нулевая ссылка!");
            // Проверки на существование файлов
            if (!File.Exists(pathUser)) 
                throw new FileNotFoundException();
            if (!File.Exists(pathMenu))
                throw new FileNotFoundException();

            this.pathMenu = pathMenu;
            this.pathUser = pathUser;
        }

        public static FileParser Instance(string pathMenu, string pathUser) => new(pathMenu, pathUser);

        /// <summary>
        /// Считываение пользователя из файла.
        /// </summary>
        /// <param name="name"> Введенный логин. </param>
        /// <param name="password"> Введенный пароль. </param>
        /// <returns> True - успешное выполнение. False - неуспешное выполнение. </returns>
        private bool ParseUser(string name, string password)
        {
            string fileLine = string.Empty;
            string ConfigLine = string.Empty;
            //Считывает в себя строчку из файла
            List<string> tokens = new List<string>();

            FileStream = new FileStream(this.pathUser, FileMode.Open, FileAccess.Read);
            using StreamReader streamReader = new StreamReader(pathUser);

            while ((fileLine = streamReader.ReadLine()) != null)
            {
                if (fileLine.StartsWith('#'))
                {
                    string[] logInData = fileLine.Trim('#').Split(' ');

                    if (logInData[0] == name && logInData[1] == password && logInData.Length == 2)
                    {
                        ConcreteUser = User.Instance(logInData[0], logInData[1]);

                        while ((ConfigLine = streamReader.ReadLine()) != null && !(ConfigLine.StartsWith('#')))
                            tokens.Add(ConfigLine);

                        UserConfig = new string[tokens.Count][];

                        for(int i=0; i< UserConfig.Length; i++)
                        {
                            UserConfig[i] = tokens[i].Split(' ');
                            ConcatStrings(ref UserConfig[i], 2);
                        }

                        FileStream.Close();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Считывание меню из файла.
        /// </summary>
        /// <param name="name"> Введенный логин. </param>
        /// <param name="password"> Введенный пароль. </param>
        /// <returns>True - успешное выполнение. False - неуспешное выполнение. </returns>
        public bool ParseMenu(string name, string password)
        {
            //Считывает пользователя, чтобы потом настроить меню под него. Если не считал, меню не парсится.
            if (!ParseUser(name, password))
                return false;

            FileStream = new FileStream(this.pathMenu, FileMode.Open, FileAccess.Read);
            string[] MenuContent = File.ReadAllLines(pathMenu);
            MenuItems = new string[MenuContent.Length][];

            for (int i = 0; i < MenuContent.Length; i++)
            {
                MenuItems[i] = MenuContent[i].Split(' ');
                ConcatStrings(ref MenuItems[i],1,i);
            }

            FileStream.Close();
            return true;
        }

        /// <summary>
        /// Склеивание строчек.
        /// </summary>
        /// <param name="fileLine"> Массив строк. </param>
        /// <param name="choice"> Для какой конфигурации происходит склеивание: 1 - для меню, 2 - для пользователя. </param>
        /// <param name="position"> Какая по счёту строчка считывается. </param>
        /// <returns> True - успешное выполнение. False - неуспешное выполнение. </returns>
        private bool ConcatStrings(ref string[] fileLine, int choice, int position = 0) 
        {
            int index = -1;
            string[]? text;
            int LengthOfSecond=0;
            string[]? temp;

            switch (choice)
            {
                case 1:
                    //Проверяет является ли первый элемент числом
                    if (!int.TryParse(fileLine[0], out _))
                        return false;
                    //Ищет следующее число в массиве
                    for (int i = 1; i < fileLine.Length; i++)
                    {
                        if (!int.TryParse(fileLine[i], out _)) continue;
                        index = i;
                        break;
                    }

                    if (index < 0)
                        return false;

                    text = new string[index - 1];

                    for (int i = 1; i < index; i++)
                        text[i - 1] = fileLine[i];
                    //Склеивает слова по пробелу
                    fileLine[1] = string.Join(" ", text);
                    LengthOfSecond = fileLine.Length - index;
                    temp = new string[2 + LengthOfSecond];
                    //Пересобирает массив
                    Array.ConstrainedCopy(fileLine, 0, temp, 0, 2);
                    Array.ConstrainedCopy(fileLine, index, temp, 2, LengthOfSecond);

                    fileLine = temp;
                    
                    // Изменяет стандартную конфигурацию меню под конфигурацию пользователя.
                    for(int i = 0; i < UserConfig.Length; i++)
                    {
                        if (UserConfig[i][0] == fileLine[1])
                            fileLine[2] = UserConfig[i][1];
                    }

                    if (fileLine.Length < 3 || fileLine.Length > 4)
                        return false;
                    return true;
                case 2:
                    for (int i = 0; i < fileLine.Length; i++)
                    {
                        if (!int.TryParse(fileLine[i], out _)) continue;
                        index = i;
                        break;
                    }

                    if (index < 0)
                        return false;

                    text = new string[index];

                    for (int i = 0; i < index; i++)
                        text[i] = fileLine[i];

                    fileLine[0] = string.Join(" ", text);

                    LengthOfSecond = fileLine.Length - index;
                    temp = new string[1 + LengthOfSecond];

                    // Копирование массива строк во временный массив для последующей пересборки массива строк.
                    Array.ConstrainedCopy(fileLine, 0, temp, 0, 1);
                    Array.ConstrainedCopy(fileLine, index, temp, 1, LengthOfSecond);

                    fileLine = temp;

                    if (fileLine.Length != 2)
                        return false;
                    return true;
                default: 
                    return false;
            }
        }
    }
}
