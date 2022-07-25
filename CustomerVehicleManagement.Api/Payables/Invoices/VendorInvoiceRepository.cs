using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menominee.Common.Utilities;

namespace CustomerVehicleManagement.Api.Payables.Invoices
{
    public class VendorInvoiceRepository : IVendorInvoiceRepository
    {
        private readonly ApplicationDbContext context;

        public VendorInvoiceRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddInvoiceAsync(VendorInvoice invoice)
        {
            if (invoice != null)
                await context.AddAsync(invoice);
        }

        public async Task DeleteInvoiceAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices.FindAsync(id);
            if (invoiceFromContext != null)
                context.Remove(invoiceFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<VendorInvoiceToRead> GetInvoiceAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices
                                                  .Include(invoice => invoice.Vendor)
                                                  .Include(invoice => invoice.LineItems)
                                                      .ThenInclude(item => item.Manufacturer)
                                                  //.Include(invoice => invoice.Payments)
                                                  //    .ThenInclude(payment => payment.PaymentMethod)
                                                  .Include(invoice => invoice.Taxes)
                                                      .ThenInclude(tax => tax.SalesTax)
                                                  .AsSplitQuery()
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(invoice => invoice.Id == id);
            Guard.ForNull(invoiceFromContext, "invoiceFromContext");

            return VendorInvoiceHelper.ConvertEntityToReadDto(invoiceFromContext);

            //return new VendorInvoiceToRead()
            //{
            //    Id = invoiceFromContext.Id,
            //    Vendor = new VendorToRead()
            //    {
            //        Id = invoiceFromContext.Vendor.Id,
            //        Name = invoiceFromContext.Vendor.Name,
            //        VendorCode = invoiceFromContext.Vendor.VendorCode,
            //        IsActive = invoiceFromContext.Vendor.IsActive == null ? true : invoiceFromContext.Vendor.IsActive
            //    },
            //    Name = invoiceFromContext.Vendor?.Name,
            //    Date = invoiceFromContext.Date,
            //    DatePosted = invoiceFromContext.DatePosted,
            //    Status = invoiceFromContext.Status.ToString(),
            //    InvoiceNumber = invoiceFromContext.InvoiceNumber,
            //    Total = invoiceFromContext.Total,
            //    LineItems = invoiceFromContext.LineItems.Select(item => new VendorInvoiceItemToRead()
            //    {
            //        Id = item.Id,
            //        VendorInvoiceId = item.VendorInvoiceId,
            //        Type = item.Type,
            //        PartNumber = item.PartNumber,
            //        MfrId = item.MfrId,
            //        Description = item.Description,
            //        Quantity = item.Quantity,
            //        Cost = item.Cost,
            //        Core = item.Core,
            //        PONumber = item.PONumber,
            //        InvoiceNumber = item.InvoiceNumber,
            //        TransactionDate = item.TransactionDate
            //    }).ToList(),
            //    Payments = invoiceFromContext.Payments.Select(payment => new VendorInvoicePaymentToRead()
            //    {
            //        Id = payment.Id,
            //        VendorInvoiceId = payment.VendorInvoiceId,
            //        PaymentMethod = payment.PaymentMethod,
            //        //PaymentMethodName = payment.PaymentMethodName,  // fix me
            //        Amount = payment.Amount
            //    }).ToList(),
            //    Taxes = invoiceFromContext.Taxes.Select(tax => new VendorInvoiceTaxToRead()
            //    {
            //        Id = tax.Id,
            //        VendorInvoiceId = tax.VendorInvoiceId,
            //        Order = tax.Order,
            //        TaxId = tax.TaxId,
            //        TaxName = tax.TaxName,  // fix me - when/where does this get populated
            //        Amount = tax.Amount
            //    }).ToList()
            //};
        }

        public async Task<Vendor> GetVendorAsync(long id)
        {
            return await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == id);
        }


        public async Task<VendorInvoice> GetInvoiceEntityAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices
                                                  .Include(invoice => invoice.Vendor)
                                                  .Include(invoice => invoice.LineItems)
                                                      .ThenInclude(item => item.Manufacturer)
                                                  .Include(invoice => invoice.Payments)
                                                      .ThenInclude(payment => payment.PaymentMethod)
                                                  .Include(invoice => invoice.Taxes)
                                                      .ThenInclude(tax => tax.SalesTax)
                                                  .AsSplitQuery()
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(invoice => invoice.Id == id);

