using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public void AddInvoice(VendorInvoice invoice)
        {
            if (invoice is not null)
                context.Attach(invoice);
        }

        public async Task DeleteInvoiceAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices.FindAsync(id);
            if (invoiceFromContext != null)
                context.Remove(invoiceFromContext);
        }

        public async Task<VendorInvoiceToRead> GetInvoiceAsync(long id)
        {
            var invoiceFromContext = context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.Manufacturer)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.SaleCode)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                        .ThenInclude(method => method.ReconcilingVendor)
                .Include(invoice => invoice.Taxes)
                    .ThenInclude(tax => tax.SalesTax)
                .AsSplitQuery()
                .AsNoTracking()

                .FirstOrDefaultAsync(invoice => invoice.Id == id);

            return await invoiceFromContext is null
            ? null
            : VendorInvoiceHelper.ConvertEntityToReadDto(await invoiceFromContext);
        }

        public async Task<Vendor> GetVendorAsync(long id)
        {
            return await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == id);
        }

        public async Task<VendorInvoice> GetInvoiceEntityAsync(long id)
        {
            var invoiceFromContext = context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.Manufacturer)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.SaleCode)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                        .ThenInclude(method => method.ReconcilingVendor)
                .Include(invoice => invoice.Taxes)
                    .ThenInclude(tax => tax.SalesTax)
                .AsSplitQuery()

                .FirstOrDefaultAsync(invoice => invoice.Id == id);

            return await invoiceFromContext;
        }

        public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceListAsync(ResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
                throw new ArgumentException(nameof(resourceParameters));

            // TODO: Fix parameter (isn't formed correctly from client); returning ALL invoices
            // for ALL vendors and ALL statuses is not sup.ported
            if (!resourceParameters.VendorId.HasValue && !resourceParameters.Status.HasValue)
                resourceParameters.Status = VendorInvoiceStatus.Open;

            var invoicesFromContext = context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                .AsSplitQuery()
                .AsNoTracking();

            if (resourceParameters.VendorId.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Vendor.Id == resourceParameters.VendorId.Value);

            if (resourceParameters.Status.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Status == resourceParameters.Status.Value);

            return await invoicesFromContext?.Select(invoice =>
                VendorInvoiceHelper.ConvertEntityToReadInListDto(invoice))
                    // Calling .ToList() at the very end of the method,
                    // EF defers execution until the entire filter is
                    // built and applied to database query, minimizing
                    // bandwidth use. Much more efficient than fetching
                    // all rows and then filtering them.
                    .ToListAsync();
        }

        public async Task<bool> InvoiceExistsAsync(long id)
        {
            return await context.VendorInvoices.AnyAsync(invoice => invoice.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void DeleteInvoice(VendorInvoice invoice)
        {
            context.Remove(invoice);
        }

        public async Task<IReadOnlyList<string>> GetVendorInvoiceNumbers(long vendorId)
        {
            return await context.VendorInvoices
                .Where(invoice => invoice.Vendor.Id == vendorId)
                .Select(invoice => invoice.InvoiceNumber)
                    .Where(invoiceNumber => !string.IsNullOrWhiteSpace(invoiceNumber))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoices(ResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
                throw new ArgumentException(nameof(resourceParameters));

            if (!resourceParameters.VendorId.HasValue && !resourceParameters.Status.HasValue)
                return await GetInvoices();

            var invoicesFromContext = GetInvoicesFromContextAsNoTracking();

            if (!resourceParameters.VendorId.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Vendor.Id == resourceParameters.VendorId.Value);

            if (!resourceParameters.Status.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Status == resourceParameters.Status.Value);

            return await invoicesFromContext?.Select(invoice =>
                VendorInvoiceHelper.ConvertEntityToReadDto(invoice))
                    .ToListAsync();
        }

        private async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceList()
        {
            return await GetInvoicesFromContextAsNoTracking()?.Select(invoice =>
                VendorInvoiceHelper.ConvertEntityToReadInListDto(invoice))
                    .ToListAsync();
        }

        private async Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoices()
        {
            return await GetInvoicesFromContextAsNoTracking()?.Select(invoice =>
                VendorInvoiceHelper.ConvertEntityToReadDto(invoice))
                    .ToListAsync();
        }

        private IQueryable<VendorInvoice> GetInvoicesFromContextAsNoTracking()
        {
            return context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.Manufacturer)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.SaleCode)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                        .ThenInclude(method => method.ReconcilingVendor)
                .Include(invoice => invoice.Taxes)
                    .ThenInclude(tax => tax.SalesTax)
                    .AsSplitQuery()
                    .AsNoTracking();
        }
    }
}
