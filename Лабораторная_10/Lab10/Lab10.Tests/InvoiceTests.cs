using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab10.ClassLibrary;
using System;

namespace Lab10.Tests
{
    [TestClass]
    public class InvoiceTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_CreatesInvoice()
        {
            // Arrange
            int amount = 100;
            int quantity = 5;
            string productType = "Test Product";
            string number = "123";
            DateTime date = DateTime.Now;
            string author = "Test Author";

            // Act
            Invoice invoice = new Invoice(amount, quantity, productType, number, date, author);

            // Assert
            Assert.AreEqual(amount, invoice.Amount);
            Assert.AreEqual(quantity, invoice.Quantity);
            Assert.AreEqual(productType, invoice.ProductType);
            Assert.AreEqual(number, invoice.Number);
            Assert.AreEqual(date, invoice.Date);
            Assert.AreEqual(author, invoice.Author);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_WithNegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            new Invoice(-100, 5, "Product", "123", DateTime.Now, "Author");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_WithNegativeQuantity_ThrowsArgumentOutOfRangeException()
        {
            new Invoice(100, -5, "Product", "123", DateTime.Now, "Author");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullProductType_ThrowsArgumentNullException()
        {
            new Invoice(100, 5, string.Empty, "123", DateTime.Now, "Author");
        }

        [TestMethod]
        public void Constructor_WithoutAuthor_SetsDefaultAuthor()
        {
            // Arrange
            int amount = 100;
            int quantity = 5;
            string productType = "Test Product";
            string number = "123";
            DateTime date = DateTime.Now;

            // Act
            Invoice invoice = new Invoice(amount, quantity, productType, number, date);

            // Assert
            Assert.AreEqual("Неизвестный", invoice.Author);
        }

        [TestMethod]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Act
            Invoice invoice = new Invoice();

            // Assert
            Assert.AreEqual(0, invoice.Amount);
            Assert.AreEqual(0, invoice.Quantity);
            Assert.AreEqual("Не указан", invoice.ProductType);
            Assert.AreEqual("Неизвестно", invoice.Number);
            Assert.IsNotNull(invoice.Date);
            Assert.AreEqual("Неизвестный", invoice.Author);
        }

        [TestMethod]
        public void CopyConstructor_CreatesDeepCopy()
        {
            // Arrange
            Invoice original = new Invoice(100, 5, "Product", "123", DateTime.Now, "Author");

            // Act
            Invoice copy = new Invoice(original);

            // Assert
            Assert.AreEqual(original.Amount, copy.Amount);
            Assert.AreEqual(original.Quantity, copy.Quantity);
            Assert.AreEqual(original.ProductType, copy.ProductType);
            Assert.AreEqual(original.Number, copy.Number);
            Assert.AreEqual(original.Date, copy.Date);
            Assert.AreEqual(original.Author, copy.Author);
        }

        [TestMethod]
        public void Equals_WithSameValues_ReturnsTrue()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Invoice invoice1 = new Invoice(100, 5, "Product", "123", date, "Author");
            Invoice invoice2 = new Invoice(100, 5, "Product", "123", date, "Author");

            // Act & Assert
            Assert.IsTrue(invoice1.Equals(invoice2));
        }

        [TestMethod]
        public void Equals_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            Invoice invoice1 = new Invoice(100, 5, "Product1", "123", DateTime.Now, "Author1");
            Invoice invoice2 = new Invoice(200, 10, "Product2", "124", DateTime.Now, "Author2");

            // Act & Assert
            Assert.IsFalse(invoice1.Equals(invoice2));
        }

        [TestMethod]
        public void GetDocumentInfo_ReturnsCorrectString()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            Invoice invoice = new Invoice(100, 5, "Product", "123", date, "Author");

            // Act
            string info = invoice.GetDocumentInfo();

            // Assert
            Assert.IsTrue(info.Contains("123"));
            Assert.IsTrue(info.Contains("5"));
            Assert.IsTrue(info.Contains("Product"));
            Assert.IsTrue(info.Contains("100"));
            Assert.IsTrue(info.Contains("01.01.2024"));
            Assert.IsTrue(info.Contains("Author"));
        }

        [TestMethod]
        public void GetAmount_ReturnsCorrectAmount()
        {
            // Arrange
            Invoice invoice = new Invoice(100, 5, "Product", "123", DateTime.Now, "Author");

            // Act & Assert
            Assert.AreEqual(100, invoice.GetAmount());
        }

        [TestMethod]
        public void ToString_ReturnsCorrectString()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            Invoice invoice = new Invoice(100, 5, "Product", "123", date, "Author");

            // Act
            string result = invoice.ToString();

            // Assert
            Assert.IsTrue(result.Contains("5"));
            Assert.IsTrue(result.Contains("Product"));
            Assert.IsTrue(result.Contains("100"));
            Assert.IsTrue(result.Contains("123"));
            Assert.IsTrue(result.Contains("01.01.2024"));
            Assert.IsTrue(result.Contains("Author"));
        }
    }
}
