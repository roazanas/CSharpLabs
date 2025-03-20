using System;
using Lab1.Helpers;
using Lab1.Tasks;

/*
 Никита Жуланов, лабораторная работа по ООП №1, вариант №8
 */

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Никита Жуланов, лабораторная работа по ООП №1, вариант №8\n\n");

            while (true)
            {
                Console.Write("Выберите задачу для выполнения (1, 2, 3) или (4 для выполнения всех задач по порядку, -1 для выхода):\n?:");
                string input = Console.ReadLine();
                if (Utils.ConvertToInt(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            if (!Task1.PerformTask1())
                                Console.WriteLine("Задача 1 не выполнена, попробуйте ещё раз.");
                            break;

                        case 2:
                            if (!Task2.PerformTask2())
                                Console.WriteLine("Задача 2 не выполнена, попробуйте ещё раз.");
                            break;

                        case 3:
                            if (!Task3.PerformTask3())
                                Console.WriteLine("Задача 3 не выполнена, попробуйте ещё раз.");
                            break;

                        case 4:
                            RunAllTasks();
                            break;

                        case -1:
                            Console.WriteLine("Выход из программы.");
                            return;

                        default:
                            Console.WriteLine("Неверный выбор, пожалуйста, введите допустимое число\n");
                            break;
                    }
                }
            }
        }

        static void RunAllTasks()
        {
            bool isTask1Successful = false,
                 isTask2Successful = false,
                 isTask3Successful = false;

            do
            {
                isTask1Successful = Task1.PerformTask1();
                if (!isTask1Successful)
                    Console.WriteLine("Задача 1 не выполнена, попробуйте ещё раз\n");
            } while (!isTask1Successful);

            do
            {
                isTask2Successful = Task2.PerformTask2();
                if (!isTask2Successful)
                    Console.WriteLine("Задача 2 не выполнена, попробуйте ещё раз\n");
            } while (!isTask2Successful);

            do
            {
                isTask3Successful = Task3.PerformTask3();
                if (!isTask3Successful)
                    Console.WriteLine("Задача 3 не выполнена, попробуйте ещё раз\n");
            } while (!isTask3Successful);
        }
    }
}
