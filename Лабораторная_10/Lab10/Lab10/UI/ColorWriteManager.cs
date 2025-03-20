using System;
using System.Text.RegularExpressions;

namespace Lab10.UI
{
    public struct ColorPair
    {
        public ConsoleColor? Foreground { get; set; }
        public ConsoleColor? Background { get; set; }

        public ColorPair(ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            Foreground = foreground;
            Background = background;
        }
    }

    public static class CColors
    {
        private static readonly Dictionary<string, ColorPair> _colorDict = new();

        public static Dictionary<string, ColorPair> ColorDict => _colorDict;

        // Метод для регистрации цвета
        private static ColorPair RegisterColor(string name, ColorPair color)
        {
            _colorDict[name] = color;
            return color;
        }

        // Цвета текста (foreground)
        public static readonly ColorPair BLACK = RegisterColor(nameof(BLACK), new(ConsoleColor.Black));
        public static readonly ColorPair DBLUE = RegisterColor(nameof(DBLUE), new(ConsoleColor.DarkBlue));
        public static readonly ColorPair DGREEN = RegisterColor(nameof(DGREEN), new(ConsoleColor.DarkGreen));
        public static readonly ColorPair DCYAN = RegisterColor(nameof(DCYAN), new(ConsoleColor.DarkCyan));
        public static readonly ColorPair DRED = RegisterColor(nameof(DRED), new(ConsoleColor.DarkRed));
        public static readonly ColorPair DMAGENTA = RegisterColor(nameof(DMAGENTA), new(ConsoleColor.DarkMagenta));
        public static readonly ColorPair DYELLOW = RegisterColor(nameof(DYELLOW), new(ConsoleColor.DarkYellow));
        public static readonly ColorPair GRAY = RegisterColor(nameof(GRAY), new(ConsoleColor.Gray));
        public static readonly ColorPair DGRAY = RegisterColor(nameof(DGRAY), new(ConsoleColor.DarkGray));
        public static readonly ColorPair BLUE = RegisterColor(nameof(BLUE), new(ConsoleColor.Blue));
        public static readonly ColorPair GREEN = RegisterColor(nameof(GREEN), new(ConsoleColor.Green));
        public static readonly ColorPair CYAN = RegisterColor(nameof(CYAN), new(ConsoleColor.Cyan));
        public static readonly ColorPair RED = RegisterColor(nameof(RED), new(ConsoleColor.Red));
        public static readonly ColorPair MAGENTA = RegisterColor(nameof(MAGENTA), new(ConsoleColor.Magenta));
        public static readonly ColorPair YELLOW = RegisterColor(nameof(YELLOW), new(ConsoleColor.Yellow));
        public static readonly ColorPair WHITE = RegisterColor(nameof(WHITE), new(ConsoleColor.White));

        // Цвета фона (background)
        public static readonly ColorPair bBLACK = RegisterColor(nameof(bBLACK), new(null, ConsoleColor.Black));
        public static readonly ColorPair bDBLUE = RegisterColor(nameof(bDBLUE), new(null, ConsoleColor.DarkBlue));
        public static readonly ColorPair bDGREEN = RegisterColor(nameof(bDGREEN), new(null, ConsoleColor.DarkGreen));
        public static readonly ColorPair bDCYAN = RegisterColor(nameof(bDCYAN), new(null, ConsoleColor.DarkCyan));
        public static readonly ColorPair bDRED = RegisterColor(nameof(bDRED), new(null, ConsoleColor.DarkRed));
        public static readonly ColorPair bDMAGENTA = RegisterColor(nameof(bDMAGENTA), new(null, ConsoleColor.DarkMagenta));
        public static readonly ColorPair bDYELLOW = RegisterColor(nameof(bDYELLOW), new(null, ConsoleColor.DarkYellow));
        public static readonly ColorPair bGRAY = RegisterColor(nameof(bGRAY), new(null, ConsoleColor.Gray));
        public static readonly ColorPair bDGRAY = RegisterColor(nameof(bDGRAY), new(null, ConsoleColor.DarkGray));
        public static readonly ColorPair bBLUE = RegisterColor(nameof(bBLUE), new(null, ConsoleColor.Blue));
        public static readonly ColorPair bGREEN = RegisterColor(nameof(bGREEN), new(null, ConsoleColor.Green));
        public static readonly ColorPair bCYAN = RegisterColor(nameof(bCYAN), new(null, ConsoleColor.Cyan));
        public static readonly ColorPair bRED = RegisterColor(nameof(bRED), new(null, ConsoleColor.Red));
        public static readonly ColorPair bMAGENTA = RegisterColor(nameof(bMAGENTA), new(null, ConsoleColor.Magenta));
        public static readonly ColorPair bYELLOW = RegisterColor(nameof(bYELLOW), new(null, ConsoleColor.Yellow));
        public static readonly ColorPair bWHITE = RegisterColor(nameof(bWHITE), new(null, ConsoleColor.White));

