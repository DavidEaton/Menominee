using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
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
                                                  .Include(invoice => invoice.LineItems)
                                                      .ThenInclude(item => item.SaleCode)
                                                  .Include(invoice => invoice.Payments)
                                                      .ThenInclude(payment => payment.PaymentMethod)
                                                          .ThenInclude(method => method.ReconcilingVendor)
                                                  .Include(invoice => invoice.Taxes)
                                                      .ThenInclude(tax => tax.SalesTax)
                                                  .AsSplitQuery()
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(invoice => invoice.Id == id);
            Guard.ForNull(invoiceFromContext, "invoiceFromContext");

            return VendorInvoiceHelper.ConvertEntityToReadDto(invoiceFromContext);
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
                                                  .Include(invoice => invoice.LineItems)
                                                      .ThenInclude(item => item.SaleCode)
//                                                  .Include(invoice => invoice.Payments)
//                                                      .ThenInclude(payment => payment.PaymentMethod)
//                                                          .AsNoTracking()
                                                  .Include(invoice => invoice.Payments)
                                                      .ThenInclude(payment => payment.PaymentMethod)
                                                          .ThenInclude(method => method.ReconcilingVendor)
//                                                          .AsNoTracking()
                                                  .Include(invoice => invoice.Taxes)
                                                      .ThenInclude(tax => tax.SalesTax)
                                                  .AsSplitQuery()
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
    }
}
