using System;
using static Lab5.Helpers.Utils;

/*
 Никита Жуланов, лабораторная работа по ООП №5, вариант №8
 */

namespace Lab5
{
    class Program
    {
        public const int NumberPadding = 2;
        public const int RandomMin = -9;   
        public const int RandomMax = 9;

        static int[]? oneDArray;
        static int[,]? twoDArray;
        static int[][]? jaggedArray;

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool exit = false;
            ColorWriteLine("Никита Жуланов, лабораторная работа по ООП №5, вариант №8\n\n", ConsoleColor.DarkGreen);

            while (!exit)
            {
                ColorWrite("1. ", ConsoleColor.White); ColorWriteLine("Работа с одномерным массивом");
                ColorWrite("2. ", ConsoleColor.White); ColorWriteLine("Работа с двумерным массивом");
                ColorWrite("3. ", ConsoleColor.White); ColorWriteLine("Работа с рваным массивом");
                ColorWrite("0. ", ConsoleColor.White); ColorWriteLine("Выход", ConsoleColor.DarkRed);
                ColorWriteLine("\nВыберите с каким типом массива провести операцию");

                int choiceInt;
                if (!ConvertToInt(GetUserInput(), out choiceInt)) continue;
                if (IsNotInArea(choiceInt, 0, 3)) continue;

                switch (choiceInt)
                {
                    case 1:
                        OneDimensionalArrayMenu(tabLevel: 1);
                        break;

                    case 2:
                        TwoDimensionalArrayMenu(tabLevel: 1);
                        break;

                    case 3:
                        JaggedArrayMenu(tabLevel: 1);
                        break;

                    case 0:
                        exit = true;
                        break;

                    default:
                        ColorWriteLine("Неверный выбор.", ConsoleColor.DarkRed);
                        break;
                }
            }
        }

        static void PrintArray(int[]? array, uint tabLevel)
        {
            if (array == null || array.Length == 0)
            {
                ColorWriteLine(GetTabs(tabLevel) + "[]", ConsoleColor.Yellow);
                return;
            }

            ColorWrite(GetTabs(tabLevel) + "[", ConsoleColor.Yellow);
            for (int i = 0; i < array.Length; i++)
            {
                ColorWrite($"{array[i], NumberPadding}", ConsoleColor.White);
                if (i < array.Length - 1)
                    ColorWrite(", ", ConsoleColor.Yellow);
            }
            ColorWriteLine("]", ConsoleColor.Yellow);
        }

