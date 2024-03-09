using MetadataProg.Data;
using MetadataProg.ViewModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MetadataProg.View
{
    /// <summary>
    /// Окно входа в приложение
    /// </summary>
    public partial class LogIn : Window
    {
        IFileParser fileParser;
        /// <summary>
        /// Словарик для вывода раскладки пользователя
        /// </summary>
        readonly Dictionary<string, string> Languages = new Dictionary<string, string>()
        {
            {"ru-RU","Русский"},
            {"en-US","Английский"},
        };

        /// <summary>
        /// Таймер для проверки статуса CapsLock и раскладки пользователся
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

        private void Form__button_Cancellation_Click(object sender, RoutedEventArgs e) => this.Close();

        // Запускает таймер при загрузке страницы и пишет версию приложения
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            Header__text_version.Content = "Версия " +  Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        // Обработка тиков таймера
        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            CapslockCheck();
            LanguageCheck();
        }

        
        private void CapslockCheck()
        {
            if (Console.CapsLock)
                Footer__text_capslock.Content = "Клавиша Capslock нажата";
            else
                Footer__text_capslock.Content = "Клавиша Capslock не нажата";
        }

        private void LanguageCheck()
        {
            Footer__text_language.Content = "Язык ввода " + Languages[InputLanguageManager.Current.CurrentInputLanguage.ToString()];
        }


        private void Form__input_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LogInVM loginVM)
                loginVM.Password = Form__input_Password.Password;
        }

        private void OpenMenuWindow()
        {
            MenuWindow menuWindow = new MenuWindow(fileParser);
            menuWindow.Show();
            dispatcherTimer.Stop();
            Close();
        }
        private void OpenErrorWindow(string message)
        {
            ErrorWindow errorWindow = new ErrorWindow(message);
            errorWindow.ShowDialog();
        }
    }
}
