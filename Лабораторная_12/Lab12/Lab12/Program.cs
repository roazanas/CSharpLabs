using System;
using System.Collections;
using Lab12.ClassLibrary;
using Lab12.ClassLibrary.Utils;

public class HTableElement<KType, DType>
{
    public HTableElement<KType, DType> next;
    private KType key;
    private DType data;

    public bool isEmpty = true;

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

    public HashTable() { }

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

class Program
{
    static void Main()
    {
        try
        {
            // ЧАСТЬ 1: БАЗОВЫЕ ОПЕРАЦИИ
            Console.WriteLine(">>> ЧАСТЬ 1: БАЗОВЫЕ ОПЕРАЦИИ");

            // Создание новой хэш-таблицы с начальной емкостью
            Console.WriteLine("-> Создание хэш-таблицы с начальной емкостью 10");
            var hashTable = new HashTable<string, string>(10);
            Console.WriteLine($"    Создана пустая хэш-таблица. Количество элементов: {hashTable.Count}\n");

            // Добавление элементов
            Console.WriteLine("-> Добавление элементов");
            hashTable.Add("key1", "value1");
            hashTable.Add("key2", "value2");
            hashTable.Add("key3", "value3");
            Console.WriteLine($"    Добавлено 3 элемента. Количество элементов: {hashTable.Count}");
            Console.WriteLine("    Содержимое хэш-таблицы:");
            Console.WriteLine($"    {hashTable}\n");

            // Поиск элементов
            Console.WriteLine("-> Поиск элементов по ключу");
            try
            {
                string value1 = hashTable["key1"];
                string value2 = hashTable["key2"];
                Console.WriteLine($"    Значение по ключу 'key1': {value1}");
                Console.WriteLine($"    Значение по ключу 'key2': {value2}");

                // Проверка метода TryGetValue
                Console.WriteLine("    Использование TryGetValue:");
                if (hashTable.TryGetValue("key3", out string value3))
                {
                    Console.WriteLine($"    Найдено значение по ключу 'key3': {value3}");
                }

                // Проверка ContainsKey
                Console.WriteLine("    Использование ContainsKey:");
                Console.WriteLine($"    Содержит ключ 'key2': {hashTable.ContainsKey("key2")}");
                Console.WriteLine($"    Содержит ключ 'nonexistent': {hashTable.ContainsKey("nonexistent")}\n");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"    ОШИБКА: {ex.Message}\n");
            }

            // Обновление значений
            Console.WriteLine("-> Обновление элементов");
            hashTable["key2"] = "updated_value2";
            Console.WriteLine($"    Значение по ключу 'key2' обновлено");
            Console.WriteLine($"    Новое значение: {hashTable["key2"]}");
            Console.WriteLine("    Содержимое хэш-таблицы после обновления:");
            Console.WriteLine($"    {hashTable}\n");

            // Удаление элементов
            Console.WriteLine("-> Удаление элементов");
            bool removed = hashTable.Remove("key1");
            Console.WriteLine($"    Удаление 'key1': {(removed ? "успешно" : "не удалось")}");
            Console.WriteLine($"    Количество элементов после удаления: {hashTable.Count}");
            Console.WriteLine("    Содержимое хэш-таблицы после удаления:");
            Console.WriteLine($"    {hashTable}\n");

            // ЧАСТЬ 2: ОБРАБОТКА КОЛЛИЗИЙ
            Console.WriteLine(">>> ЧАСТЬ 2: ОБРАБОТКА КОЛЛИЗИЙ");

            // Создаем класс с предсказуемым хеш-кодом для тестирования коллизий
            Console.WriteLine("-> Создание класса с предсказуемым хеш-кодом");

            // Объявление локального класса для тестирования коллизий
            var collision1 = new TestCollisionKey("A", 1);
            var collision2 = new TestCollisionKey("B", 1); // Имеет тот же хеш-код что и collision1
            var collision3 = new TestCollisionKey("C", 1); // Имеет тот же хеш-код что и collision1 и collision2

            Console.WriteLine($"    Хеш-код collision1: {collision1.GetHashCode()}");
            Console.WriteLine($"    Хеш-код collision2: {collision2.GetHashCode()}");
            Console.WriteLine($"    Хеш-код collision3: {collision3.GetHashCode()}\n");

            // Создаем новую хеш-таблицу для тестирования коллизий
            Console.WriteLine("-> Тестирование вставки с коллизиями");
            var collisionTable = new HashTable<TestCollisionKey, string>(5);

            collisionTable.Add(collision1, "Collision Value 1");
            collisionTable.Add(collision2, "Collision Value 2");
            collisionTable.Add(collision3, "Collision Value 3");

            Console.WriteLine($"    Добавлено 3 элемента с коллизиями. Количество элементов: {collisionTable.Count}");
            Console.WriteLine("    Содержимое хэш-таблицы с коллизиями:");
            Console.WriteLine($"    {collisionTable}\n");

            // Проверка поиска элементов с коллизиями
            Console.WriteLine("-> Поиск элементов с коллизиями");
            try
            {
                string valueC1 = collisionTable[collision1];
                string valueC2 = collisionTable[collision2];
                string valueC3 = collisionTable[collision3];

                Console.WriteLine($"    Значение для collision1: {valueC1}");
                Console.WriteLine($"    Значение для collision2: {valueC2}");
                Console.WriteLine($"    Значение для collision3: {valueC3}\n");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"    ОШИБКА: {ex.Message}\n");
            }

