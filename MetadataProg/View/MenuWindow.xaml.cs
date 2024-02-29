using MetadataProg.Data;
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

namespace MetadataProg.View
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        IFileParser fileParser;
        public MenuWindow(IFileParser fileParser)
        {
            InitializeComponent();
            this.fileParser = fileParser;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateNavigation(fileParser.MenuItems);

        }

        private void CreateNavigation(string[][] config)
        {
            MenuItem prevEl = null;
            //var prevEl = new MenuItem();
            //int prevIndex = -1;
            for (int i = 0; i< config.Length; i++)
            {
                if (config[i][0] == "0")
                {
                    if(prevEl != null)
                        Navigation.Items.Add(prevEl);
                    prevEl = DrawMenuItem(config[i][1]);
                    //prevIndex = i;
                }
                else
                {
                    prevEl.Items.Add(DrawMenuItem(config[i][1]));
                }
            }
        }

        private MenuItem DrawMenuItem(string text)
        {
            MenuItem menuItem = new();
            menuItem.Header = text;
            return menuItem;
        }
    }
}
