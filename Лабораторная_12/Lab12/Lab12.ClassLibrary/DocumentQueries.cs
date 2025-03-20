using System;
using System.Collections.Generic;

namespace Lab12.ClassLibrary.Utils
{
    public static class DocumentQueries
    {
        public static int GetInvoiceTotalAmountByProduct(Document[] documents, string? productType)
        {
            int invoiceTotal = 0;
            string? lowerProductType = productType?.ToLower();
            foreach (var doc in documents)
            {
                if (doc is Invoice)
                {
                    Invoice? invoice = doc as Invoice;
                    if (invoice?.ProductType.ToLower() == lowerProductType)
                    {
                        invoiceTotal += invoice!.Amount;
                    }
                }
            }
            return invoiceTotal;
        }

        public static int GetReceiptsWithAmountGreaterThan(Document[] documents, int minimumAmount)
        {
            int receiptsCount = 0;
            foreach (var doc in documents)
            {
                if (doc is Receipt)
                {
                    Receipt? receipt = doc as Receipt;
                    if (receipt?.Amount > minimumAmount)
                    {
                        receiptsCount++;
                    }
                }
            }
            return receiptsCount;
        }

        public static int GetTotalReceiptsAmount(Document[] documents, string? organization)
        {
            int receiptsTotal = 0;
            string? lowerOrganization = organization?.ToLower();
            
            foreach (var doc in documents)
            {
                if (doc is Receipt)
                {
                    Receipt? receipt = doc as Receipt;
                    if (receipt?.Author?.ToLower() == lowerOrganization)
                    {
                        receiptsTotal += receipt.Amount;
                    }
                }
            }
            return receiptsTotal;
        }
    }
}
