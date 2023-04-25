using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public static class VendorInvoiceHelper
    {
        public static VendorInvoiceToRead ConvertToReadDto(VendorInvoice invoice)
        {
            return
                invoice is null
                ? null
                : new()
                {
                    Id = invoice.Id,
                    Date = invoice.Date,
                    DatePosted = invoice.DatePosted,
                    Status = invoice.Status,
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Total,
                    Vendor = VendorHelper.ConvertToReadDto(invoice.Vendor),
                    LineItems = ConvertLineItemsToReadDtos(invoice.LineItems),
                    Payments = ConvertPaymentsToReadDtos(invoice.Payments),
                    Taxes = ConvertTaxsToReadDtos(invoice.Taxes)
                };
        }

        private static IReadOnlyList<VendorInvoiceLineItemToRead> ConvertLineItemsToReadDtos(IReadOnlyList<VendorInvoiceLineItem> items)
        {
            return items?.Select(ConvertLineItemToReadDto()).ToList()
                ?? new List<VendorInvoiceLineItemToRead>();
        }

        public static IReadOnlyList<VendorInvoiceLineItemToWrite> ConvertLineItemsToWriteDtos(IReadOnlyList<VendorInvoiceLineItem> items)
        {
            return items?.Select(ConvertLineItemToWriteDto()).ToList()
                ?? new List<VendorInvoiceLineItemToWrite>();
        }

        private static Func<VendorInvoiceLineItem, VendorInvoiceLineItemToWrite> ConvertLineItemToWriteDto()
        {
            return lineItem =>
                            new VendorInvoiceLineItemToWrite()
                            {
                                Id = lineItem.Id,
                                Type = lineItem.Type,
                                Item =
                                lineItem.Item is null
                                    ? null
                                    : ConvertItemToWriteDto(lineItem.Item),
                                Quantity = lineItem.Quantity,
                                Cost = lineItem.Cost,
                                Core = lineItem.Core,
                                PONumber = lineItem.PONumber,
                                TransactionDate = lineItem.TransactionDate
                            };
        }

        private static VendorInvoiceItemToWrite ConvertItemToWriteDto(VendorInvoiceItem item)
        {
            return VendorInvoiceItemHelper.ConvertEntityToWriteDto(item);
        }

        private static Func<VendorInvoiceLineItem, VendorInvoiceLineItemToRead> ConvertLineItemToReadDto()
        {
            return lineItem =>
                            new VendorInvoiceLineItemToRead()
                            {
                                Id = lineItem.Id,
                                Type = lineItem.Type,
                                Item = ConvertItemToReadDto(lineItem.Item),
                                Quantity = lineItem.Quantity,
                                Cost = lineItem.Cost,
                                Core = lineItem.Core,
                                PONumber = lineItem.PONumber,
                                TransactionDate = lineItem.TransactionDate
                            };
        }

        private static VendorInvoiceItemToRead ConvertItemToReadDto(VendorInvoiceItem item)
        {
            return item is null
                ? null
                : new()
                {
                    Description = item.Description,
                    Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(item.Manufacturer),
                    PartNumber = item.PartNumber,
                    SaleCode = SaleCodeHelper.ConvertEntityToReadDto(item.SaleCode)
                };
        }

        private static IReadOnlyList<VendorInvoicePaymentToRead> ConvertPaymentsToReadDtos(IReadOnlyList<VendorInvoicePayment> payments)
        {
            return payments?.Select(ConvertPaymentToReadDto()).ToList()
                ?? new List<VendorInvoicePaymentToRead>();
        }

        public static IReadOnlyList<VendorInvoicePaymentToWrite> ConvertPaymentsToWriteDtos(IReadOnlyList<VendorInvoicePayment> payments)
        {
            return payments?.Select(ConvertPaymentToWriteDto()).ToList()
                ?? new List<VendorInvoicePaymentToWrite>();
        }

        private static Func<VendorInvoicePayment, VendorInvoicePaymentToRead> ConvertPaymentToReadDto()
        {
            return payment =>
                new VendorInvoicePaymentToRead()
                {
                    Id = payment.Id,
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(payment.PaymentMethod),
                    Amount = payment.Amount
                };
        }

        private static Func<VendorInvoicePayment, VendorInvoicePaymentToWrite> ConvertPaymentToWriteDto()
        {
            return payment =>
                new VendorInvoicePaymentToWrite()
                {
                    Id = payment.Id,
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertEntityToWriteDto(payment.PaymentMethod),
                    Amount = payment.Amount
                };
        }

        private static IReadOnlyList<VendorInvoiceTaxToRead> ConvertTaxsToReadDtos(IReadOnlyList<VendorInvoiceTax> taxes)
        {
            return taxes?.Select(ConvertTaxToReadDto()).ToList()
                ?? new List<VendorInvoiceTaxToRead>();
        }

        private static Func<VendorInvoiceTax, VendorInvoiceTaxToRead> ConvertTaxToReadDto()
        {
            return tax =>
                new VendorInvoiceTaxToRead()
                {
                    Id = tax.Id,
                    Amount = tax.Amount,
                    SalesTax = ConvertSalesTaxToReadDto(tax.SalesTax)
                };
        }

        private static SalesTaxToRead ConvertSalesTaxToReadDto(SalesTax salesTax)
        {
            return salesTax is null
                ? null
                : new()
                {
                    Description = salesTax.Description,
                    Id = salesTax.Id,
                    IsAppliedByDefault = salesTax.IsAppliedByDefault,
                    IsTaxable = salesTax.IsTaxable,
                    LaborTaxRate = salesTax.LaborTaxRate,
                    Order = salesTax.Order,
                    PartTaxRate = salesTax.PartTaxRate,
                    ExciseFees = ExciseFeeHelper.ConvertEntitiesToReadDtos(salesTax.ExciseFees),
                    TaxIdNumber = salesTax.TaxIdNumber,
                    TaxType = salesTax.TaxType
                };
        }

        public static VendorInvoiceToReadInList ConvertToReadInListDto(VendorInvoice invoice)
        {
            return invoice is null
                ? null
                : new()
                {
                    Id = invoice.Id,
                    Vendor = VendorHelper.ConvertToReadDto(invoice.Vendor),
                    Date = invoice.Date?.ToShortDateString(),
                    DatePosted = invoice.DatePosted?.ToShortDateString(),
                    Status = invoice.Status.ToString(),
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Total
                };
        }

        public static VendorInvoiceToWrite ConvertReadToWriteDto(VendorInvoiceToRead invoice)
        {
            return invoice is null
                ? null
                : new()
                {
                    Date = invoice.Date,
                    DatePosted = invoice.DatePosted,
                    Status = invoice.Status,
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Total,
                    Vendor = invoice.Vendor,
                    LineItems = VendorInvoiceLineItemHelper.ConvertReadDtosToWriteDtos(invoice.LineItems),
                    Payments = VendorInvoicePaymentHelper.ConvertReadDtosToWriteDtos(invoice.Payments),
                    Taxes = VendorInvoiceTaxHelper.ConvertReadDtosToWriteDtos(invoice.Taxes)
                };
        }

        public static IReadOnlyList<VendorInvoiceToRead> ConvertToReadDtos(IReadOnlyList<VendorInvoice> invoices)
        {
            return invoices.Select(invoice => ConvertToReadDto(invoice))
                .ToList();
        }

        public static VendorInvoice ConvertWriteToEntity(
            VendorInvoiceToWrite invoice,
            Vendor vendor,
            IReadOnlyList<VendorInvoiceLineItem> lineItems,
            IReadOnlyList<VendorInvoicePayment> payments,
            IReadOnlyList<VendorInvoiceTax> taxes,
            IReadOnlyList<string> vendorInvoiceNumbers)
        {
            if (vendorInvoiceNumbers is null || invoice is null || vendor is null)
                throw new ArgumentNullException(nameof(vendorInvoiceNumbers));

            var invoiceEntity = VendorInvoice.Create(vendor, invoice.Status, invoice.DocumentType, invoice.Total, vendorInvoiceNumbers: null).Value;

            foreach (var lineItem in lineItems)
                invoiceEntity.AddLineItem(lineItem);

            foreach (var payment in payments)
                invoiceEntity.AddPayment(payment);

            foreach (var tax in taxes)
                invoiceEntity.AddTax(tax);

            return invoiceEntity;
        }

        public static VendorInvoiceToWrite ConvertToWriteDto(VendorInvoice invoice)
        {
            return invoice is null
                ? null
                : new()
                {
                    Id = invoice.Id,
                    Date = invoice.Date,
                    DatePosted = invoice.DatePosted,
                    Status = invoice.Status,
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Total,
                    Vendor = VendorHelper.ConvertToReadDto(invoice.Vendor),
                    LineItems = (IList<VendorInvoiceLineItemToWrite>)ConvertLineItemsToWriteDtos(invoice.LineItems),
                    Payments = (IList<VendorInvoicePaymentToWrite>)ConvertPaymentsToWriteDtos(invoice.Payments),
                    Taxes = (IList<VendorInvoiceTaxToWrite>)ConvertTaxesToWriteDtos(invoice.Taxes)
                };
        }

        public static IReadOnlyList<VendorInvoiceTaxToWrite> ConvertTaxesToWriteDtos(IReadOnlyList<VendorInvoiceTax> taxes)
        {
            return taxes?.Select(ConvertTaxToWriteDto()).ToList()
                ?? new List<VendorInvoiceTaxToWrite>();
        }

        private static Func<VendorInvoiceTax, VendorInvoiceTaxToWrite> ConvertTaxToWriteDto()
        {
            return tax =>
                new VendorInvoiceTaxToWrite()
                {
                    Id = tax.Id,
                    Amount = tax.Amount,
                    SalesTax = ConvertSalesTaxToReadDto(tax.SalesTax)
                };
        }
    }
}