        static void PrintArray(int[,]? array, uint tabLevel)
        {
            if (array == null || array.Length == 0)
            {
                ColorWriteLine(GetTabs(tabLevel) + "[]", ConsoleColor.Yellow);
                return;
            }

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                ColorWrite(GetTabs(tabLevel) + "[", ConsoleColor.Yellow);
                for (int j = 0; j < cols; j++)
                {
                    ColorWrite($"{array[i, j], NumberPadding}", ConsoleColor.White);
                    if (j < cols - 1)
                        ColorWrite(", ", ConsoleColor.Yellow);
                }
                ColorWriteLine("]", ConsoleColor.Yellow);
            }
        }

        static void PrintArray(int[][]? array, uint tabLevel)
        {
            if (array == null || array.Length == 0)
            {
                ColorWriteLine(GetTabs(tabLevel) + "[]", ConsoleColor.Yellow);
                return;
            }

            foreach (var row in array)
            {
                ColorWrite(GetTabs(tabLevel) + "[", ConsoleColor.Yellow);
                for (int i = 0; i < row.Length; i++)
                {
                    ColorWrite($"{row[i], NumberPadding}", ConsoleColor.White);
                    if (i < row.Length - 1)
                        ColorWrite(", ", ConsoleColor.Yellow);
                }
                ColorWriteLine("]", ConsoleColor.Yellow);
            }
        }

        static void OneDimensionalArrayMenu(uint tabLevel = 1)
        {
            bool exit = false;

            while (!exit)
            {
                ColorWrite(GetTabs(tabLevel) + "1. ", ConsoleColor.White);
                ColorWriteLine("Создать одномерный массив");
                ColorWrite(GetTabs(tabLevel) + "2. ", ConsoleColor.White);
                ColorWriteLine("Напечатать массив");
                ColorWrite(GetTabs(tabLevel) + "3. ", ConsoleColor.White);
                ColorWriteLine("Удалить все элементы с четными номерами");
                ColorWrite(GetTabs(tabLevel) + "0. ", ConsoleColor.White);
                ColorWriteLine("Назад", ConsoleColor.DarkRed);
                ColorWriteLine("\n" + GetTabs(tabLevel) + "Выберите операцию");

                int choiceInt;
                if (!ConvertToInt(GetUserInput(tabLevel), out choiceInt)) continue;
                if (IsNotInArea(choiceInt, 0, 3)) continue;
                ColorWriteLine();

                switch (choiceInt)
                {
                    case 1:
                        oneDArray = CreateOneDimensionalArray(tabLevel + 1);
                        ColorWrite(GetTabs(tabLevel + 1) + "Созданный массив: ", ConsoleColor.DarkGreen);
                        PrintArray(oneDArray, 0);
                        ColorWriteLine();
                        break;

                    case 2:
                        ColorWrite(GetTabs(tabLevel + 1) + "Одномерный массив: ", ConsoleColor.DarkGreen);
                        PrintArray(oneDArray, 0);
                        ColorWriteLine();
                        break;

                    case 3:
                        if (RemoveElementsOnEven(ref oneDArray, tabLevel + 1))
                        {
                            ColorWrite(GetTabs(tabLevel + 1) + "Изменённый массив: ", ConsoleColor.DarkGreen);
                            PrintArray(oneDArray, 0);
                        }
                        else
                        {
                            ColorWriteLine("Удалить элементы невозможно (массив не создан или меньше 2 элементов)",
                                            ConsoleColor.White, ConsoleColor.Red);
                        }
                        ColorWriteLine();
                        break;

                    case 0:
                        exit = true;
                        break;

                    default:
                        ColorWriteLine("Неверный выбор.", ConsoleColor.DarkRed);
                        break;
                }
            }
        }

        static int[] CreateOneDimensionalArray(uint tabLevel = 1)
        {
            int size;
            while (true)
            {
                ColorWriteLine(GetTabs(tabLevel) + "Введите размер массива");
                if (!ConvertToInt(GetUserInput(tabLevel), out size)) continue;
                if (IsNotInArea(size, leftBound: 1)) continue;
                break;
            }

            int[] array = new int[size];

            while (true)
            {
                ColorWriteLine();
                ColorWrite(GetTabs(tabLevel) + "Выберите вести массив вручную ("); ColorWrite(1, ConsoleColor.White);
                ColorWrite(") или автоматически ("); ColorWrite(2, ConsoleColor.White); ColorWriteLine(")");
                int choiceInt;
                if (!ConvertToInt(GetUserInput(tabLevel), out choiceInt)) continue;
                if (IsNotInArea(choiceInt, 1, 2)) continue;
                ColorWriteLine();

                switch (choiceInt)
                {
                    case 1:
                        ColorWriteLine(GetTabs(tabLevel) + $"Введите {size} {GetWordInForm("элемент", (uint)size)}:");
                        for (int i = 0; i < size; i++)
                        {
                            while (true)
                            {
                                if (ConvertToInt(GetUserInput(tabLevel), out array[i])) break;
                            }
                        }
                        ColorWriteLine();
                        break;

                    case 2:
                        Random rnd = new Random();
                        for (int i = 0; i < size; i++)
                        {
                            array[i] = rnd.Next(RandomMin, RandomMax);
                        }
                        break;
                }
                return array;
            }
        }

        static bool RemoveElementsOnEven(ref int[]? array, uint tabLevel = 2)
        {
            if (array == null || array.Length <= 1) return false;

            int[] newArray = new int[(array.Length + 1) / 2];
            for (int i = 0, j = 0; i < array.Length; i += 2)
            {
                newArray[j++] = array[i];
            }

            array = newArray;
            return true;
        }

        static void TwoDimensionalArrayMenu(uint tabLevel = 1)
        {
            bool exit = false;

            while (!exit)
            {
                ColorWrite(GetTabs(tabLevel) + "1. ", ConsoleColor.White);
                ColorWriteLine("Создать двумерный массив");
                ColorWrite(GetTabs(tabLevel) + "2. ", ConsoleColor.White);
                ColorWriteLine("Напечатать массив");
                ColorWrite(GetTabs(tabLevel) + "3. ", ConsoleColor.White);
                ColorWriteLine("Добавить К столбцов в конец матрицы");
                ColorWrite(GetTabs(tabLevel) + "0. ", ConsoleColor.White);
                ColorWriteLine("Назад", ConsoleColor.DarkRed);
                ColorWriteLine("\n" + GetTabs(tabLevel) + "Выберите операцию");

                int choiceInt;
                if (!ConvertToInt(GetUserInput(tabLevel), out choiceInt)) continue;
                if (IsNotInArea(choiceInt, 0, 3)) continue;
                ColorWriteLine();

                switch (choiceInt)
                {
                    case 1:
                        twoDArray = CreateTwoDimensionalArray(tabLevel + 1);
                        ColorWrite(GetTabs(tabLevel + 1) + "Созданный массив:\n", ConsoleColor.DarkGreen);
                        PrintArray(twoDArray, tabLevel + 1);
                        ColorWriteLine();
                        break;

                    case 2:
                        ColorWrite(GetTabs(tabLevel + 1) + "Двумерный массив:\n", ConsoleColor.DarkGreen);
                        PrintArray(twoDArray, tabLevel + 1);
                        ColorWriteLine();
                        break;

                    case 3:
                        if (twoDArray == null || twoDArray.Length == 0)
                        {
                            ColorWriteLine("Массив не создан или пуст",
                                            ConsoleColor.White, ConsoleColor.Red);
                            ColorWriteLine();
                            break;
                        }

                        while (true)
                        {
                            ColorWriteLine(GetTabs(tabLevel + 1) + "Введите K - количество стобцов, которые необходимо добавить");
                            int rowsToAdd;
                            if (!ConvertToInt(GetUserInput(tabLevel + 1), out rowsToAdd)) continue;
                            if (IsNotInArea(rowsToAdd, leftBound: 1)) continue;
                            AddRowsToTwoDimensionalArray(ref twoDArray, (uint)rowsToAdd, tabLevel + 1);
                            ColorWrite(GetTabs(tabLevel + 1) + "Изменённый массив:\n", ConsoleColor.DarkGreen);
                            PrintArray(twoDArray, tabLevel + 1);
                            ColorWriteLine();
                            break;
                        }
                        break;

                    case 0:
                        exit = true;
                        break;

                    default:
                        ColorWriteLine("Неверный выбор.", ConsoleColor.DarkRed);
                        break;
                }
            }
        }

        static int[,] CreateTwoDimensionalArray(uint tabLevel = 1)
        {
            int rows, columns;

            while (true)
            {
                ColorWriteLine(GetTabs(tabLevel) + "Введите количество строк массива");
                if (!ConvertToInt(GetUserInput(tabLevel), out rows)) continue;
                if (IsNotInArea(rows, leftBound: 1)) continue;
                break;
            }

            while (true)
            {
                ColorWriteLine(GetTabs(tabLevel) + "Введите количество столбцов массива");
                if (!ConvertToInt(GetUserInput(tabLevel), out columns)) continue;
                if (IsNotInArea(columns, leftBound: 1)) continue;
                break;
            }

            int[,] array = new int[rows, columns];

            while (true)
            {
                ColorWriteLine();
                ColorWrite(GetTabs(tabLevel) + "Выберите вести массив вручную ("); ColorWrite(1, ConsoleColor.White);
                ColorWrite(") или автоматически ("); ColorWrite(2, ConsoleColor.White); ColorWriteLine(")");
                int choiceInt;
                if (!ConvertToInt(GetUserInput(tabLevel), out choiceInt)) continue;
                if (IsNotInArea(choiceInt, 1, 2)) continue;
                ColorWriteLine();

                switch (choiceInt)
                {
                    case 1:
                        for (int i = 0; i < rows; i++)
                        {
                            ColorWriteLine(GetTabs(tabLevel) + $"({i + 1} строка) Введите {columns} {GetWordInForm("элемент", (uint)columns)}:");
                            for (int j = 0; j < columns; j++)
                            {
                                while (true)
                                {
                                    if (ConvertToInt(GetUserInput(tabLevel), out array[i, j])) break;
                                }
                            }
                            ColorWriteLine();
                        }
                        ColorWriteLine();
                        break;

                    case 2:
                        Random rnd = new Random();
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < columns; j++)
                            {
                                array[i, j] = rnd.Next(RandomMin, RandomMax);
                            }
                        }
                        break;
                }
                return array;
            }
        }

        static void AddRowsToTwoDimensionalArray(ref int[,] array, uint columnsToAdd, uint tabLevel = 2)
        {
            if (array == null) return;

            int rows = array.GetLength(0);
            int oldColumns = array.GetLength(1);
            int[,] newArray = new int[rows, oldColumns + columnsToAdd];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < oldColumns; j++)
                {
                    newArray[i, j] = array[i, j];
                }
            }

            ColorWrite(GetTabs(tabLevel) + "Выберите вести новые элементы вручную ("); ColorWrite(1, ConsoleColor.White);
            ColorWrite(") или автоматически ("); ColorWrite(2, ConsoleColor.White); ColorWriteLine(")");
            int choiceInt;
            if (!ConvertToInt(GetUserInput(tabLevel), out choiceInt)) return;
            if (IsNotInArea(choiceInt, 1, 2)) return;
            ColorWriteLine();

            switch (choiceInt)
            {
                case 1:
                    for (int j = oldColumns; j < oldColumns + columnsToAdd; j++)
                    {
                        ColorWriteLine(GetTabs(tabLevel) + $"({j + 1} столбец) Введите {rows} {GetWordInForm("элемент", (uint)rows)}:");
                        for (int i = 0; i < rows; i++)
                        {
                            if (!ConvertToInt(GetUserInput(tabLevel), out newArray[i, j]))
                            {
                                i--;
                            }
                        }
                        ColorWriteLine();
                    }
                    break;

                case 2:
                    Random rnd = new Random();
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = oldColumns; j < oldColumns + columnsToAdd; j++)
                        {
                            newArray[i, j] = rnd.Next(RandomMin, RandomMax);
                        }
                    }
                    break;
            }

            array = newArray;
        }

        static void JaggedArrayMenu(uint tabLevel = 1)
        {
            bool exit = false;

            while (!exit)
            {
                ColorWrite(GetTabs(tabLevel) + "1. ", ConsoleColor.White);
                ColorWriteLine("Создать рваный массив");
                ColorWrite(GetTabs(tabLevel) + "2. ", ConsoleColor.White);
                ColorWriteLine("Напечатать массив");
                ColorWrite(GetTabs(tabLevel) + "3. ", ConsoleColor.White);
                ColorWriteLine("Удалить первую строку, в которой встречается заданное число K");
                ColorWrite(GetTabs(tabLevel) + "0. ", ConsoleColor.White);
                ColorWriteLine("Назад", ConsoleColor.DarkRed);
                ColorWriteLine("\n" + GetTabs(tabLevel) + "Выберите операцию");

                int choiceInt;
                if (!ConvertToInt(GetUserInput(tabLevel), out choiceInt)) continue;
                if (IsNotInArea(choiceInt, 0, 3)) continue;
                ColorWriteLine();

                switch (choiceInt)
                {
                    case 1:
                        jaggedArray = CreateJaggedArray(tabLevel + 1);
                        ColorWrite(GetTabs(tabLevel + 1) + "Созданный массив:\n", ConsoleColor.DarkGreen);
                        PrintArray(jaggedArray, tabLevel + 1);
                        ColorWriteLine();
                        break;

                    case 2:
                        ColorWrite(GetTabs(tabLevel + 1) + "Рваный массив:\n", ConsoleColor.DarkGreen);
                        PrintArray(jaggedArray, tabLevel + 1);
                        ColorWriteLine();
                        break;

                    case 3:
                        if (jaggedArray == null || jaggedArray.Length == 0)
                        {
                            ColorWriteLine("Массив не создан или пуст",
                                            ConsoleColor.White, ConsoleColor.Red);
                            ColorWriteLine();
                            break;
                        }

                        ColorWriteLine(GetTabs(tabLevel + 1) + "Введите число K для поиска");
                        int k;
                        if (!ConvertToInt(GetUserInput(tabLevel + 1), out k)) continue;

                        if (RemoveRowWithNumber(ref jaggedArray, k, tabLevel + 1))
                        {
                            ColorWrite(GetTabs(tabLevel + 1) + "Изменённый массив:\n", ConsoleColor.DarkGreen);
                            PrintArray(jaggedArray, tabLevel + 1);
                        }
                        else
                        {
                            ColorWriteLine($"Число {k} не найдено в массиве",
                                             ConsoleColor.White, ConsoleColor.Red);
                        }
                        ColorWriteLine();
                        break;

                    case 0:
                        exit = true;
                        break;

                    default:
                        ColorWriteLine("Неверный выбор.", ConsoleColor.DarkRed);
                        break;
                }
            }
        }

        static int[][] CreateJaggedArray(uint tabLevel = 1)
        {
            int rows;
            while (true)
            {
                ColorWriteLine(GetTabs(tabLevel) + "Введите количество строк массива");
                if (!ConvertToInt(GetUserInput(tabLevel), out rows)) continue;
                if (IsNotInArea(rows, leftBound: 1)) continue;
                break;
            }

            int[][] array = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                while (true)
                {
                    ColorWriteLine(GetTabs(tabLevel) + $"Введите количество элементов в строке {i + 1}");
                    int columns;
                    if (!ConvertToInt(GetUserInput(tabLevel), out columns)) continue;
                    if (IsNotInArea(columns, leftBound: 1)) continue;
                    array[i] = new int[columns];
                    break;
                }
            }

            while (true)
            {
                ColorWriteLine();
                ColorWrite(GetTabs(tabLevel) + "Выберите вести массив вручную ("); ColorWrite(1, ConsoleColor.White);
                ColorWrite(") или автоматически ("); ColorWrite(2, ConsoleColor.White); ColorWriteLine(")");
                int choiceInt;
                if (!ConvertToInt(GetUserInput(tabLevel), out choiceInt)) continue;
                if (IsNotInArea(choiceInt, 1, 2)) continue;
                ColorWriteLine();

                switch (choiceInt)
                {
                    case 1:
                        for (int i = 0; i < rows; i++)
                        {
                            ColorWriteLine(GetTabs(tabLevel) + $"({i + 1} строка) Введите {array[i].Length} {GetWordInForm("элемент", (uint)array[i].Length)}:");
                            for (int j = 0; j < array[i].Length; j++)
                            {
                                while (true)
                                {
                                    if (ConvertToInt(GetUserInput(tabLevel), out array[i][j])) break;
                                    ColorWriteLine(GetTabs(tabLevel) + "Некорректный ввод. Повторите попытку.");
                                }
                            }
                            ColorWriteLine();
                        }
                        ColorWriteLine();
                        break;

                    case 2:
                        Random rnd = new Random();
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < array[i].Length; j++)
                            {
                                array[i][j] = rnd.Next(RandomMin, RandomMax);
                            }
                        }
                        break;
                }
                return array;
            }
        }

        static bool RemoveRowWithNumber(ref int[][] array, int number, uint tabLevel = 2)
        {
            if (array == null || array.Length == 0) return false;

            int indexToRemove = -1;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    if (array[i][j] == number)
                    {
                        indexToRemove = i;
                        break;
                    }
                }
                if (indexToRemove != -1) break;
            }

            if (indexToRemove == -1) return false;

            int[][] newArray = new int[array.Length - 1][];
            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (i == indexToRemove) continue;
                newArray[j] = array[i];
                j++;
            }

            array = newArray;
            return true;
        }
    }
}