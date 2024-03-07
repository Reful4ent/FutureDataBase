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
        IFileParser? fileParser;
        MenuItem parentItem { get; set; }
        List<MenuItem> parentInventory = new();
        List<int> levelsOfElement = new();
        public MenuWindow(IFileParser fileParser)
        {
            InitializeComponent();
            this.fileParser = fileParser;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navigation = CreateNavigation(fileParser.MenuItems,0,Navigation);
        }




        private Menu CreateNavigation(string[][] config, int position, Menu menu)
        {
            for (int i = position; i < config.Length; i++)
            {
                if (config[i][2] == "2")
                    continue;
                if (config[i][0] == "0")
                {
                    parentItem = DrawMenuItem(config[i][1]);
                    if(i + 1 != config.Length && config[i+1][0] == "0")
                    {
                        menu.Items.Add(parentItem);
                        continue;
                    }
                    if (i+1 == config.Length)
                    {
                        menu.Items.Add(parentItem);
                        return menu;
                    }
                    i++;
                    while (config[i][0] != "0")
                    {
                       
                        parentInventory.Add(DrawMenuItem(config[i][1]));
                        levelsOfElement.Add(Convert.ToInt32(config[i][0]));
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
                {
                    start = i + 1;
                }
                if (levelsOfElement[0] == levelsOfElement[1])
                {
                    start = 0;
                }
                if (levelsOfElement[i] > levelsOfElement[i + 1])
                {
                    end = i + 1;
                    for (int j = end-1; j > start-1; j--)
                        parentInventory[end].Items.Add(parentInventory[j]);
                    for (int j = start; j < end; j++)
                    {
                        parentInventory.RemoveAt(start);
                        levelsOfElement.RemoveAt(start);
                    }
                    i = -1;

                }
            }
            for(int i = parentInventory.Count-1;i>=0;i--)
                parentItem.Items.Add(parentInventory[i]);
            parentInventory.Clear();
            levelsOfElement.Clear();

        }



        private MenuItem DrawMenuItem(string text)
        {
            MenuItem menuItem = new();
            menuItem.Header = text;
            return menuItem;
        }
    }
}
//for (int i = position; i < config.Length; i++)
//{
//    if (config[i][2] == "2")
//        continue;
//    if (config[i][0] == "0")
//    {
//        parentItem = DrawMenuItem(config[i][1]);
//        int j = i + 1;
//        if (j + 1 >= config.Length)
//        {
//            menu.Items.Add(parentItem);
//            return menu;
//        }
//        while (config[j][0] != "0")
//        {
//            parentItem.Items.Add(DrawMenuItem(config[j][1]));
//            j++;
//        }
//        menu.Items.Add(parentItem);
//    }
//}
//return menu;

//for (int i = position; i < config.Length; i++)
//{
//    if (config[i][2] == "2")
//        continue;
//    if (config[i][0] == "0")
//        parentItem = DrawMenuItem(config[i][1]);
//    //if (i + 1 == config.Length || config[i + 1][0] == "0")
//    //{
//    //    foreach( var item in childrensItemsArr)
//    //        parentItem.Items.Add(item);
//    //     menu.Items.Add(parentItem);
//    //    Array.Clear(childrensItemsArr);
//    //    position++;
//    //    if (i + 1 == config.Length)
//    //        break;
//    //}
//    //if ((Convert.ToInt32(config[i][0]) < Convert.ToInt32(config[i + 1][0])))
//    //{
//    //    menuItem = DrawMenuItem(config[i + 1][1]);
//    //    position++;
//    //    menu = CreateNavigation(config, ref position, menu, menuItem);
//    //    i = position;
//    //}
//    //else
//    //{
//    //    Array.Resize(ref childrensItemsArr, childrensItemsArr.Length + 1);
//    //    childrensItemsArr[childrensItemsArr.Length - 1] = menuItem;
//    //    position++;
//    //}
//}


//if (i + 1 == config.Length || config[i + 1][0] == "0")
//    menu.Items.Add(menuItem);
//if (Convert.ToInt32(config[i][0]) > Convert.ToInt32(config[i - 1][0]))
//{
//    int j = i;
//    MenuItem Item = DrawMenuItem(config[i - 1][1]);
//    if (j + 1 >= config.Length)
//        return menu;
//    while ((Convert.ToInt32(config[j][0]) > Convert.ToInt32(config[j - 1][0])) && ((Convert.ToInt32(config[j][0]) < Convert.ToInt32(config[j + 1][0]))))
//    {
//        Item.Items.Add(DrawMenuItem(config[j][1]));
//        j++;
//    }
//    position = j;
//    if (j == i) position++;
//    string s = config[j][1];
//    if (((Convert.ToInt32(config[j - 1][0]) < Convert.ToInt32(config[j][0]))))
//        menu = CreateNavigation(config, position, menu, Item);
//    menuItem.Items.Add(Item);
//    menu = CreateNavigation(config, position, menu, menuItem);
//}