            // Обновление значений с коллизиями
            Console.WriteLine("-> Обновление элементов с коллизиями");
            collisionTable[collision2] = "Updated Collision Value 2";
            Console.WriteLine($"    Обновлено значение для collision2: {collisionTable[collision2]}");
            Console.WriteLine("    Содержимое хэш-таблицы после обновления:");
            Console.WriteLine($"    {collisionTable}\n");

            // Удаление элементов с коллизиями
            Console.WriteLine("-> Удаление элементов с коллизиями");
            bool removedCollision = collisionTable.Remove(collision1);
            Console.WriteLine($"    Удаление collision1: {(removedCollision ? "успешно" : "не удалось")}");
            Console.WriteLine($"    Количество элементов после удаления: {collisionTable.Count}");
            Console.WriteLine("    Содержимое хэш-таблицы после удаления:");
            Console.WriteLine($"    {collisionTable}\n");

            // Проверка, что оставшиеся элементы с коллизиями всё ещё доступны
            Console.WriteLine("-> Проверка доступности оставшихся элементов с коллизиями");
            try
            {
                Console.WriteLine($"    Значение для collision2: {collisionTable[collision2]}");
                Console.WriteLine($"    Значение для collision3: {collisionTable[collision3]}\n");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"    ОШИБКА: {ex.Message}\n");
            }

            // ЧАСТЬ 3: ПРОВЕРКА ГРАНИЧНЫХ СЛУЧАЕВ И ИСКЛЮЧЕНИЙ
            Console.WriteLine(">>> ЧАСТЬ 3: ГРАНИЧНЫЕ СЛУЧАИ И ИСКЛЮЧЕНИЯ");

            // Тестирование попытки поиска несуществующего ключа
            Console.WriteLine("-> Попытка поиска несуществующего ключа");
            try
            {
                string nonExistentValue = hashTable["nonexistent_key"];
                Console.WriteLine($"    Значение: {nonExistentValue}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"    ОЖИДАЕМАЯ ОШИБКА: {ex.Message}\n");
            }

            // Тестирование попытки удаления несуществующего ключа
            Console.WriteLine("-> Попытка удаления несуществующего ключа");
            bool removedNonExistent = hashTable.Remove("nonexistent_key");
            Console.WriteLine($"    Удаление 'nonexistent_key': {(removedNonExistent ? "успешно" : "не удалось (ожидаемо)")}\n");

            // Тестирование добавления элемента с существующим ключом
            Console.WriteLine("-> Добавление элемента с существующим ключом");
            hashTable.Add("key3", "duplicate_value3");
            Console.WriteLine($"    Добавлен элемент с ключом 'key3'");
            Console.WriteLine($"    Новое значение по ключу 'key3': {hashTable["key3"]}");
            Console.WriteLine("    Содержимое хэш-таблицы:");
            Console.WriteLine($"    {hashTable}\n");

            // Тестирование работы с интерфейсом ICollection и KeyValuePair
            Console.WriteLine("-> Работа с интерфейсом ICollection<KeyValuePair>");

            // Тестирование метода Clear
            Console.WriteLine("-> Очистка хэш-таблицы");
            hashTable.Clear();
            Console.WriteLine($"    Хэш-таблица очищена. Количество элементов: {hashTable.Count}");
            Console.WriteLine("    Содержимое хэш-таблицы после очистки:");
            Console.WriteLine($"    {hashTable}\n");

            // Тестирование клонирования
            Console.WriteLine(">>> ЧАСТЬ 4: КЛОНИРОВАНИЕ");
            hashTable.Add("clone_key1", "clone_value1");
            hashTable.Add("clone_key2", "clone_value2");

            Console.WriteLine("-> Создание клона хэш-таблицы");
            var clonedTable = (HashTable<string, string>)hashTable.Clone();
            Console.WriteLine($"    Клонирование выполнено. Количество элементов в клоне: {clonedTable.Count}");
            Console.WriteLine("    Содержимое оригинальной хэш-таблицы:");
            Console.WriteLine($"    {hashTable}");
            Console.WriteLine("    Содержимое клонированной хэш-таблицы:");
            Console.WriteLine($"    {clonedTable}\n");

            // Проверка независимости клона
            Console.WriteLine("-> Проверка независимости клона");
            hashTable.Add("original_only", "only_in_original");
            clonedTable.Add("clone_only", "only_in_clone");

            Console.WriteLine("    Содержимое оригинальной хэш-таблицы после изменений:");
            Console.WriteLine($"    {hashTable}");
            Console.WriteLine("    Содержимое клонированной хэш-таблицы после изменений:");
            Console.WriteLine($"    {clonedTable}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }

    // Класс для тестирования коллизий
    class TestCollisionKey
    {
        public string Value { get; }
        public int HashValue { get; }

        public TestCollisionKey(string value, int hashValue)
        {
            Value = value;
            HashValue = hashValue;
        }

        public override int GetHashCode()
        {
            return HashValue; // Всегда возвращает одинаковый хеш-код для тестирования коллизий
        }

        public override bool Equals(object obj)
        {
            if (obj is TestCollisionKey other)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override string ToString()
        {
            return $"Key[{Value}|Hash:{HashValue}]";
        }
    }
}