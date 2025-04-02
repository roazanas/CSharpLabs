using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab13
{
    //public class JournalEntry
    //{
    //    public string CollectionName { get; set; }
    //    public string ChangeType { get; set; }
    //    public string ChangedItem { get; set; }
    //    public TimeSpan Time { get; set; }

    //    public JournalEntry(string collectionName, string changeType, string changedItem, TimeSpan time)
    //    {
    //        CollectionName = collectionName;
    //        ChangeType = changeType;
    //        ChangedItem = changedItem;
    //        Time = time;
    //    }

    //    public override string ToString()
    //    {
    //        return $"[{Time}]: {CollectionName} | {ChangeType} | {ChangedItem}";
    //    }
    //}

    public class JournalEntry : CollectionHandlerEventArgs
    {
        public JournalEntry(string collectionName, string changeType, string changedItem, TimeSpan time)
            : base(collectionName, changeType, changedItem, time)
        {
        }

        public override string ToString()
        {
            return $"[{Time}]: {CollectionName} | {ChangeType} | {ChangedItem}";
        }
    }

    public class Journal
    {
        private LinkedList<JournalEntry> journalEntries;

        public Journal()
        {
            journalEntries = new();
        }

        public void HandleEvent(object source, CollectionHandlerEventArgs args)
        {
            journalEntries.AddLast(new JournalEntry(
                args.CollectionName, 
                args.ChangeType, 
                args.ChangedItem, 
                args.Time
            ));
        }

        public override string ToString()
        {
            var result = "[\n";
            foreach (var entry in journalEntries)
            {
                result += "    " + entry + ", \n";
            }
            return result + "]";
        }
    }
}
