using System;
using Lab1.Helpers;

namespace Lab1.Tasks
{
    public static class Task2
    {
        public static bool PerformTask2()
        {
            Console.WriteLine("\t\tЗадача 2:\n");
            double x, y;
            if (!Utils.ConvertToDouble(Utils.GetValueString("x", "double"), out x)
                || !Utils.ConvertToDouble(Utils.GetValueString("y", "double"), out y))
            {
                return false;
            }

            bool OK;
            // OK = (x*x + y*y <= 4 && x < 0) || (y >= x-2 && x >= 0 && y <= 0) || (y <= -x+2 && x >= 0 && y > 0);
            if (x < 0)
            {
                OK = x * x + y * y <= 4;
                Console.WriteLine($"(x < 0) x^2 + y^2 <= 4\t\tявляется {OK}");
            }
            else
            {
                if (y < 0)
                {
                    OK = y >= x - 2;
                    Console.WriteLine($"(x >= 0 и y < 0) y >= x-2\t\tявляется {OK}");
                }
                else
                {
                    OK = y <= -x + 2;
                    Console.WriteLine($"(x >= 0 и y >= 0) y <= -x+2\t\tявляется {OK}");
                }
            }
            Console.WriteLine($"\n\t\tОтвет на задачу 2: Указанная точка {(OK ? "" : "не ")}содержится в области");
            Console.WriteLine("\n");
            return true;
        }
    }
}
