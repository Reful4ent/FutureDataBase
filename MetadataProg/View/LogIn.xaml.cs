using MetadataProg.Data;
using MetadataProg.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MetadataProg.View
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        IFileParser fileParser;
        readonly Dictionary<string, string> Languages = new Dictionary<string, string>()
        {
            {"ru-RU","Русский"},
            {"en-US","Английский"},
        };
        public LogIn(IFileParser fileParser)
        {
            InitializeComponent();
            DataContext = new LogInVM(this.fileParser = fileParser);
            if(DataContext is LogInVM logInVM)
            {
                logInVM.LoginSucces += OpenMenuWindow;
            }
        }

        private void Form__button_Cancellation_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

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
            this.Close();
        }
    }
}
