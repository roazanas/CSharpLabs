using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab10.ClassLibrary;
using System;

namespace Lab10.Tests
{
    [TestClass]
    public class ReceiptTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_CreatesReceipt()
        {
            // Arrange
            int amount = 100;
            string paymentMethod = "Card";
            string number = "123";
            DateTime date = DateTime.Now;
            string author = "Test Author";

            // Act
            Receipt receipt = new Receipt(amount, paymentMethod, number, date, author);

            // Assert
            Assert.AreEqual(amount, receipt.Amount);
            Assert.AreEqual(paymentMethod, receipt.PaymentMethod);
            Assert.AreEqual(number, receipt.Number);
            Assert.AreEqual(date, receipt.Date);
            Assert.AreEqual(author, receipt.Author);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_WithNegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            new Receipt(-100, "Card", "123", DateTime.Now, "Author");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithEmptyPaymentMethod_ThrowsArgumentNullException()
        {
            new Receipt(100, string.Empty, "123", DateTime.Now, "Author");
        }

        [TestMethod]
        public void Constructor_WithoutAuthor_SetsDefaultAuthor()
        {
            // Arrange
            int amount = 100;
            string paymentMethod = "Card";
            string number = "123";
            DateTime date = DateTime.Now;

            // Act
            Receipt receipt = new Receipt(amount, paymentMethod, number, date);

            // Assert
            Assert.AreEqual("Неизвестный", receipt.Author);
        }

        [TestMethod]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Act
            Receipt receipt = new Receipt();

            // Assert
            Assert.AreEqual(0, receipt.Amount);
            Assert.AreEqual("Наличные", receipt.PaymentMethod);
            Assert.AreEqual("Неизвестно", receipt.Number);
            Assert.IsNotNull(receipt.Date);
            Assert.AreEqual("Неизвестный", receipt.Author);
        }

        [TestMethod]
        public void CopyConstructor_CreatesDeepCopy()
        {
            // Arrange
            Receipt original = new Receipt(100, "Card", "123", DateTime.Now, "Author");

            // Act
            Receipt copy = new Receipt(original);

            // Assert
            Assert.AreEqual(original.Amount, copy.Amount);
            Assert.AreEqual(original.PaymentMethod, copy.PaymentMethod);
            Assert.AreEqual(original.Number, copy.Number);
            Assert.AreEqual(original.Date, copy.Date);
            Assert.AreEqual(original.Author, copy.Author);
        }

        [TestMethod]
        public void Equals_WithSameValues_ReturnsTrue()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Receipt receipt1 = new Receipt(100, "Card", "123", date, "Author");
            Receipt receipt2 = new Receipt(100, "Card", "123", date, "Author");

            // Act & Assert
            Assert.IsTrue(receipt1.Equals(receipt2));
        }

        [TestMethod]
        public void Equals_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            Receipt receipt1 = new Receipt(100, "Card", "123", DateTime.Now, "Author1");
            Receipt receipt2 = new Receipt(200, "Cash", "124", DateTime.Now, "Author2");

            // Act & Assert
            Assert.IsFalse(receipt1.Equals(receipt2));
        }

        [TestMethod]
        public void GetDocumentInfo_ReturnsCorrectString()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            Receipt receipt = new Receipt(100, "Card", "123", date, "Author");

            // Act
            string info = receipt.GetDocumentInfo();

            // Assert
            Assert.IsTrue(info.Contains("123"));
            Assert.IsTrue(info.Contains("100"));
            Assert.IsTrue(info.Contains("Card"));
            Assert.IsTrue(info.Contains("01.01.2024"));
            Assert.IsTrue(info.Contains("Author"));
        }

        [TestMethod]
        public void GetAmount_ReturnsCorrectAmount()
        {
            // Arrange
            Receipt receipt = new Receipt(100, "Card", "123", DateTime.Now, "Author");

            // Act & Assert
            Assert.AreEqual(100, receipt.GetAmount());
        }

        [TestMethod]
        public void ToString_ReturnsCorrectString()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            Receipt receipt = new Receipt(100, "Card", "123", date, "Author");

            // Act
            string result = receipt.ToString();

            // Assert
            Assert.IsTrue(result.Contains("100"));
            Assert.IsTrue(result.Contains("Card"));
            Assert.IsTrue(result.Contains("123"));
            Assert.IsTrue(result.Contains("01.01.2024"));
            Assert.IsTrue(result.Contains("Author"));
        }
    }
}
