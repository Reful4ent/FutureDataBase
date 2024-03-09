using System.Windows;

namespace MetadataProg.ViewModel.Services
{
    /// <summary>
    /// Класс с методами навигационного меню главного окна
    /// </summary>
    public class FunctionsService
    {
        public void Others() => MessageBox.Show("Others");
        public void Stuff() => MessageBox.Show("Stuff");
        public void Orders() => MessageBox.Show("Orders");
        public void Docs() => MessageBox.Show("Docs");
        public void Departs() => MessageBox.Show("Departs");
        public void Towns() => MessageBox.Show("Towns");
        public void Posts() => MessageBox.Show("Posts");
        public void Window() => MessageBox.Show("Window");
        public void Content() => MessageBox.Show("Content");
        public void About() => MessageBox.Show("About");
        public void Denied() => MessageBox.Show("Метод недоступен");
    }
}
