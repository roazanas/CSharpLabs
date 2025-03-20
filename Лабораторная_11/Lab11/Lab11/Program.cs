using System;
using Lab11.UI;
using Lab11.ClassLibrary;
using Lab11.ClassLibrary.Utils;

namespace Lab11
{
    internal class Program
    {
        private static Menu? currentMenu;
        public static Menu? CurrentMenu 
        { 
            get => currentMenu;
            set => currentMenu = value;
        }
        private static Menu? mainMenu;

        private static void InitializeMenus()
        {
            mainMenu = new Menu();

            mainMenu.AddMenuItem(1, "3 задание", DemoTask3);
            mainMenu.AddMenuItem(0, "Выход", () => { });
        }

        private static void DemoTask3()
        {
            ColorWriteManager.HandleColorStr("<ATTENTION>Демонстрация работы с коллекциями и измерение времени поиска\n");
            Console.WriteLine();

            ColorWriteManager.HandleColorStr("<WHITE>Создание коллекций с <YELLOW>1000<WHITE> элементами...\n", Menu.TabLevel + 1);
            var collections = new TestCollections(1000);
            Console.WriteLine();

            ColorWriteManager.HandleColorStr("<WHITE>Тестирование поиска элементов в коллекциях:\n", Menu.TabLevel + 1);
            ColorWriteManager.HandleColorStr("<WHITE>1. <MAGENTA>Поиск<WHITE> в <RED>List<WHITE><<MAGENTA>Receipt<WHITE>><WHITE> через <MAGENTA>Contains<WHITE>\n", Menu.TabLevel + 2);
            ColorWriteManager.HandleColorStr("<WHITE>2. <MAGENTA>Поиск<WHITE> в <RED>List<WHITE><<MAGENTA>string<WHITE>><WHITE> через <MAGENTA>Contains<WHITE>\n", Menu.TabLevel + 2);
            ColorWriteManager.HandleColorStr("<WHITE>3. <MAGENTA>Поиск<WHITE> по ключу в <RED>SortedDictionary<WHITE><<MAGENTA>Document<WHITE>, <MAGENTA>Receipt<WHITE>><WHITE> через <MAGENTA>ContainsKey<WHITE>\n", Menu.TabLevel + 2);
            ColorWriteManager.HandleColorStr("<WHITE>4. <MAGENTA>Поиск<WHITE> по ключу в <RED>SortedDictionary<WHITE><<MAGENTA>string<WHITE>, <MAGENTA>Receipt<WHITE>><WHITE> через <MAGENTA>ContainsKey<WHITE>\n", Menu.TabLevel + 2);
            ColorWriteManager.HandleColorStr("<WHITE>5. <MAGENTA>Поиск<WHITE> по значению в <RED>SortedDictionary<WHITE><<MAGENTA>Document<WHITE>, <MAGENTA>Receipt<WHITE>><WHITE> через <MAGENTA>ContainsValue<WHITE>\n", Menu.TabLevel + 2);
            Console.WriteLine();

            var results = collections.RunAllSearchTests();

            foreach (var result in results)
            {
                ColorWriteManager.HandleColorStr($"\n<BLUE>Тест: <WHITE>{result.Key}\n", Menu.TabLevel + 1);

                var times = new[]
                {
                    (name: "List<Receipt>", time: result.Value.ReceiptListTime.Ticks),
                    (name: "List<string>", time: result.Value.StringListTime.Ticks),
                    (name: "SortedDictionary<Document, Receipt> (ключ)", time: result.Value.DocDictKeyTime.Ticks),
                    (name: "SortedDictionary<string, Receipt> (ключ)", time: result.Value.StrDictKeyTime.Ticks),
                    (name: "SortedDictionary<Document, Receipt> (значение)", time: result.Value.DocDictValueTime.Ticks)
                };

                var minTime = times.Min(x => x.time);
                var maxTime = times.Max(x => x.time);
                var fastestCollection = times.First(x => x.time == minTime).name;
                var slowestCollection = times.First(x => x.time == maxTime).name;

                ColorWriteManager.HandleColorStr("<WHITE>1. <MAGENTA>Поиск<WHITE> в <RED>List<WHITE><<MAGENTA>Receipt<WHITE>><WHITE>: \n", Menu.TabLevel + 2);
                ColorWriteManager.HandleColorStr($"{result.Value.ReceiptListTime.Ticks} <WHITE>тиков\n", Menu.TabLevel + 3);

                ColorWriteManager.HandleColorStr("<WHITE>2. <MAGENTA>Поиск<WHITE> в <RED>List<WHITE><<MAGENTA>string<WHITE>><WHITE>: \n", Menu.TabLevel + 2);
                ColorWriteManager.HandleColorStr($"{result.Value.StringListTime.Ticks} <WHITE>тиков\n", Menu.TabLevel + 3);

                ColorWriteManager.HandleColorStr("<WHITE>3. <MAGENTA>Поиск<WHITE> по ключу в <RED>SortedDictionary<WHITE><<MAGENTA>Document<WHITE>, <MAGENTA>Receipt<WHITE>><WHITE>: \n", Menu.TabLevel + 2);
                ColorWriteManager.HandleColorStr($"{result.Value.DocDictKeyTime.Ticks} <WHITE>тиков\n", Menu.TabLevel + 3);

                ColorWriteManager.HandleColorStr("<WHITE>4. <MAGENTA>Поиск<WHITE> по ключу в <RED>SortedDictionary<WHITE><<MAGENTA>string<WHITE>, <MAGENTA>Receipt<WHITE>><WHITE>: \n", Menu.TabLevel + 2);
                ColorWriteManager.HandleColorStr($"{result.Value.StrDictKeyTime.Ticks} <WHITE>тиков\n", Menu.TabLevel + 3);

                ColorWriteManager.HandleColorStr("<WHITE>5. <MAGENTA>Поиск<WHITE> по значению в <RED>SortedDictionary<WHITE><<MAGENTA>Document<WHITE>, <MAGENTA>Receipt<WHITE>><WHITE>: \n", Menu.TabLevel + 2);
                ColorWriteManager.HandleColorStr($"{result.Value.DocDictValueTime.Ticks} <WHITE>тиков\n", Menu.TabLevel + 3);

                ColorWriteManager.HandleColorStr("<WHITE>6. <MAGENTA>Результат поиска<WHITE>: \n", Menu.TabLevel + 2);
                ColorWriteManager.HandleColorStr(result.Value.Found ? "<SUCCESS>найден" : "<WARNING>не найден", Menu.TabLevel + 3);
                Console.WriteLine();

                ColorWriteManager.HandleColorStr("\n<WHITE>Анализ производительности:\n", Menu.TabLevel + 2);
                ColorWriteManager.HandleColorStr($"<SUCCESS>Самая быстрая коллекция: {fastestCollection} ({minTime} тиков)\n", Menu.TabLevel + 3);
                ColorWriteManager.HandleColorStr($"<WARNING>Самая медленная коллекция: {slowestCollection} ({maxTime} тиков)\n", Menu.TabLevel + 3);
                Console.WriteLine();
            }
        }

        private static void RunMainMenu()
        {
            if (currentMenu == null) return;

            while (true)
            {
                currentMenu.DisplayMenu();
                int choice = currentMenu.GetUserChoice();

                if (choice == 0)
                {
                    if (currentMenu.PastMenu == null)
                    {
                        return;
                    }
                    currentMenu = currentMenu.PastMenu;
                    Menu.TabLevel--;
                    Console.WriteLine();
                }
                else
                {
                    currentMenu.GoToSubmenuOrAction(choice);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ColorWriteManager.HandleColorStr("<YELLOW>Никита Жуланов<DGREEN>, лабораторная работа по ООП <YELLOW>№11<DGREEN>, вариант <YELLOW>№8\n\n");
            InitializeMenus();
            CurrentMenu = mainMenu;
            RunMainMenu();
        }
    }
}