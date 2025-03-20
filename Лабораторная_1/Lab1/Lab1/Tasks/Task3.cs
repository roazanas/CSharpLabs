using System;
using static System.Math;

namespace Lab1.Tasks
{
    public static class Task3
    {
        public static bool PerformTask3()
        {
            Console.WriteLine("\t\tЗадача 3:\n");
            Console.WriteLine($"Выражение: ((a+b)^3 - (a^3)) / (b^3 + 3ab^2 + 3a^2b)\n");

            double aD = 1000.0, bD = 0.0001;
            float aF = 1000f, bF = 0.0001f;

            CalculateExpression(aD, bD, "double");
            Console.WriteLine("\n");
            CalculateExpression(aF, bF, "float");

            Console.WriteLine("\n");
            return true;
        }

        private static bool CalculateExpression<NumType>(NumType a, NumType b, string valueType)
        {
            if (!typeof(NumType).IsValueType)
            {
                Console.WriteLine("Принимаются только типы значений!");
                return false;
            }

            Console.WriteLine($"Вычисления для {valueType}:");

            dynamic aDyn = a;
            dynamic bDyn = b;

            // ((a+b)^3 - (a^3)) /
            // (b^3 + 3ab^2 + 3a^2b)
            dynamic top1 = Pow(aDyn + bDyn, 3);
            dynamic top2 = Pow(aDyn, 3);
            dynamic top = top1 - top2;

            dynamic bottom1 = Pow(bDyn, 3);
            dynamic bottom2 = 3 * aDyn * bDyn * bDyn;
            dynamic bottom3 = 3 * aDyn * aDyn * bDyn;
            dynamic bottom = bottom1 + bottom2 + bottom3;

            dynamic result = top / bottom;

            Console.WriteLine($"a + b = {aDyn + bDyn}\n(a + b)^3 = {top1}\na^3 = {top2}\nЧислитель (верх) = {top}\n");
            Console.WriteLine($"b^3 = {bottom1}\n3ab^2 = {bottom2}\n3a^2b = {bottom3}\nЗнаменатель (низ) = {bottom}\n");
            Console.WriteLine($"\n\t\tОтвет для {valueType} = {result}");
            return true;
        }
    }
}
