using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
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
        #region <---- ConvertEntityToReadDto ---->
        public static VendorInvoiceToRead ConvertEntityToReadDto(VendorInvoice invoice)
        {
            if (invoice == null)
                return null;

            return new()
            {
                Id = invoice.Id,
                Date = invoice.Date,
                DatePosted = invoice.DatePosted,
                Status = EnumExtensions.GetDisplayName(invoice.Status),//   (VendorInvoiceStatus)Enum.Parse(typeof(VendorInvoiceStatus), invoice.Status),
                InvoiceNumber = invoice.InvoiceNumber,
                Total = invoice.Total,
                Vendor = VendorHelper.ConvertEntityToReadDto(invoice.Vendor),
                LineItems = ProjectItems(invoice.LineItems),
                Payments = ProjectPayments(invoice.Payments),
                Taxes = ProjectTaxes(invoice.Taxes)
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
                                VendorInvoiceId = item.VendorInvoiceId,
                                Type = item.Type,
                                PartNumber = item.PartNumber,
                                Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(item.Manufacturer),
                                //MfrId = item.MfrId,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                Core = item.Core,
                                PONumber = item.PONumber,
                                InvoiceNumber = item.InvoiceNumber,
                                TransactionDate = item.TransactionDate
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
                                VendorInvoiceId = payment.VendorInvoiceId,
                                PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(payment.PaymentMethod),
                                Amount = payment.Amount
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
                                VendorInvoiceId = tax.VendorInvoiceId,
                                Order = tax.Order,
                                TaxId = tax.TaxId,
                                Amount = tax.Amount
                            };
        }

        #endregion

        #region <---- ConvertWriteDtoToEntity ---->
        public static VendorInvoice ConvertWriteDtoToEntity(VendorInvoiceToWrite invoiceToWrite)
        {
            if (invoiceToWrite is null)
                return null;

            return new()
            {
                Date = invoiceToWrite.Date,
                DatePosted = invoiceToWrite.DatePosted,
                Status = invoiceToWrite.Status,
                InvoiceNumber = invoiceToWrite.InvoiceNumber,
                Total = invoiceToWrite.Total,
                //VendorId = invoiceToRead.Vendor.Id,
                Vendor = VendorHelper.ConvertWriteDtoToEntity(invoiceToWrite.Vendor),
                LineItems = ProjectItemsToWrite(invoiceToWrite.LineItems),
                Payments = ProjectPaymentsToWrite(invoiceToWrite.Payments),
                Taxes = ProjectTaxesToWrite(invoiceToWrite.Taxes)
            };
        }

        private static List<VendorInvoiceItem> ProjectItemsToWrite(IList<VendorInvoiceItemToWrite> items)
        {
            return items?.Select(TransformItem()).ToList()
                ?? new List<VendorInvoiceItem>();
        }

        private static Func<VendorInvoiceItemToWrite, VendorInvoiceItem> TransformItem()
        {
            return item =>
                            new VendorInvoiceItem()
                            {
                                //Id = item.Id,
                                VendorInvoiceId = item.VendorInvoiceId,
                                Type = item.Type,
                                PartNumber = item.PartNumber,
                                Manufacturer = ManufacturerHelper.ConvertWriteDtoToEntity(item.Manufacturer),
                                //MfrId = item.MfrId,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                Core = item.Core,
                                PONumber = item.PONumber,
                                InvoiceNumber = item.InvoiceNumber,
                                TransactionDate = item.TransactionDate
                            };
        }

        private static List<VendorInvoicePayment> ProjectPaymentsToWrite(IList<VendorInvoicePaymentToWrite> payments)
        {
            return payments?.Select(TransformPayment()).ToList()
                ?? new List<VendorInvoicePayment>();
        }

        private static Func<VendorInvoicePaymentToWrite, VendorInvoicePayment> TransformPayment()
        {
            return payment =>
                            new VendorInvoicePayment()
                            {
                                VendorInvoiceId = payment.VendorInvoiceId,
                                PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertWriteDtoToEntity(payment.PaymentMethod),
                                Amount = payment.Amount
                            };
        }

        private static List<VendorInvoiceTax> ProjectTaxesToWrite(IList<VendorInvoiceTaxToWrite> taxes)
        {
            return taxes?.Select(TransformTax()).ToList()
                ?? new List<VendorInvoiceTax>();
        }

        private static Func<VendorInvoiceTaxToWrite, VendorInvoiceTax> TransformTax()
        {
            return tax =>
                            new VendorInvoiceTax()
                            {
                                VendorInvoiceId = tax.VendorInvoiceId,
                                Order = tax.Order,
                                TaxId = tax.TaxId,
                                Amount = tax.Amount
                            };
        }

        #endregion

        #region <---- ConvertReadToWriteDto ---->
        public static VendorInvoiceToWrite ConvertReadToWriteDto(VendorInvoiceToRead invoiceToRead)
        {
            if (invoiceToRead == null)
                return null;

            return new()
            {
                Id = invoiceToRead.Id,
                Date = invoiceToRead.Date,
                DatePosted = invoiceToRead.DatePosted,
                Status = (VendorInvoiceStatus)Enum.Parse(typeof(VendorInvoiceStatus), invoiceToRead.Status),
                InvoiceNumber = invoiceToRead.InvoiceNumber,
                Total = invoiceToRead.Total,
                //VendorId = invoiceToRead.Vendor.Id,
                Vendor = VendorHelper.ConvertReadToWriteDto(invoiceToRead.Vendor),
                LineItems = ProjectItemsToRead(invoiceToRead.LineItems),
                Payments = ProjectPaymentsToRead(invoiceToRead.Payments),
                Taxes = ProjectTaxesToRead(invoiceToRead.Taxes)
            };
        }

        private static List<VendorInvoiceItemToWrite> ProjectItemsToRead(IReadOnlyList<VendorInvoiceItemToRead> items)
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
                                VendorInvoiceId = item.VendorInvoiceId,
                                Type = item.Type,
                                PartNumber = item.PartNumber,
                                Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(item.Manufacturer),
                                //MfrId = item.MfrId,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                Core = item.Core,
                                PONumber = item.PONumber,
                                InvoiceNumber = item.InvoiceNumber,
                                TransactionDate = item.TransactionDate
                            };
        }

        private static List<VendorInvoicePaymentToWrite> ProjectPaymentsToRead(IReadOnlyList<VendorInvoicePaymentToRead> payments)
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
                                VendorInvoiceId = payment.VendorInvoiceId,
                                PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadToWriteDto(payment.PaymentMethod),
                                Amount = payment.Amount
                            };
        }

        private static List<VendorInvoiceTaxToWrite> ProjectTaxesToRead(IReadOnlyList<VendorInvoiceTaxToRead> taxes)
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
                                VendorInvoiceId = tax.VendorInvoiceId,
                                Order = tax.Order,
                                TaxId = tax.TaxId,
                                Amount = tax.Amount
                            };
        }

        #endregion

        public static VendorInvoiceToReadInList ConvertEntityToReadInListDto(VendorInvoice invoice)
        {
            if (invoice is null)
                return null;

            return new()
            {
                Id = invoice.Id,
                VendorId = invoice.Vendor?.Id ?? 0,
                VendorCode = invoice.Vendor?.VendorCode,
                Name = invoice.Vendor?.Name,
                DateCreated = invoice.Date?.ToShortDateString(),
                DatePosted = invoice.DatePosted?.ToShortDateString(),
                Status = invoice.Status.ToString(),
                InvoiceNumber = invoice.InvoiceNumber,
                Total = invoice.Total
            };
        }

        public static void CopyWriteDtoToEntity(VendorInvoiceToWrite invoiceToWrite, VendorInvoice invoice)
        {
            invoice.Date = invoiceToWrite.Date;
            invoice.DatePosted = invoiceToWrite.DatePosted;
            invoice.Status = invoiceToWrite.Status;
            invoice.InvoiceNumber = invoiceToWrite.InvoiceNumber;
            invoice.Total = invoiceToWrite.Total;
            invoice.Vendor = VendorHelper.ConvertWriteDtoToEntity(invoiceToWrite.Vendor);
            invoice.LineItems = ProjectItemsToWrite(invoiceToWrite.LineItems);
            invoice.Payments = ProjectPaymentsToWrite(invoiceToWrite.Payments);
            invoice.Taxes = ProjectTaxesToWrite(invoiceToWrite.Taxes);
        }

        //private static List<VendorInvoiceItem> ProjectItems(List<VendorInvoiceItemToWrite> items)
        //{
        //    return items?.Select(TransformItem()).ToList()
        //        ?? new List<VendorInvoiceItem>();
        //}

        //private static Func<VendorInvoiceItemToWrite, VendorInvoiceItem> TransformItem()
        //{
        //    return item =>
        //                    new VendorInvoiceItem()
        //                    {
        //                        VendorInvoiceId = item.VendorInvoiceId,
        //                        Type = item.Type,
        //                        PartNumber = item.PartNumber,
        //                        MfrId = item.MfrId,
        //                        Description = item.Description,
        //                        Quantity = item.Quantity,
        //                        Cost = item.Cost,
        //                        Core = item.Core,
        //                        PONumber = item.PONumber,
        //                        InvoiceNumber = item.InvoiceNumber,
        //                        TransactionDate = item.TransactionDate
        //                    };
        //}

        //private static List<VendorInvoicePayment> ProjectPayments(List<VendorInvoicePaymentToWrite> payments)
        //{
        //    return payments?.Select(TransformPayment()).ToList()
        //        ?? new List<VendorInvoicePayment>();
        //}

        //private static Func<VendorInvoicePaymentToWrite, VendorInvoicePayment> TransformPayment()
        //{
        //    return payment =>
        //                    new VendorInvoicePayment()
        //                    {
        //                        //Id = payment.Id,
        //                        VendorInvoiceId = payment.VendorInvoiceId,
        //                        PaymentMethod = payment.PaymentMethod,
        //                        Amount = payment.Amount
        //                    };
        //}

        //private static List<VendorInvoiceTax> ProjectTaxes(List<VendorInvoiceTaxToWrite> taxes)
        //{
        //    return taxes?.Select(TransformTax()).ToList()
        //        ?? new List<VendorInvoiceTax>();
        //}

        //private static Func<VendorInvoiceTaxToWrite, VendorInvoiceTax> TransformTax()
        //{
        //    return tax =>
        //                    new VendorInvoiceTax()
        //                    {
        //                        VendorInvoiceId = tax.VendorInvoiceId,
        //                        Order = tax.Order,
        //                        TaxId = tax.TaxId,
        //                        Amount = tax.Amount
        //                    };
        //}

        //private static List<VendorInvoiceTaxToWrite> ProjectTaxes(IReadOnlyList<VendorInvoiceTaxToRead> taxes)
        //{
        //    return taxes?.Select(TransformTaxToWrite()).ToList()
        //        ?? new List<VendorInvoiceTaxToWrite>();
        //}

        //private static Func<VendorInvoiceTaxToRead, VendorInvoiceTaxToWrite> TransformTaxToWrite()
        //{
        //    return tax =>
        //                    new VendorInvoiceTaxToWrite()
        //                    {
        //                        Id = tax.Id,
        //                        VendorInvoiceId = tax.VendorInvoiceId,
        //                        Order = tax.Order,
        //                        TaxId = tax.TaxId,
        //                        Amount = tax.Amount
        //                    };
        //}
    }
}
