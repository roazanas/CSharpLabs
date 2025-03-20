using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab10.ClassLibrary;
using System;

namespace Lab10.Tests
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_CreatesDocument()
        {
            // Arrange
            string number = "123";
            DateTime date = DateTime.Now;
            string author = "Test Author";

            // Act
            Document doc = new Document(number, date, author);

            // Assert
            Assert.AreEqual(number, doc.Number);
            Assert.AreEqual(date, doc.Date);
            Assert.AreEqual(author, doc.Author);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullNumber_ThrowsArgumentNullException()
        {
            new Document(null, DateTime.Now, "Author");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullDate_ThrowsArgumentNullException()
        {
            new Document("123", null, "Author");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullAuthor_ThrowsArgumentNullException()
        {
            new Document("123", DateTime.Now, null);
        }

        [TestMethod]
        public void Constructor_WithTwoParameters_SetsDefaultAuthor()
        {
            // Arrange
            string number = "123";
            DateTime date = DateTime.Now;

            // Act
            Document doc = new Document(number, date);

            // Assert
            Assert.AreEqual(number, doc.Number);
            Assert.AreEqual(date, doc.Date);
            Assert.AreEqual("Неизвестный", doc.Author);
        }

        [TestMethod]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Act
            Document doc = new Document();

            // Assert
            Assert.AreEqual("Неизвестно", doc.Number);
            Assert.IsNotNull(doc.Date);
            Assert.AreEqual("Неизвестный", doc.Author);
        }

        [TestMethod]
        public void CopyConstructor_CreatesDeepCopy()
        {
            // Arrange
            Document original = new Document("123", DateTime.Now, "Author");
            original.applicationReceipt = new Receipt(100, "Наличные", "123", DateTime.Now);

            // Act
            Document copy = new Document(original);

            // Assert
            Assert.AreEqual(original.Number, copy.Number);
            Assert.AreEqual(original.Date, copy.Date);
            Assert.AreEqual(original.Author, copy.Author);
            Assert.IsNull(copy.applicationReceipt); // Проверяем, что это действительно глубокая копия
        }

        [TestMethod]
        public void Equals_WithSameValues_ReturnsTrue()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Document doc1 = new Document("123", date, "Author");
            Document doc2 = new Document("123", date, "Author");

            // Act & Assert
            Assert.IsTrue(doc1.Equals(doc2));
        }

        [TestMethod]
        public void Equals_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            Document doc1 = new Document("123", DateTime.Now, "Author1");
            Document doc2 = new Document("124", DateTime.Now, "Author2");

            // Act & Assert
            Assert.IsFalse(doc1.Equals(doc2));
        }

        [TestMethod]
        public void GetDocumentInfo_ReturnsCorrectString()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            Document doc = new Document("123", date, "Author");

            // Act
            string info = doc.GetDocumentInfo();

            // Assert
            Assert.IsTrue(info.Contains("123"));
            Assert.IsTrue(info.Contains("01.01.2024"));
            Assert.IsTrue(info.Contains("Author"));
        }

        [TestMethod]
        public void GetAmount_ReturnsZero()
        {
            // Arrange
            Document doc = new Document("123", DateTime.Now, "Author");

            // Act & Assert
            Assert.AreEqual(0, doc.GetAmount());
        }

        [TestMethod]
        public void CompareTo_WithNullObject_ReturnsOne()
        {
            // Arrange
            Document doc = new Document();

            // Act & Assert
            Assert.AreEqual(1, doc.CompareTo(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareTo_WithNonDocumentObject_ThrowsArgumentException()
        {
            // Arrange
            Document doc = new Document();

            // Act
            doc.CompareTo(new object());
        }

        [TestMethod]
        public void GetHashCode_WithSameValues_ReturnsSameHash()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Document doc1 = new Document("123", date, "Author");
            Document doc2 = new Document("123", date, "Author");

            // Act & Assert
            Assert.AreEqual(doc1.GetHashCode(), doc2.GetHashCode());
        }
    }
}
