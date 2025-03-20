using System;

namespace Lab12.ClassLibrary.Utils
{
    public static class DocumentGenerator
    {
        private static readonly Random random = new();
        private static readonly string[] organizations = { 
            "ООО Техника", 
            "ИП Смирнов", 
            "АО Электроника", 
            "ИП roazanas" 
        };
        private static readonly string[] persons = { 
            "Иванов И.И.", 
            "Петров П.П.", 
            "Сидоров С.С.", 
            "Жуланов Н.А." 
        };
        private static readonly string[] authors = persons.Concat(organizations).ToArray();
        private static readonly string[] paymentMethods = { 
            "Наличные", 
            "Карта", 
            "Онлайн", 
            "Криптовалюта" 
        };
        private static readonly string[] productTypes = { 
            "Компьютер", 
            "Телефон", 
            "Планшет", 
            "Ноутбук", 
            "Монитор" 
        };

        private static readonly string[] notebookTitles = new[]
        {
            "Важная заметка",
            "Напоминание",
            "Список дел",
            "Идеи",
            "Мысли"
        };

        private static readonly string[] notebookContent = new[]
        {
            "Не забыть купить молоко",
            "Позвонить маме",
            "Сделать домашнее задание",
            "Записаться к врачу",
            "Подготовиться к презентации"
        };

        private static IUserInterface? userInterface;

        public static void SetUserInterface(IUserInterface ui)
        {
            userInterface = ui;
        }

        private static string ReadFromUser(string prompt)
        {
            userInterface?.WriteColored(prompt);
            return userInterface?.ReadUserString() ?? string.Empty;
        }

        public static Document[] GenerateRandomDocuments(int count)
        {
            Document[] documents = new Document[count];
            for (int i = 0; i < count; i++)
            {
                documents[i] = GenerateRandomDocument();
            }
            return documents;
        }

        public static Document GenerateRandomDocument()
        {
            int type = random.Next(4); // 0 - Invoice, 1 - Receipt, 2 - Bill, 3 - Document
            DateTime date = DateTime.Now.AddDays(-random.Next(30));
            string author = authors[random.Next(authors.Length)];
            
            string prefix = type switch
            {
                0 => "INV",
                1 => "RCP",
                2 => "BIL",
                3 => "DOC",
                _ => throw new ArgumentException("Неизвестный тип документа")
            };
            
            string number = $"{prefix}-{date.Year}-{random.Next(1000, 10000):D4}";

            return type switch
            {
                0 => GenerateRandomInvoice(),
                1 => GenerateRandomReceipt(),
                2 => GenerateRandomBill(),
                3 => new Document(number, date, author),
                _ => throw new ArgumentException("Неизвестный тип документа")
            };
        }

        public static Invoice GenerateRandomInvoice()
        {
            string productType = productTypes[random.Next(productTypes.Length)];
            int quantity = random.Next(1, 100);
            int amount = random.Next(1000, 100000);
            DateTime date = DateTime.Now.AddDays(-random.Next(30));
            string author = authors[random.Next(authors.Length)];
            string number = $"INV-{date.Year}-{random.Next(1000, 10000)}";

            return new Invoice(amount, quantity, productType, number, date, author);
        }

        public static Receipt GenerateRandomReceipt()
        {
            string paymentMethod = paymentMethods[random.Next(paymentMethods.Length)];
            int amount = random.Next(1000, 100000);
            DateTime date = DateTime.Now.AddDays(-random.Next(30));
            string author = authors[random.Next(authors.Length)];
            string number = $"RCP-{date.Year}-{random.Next(1000, 10000)}";

            return new Receipt(amount, paymentMethod, number, date, author);
        }

        public static Bill GenerateRandomBill()
        {
            int amountDue = random.Next(1000, 100000);
            DateTime date = DateTime.Now.AddDays(-random.Next(30));
            DateTime payDate = date.AddDays(random.Next(1, 30));
            string author = authors[random.Next(authors.Length)];
            string number = $"BIL-{date.Year}-{random.Next(1000, 10000)}";

            return new Bill(amountDue, payDate, number, date, author);
        }

        public static Document CreateDocumentFromUserInput()
        {
            string number = ReadFromUser("Введите номер документа: ");
            DateTime date = userInterface?.ReadDate() ?? DateTime.Now;
            string author = ReadFromUser("Введите автора документа: ");

            return new Document(number, date, author);
        }

        public static Invoice CreateInvoiceFromUserInput()
        {
            string number = ReadFromUser("Введите номер документа: ");
            DateTime date = userInterface?.ReadDate() ?? DateTime.Now;
            string author = ReadFromUser("Введите автора документа: ");

            string productType = ReadFromUser("Введите тип товара:\n");
            for (int i = 0; i < productTypes.Length; i++)
            {
                userInterface?.WriteColored($"{i + 1}. {productTypes[i]}\n");
            }
            int productTypeIndex = userInterface?.ReadIntInArray(Enumerable.Range(1, productTypes.Length).ToArray()) ?? 0;
            productType = productTypes[productTypeIndex - 1];

            int quantity = userInterface?.ReadInt() ?? 0;
            int amount = userInterface?.ReadInt() ?? 0;

            return new Invoice(
                amount: amount,
                quantity: quantity,
                productType: productType,
                number: number,
                date: date,
                author: author
            );
        }

        public static Receipt CreateReceiptFromUserInput()
        {
            string number = ReadFromUser("Введите номер документа: ");
            DateTime date = userInterface?.ReadDate() ?? DateTime.Now;
            string author = ReadFromUser("Введите автора документа: ");

            string paymentMethod = ReadFromUser("Введите способ оплаты:\n");
            for (int i = 0; i < paymentMethods.Length; i++)
            {
                userInterface?.WriteColored($"{i + 1}. {paymentMethods[i]}\n");
            }
            int paymentMethodIndex = userInterface?.ReadIntInArray(Enumerable.Range(1, paymentMethods.Length).ToArray()) ?? 0;
            paymentMethod = paymentMethods[paymentMethodIndex - 1];

            int amount = userInterface?.ReadInt() ?? 0;

            return new Receipt(
                amount: amount,
                paymentMethod: paymentMethod,
                number: number,
                date: date,
                author: author
            );
        }

        public static Bill CreateBillFromUserInput()
        {
            string number = ReadFromUser("Введите номер документа: ");
            DateTime date = userInterface?.ReadDate() ?? DateTime.Now;
            string author = ReadFromUser("Введите автора документа: ");

            int amountDue = userInterface?.ReadInt() ?? 0;
            DateTime payDate = userInterface?.ReadDate() ?? DateTime.Now;

            return new Bill(
                amountDue: amountDue,
                payDate: payDate,
                number: number,
                date: date,
                author: author
            );
        }

        public static Notebook CreateNotebookFromUserInput()
        {
            string title = ReadFromUser("Введите заголовок заметки: ");
            string content = ReadFromUser("Введите содержание заметки: ");
            
            return new Notebook
            {
                Title = title,
                Content = content,
                CreationDate = DateTime.Now
            };
        }

        public static Notebook GenerateRandomNotebook()
        {
            Random random = new Random();
            return new Notebook
            {
                Title = notebookTitles[random.Next(notebookTitles.Length)],
                Content = notebookContent[random.Next(notebookContent.Length)],
                CreationDate = DateTime.Now.AddDays(-random.Next(30))
            };
        }
    }
}
