using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab10.ClassLibrary;
using System;

namespace Lab10.Tests
{
    [TestClass]
    public class NotebookTests
    {
        [TestMethod]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Act
            Notebook notebook = new Notebook();

            // Assert
            Assert.AreEqual("Без заголовка", notebook.Title);
            Assert.AreEqual("Пусто", notebook.Content);
            Assert.IsNotNull(notebook.CreationDate);
        }

        [TestMethod]
        public void CopyFrom_CopiesAllProperties()
        {
            // Arrange
            Notebook source = new Notebook
            {
                Title = "Test Title",
                Content = "Test Content",
                CreationDate = new DateTime(2024, 1, 1)
            };
            Notebook target = new Notebook();

            // Act
            target.CopyFrom(source);

            // Assert
            Assert.AreEqual(source.Title, target.Title);
            Assert.AreEqual(source.Content, target.Content);
            Assert.AreEqual(source.CreationDate, target.CreationDate);
        }

        [TestMethod]
        public void ToString_ReturnsCorrectString()
        {
            // Arrange
            Notebook notebook = new Notebook
            {
                Title = "Test Title",
                Content = "Test Content",
                CreationDate = new DateTime(2024, 1, 1)
            };

            // Act
            string result = notebook.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Test Title"));
            Assert.IsTrue(result.Contains("Test Content"));
            Assert.IsTrue(result.Contains("01.01.2024"));
        }

        [TestMethod]
        public void Properties_CanBeSetAndGet()
        {
            // Arrange
            string title = "New Title";
            string content = "New Content";
            DateTime date = DateTime.Now;
            Notebook notebook = new Notebook();

            // Act
            notebook.Title = title;
            notebook.Content = content;
            notebook.CreationDate = date;

            // Assert
            Assert.AreEqual(title, notebook.Title);
            Assert.AreEqual(content, notebook.Content);
            Assert.AreEqual(date, notebook.CreationDate);
        }
    }
}
