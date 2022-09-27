using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities;
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
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public static class VendorInvoiceHelper
    {
        #region <---- ConvertEntityToReadDto ---->
        public static VendorInvoiceToRead ConvertEntityToReadDto(VendorInvoice invoice)
        {
            return
                invoice == null
                ? null
                : new()
                {
                    Id = invoice.Id,
                    Date = invoice.Date,
                    DatePosted = invoice.DatePosted,
                    Status = EnumExtensions.GetDisplayName(invoice.Status),
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Total,
                    Vendor = VendorHelper.ConvertEntityToReadDto(invoice.Vendor),
                    LineItems = ConvertLineItemEntitiesToReadDtos(invoice.LineItems),
                    Payments = ConvertPaymentEntitiesToReadDtos(invoice.Payments),
                    Taxes = ConvertTaxEntitiesToReadDtos(invoice.Taxes)
                };
        }

        private static List<VendorInvoiceLineItemToRead> ConvertLineItemEntitiesToReadDtos(IList<VendorInvoiceLineItem> items)
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

        private static List<VendorInvoicePaymentToRead> ConvertPaymentEntitiesToReadDtos(IList<VendorInvoicePayment> payments)
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

        private static List<VendorInvoiceTaxToRead> ConvertTaxEntitiesToReadDtos(IList<VendorInvoiceTax> taxes)
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
                    TaxId = tax.TaxId,
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
                    VendorId = invoice.Vendor?.Id ?? 0,
                    VendorCode = invoice.Vendor?.VendorCode,
                    VendorName = invoice.Vendor?.Name,
                    Date = invoice.Date?.ToShortDateString(),
                    DatePosted = invoice.DatePosted?.ToShortDateString(),
                    Status = invoice.Status.ToString(),
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Total
                };
        }
        
        #endregion

        #region <---- ConvertWriteDtoToEntity ---->
        public static VendorInvoice ConvertWriteDtoToEntity(
            VendorInvoiceToWrite invoice,
            Vendor vendor,
            IReadOnlyList<Manufacturer> manufacturers,
            IReadOnlyList<SaleCode> saleCodes,
            IReadOnlyList<SalesTax> salesTaxes,
            IReadOnlyList<VendorInvoicePaymentMethodToRead> paymentMethods)
        {
            return invoice is null
                ? null
                : VendorInvoice.Create(
                vendor,
                invoice.Status,
                invoice.Total,
            invoice.InvoiceNumber,
            invoice.Date,
            invoice.DatePosted,
            VendorInvoiceLineItemHelper.ConvertWriteDtosToEntities(invoice.LineItems, manufacturers, saleCodes),
                VendorInvoicePaymentHelper.ConvertWriteDtosToEntities(paymentMethods, invoice.Payments),
                VendorInvoiceTaxHelper.ConvertWriteDtosToEntities(invoice.Taxes, salesTaxes))
                .Value;
        }

        #endregion

        #region <---- ConvertReadToWriteDto ---->

        public static VendorInvoiceToWrite ConvertReadToWriteDto(VendorInvoiceToRead invoice)
        {
            return invoice is null
                ? null
                :
                new()
                {
                    Date = invoice.Date,
                    DatePosted = invoice.DatePosted,
                    Status = invoice.Status.GetValueFromName<VendorInvoiceStatus>(),
                    InvoiceNumber = invoice.InvoiceNumber,
                    Total = invoice.Total,
                    Vendor = VendorHelper.ConvertReadToReadInListDto(invoice.Vendor),
                    LineItems = VendorInvoiceLineItemHelper.ConvertReadDtosToWriteDtos(invoice.LineItems),
                    Payments = VendorInvoicePaymentHelper.ConvertReadDtosToWriteDtos(invoice.Payments),
                    Taxes = VendorInvoiceTaxHelper.ConvertReadDtosToWriteDtos(invoice.Taxes)
                };
        }

        #endregion
    }
}
