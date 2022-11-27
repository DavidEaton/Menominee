using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.Helpers
{
    public static class VendorInvoiceHelper
    {
        public static VendorInvoice CreateVendorInvoice()
        {
            Vendor vendor = CreateVendor();
            VendorInvoiceStatus status = VendorInvoiceStatus.Open;
            VendorInvoiceDocumentType documentType = VendorInvoiceDocumentType.Invoice;
            double Total = 0.0;
            var vendorInvoiceNumbers = CreateVendorInvoiceNumbersList(vendor);

            return VendorInvoice.Create(vendor, status, documentType, Total, vendorInvoiceNumbers).Value;
        }

        public static IReadOnlyList<string> CreateVendorInvoiceNumbers(Vendor vendor, List<int> invoiceNumbers)
        {
            return invoiceNumbers.Select(invoiceNumber => $"{vendor.Id}{invoiceNumber}").ToList();
        }

        public static List<string> CreateVendorInvoiceNumbersList(Vendor vendor)
        {
            return new List<string>()
            {
                { $"{vendor.Id}{1}" },
                { $"{vendor.Id}{2}" },
                { $"{vendor.Id}{3}" },
            };
        }

        public static IList<VendorInvoiceLineItemToWrite> CreateLineItems(VendorInvoiceLineItemType type, int lineItemCount, double core, double cost, double lineItemQuantity)
        {
            var lineItems = new List<VendorInvoiceLineItemToWrite>();

            for (int i = 0; i < lineItemCount; i++)
            {
                lineItems.Add(
                    new VendorInvoiceLineItemToWrite()
                    {
                        Core = core,
                        Cost = cost,
                        Quantity = lineItemQuantity,
                        PONumber = string.Empty,
                        Type = type
                    });
            }

            return lineItems;
        }

        public static IList<VendorInvoiceTaxToWrite> CreateTaxes(int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTaxToWrite>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(new VendorInvoiceTaxToWrite()
                {
                    Amount = taxAmount
                });
            }

            return taxes;
        }

        public static IList<VendorInvoicePaymentToWrite> CreatePayments(int paymentCount, double paymentAmount)
        {
            var payments = new List<VendorInvoicePaymentToWrite>();

            for (int i = 0; i < paymentCount; i++)
            {
                payments.Add(new VendorInvoicePaymentToWrite()
                {
                    Amount = paymentAmount
                });
            }

            return payments;
        }

    }
}
