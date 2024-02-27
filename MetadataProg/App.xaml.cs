using MetadataProg.View;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MetadataProg
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LogIn logIn = new LogIn();
            logIn.Show();
        }
    }
}
