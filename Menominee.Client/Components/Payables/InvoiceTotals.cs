using CSharpFunctionalExtensions;
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

        public double Purchases { get; private set; }
        public double Returns { get; private set; }
        public double CoreReturns { get; private set; }
        public double Defectives { get; private set; }
        public double Warranties { get; private set; }
        public double MiscellaneousDebits { get; private set; }
        public double MiscellaneousCredits { get; private set; }
        public double BalanceForwards { get; private set; }
        public double Taxes { get; private set; }
        public double Total { get; private set; }
        public double Payments { get; private set; }
        public double TaxableTotal { get; private set; }
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

        public Result<InvoiceTotals> CalculateInvoiceTotals(VendorInvoiceToWrite invoice)
        {
            Calculate(invoice);
            return this;
        }

        public Result<double> CalculateInvoiceTaxes(VendorInvoiceToWrite invoice)
        {
            Calculate(invoice);
            return this.Taxes;
        }

        public Result<double> CalculateInvoicePayments(VendorInvoiceToWrite invoice)
        {
            Calculate(invoice);
            return this.Payments;
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

            foreach (var tax in invoice.Taxes)
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
