using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab13;
using HashTable;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab13.Tests
{
    [TestClass]
    public class CollectionHandlerEventArgsTests
    {
        [TestMethod]
        public void Constructor_AssignsPropertiesCorrectly()
        {
            string expectedCollectionName = "TestCollection";
            string expectedChangeType = "Add";
            string expectedChangedItem = "Item1";
            TimeSpan expectedTime = TimeSpan.FromSeconds(5);

            var args = new CollectionHandlerEventArgs(
                expectedCollectionName,
                expectedChangeType,
                expectedChangedItem,
                expectedTime);

            Assert.AreEqual(expectedCollectionName, args.CollectionName);
            Assert.AreEqual(expectedChangeType, args.ChangeType);
            Assert.AreEqual(expectedChangedItem, args.ChangedItem);
            Assert.AreEqual(expectedTime, args.Time);
        }

        [TestMethod]
        public void Properties_CanBeSetAfterConstruction()
        {
            var args = new CollectionHandlerEventArgs("InitialName", "InitialType", "InitialItem", TimeSpan.Zero);
            string newCollectionName = "UpdatedCollection";
            string newChangeType = "Remove";
            string newChangedItem = "Item2";
            TimeSpan newTime = TimeSpan.FromMinutes(1);

            args.CollectionName = newCollectionName;
            args.ChangeType = newChangeType;
            args.ChangedItem = newChangedItem;
            args.Time = newTime;

            Assert.AreEqual(newCollectionName, args.CollectionName);
            Assert.AreEqual(newChangeType, args.ChangeType);
            Assert.AreEqual(newChangedItem, args.ChangedItem);
            Assert.AreEqual(newTime, args.Time);
        }
    }

    [TestClass]
    public class JournalTests
    {
        [TestMethod]
        public void JournalEntry_Constructor_InitializesProperties()
        {
            string expectedCollectionName = "MyList";
            string expectedChangeType = "Update";
            string expectedChangedItem = "Old -> New";
            TimeSpan expectedTime = TimeSpan.FromMilliseconds(123);

            var entry = new JournalEntry(expectedCollectionName, expectedChangeType, expectedChangedItem, expectedTime);

            Assert.AreEqual(expectedCollectionName, entry.CollectionName);
            Assert.AreEqual(expectedChangeType, entry.ChangeType);
            Assert.AreEqual(expectedChangedItem, entry.ChangedItem);
            Assert.AreEqual(expectedTime, entry.Time);
        }

        [TestMethod]
        public void JournalEntry_ToString_FormatsCorrectly()
        {
            string collectionName = "HashTableX";
            string changeType = "Add";
            string changedItem = "Key1: Value1";
            TimeSpan time = new TimeSpan(0, 1, 2, 3, 456);
            var entry = new JournalEntry(collectionName, changeType, changedItem, time);
            string expectedString = $"[{time}]: {collectionName} | {changeType} | {changedItem}";

            string actualString = entry.ToString();

            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Journal_Constructor_CreatesEmptyJournal()
        {
            var journal = new Journal();
            string expectedString = "[\n]";

            Assert.AreEqual(expectedString, journal.ToString());
        }

        [TestMethod]
        public void HandleEvent_AddsSingleEntryToJournal()
        {
            var journal = new Journal();
            var source = new object();
            var args = new CollectionHandlerEventArgs("CollectionA", "Add", "ItemAlpha", TimeSpan.FromSeconds(1));
            var expectedEntryString = new JournalEntry(args.CollectionName, args.ChangeType, args.ChangedItem, args.Time).ToString();

            journal.HandleEvent(source, args);
            string journalContent = journal.ToString();

            // Normalize line endings for robust comparison
            journalContent = journalContent.Replace("\r\n", "\n");

            StringAssert.Contains(journalContent, expectedEntryString);
            Assert.IsTrue(journalContent.StartsWith("[\n"));

            // Corrected assertions:
            Assert.IsTrue(journalContent.EndsWith(", \n]"));
            Assert.AreEqual(1, journalContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length - 2);
        }

        [TestMethod]
        public void HandleEvent_AddsMultipleEntriesToJournalInOrder()
        {
            var journal = new Journal();
            var source = new object();
            var args1 = new CollectionHandlerEventArgs("CollectionA", "Add", "ItemAlpha", TimeSpan.FromSeconds(1));
            Thread.Sleep(10);
            var args2 = new CollectionHandlerEventArgs("CollectionB", "Remove", "ItemBeta", TimeSpan.FromSeconds(2));
            Thread.Sleep(10);
            var args3 = new CollectionHandlerEventArgs("CollectionA", "Update", "ItemAlpha -> ItemGamma", TimeSpan.FromSeconds(3));

            var entry1String = new JournalEntry(args1.CollectionName, args1.ChangeType, args1.ChangedItem, args1.Time).ToString();
            var entry2String = new JournalEntry(args2.CollectionName, args2.ChangeType, args2.ChangedItem, args2.Time).ToString();
            var entry3String = new JournalEntry(args3.CollectionName, args3.ChangeType, args3.ChangedItem, args3.Time).ToString();

            journal.HandleEvent(source, args1);
            journal.HandleEvent(source, args2);
            journal.HandleEvent(source, args3);
            string journalContent = journal.ToString();

            // Normalize line endings for robust comparison if needed elsewhere
            journalContent = journalContent.Replace("\r\n", "\n");

            StringAssert.Contains(journalContent, entry1String);
            StringAssert.Contains(journalContent, entry2String);
            StringAssert.Contains(journalContent, entry3String);

            int index1 = journalContent.IndexOf(entry1String);
            int index2 = journalContent.IndexOf(entry2String);
            int index3 = journalContent.IndexOf(entry3String);

            Assert.IsTrue(index1 >= 0 && index2 >= 0 && index3 >= 0);
            Assert.IsTrue(index1 < index2);
            Assert.IsTrue(index2 < index3);

            // Corrected assertion: Subtract 2 for the '[' line and the ']' line
            Assert.AreEqual(3, journalContent.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length - 2);
        }

        [TestMethod]
        public void Journal_ToString_EmptyJournalFormatsCorrectly()
        {
            var journal = new Journal();
            string expectedString = "[\n]";

            string actualString = journal.ToString();

            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Journal_ToString_SingleEntryJournalFormatsCorrectly()
        {
            var journal = new Journal();
            var source = new object();
            var args = new CollectionHandlerEventArgs("C", "T", "I", TimeSpan.Zero);
            journal.HandleEvent(source, args);

            var entryString = new JournalEntry(args.CollectionName, args.ChangeType, args.ChangedItem, args.Time).ToString();
            string expectedString = $"[\n    {entryString}, \n]";

            string actualString = journal.ToString();

            expectedString = expectedString.Replace("\r\n", "\n");
            actualString = actualString.Replace("\r\n", "\n");
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Journal_ToString_MultipleEntriesJournalFormatsCorrectly()
        {
            var journal = new Journal();
            var source = new object();
            var args1 = new CollectionHandlerEventArgs("C1", "T1", "I1", TimeSpan.FromSeconds(1));
            Thread.Sleep(10);
            var args2 = new CollectionHandlerEventArgs("C2", "T2", "I2", TimeSpan.FromSeconds(2));
            journal.HandleEvent(source, args1);
            journal.HandleEvent(source, args2);

            var entry1String = new JournalEntry(args1.CollectionName, args1.ChangeType, args1.ChangedItem, args1.Time).ToString();
            var entry2String = new JournalEntry(args2.CollectionName, args2.ChangeType, args2.ChangedItem, args2.Time).ToString();

            string expectedString = $"[\n    {entry1String}, \n    {entry2String}, \n]";

            string actualString = journal.ToString();

            expectedString = expectedString.Replace("\r\n", "\n");
            actualString = actualString.Replace("\r\n", "\n");
            Assert.AreEqual(expectedString, actualString);
        }
    }

    [TestClass]
    public class NewHashTableTests
    {
        private NewHashTable<string, int> _hashTable;
        private int _countChangedEventFiredCount;
        private CollectionHandlerEventArgs _lastCountChangedEventArgs;
        private int _refChangedEventFiredCount;
        private CollectionHandlerEventArgs _lastRefChangedEventArgs;
        private const string TestTableName = "TestTable";

        [TestInitialize]
        public void TestInitialize()
        {
            _hashTable = new NewHashTable<string, int>();
            _hashTable.Name = TestTableName;

            _countChangedEventFiredCount = 0;
            _lastCountChangedEventArgs = null;
            _refChangedEventFiredCount = 0;
            _lastRefChangedEventArgs = null;

            _hashTable.CollectionCountChanged += (sender, args) =>
            {
                _countChangedEventFiredCount++;
                _lastCountChangedEventArgs = args;
            };
            _hashTable.CollectionReferenceChanged += (sender, args) =>
            {
                _refChangedEventFiredCount++;
                _lastRefChangedEventArgs = args;
            };
        }

        [TestMethod]
        public void Name_Property_SetAndGet()
        {
            string newName = "MySpecialTable";

            _hashTable.Name = newName;

            Assert.AreEqual(newName, _hashTable.Name);
        }

        [TestMethod]
        public void Add_NewElement_FiresCollectionCountChangedEvent()
        {
            string key = "key1";
            int value = 10;
            string expectedChangedItem = $"{key}: {value}";

            _hashTable.Add(key, value);

            Assert.AreEqual(1, _countChangedEventFiredCount);
            Assert.AreEqual(0, _refChangedEventFiredCount);
            Assert.IsNotNull(_lastCountChangedEventArgs);
            Assert.AreEqual(TestTableName, _lastCountChangedEventArgs.CollectionName);
            Assert.AreEqual("Add", _lastCountChangedEventArgs.ChangeType);
            Assert.AreEqual(expectedChangedItem, _lastCountChangedEventArgs.ChangedItem);
            Assert.IsTrue(_lastCountChangedEventArgs.Time > TimeSpan.Zero);
            Assert.AreEqual(1, _hashTable.Count);
        }

        [TestMethod]
        public void Add_UsingHTableElement_FiresCollectionCountChangedEvent()
        {
            var element = new HTableElement<string, int>("keyElement", 100);
            string expectedChangedItem = element.ToString();

            _hashTable.Add(element);

            Assert.AreEqual(1, _countChangedEventFiredCount);
            Assert.AreEqual(0, _refChangedEventFiredCount);
            Assert.IsNotNull(_lastCountChangedEventArgs);
            Assert.AreEqual(TestTableName, _lastCountChangedEventArgs.CollectionName);
            Assert.AreEqual("Add", _lastCountChangedEventArgs.ChangeType);
            Assert.AreEqual(expectedChangedItem, _lastCountChangedEventArgs.ChangedItem);
            Assert.AreEqual(1, _hashTable.Count);
        }

        [TestMethod]
        public void Remove_ExistingElement_FiresCollectionCountChangedEvent()
        {
            string key = "keyToRemove";
            int value = 20;
            _hashTable.Add(key, value);

            _countChangedEventFiredCount = 0;
            _lastCountChangedEventArgs = null;

            string expectedChangedItem = $"{key}: {value}";

            bool removed = _hashTable.Remove(key);

            Assert.IsTrue(removed);
            Assert.AreEqual(1, _countChangedEventFiredCount);
            Assert.AreEqual(0, _refChangedEventFiredCount);
            Assert.IsNotNull(_lastCountChangedEventArgs);
            Assert.AreEqual(TestTableName, _lastCountChangedEventArgs.CollectionName);
            Assert.AreEqual("Remove", _lastCountChangedEventArgs.ChangeType);
            Assert.AreEqual(expectedChangedItem, _lastCountChangedEventArgs.ChangedItem);
            Assert.IsTrue(_lastCountChangedEventArgs.Time > TimeSpan.Zero);
            Assert.AreEqual(0, _hashTable.Count);
        }

        [TestMethod]
        public void Remove_NonExistingElement_ThrowsKeyNotFoundExceptionAndDoesNotFireEvent()
        {
            string key = "nonExistingKey";
            _hashTable.Add("someOtherKey", 1);

            _countChangedEventFiredCount = 0;
            _refChangedEventFiredCount = 0;

            Assert.ThrowsException<KeyNotFoundException>(() => _hashTable.Remove(key));

            Assert.AreEqual(0, _countChangedEventFiredCount);
            Assert.AreEqual(0, _refChangedEventFiredCount);
            Assert.AreEqual(1, _hashTable.Count);
        }


        [TestMethod]
        public void IndexerSet_ExistingElement_FiresCollectionReferenceChangedEvent()
        {
            string key = "keyToUpdate";
            int initialValue = 30;
            int newValue = 300;
            _hashTable.Add(key, initialValue);

            _countChangedEventFiredCount = 0;
            _refChangedEventFiredCount = 0;
            _lastRefChangedEventArgs = null;

            string expectedChangedItem = $"{key}: {initialValue} -> {newValue}";

            _hashTable[key] = newValue;

            Assert.AreEqual(0, _countChangedEventFiredCount);
            Assert.AreEqual(1, _refChangedEventFiredCount);
            Assert.IsNotNull(_lastRefChangedEventArgs);
            Assert.AreEqual(TestTableName, _lastRefChangedEventArgs.CollectionName);
            Assert.AreEqual("Change", _lastRefChangedEventArgs.ChangeType);
            Assert.AreEqual(expectedChangedItem, _lastRefChangedEventArgs.ChangedItem);
            Assert.IsTrue(_lastRefChangedEventArgs.Time > TimeSpan.Zero);
            Assert.AreEqual(newValue, _hashTable[key]);
            Assert.AreEqual(1, _hashTable.Count);
        }

        [TestMethod]
        public void IndexerSet_NonExistingElement_ThrowsKeyNotFoundExceptionAndDoesNotFireEvent()
        {
            string key = "nonExistingKeyToSet";
            int newValue = 400;
            _hashTable.Add("someOtherKey", 1);

            _countChangedEventFiredCount = 0;
            _refChangedEventFiredCount = 0;

            Assert.ThrowsException<KeyNotFoundException>(() => _hashTable[key] = newValue);

            Assert.AreEqual(0, _countChangedEventFiredCount);
            Assert.AreEqual(0, _refChangedEventFiredCount);
            Assert.AreEqual(1, _hashTable.Count);
        }

        [TestMethod]
        public void AddThenRemove_FiresCorrectEventsInOrder()
        {
            string key = "tempKey";
            int value = 50;
            string expectedAddChangedItem = $"{key}: {value}";
            string expectedRemoveChangedItem = $"{key}: {value}";
            List<CollectionHandlerEventArgs> receivedEvents = new List<CollectionHandlerEventArgs>();

            _hashTable.CollectionCountChanged += (s, a) => receivedEvents.Add(a);
            _hashTable.CollectionReferenceChanged += (s, a) => receivedEvents.Add(a);

            _countChangedEventFiredCount = 0;
            _refChangedEventFiredCount = 0;

            _hashTable.Add(key, value);
            bool removed = _hashTable.Remove(key);

            Assert.IsTrue(removed);
            Assert.AreEqual(2, receivedEvents.Count);

            Assert.AreEqual("Add", receivedEvents[0].ChangeType);
            Assert.AreEqual(TestTableName, receivedEvents[0].CollectionName);
            Assert.AreEqual(expectedAddChangedItem, receivedEvents[0].ChangedItem);

            Assert.AreEqual("Remove", receivedEvents[1].ChangeType);
            Assert.AreEqual(TestTableName, receivedEvents[1].CollectionName);
            Assert.AreEqual(expectedRemoveChangedItem, receivedEvents[1].ChangedItem);

            Assert.IsTrue(receivedEvents[1].Time >= receivedEvents[0].Time);
            Assert.AreEqual(0, _hashTable.Count);
        }

        [TestMethod]
        public void AddThenSet_FiresCorrectEventsInOrder()
        {
            string key = "updateKey";
            int value1 = 60;
            int value2 = 600;
            string expectedAddChangedItem = $"{key}: {value1}";
            string expectedChangeChangedItem = $"{key}: {value1} -> {value2}";
            List<CollectionHandlerEventArgs> receivedEvents = new List<CollectionHandlerEventArgs>();

            _hashTable.CollectionCountChanged -= (sender, args) => { _countChangedEventFiredCount++; _lastCountChangedEventArgs = args; };
            _hashTable.CollectionReferenceChanged -= (sender, args) => { _refChangedEventFiredCount++; _lastRefChangedEventArgs = args; };

            _hashTable.CollectionCountChanged += (s, a) => receivedEvents.Add(a);
            _hashTable.CollectionReferenceChanged += (s, a) => receivedEvents.Add(a);

            _hashTable.Add(key, value1);
            _hashTable[key] = value2;

            Assert.AreEqual(2, receivedEvents.Count);

            Assert.AreEqual("Add", receivedEvents[0].ChangeType);
            Assert.AreEqual(TestTableName, receivedEvents[0].CollectionName);
            Assert.AreEqual(expectedAddChangedItem, receivedEvents[0].ChangedItem);
            Assert.IsTrue(receivedEvents[0] is CollectionHandlerEventArgs);

            Assert.AreEqual("Change", receivedEvents[1].ChangeType);
            Assert.AreEqual(TestTableName, receivedEvents[1].CollectionName);
            Assert.AreEqual(expectedChangeChangedItem, receivedEvents[1].ChangedItem);
            Assert.IsTrue(receivedEvents[1] is CollectionHandlerEventArgs);

            Assert.IsTrue(receivedEvents[1].Time >= receivedEvents[0].Time);
            Assert.AreEqual(1, _hashTable.Count);
            Assert.AreEqual(value2, _hashTable[key]);
        }
    }
}