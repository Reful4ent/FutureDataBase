using MetadataProg.Data;
using MetadataProg.ViewModel.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        IFileParser? fileParser;
        
        MenuItem parentItem { get; set; }
        List<MenuItem> parentInventory = new();
        List<int> levelsOfElement = new();
        MethodInfo[] methodInfo = typeof(FunctionsService).GetMethods(BindingFlags.Instance | BindingFlags.Public);
        Dictionary<string, string> attributes = new Dictionary<string, string>();
        List<string> methodsName = new List<string>();
            

        public MenuWindow(IFileParser fileParser)
        {
            InitializeComponent();
            this.fileParser = fileParser;
            foreach (var item in methodInfo)
                methodsName.Add(item.Name);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navigation = CreateNavigation(fileParser.MenuItems, Navigation);
        }



        //Cоздает меню
        private Menu CreateNavigation(string[][] config, Menu menu)
        {
            for (int i = 0; i < config.Length; i++)
            {
                if (config[i][2] == "2")
                    continue;

                if (config[i][0] == "0")
                {
                    if (config[i].Length == 4)
                        parentItem = DrawMenuItem(config[i][1], Convert.ToInt32(config[i][2]), config[i][3]);
                    else
                        parentItem = DrawMenuItem(config[i][1], Convert.ToInt32(config[i][2]), null);


                    if (i + 1 != config.Length && config[i + 1][0] == "0")
                    {
                        menu.Items.Add(parentItem);
                        continue;
                    }

                    if (i + 1 == config.Length)
                    {
                        menu.Items.Add(parentItem);
                        return menu;
                    }

                    i++;

                    while (config[i][0] != "0")
                    {
                        if (config[i][2] != "2")
                        {
                            if (config[i].Length == 4)
                                parentInventory.Add(DrawMenuItem(config[i][1], Convert.ToInt32(config[i][2]), config[i][3])); 
                            else
                                parentInventory.Add(DrawMenuItem(config[i][1], Convert.ToInt32(config[i][2]), null));
                            levelsOfElement.Add(Convert.ToInt32(config[i][0]));
                        }

                        i++;

                        if (i == config.Length)
                            break;
                    }

                    i--;
                    EditParentItem();
                    menu.Items.Add(parentItem);
                }
            }

            return menu;
        }

        private void EditParentItem()
        {
            int start = 0,
                end = 0;
            parentInventory.Reverse();
            levelsOfElement.Reverse();
            for (int i = 0; i < levelsOfElement.Count; i++)
            {
                if (i + 1 == levelsOfElement.Count || levelsOfElement[i] == 1)
                {
                    start = i + 1;
                    continue;
                }

                if (levelsOfElement[i] < levelsOfElement[i + 1])
                    start = i + 1;

                if (levelsOfElement[0] == levelsOfElement[1])
                    start = 0;

                if (levelsOfElement[i] > levelsOfElement[i + 1])
                {
                    end = i + 1;
                    for (int j = end - 1; j > start - 1; j--)
                        parentInventory[end].Items.Add(parentInventory[j]);
                    for (int j = start; j < end; j++)
                    {
                        parentInventory.RemoveAt(start);
                        levelsOfElement.RemoveAt(start);
                    }
                    i = -1;
                }
            }

            for (int i = parentInventory.Count - 1; i >= 0; i--)
                parentItem.Items.Add(parentInventory[i]);
            parentInventory.Clear();
            levelsOfElement.Clear();

        }



        private MenuItem DrawMenuItem(string text, int condition, string function)
        {
            MenuItem menuItem = new();
            menuItem.Header = text;

            if (condition == 1)
            {
                menuItem.Foreground = Brushes.LightGray;
                attributes.Add(text, "Denied");
                menuItem.Items.Clear();
            }
            else
                attributes.Add(text, function);
            menuItem.Click += MenuItem_Click;
            return menuItem;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            MenuItem menuItem = sender as MenuItem;
            var myFunc = new FunctionsService();
            int index = methodsName.IndexOf(attributes[menuItem.Header.ToString()]);
            if (index == -1)
                return;
            methodInfo[index]?.Invoke(myFunc, null);
        }

    }
}
