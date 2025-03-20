using System;
using static System.Math;
using Lab1.Helpers;

namespace Lab1.Tasks
{
    public static class Task1
    {
        public static bool PerformTask1()
        {
            Console.WriteLine("\t\tЗадача 1:\n");
            int m, n;
            double x;
            if (!Utils.ConvertToInt(Utils.GetValueString("m"), out m) 
                || !Utils.ConvertToInt(Utils.GetValueString("n"), out n) 
                || !Utils.ConvertToDouble(Utils.GetValueString("x", "double"), out x))
            {
                return false;
            }

            if (Utils.IsRestrictedInt(m, 0) || Utils.IsRestrictedInt(n, 0))
            {
                return false;
            }

            Console.WriteLine("\n\t\tОтветы на задачу 1:\n");
            Console.WriteLine($"1) n/m++ = {n / m++} (m = {m}; n = {n})");
            m--;        // Коррекция значений, чтобы оставить изначальные значения m и n
            Console.WriteLine($"2) m++<--n = {m++ < --n} (m = {m}; n = {n})");
            m--; n++;
            int intermediateValue = m / n;
            Console.WriteLine($"3) (m/n)++<n/m = {intermediateValue++ < n / m} (m = {m}; n = {n})");
            Console.WriteLine($"4) sqrt(|x^3 - 1|) - 7*cos((x^4 + x)^(1/3)) = " +
                              //$"{Sqrt(Abs(Pow(x, 3) - 1)) - 7 * Cos(Pow(Pow(x, 4) + x, 1.0 / 3.0))}");
                              $"{Sqrt(Abs(Pow(x, 3) - 1)) - 7 * Cos(Utils.Cbrt(Pow(x, 4) + x))}");

            Console.WriteLine("\n");
            return true;
        }
    }
}
