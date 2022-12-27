using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.Helpers.Payables
{
    public static class VendorInvoiceTestHelper
    {
        public static VendorInvoiceToWrite CreateVendorInvoiceToWrite() => new()
        {
            Date = DateTime.Now,
            InvoiceNumber = "123",
            Vendor = CreateVendorToRead(),
            Status = VendorInvoiceStatus.Open,
            Total = 0
        };

        public static VendorInvoiceToWrite CreateVendorInvoiceToWrite(Vendor vendor)
        {
            return new()
            {
                Date = DateTime.Today,
                DocumentType = VendorInvoiceDocumentType.Invoice,
                Status = VendorInvoiceStatus.Open,
                Total = 10.0,
                Vendor = new VendorToRead()
                {
                    Id = vendor.Id,
                    IsActive = vendor.IsActive,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode
                }
            };
        }

        public static VendorInvoiceToWrite CreateVendorInvoiceToWrite(VendorToRead vendor)
        {
            return new()
            {
                Date = DateTime.Today,
                DocumentType = VendorInvoiceDocumentType.Invoice,
                Status = VendorInvoiceStatus.Open,
                Total = 10.0,
                Vendor = vendor
            };
        }

        private static VendorToRead CreateVendorToRead()
        {
            return new VendorToRead()
            {
                Id = 1,
                IsActive = true,
                Name = RandomCharacters(Vendor.MinimumLength) + 1,
                VendorCode = RandomCharacters(Vendor.MinimumLength + 1)
            };
        }

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

        public static IList<VendorInvoiceLineItemToWrite> CreateLineItemsToWrite(LineItemTestOptions options)
        {
            var result = new List<VendorInvoiceLineItemToWrite>();

            for (int i = 0; i < options.RowCount; i++)
            {
                result.Add(
                    new VendorInvoiceLineItemToWrite()
                    {
                        Core = options.Core,
                        Cost = options.Cost,
                        Quantity = options.Quantity,
                        PONumber = options.PONumber,
                        Type = options.Type,
                        Item = options.Item,
                        TransactionDate = options.TransactionDate
                    });
            }

            return result;
        }

        public static IList<VendorInvoiceLineItemToWrite> CreateLineItemsToWrite(VendorInvoiceLineItemType lineItemType, int lineItemCount, double core, double cost, double lineItemQuantity)
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
                        Type = lineItemType,
                        Item = new VendorInvoiceItemToWrite()
                        {
                            Description = $"a desription for {i}",
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

        public static IList<VendorInvoiceTaxToWrite> CreateTaxesToWrite(SalesTax salesTax, int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTaxToWrite>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(new VendorInvoiceTaxToWrite()
                {
                    Amount = taxAmount,
                    SalesTax = new()
                    {
                        Id = salesTax.Id,
                        Description = salesTax.Description,
                        IsAppliedByDefault = salesTax.IsAppliedByDefault,
                        IsTaxable = salesTax.IsTaxable,
                        LaborTaxRate = salesTax.LaborTaxRate,
                        Order = salesTax.Order,
                        PartTaxRate = salesTax.PartTaxRate,
                        TaxIdNumber = salesTax.TaxIdNumber,
                        TaxType = salesTax.TaxType
                        //ExciseFees = salesTax.ExciseFees
                    }
                });
            }

            return taxes;
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

        public static VendorInvoicePaymentMethodToRead CreateVendorInvoicePaymentMethodToRead()
        {
            return new VendorInvoicePaymentMethodToRead()
            {
                Id = 1,
                IsActive = true,
                PaymentType = VendorInvoicePaymentMethodType.Normal,
                Name = nameof(VendorInvoicePaymentMethodToRead),
                ReconcilingVendor = CreateVendorToRead()
            };
        }

        public static IList<VendorInvoicePaymentToWrite> CreatePaymentsToWrite(int paymentCount, double paymentAmount, VendorInvoicePaymentMethod paymentMethod)
        {
            var payments = new List<VendorInvoicePaymentToWrite>();

            for (int i = 0; i < paymentCount; i++)
            {
                payments.Add(new VendorInvoicePaymentToWrite()
                {
                    Amount = paymentAmount,
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(paymentMethod)
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
                        VendorInvoicePaymentMethodType.Normal,
                        reconcilingVendor: null).Value,
                    paymentAmount + i).Value);
            }

            return payments;
        }
    }
}
