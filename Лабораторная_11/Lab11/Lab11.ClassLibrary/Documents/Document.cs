using System;
using Lab11.ClassLibrary.Utils;

namespace Lab11.ClassLibrary
{
    public class Document : IInit, IComparable, ICloneable
    {
        private string? number;
        private DateTime? date;
        private string? author;

        // Класс для демонстрации разницы между глубоким и поверхностным копированием
        // (приложенный чек для документа)
        public Receipt? applicationReceipt;

        public string? Number
        {
            get => number;
            set => number = !string.IsNullOrEmpty(value) 
                ? value 
                : throw new ArgumentNullException(nameof(value), "Номер документа не может быть пустым");
        }

        public DateTime? Date
        {
            get => date;
            set => date = value ?? throw new ArgumentNullException(nameof(value), "Дата не может быть пустой");
        }

        public string? Author
        {
            get => author;
            set => author = !string.IsNullOrEmpty(value) 
                ? value 
                : throw new ArgumentNullException(nameof(value), "Автор не может быть пустым");
        }

        public Document(string? number, DateTime? date, string? author)
        {
            Number = number;
            Date = date;
            Author = author;
        }

        public Document(string? number, DateTime? date) : this(number, date, "Неизвестный") { }

        public Document() : this("Неизвестно", DateTime.Now, "Неизвестный") { }

        public Document(Document? other)
        {
            ArgumentNullException.ThrowIfNull(other);
            Number = other.Number;
            Date = other.Date;
            Author = other.Author;
        }

        public override string ToString()
        {
            return $"Документ №{Number} от {Date:dd.MM.yyyy}. Составитель: <YELLOW>{Author}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Document other = (Document)obj;
            return Number == other.Number 
                && Date == other.Date 
                && Author == other.Author;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Date, Author);
        }

        public string GetDocumentInfo()
        {
            return $"Информация о документе: №<YELLOW>{Number}<WHITE>, дата {Date:dd.MM.yyyy}, автор <YELLOW>{Author}";
        }

        // Базовый класс возвращает 0, так как у него нет суммы
        public virtual int GetAmount() => 0;

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            if (obj is Document other)
            {
                if (Number == null && other.Number == null) return 0;
                if (Number == null) return -1;
                if (other.Number == null) return 1;
                return string.Compare(Number, other.Number, StringComparison.Ordinal);
            }
            throw new ArgumentException("Объект не является документом");
        }

        public virtual void Init()
        {
            CopyFrom(DocumentGenerator.CreateDocumentFromUserInput());
        }

        public virtual void RandomInit()
        {
            CopyFrom(DocumentGenerator.GenerateRandomDocument());
        }

        protected virtual void CopyFrom(Document other)
        {
            if (other == null) return;
            
            this.number = other.number;
            this.date = other.date;
            this.author = other.author;
        }

        public virtual object Clone() 
        {
            Document clonedDoc = new Document(Number, Date, Author);
            if (this.applicationReceipt != null)
            {
                clonedDoc.applicationReceipt = (Receipt)this.applicationReceipt.Clone();
            }
            return clonedDoc;
        }

        public virtual Document ShallowCopy() 
        {
            return (Document)this.MemberwiseClone();
        }   
    }
}
