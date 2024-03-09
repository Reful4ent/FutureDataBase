using MetadataProg.Data;
using MetadataProg.ViewModel.Services;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MetadataProg.View
{
    /// <summary>
    /// Главное окно
    /// </summary>
    public partial class MenuWindow : Window
    {
        /// <summary>
        /// Содержит в себе конфигурацию меню пользователя
        /// </summary>
        IFileParser? fileParser;
        
        /// <summary>
        /// Родительская кнопка (уровень в иерархии 0)
        /// </summary>
        MenuItem parentItem { get; set; }

        /// <summary>
        /// Список дочерних кнопок
        /// </summary>
        List<MenuItem> parentInventory = new();

        /// <summary>
        /// Список уровней дочерних кнопок
        /// </summary>
        List<int> levelsOfElement = new();

        /// <summary>
        /// Список методов для рефлексии
        /// </summary>
        MethodInfo[] methodInfo = typeof(FunctionsService).GetMethods(BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// Словарь ключ - название кнопки, значение - название метода
        /// </summary>
        Dictionary<string, string> attributes = new Dictionary<string, string>();

        /// <summary>
        /// Названия методов содержащихся в methodInfo
        /// </summary>
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

        /// <summary>
        /// Создает меню с кнопками под конфигурацию пользователя
        /// </summary>
        /// <param name="config"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        private Menu CreateNavigation(string[][] config, Menu menu)
        {
            for (int i = 0; i < config.Length; i++)
            {
                // Если статус элемента 2 - не создает
                if (config[i][2] == "2")
                    continue;

                /* Если статус элемента 0 - создает родительскую (главную кнопку)
                и записывает в соответствие с иерархией в нее другие кнопки */
                if (config[i][0] == "0")
                {

                    if (config[i].Length == 4)
                        parentItem = DrawMenuItem(config[i][1], Convert.ToInt32(config[i][2]), config[i][3]);
                    else
                        parentItem = DrawMenuItem(config[i][1], Convert.ToInt32(config[i][2]), null);

                    /* Если элемент не последний или следующий элемент имеет уровень иерархии 0
                    записывается элемент в меню*/
                    if (i + 1 != config.Length && config[i + 1][0] == "0")
                    {
                        menu.Items.Add(parentItem);
                        continue;
                    }

                    // Если элемент является последним - добавляется в меню
                    if (i + 1 == config.Length)
                    {
                        menu.Items.Add(parentItem);
                        return menu;
                    }

                    i++;

                    /*Добавление в список дочерних элеементов и их уровень иерархии*/
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


        /// <summary>
        /// Создаение иерархии кнопок для главной кнопки с номером уровня 0
        /// </summary>
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

        /// <summary>
        /// Отрисовывает стили кнопки и привязывает методы к ней
        /// </summary>
        /// <param name="text" > Текст на кнопке </param>
        /// <param name="condition"> Статус кнопки </param>
        /// <param name="function"> Метод для кнопки </param>
        /// <returns></returns>
        private MenuItem DrawMenuItem(string text, int condition, string function)
        {
            MenuItem menuItem = new();
            menuItem.Header = text;

            /* Настройки кнопки если статус в файле 1 - кнопка видима
            но не используется (Открывается окно о недоступности вызова).*/
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


        /// <summary>
        /// Вызывает метод данной кнопки, вызов метода осуществляется через рефлексию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
