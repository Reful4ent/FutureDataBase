using MetadataProg.Data;
using MetadataProg.ViewModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MetadataProg.View
{
    /// <summary>
    /// Окно входа в приложение.
    /// </summary>
    public partial class LogIn : Window
    {
        IFileParser fileParser;

        /// <summary>
        /// Словарик для вывода раскладки пользователя.
        /// </summary>
        readonly Dictionary<string, string> Languages = new Dictionary<string, string>()
        {
            {"ru-RU","Русский"},
            {"en-US","Английский"},
        };

        /// <summary>
        /// Таймер для проверки статуса CapsLock и раскладки пользователя.
        /// </summary>
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public LogIn(IFileParser fileParser)
        {
            InitializeComponent();
            DataContext = new LogInVM(this.fileParser = fileParser);
            if(DataContext is LogInVM logInVM)
            {
                logInVM.LoginSucces += OpenMenuWindow;
                logInVM.LoginDenied += OpenErrorWindow;
            }
        }

        /// <summary>
        /// Обработка клика по кнопке отмены.
        /// </summary>
        /// <param name="sender"> Отправитель. </param>
        /// <param name="e"> Объект, содержащий информацию о связанном событии. </param>
        private void Form__button_Cancellation_Click(object sender, RoutedEventArgs e) => this.Close();

        /// <summary>
        /// Запуск таймера при загрузки страницы, вывод версии приложения.
        /// </summary>
        /// <param name="sender"> Отправитель. </param>
        /// <param name="e"> Объект, содержащий информацию о связанном событии. </param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            Header__text_version.Content = "Версия " +  Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Обработка тиков таймера.
        /// </summary>
        /// <param name="sender"> Отправитель. </param>
        /// <param name="e"> Объект, содержащий информацию о связанном событии. </param>
        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            CapslockCheck();
            LanguageCheck();
        }

        /// <summary>
        /// Проверка статуса CapsLock.
        /// </summary>
        private void CapslockCheck()
        {
            if (Console.CapsLock)
                Footer__text_capslock.Content = "Клавиша Capslock нажата";
            else
                Footer__text_capslock.Content = "Клавиша Capslock не нажата";
        }

        /// <summary>
        /// Проверка раскладки.
        /// </summary>
        private void LanguageCheck()
        {
            Footer__text_language.Content = "Язык ввода " + Languages[InputLanguageManager.Current.CurrentInputLanguage.ToString()];
        }

        /// <summary>
        /// Обработка изменения пароля в поле ввода.
        /// </summary>
        /// <param name="sender"> Отправитель. </param>
        /// <param name="e"> Объект, содержащий информацию о связанном событии. </param>
        private void Form__input_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LogInVM loginVM)
                loginVM.Password = Form__input_Password.Password;
        }

        /// <summary>
        /// Открытие окна меню.
        /// </summary>
        private void OpenMenuWindow()
        {
            MenuWindow menuWindow = new MenuWindow(fileParser);
            menuWindow.Show();
            dispatcherTimer.Stop();
            Close();
        }

        /// <summary>
        /// Открытие окна ошибки.
        /// </summary>
        /// <param name="message"> Сообщение об ошибке. </param>
        private void OpenErrorWindow(string message)
        {
            ErrorWindow errorWindow = new ErrorWindow(message);
            errorWindow.ShowDialog();
        }
    }
}
