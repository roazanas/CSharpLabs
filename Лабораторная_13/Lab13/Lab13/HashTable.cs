using System;
using System.Collections;
using Lab13.ClassLibrary;
using Lab13.ClassLibrary.Utils;

namespace HashTable
{
    public class HTableElement<KType, DType>
    {
        private KType key;
        private DType data;

        public bool isEmpty = true;
        public HTableElement<KType, DType> next;

        public KType Key
        {
            get { return key; }
            set
            {
                key = value;
                isEmpty = false;
            }
        }

        public DType Data
        {
            get { return data; }
            set
            {
                data = value;
                //isEmpty = false;
            }
        }

        public HTableElement() { }

        public HTableElement(KType key, DType data)
        {
            this.key = key;
            this.data = data;
            next = null;
            isEmpty = false;
        }
  
        public override string ToString()
        {
            return $"{key}: {data}";
        }

        public override int GetHashCode()
        {
            return Math.Abs(key.GetHashCode());
        }
    }

    public class HashTable<KType, DType> : IDictionary<KType, DType>, ICloneable
    {
        private List<HTableElement<KType, DType>> table;
        private List<KType> keys;
        private List<DType> values;

        public int Count { get; set; } = 0;

        public ICollection<KType> Keys => keys;
        public ICollection<DType> Values => values;

        public bool IsReadOnly => false;

        public HashTable() 
        {
            table = new(1);
            keys = new(1);
            values = new(1);
            
            table.Add(new HTableElement<KType, DType>());
        }

        public HashTable(int capacity)
        {
            table = new(capacity);
            keys = new(capacity);
            values = new(capacity);

            for (int i = 0; i < capacity; i++)
            {
                var item = new HTableElement<KType, DType>();
                table.Add(item);
            }
        }

        public override string ToString()
        {
            string result = "HashTable(";
            foreach (var item in this)
            {
                result += "\n    " + item + ", ";
            }

            if (result != "HashTable(")
                return string.Concat(result.AsSpan(0, result.Length - 2), "\n)");
            else
                return result + "\n)";
        }

        public void Add(HTableElement<KType, DType> element)
        {
            int index = element.GetHashCode() % table.Capacity;

            if (table[index].isEmpty || table[index].Key.Equals(element.Key))
            {
                table[index] = element;
            }
            else
            {
                var current = table[index];
                while (current.next != null)
                {
                    current = current.next;
                }
                current.next = element;
            }

            keys.Add(element.Key);
            values.Add(element.Data);
            Count++;
        }

        public void Add(KType key, DType value) => Add(new HTableElement<KType, DType>(key, value));

        public void Add(params HTableElement<KType, DType>[] elements)
        {
            foreach (var element in elements)
            {
                Add(element);
            }
        }

        public bool Remove(KType key)
        {
            int index = Math.Abs(key.GetHashCode()) % table.Capacity;

            if (table[index].isEmpty)
                return false;

            var current = table[index];
            HTableElement<KType, DType> prev = null;
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    if (prev == null)
                    {
                        table[index] = current.next ?? new HTableElement<KType, DType>();
                    }
                    else
                    {
                        prev.next = current.next;
                    }
                    keys.Remove(key);
                    values.Remove(current.Data);
                    Count--;
                    return true;
                }

                prev = current;
                current = current.next;
            }

            return false;
        }

        public DType this[KType key]
        {
            get
            {
                int index = Math.Abs(key.GetHashCode()) % table.Count;

                var current = table[index];
                while (current != null && !current.isEmpty)
                {
                    if (current.Key.Equals(key))
                        return current.Data;
                    current = current.next;
                }
                throw new KeyNotFoundException($"Значение по ключу {key} не найдено");
            }
            set
            {
                int index = Math.Abs(key.GetHashCode()) % table.Capacity;

                var current = table[index];
                while (current != null)
                {
                    if (current.Key.Equals(key))
                    {
                        values.Remove(current.Data);
                        current.Data = value;
                        values.Add(current.Data);
                        return;
                    }
                    current = current.next;
                }
                throw new KeyNotFoundException($"Значение по ключу {key} не найдено");
            }
        }

        public bool ContainsKey(KType key) => keys.Contains(key);

        public bool TryGetValue(KType key, out DType value)
        {
            try
            {
                value = this[key];
                return true;
            }
            catch (KeyNotFoundException)
            {
                value = default;
                return false;
            }
        }

        public void Add(KeyValuePair<KType, DType> item)
        {
            Add(new HTableElement<KType, DType>(item.Key, item.Value));
        }

        public void Clear()
        {
            table.Clear();
            keys.Clear();
            values.Clear();
            Count = 0;

            for (int i = 0; i < table.Capacity; i++)
            {
                var item = new HTableElement<KType, DType>();
                table.Add(item);
            }
        }

        public bool Contains(KeyValuePair<KType, DType> item)
        {
            foreach (var elem in this)
            {
                if (elem.Key.Equals(item) && elem.Data.Equals(item))
                    return true;
            }
            return false;
        }

        public void CopyTo(KeyValuePair<KType, DType>[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex++] = new KeyValuePair<KType, DType>(item.Key, item.Data);
            }
        }

        public void CopyTo(HTableElement<KType, DType>[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(KeyValuePair<KType, DType> item) => Remove(item.Key);

        public IEnumerator<HTableElement<KType, DType>> GetEnumerator()
        {
            for (int i = 0; i < table.Count; i++)
            {
                var current = table[i];
                while (current != null && !current.isEmpty)
                {
                    yield return current;
                    current = current.next;
                }
            }
        }

        IEnumerator<KeyValuePair<KType, DType>> IEnumerable<KeyValuePair<KType, DType>>.GetEnumerator()
        {
            foreach (var item in this)
            {
                yield return new KeyValuePair<KType, DType>(item.Key, item.Data);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public object Clone()
        {
            HashTable<KType, DType> hashTableClone = new(table.Capacity);
            foreach (var item in this)
            {
                hashTableClone.Add(item);
            }

            return hashTableClone;
        }
    }
}