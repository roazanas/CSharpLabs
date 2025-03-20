using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab10.ClassLibrary;
using System;

namespace Lab10.Tests
{
    [TestClass]
    public class BillTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_CreatesBill()
        {
            // Arrange
            int amountDue = 100;
            DateTime date = DateTime.Now;
            DateTime payDate = date.AddDays(30);
            string number = "123";
            string author = "Test Author";

            // Act
            Bill bill = new Bill(amountDue, payDate, number, date, author);

            // Assert
            Assert.AreEqual(amountDue, bill.AmountDue);
            Assert.AreEqual(payDate, bill.PayDate);
            Assert.AreEqual(number, bill.Number);
            Assert.AreEqual(date, bill.Date);
            Assert.AreEqual(author, bill.Author);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_WithNegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            DateTime date = DateTime.Now;
            new Bill(-100, date.AddDays(30), "123", date, "Author");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullPayDate_ThrowsArgumentNullException()
        {
            new Bill(100, null, "123", DateTime.Now, "Author");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_WithPayDateBeforeDocumentDate_ThrowsArgumentException()
        {
            DateTime date = DateTime.Now;
            new Bill(100, date.AddDays(-1), "123", date, "Author");
        }

        [TestMethod]
        public void Constructor_WithoutAuthor_SetsDefaultAuthor()
        {
            // Arrange
            int amountDue = 100;
            DateTime date = DateTime.Now;
            DateTime payDate = date.AddDays(30);
            string number = "123";

            // Act
            Bill bill = new Bill(amountDue, payDate, number, date);

            // Assert
            Assert.AreEqual("Неизвестный", bill.Author);
        }

        [TestMethod]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Act
            Bill bill = new Bill();

            // Assert
            Assert.AreEqual(0, bill.AmountDue);
            Assert.IsNotNull(bill.PayDate);
            Assert.AreEqual("Неизвестно", bill.Number);
            Assert.IsNotNull(bill.Date);
            Assert.AreEqual("Неизвестный", bill.Author);
        }

        [TestMethod]
        public void CopyConstructor_CreatesDeepCopy()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Bill original = new Bill(100, date.AddDays(30), "123", date, "Author");

            // Act
            Bill copy = new Bill(original);

            // Assert
            Assert.AreEqual(original.AmountDue, copy.AmountDue);
            Assert.AreEqual(original.PayDate, copy.PayDate);
            Assert.AreEqual(original.Number, copy.Number);
            Assert.AreEqual(original.Date, copy.Date);
            Assert.AreEqual(original.Author, copy.Author);
        }

        [TestMethod]
        public void Equals_WithSameValues_ReturnsTrue()
        {
            // Arrange
            DateTime date = DateTime.Now;
            DateTime payDate = date.AddDays(30);
            Bill bill1 = new Bill(100, payDate, "123", date, "Author");
            Bill bill2 = new Bill(100, payDate, "123", date, "Author");

            // Act & Assert
            Assert.IsTrue(bill1.Equals(bill2));
        }

        [TestMethod]
        public void Equals_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Bill bill1 = new Bill(100, date.AddDays(30), "123", date, "Author1");
            Bill bill2 = new Bill(200, date.AddDays(60), "124", date, "Author2");

            // Act & Assert
            Assert.IsFalse(bill1.Equals(bill2));
        }

        [TestMethod]
        public void GetDocumentInfo_ReturnsCorrectString()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            DateTime payDate = date.AddDays(30);
            Bill bill = new Bill(100, payDate, "123", date, "Author");

            // Act
            string info = bill.GetDocumentInfo();

            // Assert
            Assert.IsTrue(info.Contains("123"));
            Assert.IsTrue(info.Contains("100"));
            Assert.IsTrue(info.Contains("31.01.2024")); // PayDate
            Assert.IsTrue(info.Contains("01.01.2024")); // Date
            Assert.IsTrue(info.Contains("Author"));
        }

        [TestMethod]
        public void GetAmount_ReturnsCorrectAmount()
        {
            // Arrange
            Bill bill = new Bill(100, DateTime.Now.AddDays(30), "123", DateTime.Now, "Author");

            // Act & Assert
            Assert.AreEqual(100, bill.GetAmount());
        }

        [TestMethod]
        public void ToString_ReturnsCorrectString()
        {
            // Arrange
            DateTime date = new DateTime(2024, 1, 1);
            DateTime payDate = date.AddDays(30);
            Bill bill = new Bill(100, payDate, "123", date, "Author");

            // Act
            string result = bill.ToString();

            // Assert
            Assert.IsTrue(result.Contains("100"));
            Assert.IsTrue(result.Contains("31.01.2024")); // PayDate
            Assert.IsTrue(result.Contains("123"));
            Assert.IsTrue(result.Contains("01.01.2024")); // Date
            Assert.IsTrue(result.Contains("Author"));
        }
    }
}
