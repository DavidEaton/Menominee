using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public static class VendorInvoiceHelper
    {
        public static VendorInvoiceToWrite Transform(VendorInvoiceToRead invoiceToRead)
        {
            return new VendorInvoiceToWrite()
            {
                Id = invoiceToRead.Id,
                Date = invoiceToRead.Date,
                DatePosted = invoiceToRead.DatePosted,
                Status = (VendorInvoiceStatus)Enum.Parse(typeof(VendorInvoiceStatus), invoiceToRead.Status),
                InvoiceNumber = invoiceToRead.InvoiceNumber,
                Total = invoiceToRead.Total,
                VendorId = invoiceToRead.Vendor.Id,
                LineItems = ProjectItems(invoiceToRead.LineItems),
                Payments = ProjectPayments(invoiceToRead.Payments),
                Taxes = ProjectTaxes(invoiceToRead.Taxes)
            };
        }

        public static VendorInvoiceToRead Transform(VendorInvoice invoice)
        {
            return new VendorInvoiceToRead()
            {
                Id = invoice.Id,
                Date = invoice.Date,
                DatePosted = invoice.DatePosted,
                Status = EnumExtensions.GetDisplayName(invoice.Status),//   (VendorInvoiceStatus)Enum.Parse(typeof(VendorInvoiceStatus), invoice.Status),
                InvoiceNumber = invoice.InvoiceNumber,
                Total = invoice.Total,
                Vendor = VendorHelper.Transform(invoice.Vendor),
                LineItems = ProjectItems(invoice.LineItems),
                Payments = ProjectPayments(invoice.Payments),
                Taxes = ProjectTaxes(invoice.Taxes)
            };
        }

        private static List<VendorInvoiceItem> ProjectItems(List<VendorInvoiceItemToWrite> items)
        {
            return items?.Select(TransformItem()).ToList()
                ?? new List<VendorInvoiceItem>();
        }

        private static Func<VendorInvoiceItemToWrite, VendorInvoiceItem> TransformItem()
        {
            return item =>
                            new VendorInvoiceItem()
                            {
                                InvoiceId = item.InvoiceId,
                                Type = item.Type,
                                PartNumber = item.PartNumber,
                                MfrId = item.MfrId,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                Core = item.Core,
                                PONumber = item.PONumber,
                                InvoiceNumber = item.InvoiceNumber,
                                TransactionDate = item.TransactionDate
                            };
        }

        private static List<VendorInvoiceItemToWrite> ProjectItems(IReadOnlyList<VendorInvoiceItemToRead> items)
        {
            return items?.Select(TransformItemToWrite()).ToList()
                ?? new List<VendorInvoiceItemToWrite>();
        }

        private static Func<VendorInvoiceItemToRead, VendorInvoiceItemToWrite> TransformItemToWrite()
        {
            return item =>
                            new VendorInvoiceItemToWrite()
                            {
                                Id = item.Id,
                                InvoiceId = item.InvoiceId,
                                Type = item.Type,
                                PartNumber = item.PartNumber,
                                MfrId = item.MfrId,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                Core = item.Core,
                                PONumber = item.PONumber,
                                InvoiceNumber = item.InvoiceNumber,
                                TransactionDate = item.TransactionDate
                            };
        }

        private static List<VendorInvoiceItemToRead> ProjectItems(IList<VendorInvoiceItem> items)
        {
            return items?.Select(TransformItemToRead()).ToList()
                ?? new List<VendorInvoiceItemToRead>();
        }

        private static Func<VendorInvoiceItem, VendorInvoiceItemToRead> TransformItemToRead()
        {
            return item =>
                            new VendorInvoiceItemToRead()
                            {
                                Id = item.Id,
                                InvoiceId = item.InvoiceId,
                                Type = item.Type,
                                PartNumber = item.PartNumber,
                                MfrId = item.MfrId,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                Core = item.Core,
                                PONumber = item.PONumber,
                                InvoiceNumber = item.InvoiceNumber,
                                TransactionDate = item.TransactionDate
                            };
        }

        private static List<VendorInvoicePayment> ProjectPayments(List<VendorInvoicePaymentToWrite> payments)
        {
            return payments?.Select(TransformPayment()).ToList()
                ?? new List<VendorInvoicePayment>();
        }

        private static Func<VendorInvoicePaymentToWrite, VendorInvoicePayment> TransformPayment()
        {
            return payment =>
                            new VendorInvoicePayment()
                            {
                                //Id = payment.Id,
                                InvoiceId = payment.InvoiceId,
                                PaymentMethod = payment.PaymentMethod,
                                Amount = payment.Amount
                            };
        }

        private static List<VendorInvoicePaymentToWrite> ProjectPayments(IReadOnlyList<VendorInvoicePaymentToRead> payments)
        {
            return payments?.Select(TransformPaymentToWrite()).ToList()
                ?? new List<VendorInvoicePaymentToWrite>();
        }

        private static Func<VendorInvoicePaymentToRead, VendorInvoicePaymentToWrite> TransformPaymentToWrite()
        {
            return payment =>
                            new VendorInvoicePaymentToWrite()
                            {
                                Id = payment.Id,
                                InvoiceId = payment.InvoiceId,
                                PaymentMethod = payment.PaymentMethod,
                                Amount = payment.Amount
                            };
        }

        private static List<VendorInvoicePaymentToRead> ProjectPayments(IList<VendorInvoicePayment> payments)
        {
            return payments?.Select(TransformPaymentToRead()).ToList()
                ?? new List<VendorInvoicePaymentToRead>();
        }

        private static Func<VendorInvoicePayment, VendorInvoicePaymentToRead> TransformPaymentToRead()
        {
            return payment =>
                            new VendorInvoicePaymentToRead()
                            {
                                Id = payment.Id,
                                InvoiceId = payment.InvoiceId,
                                PaymentMethod = payment.PaymentMethod,
                                Amount = payment.Amount
                            };
        }

        private static List<VendorInvoiceTax> ProjectTaxes(List<VendorInvoiceTaxToWrite> taxes)
        {
            return taxes?.Select(TransformTax()).ToList()
                ?? new List<VendorInvoiceTax>();
        }

        private static Func<VendorInvoiceTaxToWrite, VendorInvoiceTax> TransformTax()
        {
            return tax =>
                            new VendorInvoiceTax()
                            {
                                InvoiceId = tax.InvoiceId,
                                Order = tax.Order,
                                TaxId = tax.TaxId,
                                Amount = tax.Amount
                            };
        }

        private static List<VendorInvoiceTaxToWrite> ProjectTaxes(IReadOnlyList<VendorInvoiceTaxToRead> taxes)
        {
            return taxes?.Select(TransformTaxToWrite()).ToList()
                ?? new List<VendorInvoiceTaxToWrite>();
        }

        private static Func<VendorInvoiceTaxToRead, VendorInvoiceTaxToWrite> TransformTaxToWrite()
        {
            return tax =>
                            new VendorInvoiceTaxToWrite()
                            {
                                Id = tax.Id,
                                InvoiceId = tax.InvoiceId,
                                Order = tax.Order,
                                TaxId = tax.TaxId,
                                Amount = tax.Amount
                            };
        }

        private static List<VendorInvoiceTaxToRead> ProjectTaxes(IList<VendorInvoiceTax> taxes)
        {
            return taxes?.Select(TransformTaxToRead()).ToList()
                ?? new List<VendorInvoiceTaxToRead>();
        }

        private static Func<VendorInvoiceTax, VendorInvoiceTaxToRead> TransformTaxToRead()
        {
            return tax =>
                            new VendorInvoiceTaxToRead()
                            {
                                Id = tax.Id,
                                InvoiceId = tax.InvoiceId,
                                Order = tax.Order,
                                TaxId = tax.TaxId,
                                Amount = tax.Amount
                            };
        }
    }
}
