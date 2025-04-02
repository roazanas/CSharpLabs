using System;
using Lab13.ClassLibrary.Utils;

namespace Lab13.ClassLibrary
{
    public class Invoice : Document
    {
        private int amount;
        private int quantity;
        private string productType;

        public Invoice(int amount, int quantity, string productType, string? number, DateTime? date, string? author) : base(number, date, author)
        {
            Amount = amount;
            Quantity = quantity;
            ProductType = productType;
        }

        public Invoice(int amount, int quantity, string productType, string? number, DateTime? date) : base(number, date)
        {
            Amount = amount;
            Quantity = quantity;
            ProductType = productType;
        }

        public Invoice() : base()
        {
            Amount = 0;
            Quantity = 0;
            ProductType = "Не указан";
        }

        public Invoice(Invoice other) : base(other)
        {
            Amount = other.Amount;
            Quantity = other.Quantity;
            ProductType = other.ProductType;
        }

        public int Amount
        {
            get => amount;
            set => amount = value >= 0 
                ? value 
                : throw new ArgumentOutOfRangeException(nameof(value), "Сумма не может быть отрицательной");
        }

        public int Quantity
        {
            get => quantity;
            set => quantity = value >= 0 
                ? value 
                : throw new ArgumentOutOfRangeException(nameof(value), "Количество не может быть отрицательным");
        }

        public string ProductType
        {
            get => productType;
            set => productType = !string.IsNullOrEmpty(value) 
                ? value 
                : throw new ArgumentNullException(nameof(value), "Тип продукта не может быть пустым");
        }

        public override string ToString()
        {
            return $"Накладная: {Quantity} единиц товара \"{ProductType}\" на сумму {Amount} руб. | {base.ToString()}";
        }

        public override bool Equals(object? obj)
        {
            if (!base.Equals(obj))
                return false;

            Invoice other = (Invoice)obj;
            return Amount == other.Amount 
                   && Quantity == other.Quantity 
                   && ProductType == other.ProductType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Amount, Quantity, ProductType);
        }

        public new string GetDocumentInfo()
        {
            return $"Информация о накладной: №{Number}, {Quantity} единиц товара \"{ProductType}\" "
                 + $"на сумму {Amount} руб., дата: {Date:dd.MM.yyyy}, поставщик: {Author}";
        }

        public override int GetAmount() => Amount;

        public override void Init()
        {
            CopyFrom(DocumentGenerator.CreateInvoiceFromUserInput());
        }

        public override void RandomInit()
        {
            CopyFrom(DocumentGenerator.GenerateRandomInvoice());
        }

        protected override void CopyFrom(Document other)
        {
            if (other == null) return;
            
            base.CopyFrom(other);
            if (other is Invoice invoice)
            {
                Amount = invoice.Amount;
                Quantity = invoice.Quantity;
                ProductType = invoice.ProductType;
            }
        }
    }
}
