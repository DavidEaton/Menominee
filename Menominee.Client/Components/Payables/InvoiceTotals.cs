using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Common.Enums;

namespace Menominee.Client.Components.Payables
{
    public class InvoiceTotals
    {
        public InvoiceTotals()
        {
            Clear();
        }

        public double Purchases { get; set; }
        public double Returns { get; set; }
        public double CoreReturns { get; set; }
        public double Defectives { get; set; }
        public double Warranties { get; set; }
        public double MiscellaneousDebits { get; set; }
        public double MiscellaneousCredits { get; set; }
        public double BalanceForwards { get; set; }
        public double Taxes { get; set; }
        public double Total { get; set; }
        public double Payments { get; set; }
        public double TaxableTotal { get; set; }

        public void Clear()
        {
            Purchases = 0; 
            Returns = 0; 
            CoreReturns = 0;
            Defectives = 0; 
            Warranties = 0;
            MiscellaneousDebits = 0; 
            MiscellaneousCredits = 0; 
            BalanceForwards = 0;
            Taxes = 0;
            Total = 0;
            Payments = 0;
            TaxableTotal = 0;
        }

        public void Calculate(VendorInvoiceToWrite invoice)
        {
            Clear();

            if (invoice is null)
                return;

            foreach (var item in invoice.LineItems)
            {
                double amount = (item.Cost + item.Core) * item.Quantity;
                switch (item.Type)
                {
                    case VendorInvoiceLineItemType.Purchase:
                        Purchases += amount;
                        break;
                    case VendorInvoiceLineItemType.Return:
                        Returns += amount;
                        break;
                    case VendorInvoiceLineItemType.CoreReturn:
                        CoreReturns += amount;
                        break;
                    case VendorInvoiceLineItemType.Defective:
                        Defectives += amount;
                        break;
                    case VendorInvoiceLineItemType.Warranty:
                        Warranties += amount;
                        break;
                    case VendorInvoiceLineItemType.MiscDebit:
                        MiscellaneousDebits += amount;
                        break;
                    case VendorInvoiceLineItemType.MiscCredit:
                        MiscellaneousCredits += amount;
                        break;
                    case VendorInvoiceLineItemType.BalanceForward:
                        BalanceForwards += amount;
                        break;
                    default:
                        break;
                }
            }

            foreach(var tax in invoice.Taxes)
            {
                Taxes += tax.Amount;
            }

            foreach (var payment in invoice.Payments)
            {
                Payments += payment.Amount;
            }

            Total = Purchases + Returns + CoreReturns + Defectives + Warranties
                  + MiscellaneousDebits + MiscellaneousCredits + BalanceForwards + Taxes;

            TaxableTotal = Total - BalanceForwards - Taxes;
        }
    }
}
