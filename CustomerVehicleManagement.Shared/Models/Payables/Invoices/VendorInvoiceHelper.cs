﻿using CustomerVehicleManagement.Domain.Entities.Payables;
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
        public static VendorInvoiceToRead ConvertEntityToReadDto(VendorInvoice invoice)
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
                    Vendor = VendorHelper.ConvertEntityToReadDto(invoice.Vendor),
                    LineItems = ConvertLineItemEntitiesToReadDtos(invoice.LineItems),
                    Payments = ConvertPaymentEntitiesToReadDtos(invoice.Payments),
                    Taxes = ConvertTaxEntitiesToReadDtos(invoice.Taxes)
                };
        }

        private static IReadOnlyList<VendorInvoiceLineItemToRead> ConvertLineItemEntitiesToReadDtos(IReadOnlyList<VendorInvoiceLineItem> items)
        {
            return items?.Select(ConvertLineItemToReadDto()).ToList()
                ?? new List<VendorInvoiceLineItemToRead>();
        }

        private static Func<VendorInvoiceLineItem, VendorInvoiceLineItemToRead> ConvertLineItemToReadDto()
        {
            return lineItem =>
                            new VendorInvoiceLineItemToRead()
                            {
                                Id = lineItem.Id,
                                Type = lineItem.Type,
                                Item = ConvertItemEntityToReadDto(lineItem.Item),
                                Quantity = lineItem.Quantity,
                                Cost = lineItem.Cost,
                                Core = lineItem.Core,
                                PONumber = lineItem.PONumber,
                                TransactionDate = lineItem.TransactionDate
                            };
        }

        private static VendorInvoiceItemToRead ConvertItemEntityToReadDto(VendorInvoiceItem item)
        {
            return
                new VendorInvoiceItemToRead()
                {
                    Description = item.Description,
                    Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(item.Manufacturer),
                    PartNumber = item.PartNumber,
                    SaleCode = SaleCodeHelper.ConvertEntityToReadDto(item.SaleCode)
                };
        }

        private static IReadOnlyList<VendorInvoicePaymentToRead> ConvertPaymentEntitiesToReadDtos(IReadOnlyList<VendorInvoicePayment> payments)
        {
            return payments?.Select(ConvertPaymentEntityToReadDto()).ToList()
                ?? new List<VendorInvoicePaymentToRead>();
        }

        private static Func<VendorInvoicePayment, VendorInvoicePaymentToRead> ConvertPaymentEntityToReadDto()
        {
            return payment =>
                new VendorInvoicePaymentToRead()
                {
                    Id = payment.Id,
                    PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(payment.PaymentMethod),
                    Amount = payment.Amount
                };
        }

        private static IReadOnlyList<VendorInvoiceTaxToRead> ConvertTaxEntitiesToReadDtos(IReadOnlyList<VendorInvoiceTax> taxes)
        {
            return taxes?.Select(ConvertTaxEntityToReadDto()).ToList()
                ?? new List<VendorInvoiceTaxToRead>();
        }

        private static Func<VendorInvoiceTax, VendorInvoiceTaxToRead> ConvertTaxEntityToReadDto()
        {
            return tax =>
                new VendorInvoiceTaxToRead()
                {
                    Id = tax.Id,
                    Amount = tax.Amount,
                    SalesTax = ConvertSalesTaxEntityToReadDto(tax.SalesTax)
                };
        }

        private static SalesTaxToRead ConvertSalesTaxEntityToReadDto(SalesTax salesTax)
        {
            return new SalesTaxToRead()
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

        public static VendorInvoiceToReadInList ConvertEntityToReadInListDto(VendorInvoice invoice)
        {
            return invoice is null
                ? null
                : new()
                {
                    Id = invoice.Id,
                    Vendor = VendorHelper.ConvertEntityToReadDto(invoice.Vendor),
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
                :
                new()
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

        public static IReadOnlyList<VendorInvoiceToRead> ConvertEntitiesToReadDto(IReadOnlyList<VendorInvoice> invoices)
        {
            return invoices.Select(invoice => ConvertEntityToReadDto(invoice))
                .ToList();
        }

        public static VendorInvoice ConvertWriteToEntity(VendorInvoiceToWrite invoice, Vendor vendor)
        {
            return invoice is null
                ? null
                : VendorInvoice.Create(vendor, invoice.Status, invoice.DocumentType, invoice.Total, vendorInvoiceNumbers: null).Value;
        }
    }
}
