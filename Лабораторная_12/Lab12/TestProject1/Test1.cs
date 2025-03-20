using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab12.ClassLibrary;
using Lab12.ClassLibrary.Utils;

namespace Lab12.Tests
{
    [TestClass]
    public class HashTableTests
    {
        // Вспомогательный класс для тестирования коллизий
        private class CollisionKey : IEquatable<CollisionKey>
        {
            public string Value { get; }
            public int HashValue { get; }

            public CollisionKey(string value, int hashValue)
            {
                Value = value;
                HashValue = hashValue;
            }

            public override int GetHashCode() => HashValue;

            public override bool Equals(object obj) => obj is CollisionKey other && Equals(other);

            public bool Equals(CollisionKey other)
            {
                if (other is null) return false;
                return Value.Equals(other.Value);
            }

            public override string ToString() => $"Key[{Value}|Hash:{HashValue}]";
        }

        // Основные операции

        [TestMethod]
        public void Constructor_InitializesEmptyTable()
        {
            // Arrange & Act
            var table = new HashTable<string, int>(10);

            // Assert
            Assert.AreEqual(0, table.Count);
            Assert.IsNotNull(table.Keys);
            Assert.IsNotNull(table.Values);
            Assert.AreEqual(0, table.Keys.Count);
            Assert.AreEqual(0, table.Values.Count);
        }

        [TestMethod]
        public void Add_SingleElement_AddsSuccessfully()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act
            table.Add("test", 123);

            // Assert
            Assert.AreEqual(1, table.Count);
            Assert.IsTrue(table.ContainsKey("test"));
            Assert.AreEqual(123, table["test"]);
        }

        [TestMethod]
        public void Add_MultipleElements_AddsSuccessfully()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act
            table.Add("one", 1);
            table.Add("two", 2);
            table.Add("three", 3);

