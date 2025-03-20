using Lab10.ClassLibrary;
using Lab10.UI;
using Lab10.ClassLibrary.Utils;

/*
 * Никита Жуланов, лабораторная работа по ООП №10, вариант №8
 */

namespace Lab10
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
        private static List<Document> documents = new();

        private static void InitializeMenus()
        {
            mainMenu = new Menu();

            mainMenu.AddMenuItem(1, "Демо 1 части - виртуальные и невиртуальные функции", DemoPart1);
            mainMenu.AddMenuItem(2, "Демо 2 части - запросы (операторы as и is)", DemoPart2);
            mainMenu.AddMenuItem(3, "Демо 3 части - интерфейсы IClonable, IComparable и IComparer", DemoPart3);
            mainMenu.AddMenuItem(0, "Выход", () => { });
        }

        private static void DemoPart1()
        {
            ColorWriteManager.HandleColorStr("<ATTENTION>Демонстрация работы с массивом объектов разных классов\n");
            Console.WriteLine();
            
            Document[] documents = DocumentGenerator.GenerateRandomDocuments(8);

            // Виртуальные функции
            ColorWriteManager.HandleColorStr("<WHITE>1. Демонстрация работы виртуальных функций (<RED>ToString<WHITE>):\n"
                + "<WHITE>При вызове <RED>ToString<WHITE>() используется механизм позднего связывания, поэтому конкретная реализация метода\n"
                + "<WHITE>определяется во время выполнения программы в зависимости от фактического типа объекта:\n");
            Console.WriteLine();
            
            for (int i = 0; i < documents.Length; i++)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>{i + 1}. {documents[i]}\n", Menu.TabLevel+1, digitColor: "<YELLOW>");
            }
            Console.WriteLine();

            // Невиртуальные функции
            ColorWriteManager.HandleColorStr("<WHITE>2. Демонстрация работы невиртуальных функций (<RED>GetDocumentInfo<WHITE>):\n"
                + "<WHITE>При вызове <RED>GetDocumentInfo<WHITE>() используется раннее связывание - версия метода\n"
                + "<WHITE>определяется компилятором на этапе компиляции в зависимости от типа переменной:\n");
            Console.WriteLine();

            foreach (var doc in documents)
            {
                string typeName = doc.GetType().Name;
                ColorWriteManager.HandleColorStr($"<RED>Объект типа <MAGENTA>{typeName}<WHITE>:\n", Menu.TabLevel+1);

                string info = doc switch
                {
                    Receipt receipt => receipt.GetDocumentInfo(),
                    Bill bill => bill.GetDocumentInfo(),
                    Invoice invoice => invoice.GetDocumentInfo(),
                    _ => doc.GetDocumentInfo() // базовый класс
                };
                
                ColorWriteManager.HandleColorStr($"<WHITE>{info}\n", Menu.TabLevel+2, digitColor: "<YELLOW>");
            }
        }
        
        private static void DemoPart2()
        {
            ColorWriteManager.HandleColorStr("<ATTENTION>Демонстрация работы с запросами (динамическая идентификация типов)\n");
            Console.WriteLine();
            Document[] documents = DocumentGenerator.GenerateRandomDocuments(20);

            ColorWriteManager.HandleColorStr("<WHITE>Сгенерированные документы:\n");
        
            for (int i = 0; i < documents.Length; i++)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>{i + 1}. {documents[i]}\n", 
                    Menu.TabLevel + 1, digitColor: "<YELLOW>");
            }
            Console.WriteLine();

            // Запрос 1: Суммарная стоимость продукции заданного наименования по всем накладным
            ColorWriteManager.HandleColorStr("<WHITE>1 Запрос: Суммарная стоимость продукции заданного наименования по всем накладным:\n", digitColor: "<YELLOW>");
            string[] products = { "компьютер", "телефон", "планшет", "ноутбук", "монитор" };
            ColorWriteManager.HandleColorStr("<WHITE>Доступные варианты:\n", Menu.TabLevel + 1);
            foreach (var product in products)
            {
                ColorWriteManager.HandleColorStr($"<WHITE> - <YELLOW>{product}\n", Menu.TabLevel + 2);
            }
            string productType = UserIO.ReadUserStringInArray(
                products,
                inputColor: ConsoleColor.Yellow
            );

            int totalAmount = DocumentQueries.GetInvoiceTotalAmountByProduct(documents, productType);
            ColorWriteManager.HandleColorStr(
                $"<WHITE>Суммарная стоимость продукции типа <YELLOW>{productType}<WHITE>: <GREEN>{totalAmount} <WHITE>руб.\n",
                Menu.TabLevel + 1
            );
            Console.WriteLine();

            // Запрос 2: Количество чеков на сумму превышающую заданную
            ColorWriteManager.HandleColorStr("<WHITE>2 Запрос: Количество чеков на сумму превышающую заданную:", digitColor: "<YELLOW>");
            int minAmount = UserIO.ReadUserIntInRange(
                leftBound: 0,
                inputColor: ConsoleColor.Yellow
            );

            int count = DocumentQueries.GetReceiptsWithAmountGreaterThan(documents, minAmount);
            ColorWriteManager.HandleColorStr(
                $"<WHITE>Количество чеков на сумму больше <YELLOW>{minAmount}<WHITE>: <GREEN>{count}\n",
                Menu.TabLevel + 1
            );

            Console.WriteLine();

            // Запрос 3: Общая сумма по всем чекам, выписанным в организации
            ColorWriteManager.HandleColorStr("<WHITE>3 Запрос: Общая сумма по всем чекам, выписанным в организации:\n", digitColor: "<YELLOW>");
            string[] organizations = { "ООО Техника", "ИП Смирнов", "АО Электроника", "ИП roazanas" };
            
            ColorWriteManager.HandleColorStr("<WHITE>Доступные организации:\n", Menu.TabLevel + 1);
            foreach (var org in organizations)
            {
                ColorWriteManager.HandleColorStr($"<WHITE> - <YELLOW>{org}\n", Menu.TabLevel + 2);
            }
            string organization = UserIO.ReadUserStringInArray(organizations, ConsoleColor.Yellow);
            int totalReceiptAmount = DocumentQueries.GetTotalReceiptsAmount(documents, organization);
            ColorWriteManager.HandleColorStr($"<WHITE>Общая сумма по всем чекам, выписанным в организации <YELLOW>{organization}<WHITE>: <GREEN>{totalReceiptAmount} <WHITE>руб.\n", 
                Menu.TabLevel + 1);
        }

        private static void DemoPart3()
        {
            ColorWriteManager.HandleColorStr("<ATTENTION>Демонстрация работы с интерфейсами\n");
            Console.WriteLine();

            Document[] documents = DocumentGenerator.GenerateRandomDocuments(8);

            ColorWriteManager.HandleColorStr("<WHITE>Исходный массив документов:\n");
            for (int i = 0; i < documents.Length; i++)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>{i + 1}. {documents[i]}\n", 
                    Menu.TabLevel + 1, digitColor: "<YELLOW>");
            }
            Console.WriteLine();

            // Сортировка через IComparable
            Array.Sort(documents);
            ColorWriteManager.HandleColorStr("<WHITE>Отсортированный массив документов (по сумме - <BLUE>IComparable<WHITE>):\n");
            for (int i = 0; i < documents.Length; i++)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>{i + 1}. {documents[i]}\n", 
                    Menu.TabLevel + 1, digitColor: "<YELLOW>");
            }
            Console.WriteLine();

            // Сортировка через IComparer
            Array.Sort(documents, new DocumentDateComparer());
            ColorWriteManager.HandleColorStr("<WHITE>Отсортированный массив документов (по дате - <BLUE>IComparer<WHITE>):\n");
            for (int i = 0; i < documents.Length; i++)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>{i + 1}. {documents[i]}\n", 
                    Menu.TabLevel + 1, digitColor: "<YELLOW>");
            }

            // Демонстрация поиска
            ColorWriteManager.HandleColorStr("<WHITE>Введите дату для поиска документа (в формате <MAGENTA>дд<WHITE>.<MAGENTA>мм<WHITE>.<MAGENTA>гггг<WHITE>):");
            DateTime searchDate = UserIO.ReadUserDate();

            Document? foundDocument = null;
            DocumentDateComparer docDateComparer = new DocumentDateComparer();
            Document searchDoc = new Document("searchDoc", searchDate, "searchDoc");

            int index = Array.BinarySearch(documents, searchDoc, docDateComparer);
            if (index >= 0)
            {
                foundDocument = documents[index];
            }

            if (foundDocument != null)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>Найден документ с датой {searchDate:dd.MM.yyyy}:\n", 
                    Menu.TabLevel + 1,
                    digitColor: "<YELLOW>");
                ColorWriteManager.HandleColorStr($"<WHITE>{foundDocument}\n", Menu.TabLevel + 2,
                    digitColor: "<YELLOW>");
            }
            else
            {
                ColorWriteManager.HandleColorStr($"<ERROR>Документ с датой {searchDate:dd.MM.yyyy} не найден\n", 
                    Menu.TabLevel + 1);
            }

            // Демонстрация работы с IInit
            Console.WriteLine();
            ColorWriteManager.HandleColorStr("<WHITE>Работа с интерфейсом <BLUE>IInit:\n");
            
            IInit[] initDocuments =
            [
                new Document(),
                new Invoice(),
                new Receipt(),
                new Bill(),
                new Notebook()
            ];
            
            ColorWriteManager.HandleColorStr("<WHITE>Инициализация объектов через <RED>RandomInit<WHITE>():\n");
            foreach (var doc in initDocuments)
            {
                doc.RandomInit();
            }

            for (int i = 0; i < initDocuments.Length; i++)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>{i + 1}. {initDocuments[i]}\n", 
                    Menu.TabLevel + 1, digitColor: "<YELLOW>");
            }

            // Демонстрация глубокого и поверхностного копирования
            Console.WriteLine();
            ColorWriteManager.HandleColorStr("<WHITE>Демонстрация глубокого и поверхностного копирования:\n", digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr("<WHITE>При копировании объектов есть два способа:\n", Menu.TabLevel + 1, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr("<WHITE>1. <RED>Глубокое копирование<WHITE> - создаёт полностью независимую копию объекта\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr("<WHITE>2. <RED>Поверхностное копирование<WHITE> - копирует только ссылки на вложенные объекты\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            Console.WriteLine();

            Document originalDoc = new Document();
            originalDoc.RandomInit();
            Receipt attachedReceipt = new Receipt();
            attachedReceipt.RandomInit();
            originalDoc.applicationReceipt = attachedReceipt;
            
            ColorWriteManager.HandleColorStr("<WHITE>Оригинал:\n", Menu.TabLevel + 1, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr($"<WHITE>Документ: {originalDoc}\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr($"<WHITE>Приложенный чек: {originalDoc.applicationReceipt}\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            Console.WriteLine();

            Document deepCopy = (Document)originalDoc.Clone();
            ColorWriteManager.HandleColorStr("<WHITE>1. Глубокое копирование документа через метод <RED>Clone<WHITE>():\n", Menu.TabLevel + 1, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr("<WHITE>При глубоком копировании создаются новые объекты для всех полей\n\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");

            originalDoc.applicationReceipt.Amount = 2000;
            ColorWriteManager.HandleColorStr("<WHITE>После изменения суммы чека в оригинале:\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr($"<WHITE>Оригинал: {originalDoc.applicationReceipt}\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr($"<WHITE>Глубокий клон: {deepCopy.applicationReceipt}\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr("<WHITE>Изменение оригинала не влияет на клон\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            Console.WriteLine();

            Document shallowCopy = originalDoc.ShallowCopy();
            ColorWriteManager.HandleColorStr("<WHITE>2. Поверхностное копирование документа через метод <RED>ShallowCopy<WHITE>():\n", Menu.TabLevel + 1, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr("<WHITE>При поверхностном копировании копируются только ссылки на объекты\n\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");

            originalDoc.applicationReceipt.Amount = 3000;
            ColorWriteManager.HandleColorStr("<WHITE>После изменения суммы чека в оригинале:\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr($"<WHITE>Оригинал: {originalDoc.applicationReceipt}\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr($"<WHITE>Поверхностный клон: {shallowCopy.applicationReceipt}\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
            ColorWriteManager.HandleColorStr("<WHITE>Изменение оригинала отражается в клоне, так как они используют одну и ту же ссылку\n", Menu.TabLevel + 2, digitColor: "<YELLOW>");
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
            DocumentGenerator.SetUserInterface(new ConsoleUserInterface());
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ColorWriteManager.HandleColorStr("<YELLOW>Никита Жуланов<DGREEN>, лабораторная работа по ООП <YELLOW>№10<DGREEN>, вариант <YELLOW>№8\n\n");
            InitializeMenus();
            CurrentMenu = mainMenu;
            RunMainMenu();
        }
    }
}
