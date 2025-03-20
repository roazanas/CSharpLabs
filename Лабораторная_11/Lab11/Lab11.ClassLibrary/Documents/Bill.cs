using System;
using Lab11.ClassLibrary.Utils;

namespace Lab11.ClassLibrary
{
    public class Bill : Document
    {
        private int amountDue;
        private DateTime? payDate;

        public Document BaseDocument
        {
            get
            {
                return new Document(Number, Date, Author);
            }
        }

        public Bill(int amountDue, DateTime? payDate, string? number, DateTime? date, string? author) : base(number, date, author)
        {
            AmountDue = amountDue;
            PayDate = payDate;
        }

        public Bill(int amountDue, DateTime? payDate, string? number, DateTime? date) : base(number, date)
        {
            AmountDue = amountDue;
            PayDate = payDate;
        }

        public Bill() : base()
        {
            AmountDue = 0;
            PayDate = DateTime.Now;
        }

        public Bill(Bill other) : base(other)
        {
            AmountDue = other.AmountDue;
            PayDate = other.PayDate;
        }

        public int AmountDue
        {
            get => amountDue;
            set => amountDue = value >= 0 
                ? value 
                : throw new ArgumentOutOfRangeException(nameof(value), "Сумма не может быть отрицательной");
        }

        public DateTime? PayDate
        {
            get => payDate;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "Дата для оплаты счета не может быть пустой");
                if (value < Date)
                    throw new ArgumentException("Дата оплаты не может быть раньше даты выставления счета", nameof(value));
                payDate = value;
            }
        }

        public override string ToString()
        {
            return $"<DBLUE>Счёт<WHITE>: Сумма к оплате: {AmountDue} руб., оплатить до: {PayDate:dd.MM.yyyy} | {base.ToString()}";
        }

        public override bool Equals(object? obj)
        {
            if (!base.Equals(obj))
                return false;

            Bill other = (Bill)obj;
            return AmountDue == other.AmountDue && PayDate == other.PayDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), AmountDue, PayDate);
        }

        public override int GetAmount() => AmountDue;

        public new string GetDocumentInfo()
        {
            return $"Информация о счёте: №{Number}, сумма к оплате: {AmountDue} руб., оплатить до: {PayDate:dd.MM.yyyy}, "
                 + $"дата выставления: {Date:dd.MM.yyyy}, выставитель: <YELLOW>{Author}";
        }

        public override void Init()
        {
            Bill bill = DocumentGenerator.CreateBillFromUserInput();
            CopyFrom(bill);
        }

        public override void RandomInit()
        {
            Bill bill = DocumentGenerator.GenerateRandomBill();
            CopyFrom(bill);
        }

        protected override void CopyFrom(Document other)
        {
            if (other == null) return;
            
            base.CopyFrom(other);
            if (other is Bill bill)
            {
                AmountDue = bill.AmountDue;
                PayDate = bill.PayDate;
            }
        }
    }
}
