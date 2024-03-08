using System.Windows;

namespace MetadataProg.View
{
    /// <summary>
    /// Логика взаимодействия для ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(string text)
        {
            InitializeComponent();
            Error_text.Text = text;
        }
    }
}