            // Assert
            Assert.AreEqual(3, table.Count);
            Assert.IsTrue(table.ContainsKey("one"));
            Assert.IsTrue(table.ContainsKey("two"));
            Assert.IsTrue(table.ContainsKey("three"));
            Assert.AreEqual(1, table["one"]);
            Assert.AreEqual(2, table["two"]);
            Assert.AreEqual(3, table["three"]);
        }

        [TestMethod]
        public void Add_DuplicateKey_UpdatesValueAndDoesNotIncreaseCount()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);

            // Act
            table.Add("test", 456);

            // Assert
            // При добавлении дубликата ожидается обновление значения, а не увеличение количества элементов.
            Assert.AreEqual(1, table.Count);
            Assert.IsTrue(table.ContainsKey("test"));
            Assert.AreEqual(456, table["test"]);
        }

        [TestMethod]
        public void Add_KeyValuePair_AddsSuccessfully()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            var pair = new KeyValuePair<string, int>("test", 123);

            // Act
            table.Add(pair);

            // Assert
            Assert.AreEqual(1, table.Count);
            Assert.IsTrue(table.ContainsKey("test"));
            Assert.AreEqual(123, table["test"]);
        }

        [TestMethod]
        public void Indexer_ExistingKey_GetsValue()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);

            // Act
            var result = table["test"];

            // Assert
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void Indexer_NonExistingKey_ThrowsException()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => { var result = table["nonexistent"]; });
        }

        [TestMethod]
        public void Indexer_SetExistingKey_UpdatesValue()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);

            // Act
            table["test"] = 456;

            // Assert
            Assert.AreEqual(456, table["test"]);
        }

        [TestMethod]
        public void Indexer_SetNonExistingKey_ThrowsException()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => { table["nonexistent"] = 123; });
        }

        [TestMethod]
        public void Remove_ExistingKey_RemovesAndReturnsTrue()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);

            // Act
            bool result = table.Remove("test");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, table.Count);
            Assert.IsFalse(table.ContainsKey("test"));
        }

        [TestMethod]
        public void Remove_NonExistingKey_ReturnsFalse()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act
            bool result = table.Remove("nonexistent");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, table.Count);
        }

        [TestMethod]
        public void Remove_KeyValuePair_RemovesAndReturnsTrue()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);
            var pair = new KeyValuePair<string, int>("test", 123);

            // Act
            bool result = table.Remove(pair);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, table.Count);
            Assert.IsFalse(table.ContainsKey("test"));
        }

        [TestMethod]
        public void Clear_RemovesAllElements()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("one", 1);
            table.Add("two", 2);
            table.Add("three", 3);

            // Act
            table.Clear();

            // Assert
            Assert.AreEqual(0, table.Count);
            Assert.AreEqual(0, table.Keys.Count);
            Assert.AreEqual(0, table.Values.Count);
            Assert.IsFalse(table.ContainsKey("one"));
            Assert.IsFalse(table.ContainsKey("two"));
            Assert.IsFalse(table.ContainsKey("three"));
        }

        [TestMethod]
        public void ContainsKey_ExistingKey_ReturnsTrue()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);

            // Act
            bool result = table.ContainsKey("test");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsKey_NonExistingKey_ReturnsFalse()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act
            bool result = table.ContainsKey("nonexistent");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_ExistingKeyValuePair_ReturnsTrue()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);
            var pair = new KeyValuePair<string, int>("test", 123);

            // Act
            bool result = table.Contains(pair);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_NonExistingKeyValuePair_ReturnsFalse()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            var pair = new KeyValuePair<string, int>("nonexistent", 123);

            // Act
            bool result = table.Contains(pair);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetValue_ExistingKey_ReturnsTrueAndValue()
        {
            // Arrange
            var table = new HashTable<string, int>(10);
            table.Add("test", 123);

            // Act
            bool result = table.TryGetValue("test", out int value);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(123, value);
        }

        [TestMethod]
        public void TryGetValue_NonExistingKey_ReturnsFalseAndDefault()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act
            bool result = table.TryGetValue("nonexistent", out int value);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(default(int), value);
        }

        // Тесты обработки коллизий

        [TestMethod]
        public void Add_CollisionKeys_AddsSuccessfully()
        {
            // Arrange
            var table = new HashTable<CollisionKey, string>(5);
            var key1 = new CollisionKey("A", 1);
            var key2 = new CollisionKey("B", 1); // та же хеш-функция, что и у key1
            var key3 = new CollisionKey("C", 1); // та же хеш-функция, что и у key1 и key2

            // Act
            table.Add(key1, "Value A");
            table.Add(key2, "Value B");
            table.Add(key3, "Value C");

            // Assert
            Assert.AreEqual(3, table.Count);
            Assert.IsTrue(table.ContainsKey(key1));
            Assert.IsTrue(table.ContainsKey(key2));
            Assert.IsTrue(table.ContainsKey(key3));
            Assert.AreEqual("Value A", table[key1]);
            Assert.AreEqual("Value B", table[key2]);
            Assert.AreEqual("Value C", table[key3]);
        }

        [TestMethod]
        public void Update_CollisionKey_UpdatesCorrectValue()
        {
            // Arrange
            var table = new HashTable<CollisionKey, string>(5);
            var key1 = new CollisionKey("A", 1);
            var key2 = new CollisionKey("B", 1); // та же хеш-функция, что и у key1

            table.Add(key1, "Value A");
            table.Add(key2, "Value B");

            // Act
            table[key2] = "Updated Value B";

            // Assert
            Assert.AreEqual("Value A", table[key1]); // значение для key1 не изменяется
            Assert.AreEqual("Updated Value B", table[key2]); // значение для key2 обновлено
        }

        [TestMethod]
        public void Remove_FirstCollisionKey_RemovesCorrectly()
        {
            // Arrange
            var table = new HashTable<CollisionKey, string>(5);
            var key1 = new CollisionKey("A", 1);
            var key2 = new CollisionKey("B", 1);
            var key3 = new CollisionKey("C", 1);

            table.Add(key1, "Value A");
            table.Add(key2, "Value B");
            table.Add(key3, "Value C");

            // Act
            bool result = table.Remove(key1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, table.Count);
            Assert.IsFalse(table.ContainsKey(key1));
            Assert.IsTrue(table.ContainsKey(key2));
            Assert.IsTrue(table.ContainsKey(key3));
            Assert.AreEqual("Value B", table[key2]);
            Assert.AreEqual("Value C", table[key3]);
        }

        [TestMethod]
        public void Remove_MiddleCollisionKey_RemovesCorrectly()
        {
            // Arrange
            var table = new HashTable<CollisionKey, string>(5);
            var key1 = new CollisionKey("A", 1);
            var key2 = new CollisionKey("B", 1);
            var key3 = new CollisionKey("C", 1);

            table.Add(key1, "Value A");
            table.Add(key2, "Value B");
            table.Add(key3, "Value C");

            // Act
            bool result = table.Remove(key2);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, table.Count);
            Assert.IsTrue(table.ContainsKey(key1));
            Assert.IsFalse(table.ContainsKey(key2));
            Assert.IsTrue(table.ContainsKey(key3));
            Assert.AreEqual("Value A", table[key1]);
            Assert.AreEqual("Value C", table[key3]);
        }

        [TestMethod]
        public void Remove_LastCollisionKey_RemovesCorrectly()
        {
            // Arrange
            var table = new HashTable<CollisionKey, string>(5);
            var key1 = new CollisionKey("A", 1);
            var key2 = new CollisionKey("B", 1);
            var key3 = new CollisionKey("C", 1);

            table.Add(key1, "Value A");
            table.Add(key2, "Value B");
            table.Add(key3, "Value C");

            // Act
            bool result = table.Remove(key3);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, table.Count);
            Assert.IsTrue(table.ContainsKey(key1));
            Assert.IsTrue(table.ContainsKey(key2));
            Assert.IsFalse(table.ContainsKey(key3));
            Assert.AreEqual("Value A", table[key1]);
            Assert.AreEqual("Value B", table[key2]);
        }

        // Граничные случаи

        [TestMethod]
        public void Add_CapacityIsZero_HandlesCorrectly()
        {
            // Arrange & Act
            // Если емкость равна 0, ожидаем, что таблица сможет динамически расширяться или хотя бы не кинет исключение.
            var table = new HashTable<string, int>(0);
            Assert.DoesNotThrow(() => table.Add("test", 123));
            Assert.AreEqual(1, table.Count);
            Assert.IsTrue(table.ContainsKey("test"));
            Assert.AreEqual(123, table["test"]);
        }

        [TestMethod]
        public void Add_ManyElements_HandlesCorrectly()
        {
            // Arrange
            var table = new HashTable<string, int>(5); // маленькая начальная емкость
            const int count = 100;

            // Act
            for (int i = 0; i < count; i++)
            {
                table.Add($"key{i}", i);
            }

            // Assert
            Assert.AreEqual(count, table.Count);
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(i, table[$"key{i}"]);
            }
        }

        [TestMethod]
        public void Add_NullKey_HandlesCorrectly()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act & Assert
            Assert.DoesNotThrow(() => table.Add(null, 123));
            Assert.AreEqual(1, table.Count);
            Assert.IsTrue(table.ContainsKey(null));
            Assert.AreEqual(123, table[null]);
        }

        [TestMethod]
        public void Add_NullValue_HandlesCorrectly()
        {
            // Arrange
            var table = new HashTable<string, string>(10);

            // Act & Assert
            Assert.DoesNotThrow(() => table.Add("key", null));
            Assert.AreEqual(1, table.Count);
            Assert.IsTrue(table.ContainsKey("key"));
            Assert.IsNull(table["key"]);
        }

        [TestMethod]
        public void ComplexScenario_MultipleOperations()
        {
            // Arrange
            var table = new HashTable<string, int>(10);

            // Act & Assert
            // Добавление элементов
            table.Add("one", 1);
            table.Add("two", 2);
            table.Add("three", 3);
            Assert.AreEqual(3, table.Count);

            // Обновление элемента
            table["two"] = 22;
            Assert.AreEqual(22, table["two"]);

            // Удаление элемента
            table.Remove("one");
            Assert.AreEqual(2, table.Count);
            Assert.IsFalse(table.ContainsKey("one"));

            // Добавление нового элемента
            table.Add("four", 4);
            Assert.AreEqual(3, table.Count);

            // Проверка наличия ключей и значений
            Assert.IsTrue(table.ContainsKey("two"));
            Assert.IsTrue(table.ContainsKey("three"));
            Assert.IsTrue(table.ContainsKey("four"));
            Assert.AreEqual(22, table["two"]);
            Assert.AreEqual(3, table["three"]);
            Assert.AreEqual(4, table["four"]);

            // Очистка таблицы
            table.Clear();
            Assert.AreEqual(0, table.Count);
            Assert.IsFalse(table.ContainsKey("two"));
            Assert.IsFalse(table.ContainsKey("three"));
            Assert.IsFalse(table.ContainsKey("four"));
        }

        [TestMethod]
        public void CollisionScenario_ComplexOperations()
        {
            // Arrange
            var table = new HashTable<CollisionKey, string>(5);
            var keys = new List<CollisionKey>();

            // Создаем 10 ключей с одинаковым хеш-кодом
            for (int i = 0; i < 10; i++)
            {
                keys.Add(new CollisionKey($"Key{i}", 1));
            }

            // Act & Assert

            // Добавляем все ключи
            for (int i = 0; i < keys.Count; i++)
            {
                table.Add(keys[i], $"Value{i}");
            }

            // Проверяем, что все ключи и значения корректны
            Assert.AreEqual(keys.Count, table.Count);
            for (int i = 0; i < keys.Count; i++)
            {
                Assert.IsTrue(table.ContainsKey(keys[i]));
                Assert.AreEqual($"Value{i}", table[keys[i]]);
            }

            // Обновляем каждый второй ключ
            for (int i = 0; i < keys.Count; i += 2)
            {
                table[keys[i]] = $"UpdatedValue{i}";
            }

            // Проверяем обновленные значения
            for (int i = 0; i < keys.Count; i++)
            {
                if (i % 2 == 0)
                    Assert.AreEqual($"UpdatedValue{i}", table[keys[i]]);
                else
                    Assert.AreEqual($"Value{i}", table[keys[i]]);
            }

            // Удаляем каждый третий ключ
            for (int i = 0; i < keys.Count; i += 3)
            {
                table.Remove(keys[i]);
            }

            // Проверяем, что удаленные ключи отсутствуют, а остальные присутствуют
            for (int i = 0; i < keys.Count; i++)
            {
                if (i % 3 == 0)
                    Assert.IsFalse(table.ContainsKey(keys[i]));
                else
                {
                    Assert.IsTrue(table.ContainsKey(keys[i]));
                    if (i % 2 == 0)
                        Assert.AreEqual($"UpdatedValue{i}", table[keys[i]]);
                    else
                        Assert.AreEqual($"Value{i}", table[keys[i]]);
                }
            }
        }
    }

    [TestClass]
    public class HTableElementTests
    {
        [TestMethod]
        public void Constructor_Default_CreatesEmptyElement()
        {
            // Arrange & Act
            var element = new HTableElement<string, int>();

            // Assert
            Assert.IsTrue(element.isEmpty);
            Assert.IsNull(element.next);
        }

        [TestMethod]
        public void Constructor_WithKeyAndData_CreatesNonEmptyElement()
        {
            // Arrange
            string key = "testKey";
            int data = 123;

            // Act
            var element = new HTableElement<string, int>(key, data);

            // Assert
            Assert.IsFalse(element.isEmpty);
            Assert.AreEqual(key, element.Key);
            Assert.AreEqual(data, element.Data);
            Assert.IsNull(element.next);
        }

        [TestMethod]
        public void Properties_SetKey_UpdatesKey()
        {
            // Arrange
            var element = new HTableElement<string, int>();

            // Act
            element.Key = "testKey";

            // Assert
            Assert.AreEqual("testKey", element.Key);
            Assert.IsFalse(element.isEmpty);
        }

        [TestMethod]
        public void Properties_SetData_UpdatesData()
        {
            // Arrange
            var element = new HTableElement<string, int>();

            // Act
            element.Data = 123;

            // Assert
            Assert.AreEqual(123, element.Data);
            // isEmpty должен оставаться true, так как он обновляется только при установке Key
            Assert.IsTrue(element.isEmpty);
        }

        [TestMethod]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            var element = new HTableElement<string, int>("testKey", 123);

            // Act
            string result = element.ToString();

            // Assert
            Assert.AreEqual("testKey: 123", result);
        }

        [TestMethod]
        public void GetHashCode_ReturnsAbsoluteValueOfKeyHashCode()
        {
            // Arrange
            var element = new HTableElement<string, int>("testKey", 123);
            var expectedHashCode = Math.Abs("testKey".GetHashCode());

            // Act
            int result = element.GetHashCode();

            // Assert
            Assert.AreEqual(expectedHashCode, result);
        }
    }
}
