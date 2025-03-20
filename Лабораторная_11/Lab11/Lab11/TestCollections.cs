using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lab11.ClassLibrary;
using Lab11.ClassLibrary.Utils;

namespace Lab11
{
    public class TestCollections
    {
        public List<Receipt> ReceiptList { get; set; }
        public List<string> StringList { get; set; }
        public SortedDictionary<Document, Receipt> DocumentReceiptDictionary { get; set; }
        public SortedDictionary<string, Receipt> StringReceiptDictionary { get; set; }

        public TestCollections()
        {
            ReceiptList = new List<Receipt>();
            StringList = new List<string>();
            DocumentReceiptDictionary = new SortedDictionary<Document, Receipt>();
            StringReceiptDictionary = new SortedDictionary<string, Receipt>();
        }

        public TestCollections(int count) : this()
        {
            for (int i = 0; i < count; i++)
            {
                Receipt receipt = DocumentGenerator.GenerateRandomReceipt();
                AddElement(receipt);
            }
        }

        public void AddElement(Receipt receipt)
        {
            ReceiptList.Add(receipt);
            StringList.Add(receipt.ToString());
            DocumentReceiptDictionary.Add(receipt.BaseDocument, receipt);
            StringReceiptDictionary.Add(receipt.ToString(), receipt);
        }

        public bool RemoveElement(Receipt receipt)
        {
            bool success = true;
            success &= ReceiptList.Remove(receipt);
            success &= StringList.Remove(receipt.ToString());
            success &= DocumentReceiptDictionary.Remove(receipt.BaseDocument);
            success &= StringReceiptDictionary.Remove(receipt.ToString());
            return success;
        }

        public Receipt AddRandomElement()
        {
            Receipt receipt = DocumentGenerator.GenerateRandomReceipt();
            AddElement(receipt);
            return receipt;
        }

        public void Clear()
        {
            ReceiptList.Clear();
            StringList.Clear();
            DocumentReceiptDictionary.Clear();
            StringReceiptDictionary.Clear();
        }

        public struct SearchResult
        {
            public TimeSpan ReceiptListTime;
            public TimeSpan StringListTime;
            public TimeSpan DocDictKeyTime;
            public TimeSpan StrDictKeyTime;
            public TimeSpan DocDictValueTime;
            public bool Found;
        }

        public SearchResult MeasureSearchTime(Receipt receipt)
        {
            const int measureCount = 5;
            var result = new SearchResult();
            var sw = new Stopwatch();

            long receiptListTicks = 0;
            long stringListTicks = 0;
            long docDictKeyTicks = 0;
            long strDictKeyTicks = 0;
            long docDictValueTicks = 0;

            for (int i = 0; i < measureCount; i++)
            {
                sw.Restart();
                result.Found = ReceiptList.Contains(receipt);
                sw.Stop();
                receiptListTicks += sw.ElapsedTicks;

                sw.Restart();
                result.Found = StringList.Contains(receipt.ToString());
                sw.Stop();
                stringListTicks += sw.ElapsedTicks;

                sw.Restart();
                result.Found = DocumentReceiptDictionary.ContainsKey(receipt.BaseDocument);
                sw.Stop();
                docDictKeyTicks += sw.ElapsedTicks;

                sw.Restart();
                result.Found = StringReceiptDictionary.ContainsKey(receipt.ToString());
                sw.Stop();
                strDictKeyTicks += sw.ElapsedTicks;

                sw.Restart();
                result.Found = DocumentReceiptDictionary.ContainsValue(receipt);
                sw.Stop();
                docDictValueTicks += sw.ElapsedTicks;
            }

            result.ReceiptListTime = TimeSpan.FromTicks(receiptListTicks / measureCount);
            result.StringListTime = TimeSpan.FromTicks(stringListTicks / measureCount);
            result.DocDictKeyTime = TimeSpan.FromTicks(docDictKeyTicks / measureCount);
            result.StrDictKeyTime = TimeSpan.FromTicks(strDictKeyTicks / measureCount);
            result.DocDictValueTime = TimeSpan.FromTicks(docDictValueTicks / measureCount);

            return result;
        }

        public (Receipt First, Receipt Middle, Receipt Last, Receipt NotInCollection) GetTestElements()
        {
            var first = ReceiptList[0];
            var middle = ReceiptList[ReceiptList.Count / 2];
            var last = ReceiptList[ReceiptList.Count - 1];
            var notInCollection = DocumentGenerator.GenerateRandomReceipt();

            return (first, middle, last, notInCollection);
        }

        public Dictionary<string, SearchResult> RunAllSearchTests()
        {
            var results = new Dictionary<string, SearchResult>();
            var elements = GetTestElements();

            results.Add("Первый элемент", MeasureSearchTime(elements.First));
            results.Add("Центральный элемент", MeasureSearchTime(elements.Middle));
            results.Add("Последний элемент", MeasureSearchTime(elements.Last));
            results.Add("Элемент не из коллекции", MeasureSearchTime(elements.NotInCollection));

            return results;
        }
    }
}
