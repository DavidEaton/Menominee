using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Enums;
using Microsoft.Extensions.Azure;
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

        public static IList<VendorInvoiceLineItemToWrite> CreateLineItemsToWrite(VendorInvoiceLineItemType type, int lineItemCount, double core, double cost, double lineItemQuantity)
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
                        Type = type,
                        Item = new VendorInvoiceItemToWrite()
                        {
                            Description = "a desription",
                            PartNumber = $"Part {RandomCharacters(i)}"
                        }
                    });
            }

            return lineItems;
        }
        
        public static IList<VendorInvoiceLineItem> CreateLineItems(VendorInvoiceLineItemType type, int lineItemCount, double core, double cost, double itemQuantity)
        {
            var lineItems = new List<VendorInvoiceLineItem>();
            for (int i = 0; i < lineItemCount; i++)
            {
                VendorInvoiceItem item = VendorInvoiceItem.Create(
                    $"Part {RandomCharacters(i)}",
                    "a desription")
                    .Value;
                            lineItems.Add(
                                VendorInvoiceLineItem.Create(type, item, itemQuantity, cost, core)
                                .Value);
            }

            return lineItems;
        }

        public static IList<VendorInvoiceTaxToWrite> CreateTaxesToWrite(SalesTaxToRead salesTax, int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTaxToWrite>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(new VendorInvoiceTaxToWrite()
                {
                    Amount = taxAmount,
                    SalesTax = salesTax
                });
            }

            return taxes;
        }

        public static IList<VendorInvoiceTax> CreateTaxes(int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTax>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(VendorInvoiceTax.Create(
                    CreateSalesTax(),
                    taxAmount)
                    .Value);
            }

            return taxes;
        }

        public static IList<VendorInvoicePaymentToWrite> CreatePaymentsToWrite(int paymentCount, double paymentAmount, VendorInvoicePaymentMethodToRead paymentMethod)
        {
            var payments = new List<VendorInvoicePaymentToWrite>();

            for (int i = 0; i < paymentCount; i++)
            {
                payments.Add(new VendorInvoicePaymentToWrite()
                {
                    Amount = paymentAmount,
                    PaymentMethod = paymentMethod
                });
            }

            return payments;
        }

        public static IList<VendorInvoicePayment> CreatePayments(int paymentCount, double paymentAmount)
        {
            var payments = new List<VendorInvoicePayment>();

            for (int i = 0; i < paymentCount; i++)
            {
                payments.Add(VendorInvoicePayment.Create(
                    VendorInvoicePaymentMethod.Create(
                        CreatePaymentMethodNames(5),
                        RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + i),
                        isActive: true,
                        paymentType: VendorInvoicePaymentMethodType.Normal,
                        reconcilingVendor: null).Value,
                    paymentAmount + i).Value);
            }

            return payments;
        }

    }
}
