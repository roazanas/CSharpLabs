using HashTable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab13
{
    public class NewHashTable<KType, DType> : HashTable<KType, DType>
    {
        private Stopwatch stopwatch;
        public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);
        
        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;

        public string Name { get; set; }

        public NewHashTable() : base() 
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public new void Add(HTableElement<KType, DType> element)
        {
            base.Add(element);
            CollectionCountChanged?.Invoke(this, new(Name, "Add", element.ToString(), stopwatch.Elapsed));
        }

        public new void Add(KType key, DType value) => Add(new HTableElement<KType, DType>(key, value));

        public new bool Remove(KType key)
        {
            var data = this[key];
            var result = base.Remove(key);
            CollectionCountChanged?.Invoke(this, new(Name, "Remove", $"{key}: {data}", stopwatch.Elapsed));
            return result;
        }

        public new DType this[KType key]
        {
            get
            {
                return base[key];
            }
            set
            {
                var data = this[key];
                base[key] = value;
                CollectionReferenceChanged?.Invoke(this, new(Name, "Change", $"{key}: {data} -> {value}", stopwatch.Elapsed));
            }
        }
    }
}
