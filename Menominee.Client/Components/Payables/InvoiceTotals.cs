using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Common.Enums;
using System.Collections.Generic;

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
                    case VendorInvoiceItemType.Purchase:
                        Purchases += amount;
                        break;
                    case VendorInvoiceItemType.Return:
                        Returns += amount;
                        break;
                    case VendorInvoiceItemType.CoreReturn:
                        CoreReturns += amount;
                        break;
                    case VendorInvoiceItemType.Defective:
                        Defectives += amount;
                        break;
                    case VendorInvoiceItemType.Warranty:
                        Warranties += amount;
                        break;
                    case VendorInvoiceItemType.MiscDebit:
                        MiscellaneousDebits += amount;
                        break;
                    case VendorInvoiceItemType.MiscCredit:
                        MiscellaneousCredits += amount;
                        break;
                    case VendorInvoiceItemType.BalanceForward:
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
        }
    }
}
