using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab11.ClassLibrary.Utils;

namespace Lab11.ClassLibrary
{
    public class Receipt : Document
    {
        private int amount;
        private string? paymentMethod;

        public Document BaseDocument
        {
            get
            {
                return new Document(Number, Date, Author);
            }
        }

        public Receipt(Receipt receipt) : base(receipt.Number, receipt.Date, receipt.Author)
        {
            Amount = receipt.Amount;
            PaymentMethod = receipt.PaymentMethod;
        }

        public Receipt(int amount, string? paymentMethod, string? number, DateTime? date, string? author) : base(number, date, author)
        {
            Amount = amount;
            PaymentMethod = paymentMethod;
        }

        public Receipt(int amount, string? paymentMethod, string? number, DateTime? date) : base(number, date)
        {
            Amount = amount;
            PaymentMethod = paymentMethod;
        }

        public Receipt() : base()
        {
            Amount = 0;
            PaymentMethod = "Наличные";
        }

        public int Amount
        {
            get => amount;
            set => amount = value >= 0 
                ? value 
                : throw new ArgumentOutOfRangeException(nameof(value), "Сумма не может быть отрицательной");
        }

        public string? PaymentMethod
        {
            get => paymentMethod;
            set => paymentMethod = !string.IsNullOrEmpty(value) 
                ? value 
                : throw new ArgumentNullException(nameof(value), "Способ оплаты не может быть пустым");
        }

        public override string ToString()
        {
            return $"<MAGENTA>Чек<WHITE>: Сумма: {Amount} руб., способ оплаты: \"<YELLOW>{PaymentMethod}<WHITE>\" | {base.ToString()}";
        }

        public override bool Equals(object? obj)
        {
            if (!base.Equals(obj))
                return false;

            Receipt other = (Receipt)obj;
            return Amount == other.Amount && PaymentMethod == other.PaymentMethod;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Amount, PaymentMethod);
        }

        public new string GetDocumentInfo()
        {
            return $"Информация о чеке: №{Number}, сумма: {Amount} руб., способ оплаты: \"<YELLOW>{PaymentMethod}<WHITE>\", "
                 + $"дата: {Date:dd.MM.yyyy}, кассир: <YELLOW>{Author}";
        }

        public override int GetAmount() => Amount;

        public override void Init()
        {
            CopyFrom(DocumentGenerator.CreateReceiptFromUserInput());
        }

        public override void RandomInit()
        {
            CopyFrom(DocumentGenerator.GenerateRandomReceipt());
        }

        protected override void CopyFrom(Document other)
        {
            if (other == null) return;
            
            base.CopyFrom(other);
            if (other is Receipt receipt)
            {
                Amount = receipt.Amount;
                PaymentMethod = receipt.PaymentMethod;
            }
        }

        public new object Clone()
        {
            Receipt clonedReceipt = new Receipt(this);
            if (applicationReceipt != null)
            {
                clonedReceipt.applicationReceipt = (Receipt)applicationReceipt.Clone();
            }
            return clonedReceipt;
        }

        public new Receipt ShallowCopy()
        {
            return (Receipt)MemberwiseClone();
        }
    }
}
