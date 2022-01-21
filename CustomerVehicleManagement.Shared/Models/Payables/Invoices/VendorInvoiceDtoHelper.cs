using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public static class VendorInvoiceDtoHelper
    {
        //public static VendorInvoiceToReadInList ToInvoiceInListDto(VendorInvoice invoice)
        //{
        //    return new VendorInvoiceToReadInList()
        //    {
        //        Id = invoice.Id,
        //        VendorId = invoice.Vendor.VendorId,
        //        Name = invoice.Vendor.Name,
        //        DateCreated = invoice.Date?.ToShortDateString(),
        //        DatePosted = invoice.DatePosted?.ToShortDateString(),
        //        Status = invoice.Status.ToString(),
        //        InvoiceNumber = invoice.InvoiceNumber,
        //        Total = invoice.Total
        //    };
        //}

        //public static VendorInvoiceToRead ToInvoiceReadDto(VendorInvoice invoice)
        //{
        //    var model = new VendorInvoiceToRead();

        //    model.Id = invoice.Id;
        //    model.VendorId = invoice.Vendor.VendorId;
        //    model.Name = invoice.Vendor.Name;
        //    model.Date = invoice.Date;
        //    model.DatePosted = invoice.DatePosted;
        //    model.Status = invoice.Status.ToString();
        //    model.InvoiceNumber = invoice.InvoiceNumber;
        //    model.Total = invoice.Total;

        //    // FIX ME -- IReadOnlyList or List ???????????
        //    //model.LineItems = (IReadOnlyList<VendorInvoiceItemReadDto>)VendorInvoiceItemDtoHelper.ToItemListReadDto(invoice?.LineItems);
        //    //model.Payments = (IReadOnlyList<VendorInvoicePaymentReadDto>)VendorInvoicePaymentDtoHelper.ToPaymentListReadDto(invoice?.Payments);
        //    //model.Taxes = (IReadOnlyList<VendorInvoiceTaxReadDto>)VendorInvoiceTaxDtoHelper.ToTaxListReadDto(invoice?.Taxes);
        //    model.LineItems = (List<VendorInvoiceItemToRead>)VendorInvoiceItemDtoHelper.ToItemListReadDto(invoice?.LineItems);
        //    model.Payments = (List<VendorInvoicePaymentToRead>)VendorInvoicePaymentDtoHelper.ToPaymentListReadDto(invoice?.Payments);
        //    model.Taxes = (List<VendorInvoiceTaxToRead>)VendorInvoiceTaxDtoHelper.ToTaxListReadDto(invoice?.Taxes);

        //    return model;
        //}

        //public static VendorInvoice InvoiceUpdateDtoToEntity(VendorInvoiceToEdit invoiceUpdateDto)
        //{
        //    var invoice = new VendorInvoice();

        //    invoice.Id = invoiceUpdateDto.Id;
        //    invoice.Vendor.VendorId = invoiceUpdateDto.VendorId;
        //    //invoice.Vendor.Name = invoiceUpdateDto.VendorName;
        //    invoice.Date = invoiceUpdateDto.Date;
        //    invoice.DatePosted = invoiceUpdateDto.DatePosted;
        //    invoice.Status = invoiceUpdateDto.Status;
        //    invoice.InvoiceNumber = invoiceUpdateDto.InvoiceNumber;
        //    invoice.Total = invoiceUpdateDto.Total;

        //    invoice.LineItems = VendorInvoiceItemDtoHelper.ToItemEntityList(invoiceUpdateDto.LineItems);
        //    invoice.Payments = VendorInvoicePaymentDtoHelper.ToPaymentEntityList(invoiceUpdateDto.Payments);
        //    invoice.Taxes = VendorInvoiceTaxDtoHelper.ToTaxEntityList(invoiceUpdateDto.Taxes);

        //    return invoice;
        //}

        public static VendorInvoiceToWrite ReadDtoToWriteDto(VendorInvoiceToRead invoiceToRead)
        {
            var invoiceToWrite = new VendorInvoiceToWrite
            {
                Id = invoiceToRead.Id,
                Date = invoiceToRead.Date,
                DatePosted = invoiceToRead.DatePosted,
                Status = (VendorInvoiceStatus)Enum.Parse(typeof(VendorInvoiceStatus), invoiceToRead.Status),
                InvoiceNumber = invoiceToRead.InvoiceNumber,
                Total = invoiceToRead.Total,
                VendorId = invoiceToRead.Vendor.Id
            };

            if (invoiceToRead?.LineItems.Count > 0)
            {
                foreach (var item in invoiceToRead.LineItems)
                {
                    invoiceToWrite.LineItems.Add(new VendorInvoiceItemToWrite()
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
                    });
                }
            }

            if (invoiceToRead?.Payments.Count > 0)
            {
                foreach (var payment in invoiceToRead.Payments)
                {
                    invoiceToWrite.Payments.Add(new VendorInvoicePaymentToWrite()
                    {
                        Id = payment.Id,
                        InvoiceId = payment.InvoiceId,
                        PaymentMethod = payment.PaymentMethod,
                        Amount = payment.Amount
                    });
                }
            }

            if (invoiceToRead?.Taxes.Count > 0)
            {
                foreach (var tax in invoiceToRead.Taxes)
                {
                    invoiceToWrite.Taxes.Add(new VendorInvoiceTaxToWrite()
                    {
                        Id = tax.Id,
                        InvoiceId = tax.InvoiceId,
                        Order = tax.Order,
                        TaxId = tax.TaxId,
                        Amount = tax.Amount
                    });
                }
            }

            return invoiceToWrite;
        }

        //public static VendorInvoiceToAdd EditDtoToAddDto(VendorInvoiceToEdit editDto)
        //{
        //    var addDto = new VendorInvoiceToAdd();

        //    //addDto.Id = editDto.Id;
        //    addDto.VendorId = editDto.VendorId;
        //    //addDto.Name = editDto.Name;
        //    addDto.Date = editDto.Date;
        //    addDto.DatePosted = editDto.DatePosted;
        //    //addDto.Status = (VendorInvoiceStatus)Enum.Parse(typeof(VendorInvoiceStatus), editDto.Status);
        //    addDto.Status = editDto.Status;
        //    addDto.InvoiceNumber = editDto.InvoiceNumber;
        //    addDto.Total = editDto.Total;

        //    if (editDto?.LineItems.Count > 0)
        //    {
        //        foreach (var item in editDto.LineItems)
        //        {
        //            addDto.LineItems.Add(VendorInvoiceItemDtoHelper.EditDtoToAddDto(item));
        //        }
        //    }
        //    if (editDto?.Payments.Count > 0)
        //    {
        //        foreach (var payment in editDto.Payments)
        //        {
        //            addDto.Payments.Add(VendorInvoicePaymentDtoHelper.EditDtoToAddDto(payment));
        //        }
        //    }
        //    if (editDto?.Taxes.Count > 0)
        //    {
        //        foreach (var tax in editDto.Taxes)
        //        {
        //            addDto.Taxes.Add(VendorInvoiceTaxDtoHelper.EditDtoToAddDto(tax));
        //        }
        //    }

        //    return addDto;
        //}
    }
}