            return invoiceFromContext;
        }

        public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync()
        {
            IReadOnlyList<VendorInvoice> invoices = await context.VendorInvoices
                                                                 .Include(invoice => invoice.Vendor)
                                                                 .AsSplitQuery()
                                                                 .AsNoTracking()
                                                                 .ToListAsync();

            return invoices.Select(invoice => VendorInvoiceHelper.ConvertEntityToReadInListDto(invoice))
                                   .ToList();
        }

        //public async Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoicesAsync()
        //{
        //    IReadOnlyList<VendorInvoice> invoices = await context.VendorInvoices
        //                                                         .Include(invoice => invoice.Vendor)
        //                                                         .Include(invoice => invoice.LineItems)
        //                                                         .Include(invoice => invoice.Payments)
        //                                                         .Include(invoice => invoice.Taxes)
        //                                                         .ToListAsync();

        //    return invoices
        //        .Select(invoice => VendorInvoiceToRead.ConvertToDto(invoice))
        //        .ToList();
        //}

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateInvoiceAsync(VendorInvoice invoice)
        {
            // No code in this implementation
        }

        public async Task<bool> InvoiceExistsAsync(long id)
        {
            return await context.VendorInvoices.AnyAsync(invoice => invoice.Id == id);
        }

        //private readonly ApplicationDbContext context;
        ////private readonly IHttpService httpService;
        //private string url = "api/vendorinvoices";

        //public VendorInvoiceRepository(ApplicationDbContext context)
        //{
        //    this.context = context ??
        //        throw new ArgumentNullException(nameof(context));
        //}

        ////public VendorInvoiceRepository(HttpService httpService)
        ////{
        ////    this.httpService = httpService;
        ////}

        ////public async Task AddInvoiceAsync(VendorInvoice invoice)
        ////{
        ////    if (invoice != null)
        ////    {
        ////        await context.AddAsync(invoice);
        ////    }
        ////}

        ////public void DeleteInvoice(VendorInvoice invoice)
        ////{
        ////    context.Remove(invoice);
        ////}

        //public void FixTrackingState()
        //{
        //    //context.FixState();
        //    throw new NotImplementedException();
        //}

        //public async Task<VendorInvoiceToRead> GetInvoiceAsync(int id)
        //{
        //    var invoiceFromContext = await context.VendorInvoices
        //        .Include(vendorInvoice => vendorInvoice.LineItems)
        //        .Include(vendorInvoice => vendorInvoice.Taxes)
        //        .Include(vendorInvoice => vendorInvoice.Payments)
        //        .Include(vendorInvoice => vendorInvoice.Vendor)
        //        .FirstOrDefaultAsync(vendorInvoice => vendorInvoice.Id == id);

        //    return VendorInvoiceDtoHelper.ToInvoiceReadDto(invoiceFromContext);
        //}

        //public async Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoicesAsync()
        //{
        //    var invoicesFromContext = await context.VendorInvoices
        //        .Include(vendorInvoice => vendorInvoice.LineItems)
        //        .Include(vendorInvoice => vendorInvoice.Taxes)
        //        .Include(vendorInvoice => vendorInvoice.Payments)
        //        .Include(vendorInvoice => vendorInvoice.Vendor)
        //        .ToArrayAsync();

        //    var list = new List<VendorInvoiceToRead>();

        //    foreach (var invoice in invoicesFromContext)
        //        list.Add(VendorInvoiceDtoHelper.ToInvoiceReadDto(invoice));

        //    return list;
        //}

        //public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync()
        //{
        //    var invoicesFromContext = await context.VendorInvoices
        //        .Include(vendorInvoice => vendorInvoice.LineItems)
        //        .Include(vendorInvoice => vendorInvoice.Taxes)
        //        .Include(vendorInvoice => vendorInvoice.Payments)
        //        .Include(vendorInvoice => vendorInvoice.Vendor)
        //        .ToArrayAsync();

        //    var list = new List<VendorInvoiceToReadInList>();

        //    foreach (var invoice in invoicesFromContext)
        //        list.Add(VendorInvoiceDtoHelper.ToInvoiceInListDto(invoice));

        //    return list;
        //}

        ////public async Task<VendorInvoice> GetVendorInvoiceEntityAsync(int id)
        ////{
        ////    var invoiceFromContext = await context.VendorInvoices
        ////        .Include(vendorInvoice => vendorInvoice.LineItems)
        ////        .Include(vendorInvoice => vendorInvoice.Taxes)
        ////        .Include(vendorInvoice => vendorInvoice.Payments)
        ////        .Include(vendorInvoice => vendorInvoice.Vendor)
        ////        .FirstOrDefaultAsync(vendorInvoice => vendorInvoice.Id == id);

        ////    return invoiceFromContext;
        ////}

        //public async Task<bool> InvoiceExistsAsync(int id)
        //{
        //    return await context.VendorInvoices
        //        .AnyAsync(invoice => invoice.Id == id);
        //}

        //public async Task<bool> SaveChangesAsync()
        //{
        //    return (await context.SaveChangesAsync()) > 0;
        //}

        //public async Task UpdateInvoiceAsync(VendorInvoiceToEdit invoice)
        //{
        ////    // see notes in Menominee.PersonRepository.UpdatePersonAsync

        ////    var response = await httpService.Put(url, invoice);
        ////    //if (!response.Success)
        ////    //{
        ////    //    throw new ApplicationException(await response.GetBody());
        ////    //}
        //}
    }
}
