using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab13
{
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public string ChangedItem { get; set; }
        public TimeSpan Time { get; set; }

        public CollectionHandlerEventArgs(string collectionName, string changeType, string changedItem, TimeSpan time)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ChangedItem = changedItem;
            Time = time;
        }
    }
}
