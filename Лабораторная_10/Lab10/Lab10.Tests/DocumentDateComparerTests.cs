using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab10.ClassLibrary;
using System;

namespace Lab10.Tests
{
    [TestClass]
    public class DocumentDateComparerTests
    {
        private readonly DocumentDateComparer _comparer = new();

        [TestMethod]
        public void Compare_WithNullObjects_ReturnsZero()
        {
            // Act & Assert
            Assert.AreEqual(0, _comparer.Compare(null, null));
            Assert.AreEqual(0, _comparer.Compare(new Document(), null));
            Assert.AreEqual(0, _comparer.Compare(null, new Document()));
        }

        [TestMethod]
        public void Compare_WithSameDates_ReturnsZero()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            Document doc1 = new Document("1", date, "Author1");
            Document doc2 = new Document("2", date, "Author2");

            // Act & Assert
            Assert.AreEqual(0, _comparer.Compare(doc1, doc2));
        }

        [TestMethod]
        public void Compare_WithDifferentDates_ReturnsCorrectOrder()
        {
            // Arrange
            Document doc1 = new Document("1", new DateTime(2024, 1, 1), "Author1");
            Document doc2 = new Document("2", new DateTime(2024, 1, 2), "Author2");

            // Act & Assert
            Assert.IsTrue(_comparer.Compare(doc1, doc2) < 0); // doc1 раньше doc2
            Assert.IsTrue(_comparer.Compare(doc2, doc1) > 0); // doc2 позже doc1
        }

        [TestMethod]
        public void Compare_WithDifferentTimesSameDate_ReturnsZero()
        {
            // Arrange
            Document doc1 = new Document("1", new DateTime(2024, 1, 1, 9, 0, 0), "Author1");
            Document doc2 = new Document("2", new DateTime(2024, 1, 1, 18, 0, 0), "Author2");

            // Act & Assert
            Assert.AreEqual(0, _comparer.Compare(doc1, doc2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Compare_WithNonDocumentObjects_ThrowsArgumentException()
        {
            // Act
            _comparer.Compare(new object(), new object());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Compare_WithOneDocumentOneObject_ThrowsArgumentException()
        {
            // Act
            _comparer.Compare(new Document(), new object());
        }
    }
}
