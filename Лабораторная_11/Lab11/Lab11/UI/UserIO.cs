using System;
using Lab11.ClassLibrary;

namespace Lab11.UI
{
    public static class UserIO
    {
        public static string GetTabs(uint? tabCount = null, char? bullet = '•')
        {
            string result = "";
            uint actualTabCount = tabCount ?? Menu.TabLevel;
            if (bullet != null && actualTabCount > 0)
                result += bullet;
            for (int i = 0; i < actualTabCount; i++)
            {
                result += "\t";
            }
            return result;
        }

        public static string? GetUserInput(char sign = '?', ConsoleColor inputColor = ConsoleColor.Magenta)
        {
            Console.WriteLine();
            ColorWriteManager.HandleColorStr($"<BLACK><bWHITE>{sign}:<RESET> ");
            Console.ForegroundColor = inputColor;
            string? input = Console.ReadLine();
            Console.ResetColor();
            return input;
        }

        public static bool IsNotInRange(double toCheck,
                                        double leftBound = double.NegativeInfinity,
                                        double rightBound = double.PositiveInfinity)
        {
            if (!(leftBound <= toCheck && toCheck <= rightBound))
            {
                ColorWriteManager.HandleColorStr($"<ERROR>Значение <WHITE>{toCheck} <ERROR>не находится в диапазоне <YELLOW>" 
                    + (leftBound == double.NegativeInfinity ? "(" : "[") 
                    + $"<WHITE>{leftBound}<YELLOW>; <WHITE>{rightBound}<YELLOW>" 
                    + (rightBound == double.PositiveInfinity ? ")" : "]"));
                return true;
            }
            return false;
        }

        public static bool IsNotInArray(double toCheck, params int[] values)
        {
            if (!values.Contains((int)toCheck))
            {
                ColorWriteManager.HandleColorStr($"<ERROR>Значение <WHITE>{toCheck} <ERROR>не находится среди следующих значений: "
                    + "<YELLOW>["
                    + $"<WHITE>{string.Join("<YELLOW>, <WHITE>", values)}"
                    + "<YELLOW>]");
                return true;
            }
            return false;
        }

        public static bool IsNotInArray(string toCheck, params string[] values)
        {
            if (!values.Any(v => v.ToLower() == toCheck.ToLower()))
            {
                ColorWriteManager.HandleColorStr($"<ERROR>Строка <WHITE>{toCheck} <ERROR>не находится среди следующих строк: "
                    + "<YELLOW>["
                    + $"<WHITE>{string.Join("<YELLOW>, <WHITE>", values)}"
                    + "<YELLOW>]");
                return true;
            }
            return false;
        }

        public static int ReadUserInt(ConsoleColor inputColor = ConsoleColor.Magenta)
        {
            bool intExit = false;
            string? strValue;
            int value = 0;

            while (!intExit)
            {
                strValue = GetUserInput('?', inputColor);
                intExit = ConvertToInt(strValue, out value);
            }

            return value;
        }

        public static int ReadUserIntInRange(double leftBound = double.NegativeInfinity,
                                             double rightBound = double.PositiveInfinity,
                                             ConsoleColor inputColor = ConsoleColor.Magenta)
        {
            bool exit = false;
            int value = 0;

            while (!exit)
            {
                value = ReadUserInt(inputColor);
                exit = !IsNotInRange(value, leftBound, rightBound);
            }

            return value;
        }

        public static int ReadUserIntInArray(int[] values, ConsoleColor inputColor = ConsoleColor.Magenta)
        {
            bool exit = false;
            int value = 0;

            while (!exit)
            {
                value = ReadUserInt(inputColor);
                exit = !IsNotInArray(value, values);
            }

            return value;
        }

        public static string ReadUserStringInArray(string[] values, ConsoleColor inputColor = ConsoleColor.Magenta)
        {
            bool exit = false;
            string? value;

            while (!exit)
            {
                value = GetUserInput('?', inputColor);
                if (value != null)
                {
                    exit = !IsNotInArray(value, values);
                    if (exit) return value;
                }
            }

            return string.Empty;
        }

        public static string ReadUserString(ConsoleColor inputColor = ConsoleColor.Magenta)
        {
            string? value = GetUserInput('?', inputColor);
            return value ?? string.Empty;
        }

        public static bool ConvertToInt(string? value, out int intValue)
        {
            bool isConverted = int.TryParse(value, out intValue);
            if (!isConverted)
            {
                ColorWriteManager.HandleColorStr($"<ERROR>Значение <WHITE>{value} <ERROR>не является целым числом");
            }
            return isConverted;
        }

        public static DateTime ReadUserDate(ConsoleColor inputColor = ConsoleColor.Magenta)
        {
            bool dateExit = false;
            string? strValue;
            DateTime value = DateTime.Now;

            while (!dateExit)
            {
                strValue = GetUserInput('?', inputColor);
                dateExit = ConvertToDate(strValue, out value);
            }

            return value;
        }

        public static bool ConvertToDate(string? value, out DateTime dateValue)
        {
            bool isConverted = DateTime.TryParse(value, out dateValue);
            if (!isConverted)
            {
                ColorWriteManager.HandleColorStr($"<ERROR>Значение <WHITE>{value} <ERROR>не является корректной датой. " +
                    "Используйте формат дд.мм.гггг");
            }
            return isConverted;
        }
    }
}