        // Предопределённые комбинации
        public static readonly ColorPair WARNING = RegisterColor(nameof(WARNING), new(ConsoleColor.Black, ConsoleColor.Yellow));
        public static readonly ColorPair ERROR = RegisterColor(nameof(ERROR), new(ConsoleColor.Black, ConsoleColor.Red));
        public static readonly ColorPair SUCCESS = RegisterColor(nameof(SUCCESS), new(ConsoleColor.Black, ConsoleColor.Green));
        public static readonly ColorPair ATTENTION = RegisterColor(nameof(ATTENTION), new(ConsoleColor.Black, ConsoleColor.White));

        // Сбросы
        public static readonly ColorPair fRESET = RegisterColor(nameof(fRESET), new(ConsoleColor.Gray));
        public static readonly ColorPair bRESET = RegisterColor(nameof(bRESET), new(null, ConsoleColor.Black));
        public static readonly ColorPair RESET = RegisterColor(nameof(RESET), new(ConsoleColor.Gray, ConsoleColor.Black));
    }

    public static class ColorWriteManager
    {
        private static readonly ColorPair DefaultColors = new(ConsoleColor.Gray, ConsoleColor.Black);
        private static ColorPair CurrentColors = DefaultColors;

        // Словарь для хранения всех возможных цветов
        private static Dictionary<string, ColorPair> ColorDict => CColors.ColorDict;

        public static void ColorWrite(object obj, ColorPair colors)
        {
            if (obj == null) return;

            string? text = obj.ToString();
            ApplyColors(colors);
            Console.Write(text);
            ResetColors();
        }

        public static void ColorWriteLine(object obj, ColorPair colors)
        {
            ColorWrite(obj, colors);
            Console.WriteLine();
        }

        private static void ApplyColors(ColorPair colors)
        {
            // Обновление текущих цветов, если они не null
            CurrentColors = new ColorPair(
                colors.Foreground ?? CurrentColors.Foreground,
                colors.Background ?? CurrentColors.Background
            );

            if (CurrentColors.Foreground.HasValue)
                Console.ForegroundColor = CurrentColors.Foreground.Value;
            if (CurrentColors.Background.HasValue)
                Console.BackgroundColor = CurrentColors.Background.Value;
        }

        private static void ResetColors()
        {
            Console.ResetColor();
            CurrentColors = DefaultColors;
        }

