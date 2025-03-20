using System;

namespace Lab3.Helpers
{
    public static class Utils
    {
        public static bool ConvertToInt(string value, out int intValue)
        {
            bool isConverted = int.TryParse(value, out intValue);
            if (!isConverted)
            {
                Console.WriteLine($"Ошибка преобразования в целое число, проверьте ваше значение:\t\t{value}\n");
            }
            return isConverted;
        }

        public static bool ConvertToDouble(string value, out double doubleValue)
        {
            bool isConverted = double.TryParse(value, out doubleValue);
            if (!isConverted)
            {
                Console.WriteLine($"Ошибка преобразования в вещественное число с двойной точностью, проверьте ваше значение:\t{value}\n");
            }
            return isConverted;
        }

        public static bool IsRestrictedInt(int toCheck, int value)
        {
            if (toCheck == value) Console.WriteLine($"Запрещённое целочисленное значение:\t\t{toCheck}\n");
            return toCheck == value;
        }

        public static string GetValueString(string name, string valueType = "int")
        {
            Console.Write($"Введите {name}:\t\t?({valueType}): ");
            return Console.ReadLine();
        }

        public static double Cbrt(double x)
        {
            double factor = x < 0 ? -1 : 1;
            return factor * Math.Pow(Math.Abs(x), 1.0 / 3.0);
        }

        public static void ColorWrite(Object? obj = null, 
                                      ConsoleColor? foregroundColor = null, 
                                      ConsoleColor? backgroundColor = null)
        {
            if (obj == null) return;

            string? text = obj.ToString();
            //Console.ResetColor();
            if (foregroundColor != null)
            { 
                Console.ForegroundColor = (ConsoleColor)foregroundColor; 
            }
            if (backgroundColor != null)
            { 
                Console.BackgroundColor = (ConsoleColor)backgroundColor;
            }
            Console.Write(text);
            Console.ResetColor();
        }

        public static void ColorWriteLine(Object? obj = null,
                                          ConsoleColor? foregroundColor = null,
                                          ConsoleColor? backgroundColor = null)
        {
            ColorWrite(obj, foregroundColor, backgroundColor);
            Console.WriteLine();
        }
    }
}
