using MetadataProg.Data;
using MetadataProg.View;
using System.Windows;

namespace MetadataProg
{
    /// <summary>
    /// Логика взаимодейсвия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IFileParser fileParser;

        public App() : base()
        {
            fileParser = FileParser.Instance("menu.txt","USERS.txt");
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LogIn logIn = new LogIn(fileParser);
            logIn.Show();
        }
    }
}
