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
        readonly Dictionary<string, string> Languages = new Dictionary<string, string>()
        {
            {"ru-RU","Русский"},
            {"en-US","Английский"},
        };
        public LogIn()
        {
            InitializeComponent(); 
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

        private void Form__button_SignUp_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }
    }
}
