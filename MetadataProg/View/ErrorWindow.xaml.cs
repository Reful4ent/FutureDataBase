using System.Windows;

namespace MetadataProg.View
{
    /// <summary>
    /// Окно для вывода ошибки
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
