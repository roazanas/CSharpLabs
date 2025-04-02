using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTable;

namespace Lab13
{
    static class Program
    {
        static void Main()
        {
            var hashtableA = new NewHashTable<string, int>();
            hashtableA.Name = "Хэштаблица A";
            var hashtableB = new NewHashTable<string, int>();
            hashtableB.Name = "Хэштаблица B";

            var journalA = new Journal();
            var journalB = new Journal();

            hashtableA.CollectionCountChanged += journalA.HandleEvent;
            hashtableA.CollectionReferenceChanged += journalA.HandleEvent;

            hashtableA.CollectionReferenceChanged += journalB.HandleEvent;
            hashtableB.CollectionReferenceChanged += journalB.HandleEvent;

            hashtableA.Add("one", 1);
            hashtableA.Add("two", 2);
            hashtableA.Add("three", 3);
            hashtableA["two"] = 22;

            hashtableB.Add("four", 4);
            hashtableB.Add("five", 5);
            hashtableB.Add("six", 6);

            hashtableA.Remove("two");
            hashtableB.Remove("four");

            hashtableB["five"] = 55;
            hashtableB.Add("four", 4);
            hashtableB["four"] = 8;

            Console.WriteLine("Первый журнал (оба события первой коллекции): ");
            Console.WriteLine(journalA + "\n");
            Console.WriteLine("Второй журнал (только CollectionReferenceChanged обеих коллекций): ");
            Console.WriteLine(journalB);
        }
    }
}
