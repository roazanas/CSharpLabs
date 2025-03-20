using System;
using Lab3.Helpers;

/*
 Никита Жуланов, лабораторная работа по ООП №3, вариант №8
 */

namespace Lab3
{
    class Program
    {
        const double a = 0.1;
        const double b = 0.8;
        const int n = 40;
        const double epsilon = 1e-4;    // 0,0001
        const int k = 10;
        const double step = (b - a) / k;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ShowGreetings();

            for (double x = a; x <= b; x += step)
            {
                double sn = CalculateSumForN(x, n);
                double se = CalculateSumForEpsilon(x, epsilon);
                double y = GetExactFunctionValue(x);

                Utils.ColorWrite("X = ");  Utils.ColorWrite($"{x:F4}\t",  ConsoleColor.White);
                Utils.ColorWrite("SN = "); Utils.ColorWrite($"{sn:F6}\t", ConsoleColor.White);
                Utils.ColorWrite("SE = "); Utils.ColorWrite($"{se:F6}\t", ConsoleColor.White);
                Utils.ColorWrite("Y = ");  Utils.ColorWrite($"{y:F6}\n",  ConsoleColor.White);
            }
        }

        static void ShowGreetings()
        {
            Utils.ColorWrite("Никита Жуланов, лабораторная работа по ООП №3, вариант №8\n\n");
            Utils.ColorWrite("Вычисление функции с разложением в степенной ряд для функции: ");
            Utils.ColorWriteLine("y = (x * sin(π/4)) / (1 - 2x * cos(π/4) + x^2)", ConsoleColor.Yellow);

            Utils.ColorWrite("Формула общего члена суммы: ");
            Utils.ColorWriteLine("x^n * sin(n * π/4))", ConsoleColor.Yellow);
            Utils.ColorWrite("Ссылка на визуализацию разложения в степенной ряд: ");
            Utils.ColorWriteLine("https://www.desmos.com/calculator/ugkolq7nti?lang=ru\n", ConsoleColor.DarkBlue);


            Utils.ColorWrite("Параметры: n = "); Utils.ColorWrite(n, ConsoleColor.Green);
            Utils.ColorWrite(", ε = "); Utils.ColorWriteLine(epsilon.ToString("G"), ConsoleColor.Green);
            Utils.ColorWriteLine();
        }

        static double Phi(double x, int n)
        {
            return Math.Sin(n * Math.PI / 4);
        }

        static double CalculateSumForN(double x, int n)
        {
            double sum = 0.0;
            double c_n = x; // Начальное значение c0

            for (int k = 1; k <= n; k++)
            {
                double a_n = Phi(x, k) * c_n;
                sum += a_n;
                c_n *= x; // Рекуррентное обновление c_n
            }
            return sum;
        }

        static double CalculateSumForEpsilon(double x, double epsilon)
        {
            double sum = 0.0;
            double member;
            double c_n = x; // Начальное значение c0
            int k = 1;

            do
            {
                double a_n = Phi(x, k) * c_n;
                member = a_n;
                sum += member;
                c_n *= x; // Рекуррентное обновление c_n
                //Utils.ColorWriteLine($"|R_{k}| = {Math.Abs(member):F20}", ConsoleColor.Green);
                k++;
            } while (Math.Abs(member) > epsilon);

            return sum;
        }

        static double GetExactFunctionValue(double x)
        {
            return (x * Math.Sin(Math.PI / 4))
                 / (1 - 2 * x * Math.Cos(Math.PI / 4) + Math.Pow(x, 2));
        }
    }
}