        public static void HandleColorStr(string colorStr, uint? tabCount = null, string? digitColor = null)
        {
            // Проверяем, заканчивается ли строка на \n
            bool endsWithNewLine = colorStr.EndsWith("\n");
            
            // Разбиваем строку на линии
            var lines = colorStr.Split('\n');
            
            // Обрабатываем каждую линию
            for (int i = 0; i < lines.Length; i++)
            {
                // Добавляем отступ в начале каждой строки
                if (i > 0) // для всех строк кроме первой
                {
                    // Если это не последняя пустая строка после \n в конце
                    if (!(i == lines.Length - 1 && endsWithNewLine && string.IsNullOrEmpty(lines[i])))
                    {
                        Console.Write("\n" + UserIO.GetTabs(tabCount));
                    }
                    else
                    {
                        Console.Write("\n");
                    }
                }
                else // для первой строки
                {
                    Console.Write(UserIO.GetTabs(tabCount));
                }
                
                var regex = new Regex(@"<(?<color>[a-zA-Z_]+)>");
                var matches = regex.Matches(lines[i]);
                int lastIndex = 0;
                string currentColor = string.Empty;
                ColorPair? currentColors = null;

                foreach (Match match in matches)
                {
                    // Печатаем текст до текущего цветового маркера
                    string precedingText = lines[i].Substring(lastIndex, match.Index - lastIndex);
                    if (!string.IsNullOrEmpty(precedingText))
                    {
                        if (digitColor != null)
                        {
                            var numberRegex = new Regex(@"\b\d+\b");
                            var numberMatches = numberRegex.Matches(precedingText);
                            int textLastIndex = 0;

                            foreach (Match numberMatch in numberMatches)
                            {
                                // Печатаем текст до числа
                                Console.Write(precedingText.Substring(textLastIndex, numberMatch.Index - textLastIndex));
                                
                                // Применяем цвет к числу
                                if (ColorDict.TryGetValue(digitColor.Trim('<', '>'), out ColorPair numberColors))
                                {
                                    ApplyColors(numberColors);
                                }
                                Console.Write(numberMatch.Value);
                                if (currentColors.HasValue)
                                {
                                    ApplyColors(currentColors.Value);
                                }
                                else
                                {
                                    ResetColors();
                                }
                                
                                textLastIndex = numberMatch.Index + numberMatch.Length;
                            }

                            // Печатаем оставшийся текст после последнего числа
                            if (textLastIndex < precedingText.Length)
                            {
                                Console.Write(precedingText.Substring(textLastIndex));
                            }
                        }
                        else
                        {
                            Console.Write(precedingText);
                        }
                    }

                    string colorName = match.Groups["color"].Value;
                    currentColor = colorName;

                    // Проверяем, существует ли такой цвет в словаре
                    if (ColorDict.TryGetValue(colorName, out ColorPair colors))
                    {
                        currentColors = colors;
                        ApplyColors(colors);
                    }
                    else
                    {
                        // Если цвета нет в словаре, выводим тег как есть
                        Console.Write(match.Value);
                    }

                    lastIndex = match.Index + match.Length;
                }

                // Печатаем оставшийся текст после последнего совпадения
                if (lastIndex < lines[i].Length)
                {
                    string remainingText = lines[i].Substring(lastIndex);
                    if (digitColor != null)
                    {
                        var numberRegex = new Regex(@"\b\d+\b");
                        var numberMatches = numberRegex.Matches(remainingText);
                        int textLastIndex = 0;

                        foreach (Match numberMatch in numberMatches)
                        {
                            // Печатаем текст до числа
                            Console.Write(remainingText.Substring(textLastIndex, numberMatch.Index - textLastIndex));
                            
                            // Применяем цвет к числу
                            if (ColorDict.TryGetValue(digitColor.Trim('<', '>'), out ColorPair numberColors))
                            {
                                ApplyColors(numberColors);
                            }
                            Console.Write(numberMatch.Value);
                            if (currentColors.HasValue)
                            {
                                ApplyColors(currentColors.Value);
                            }
                            else
                            {
                                ResetColors();
                            }
                            
                            textLastIndex = numberMatch.Index + numberMatch.Length;
                        }

                        // Печатаем оставшийся текст после последнего числа
                        if (textLastIndex < remainingText.Length)
                        {
                            Console.Write(remainingText.Substring(textLastIndex));
                        }
                    }
                    else
                    {
                        Console.Write(remainingText);
                    }
                }

                ResetColors();
            }
        }
    }
}