using System;

namespace Lab5.Helpers
{
    public static class Utils
    {
        public static bool ConvertToInt(string? value, out int intValue)
        {
            bool isConverted = int.TryParse(value, out intValue);
            if (!isConverted)
            {
                ColorWriteLine($"Кажется, ваше значение не является целым числом: {value}", 
                               ConsoleColor.White, ConsoleColor.Red);
                Console.WriteLine();
            }
            return isConverted;
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

        public static bool IsNotInArea(double toCheck, 
                                       double leftBound = double.NegativeInfinity, 
                                       double rightBound = double.PositiveInfinity)
        {
            string strLeftBound = leftBound == double.NegativeInfinity ? "(": "[" + leftBound.ToString();
            string strRightBound = rightBound.ToString() + (rightBound == double.PositiveInfinity ? ")" : "]");
            if (!(leftBound <= toCheck && toCheck <= rightBound))
            {
                ColorWriteLine($"Значение {toCheck} находится вне диапазона {strLeftBound}; {strRightBound}",
                               ConsoleColor.White, ConsoleColor.Red);
                Console.WriteLine();
            }
            return !(leftBound <= toCheck && toCheck <= rightBound);
        }

        public static string GetTabs(uint tabCount, char? bullet = '-')
        {
            string result = new('\t', (int)tabCount);
            if (tabCount != 0 && bullet != null)
                result = bullet + result;
            return result;
        }

        public static string GetWordInForm(string wordBase, uint count)
        {
            string additionalLetters = "";
            if (count < 2) return wordBase;
            if (count < 5) additionalLetters = "а";
            if (count >= 5) additionalLetters = "ов";
            return wordBase + additionalLetters;
        }

        public static string? GetUserInput(uint tabLevel = 0, char? sign = '?')
        {
            Console.Write(GetTabs(tabLevel) + sign + ": ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            string? input = Console.ReadLine();
            Console.ResetColor();
            return input;
        }
    }
}
