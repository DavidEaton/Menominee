using Menominee.Api.Data;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Payables.Invoices
{
    public class VendorInvoiceRepository : IVendorInvoiceRepository
    {
        private readonly ApplicationDbContext context;

        public VendorInvoiceRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(VendorInvoice invoice)
        {
            if (invoice is not null)
                context.Attach(invoice);
        }

        public async Task<VendorInvoiceToRead> GetAsync(long id)
        {
            var invoiceFromContext = context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                    .ThenInclude(vendor => vendor.DefaultPaymentMethod)
                        .ThenInclude(defaultPaymentMethod => defaultPaymentMethod.PaymentMethod)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.Manufacturer)
                .Include(invoice => invoice.LineItems)
                    .ThenInclude(item => item.Item.SaleCode)
                .Include(invoice => invoice.Payments)
                    .ThenInclude(payment => payment.PaymentMethod)
                        .ThenInclude(method => method.ReconcilingVendor)
                .Include(invoice => invoice.Taxes)
                    .ThenInclude(tax => tax.SalesTax)
                .AsSplitQuery()
                .AsNoTracking()

                .FirstOrDefaultAsync(invoice => invoice.Id == id);

            return await invoiceFromContext is null
            ? new()
            : VendorInvoiceHelper.ConvertToReadDto(await invoiceFromContext);
        }

        public async Task<VendorInvoice> GetEntityAsync(long id)
        {
            var invoiceFromContext = context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                    .ThenInclude(vendor => vendor.DefaultPaymentMethod)
                        .ThenInclude(defaultPaymentMethod => defaultPaymentMethod.PaymentMethod)
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

        public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetListByParametersAsync(ResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
                return null;

            var invoicesFromContext = context.VendorInvoices
                .Include(invoice => invoice.Vendor.DefaultPaymentMethod.PaymentMethod)
                .AsSplitQuery()
                .AsNoTracking();

            if (resourceParameters.VendorId.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Vendor.Id == resourceParameters.VendorId.Value);

            if (resourceParameters.Status.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Status == resourceParameters.Status.Value);

            var result = await invoicesFromContext?.Select(invoice =>
                VendorInvoiceHelper.ConvertToReadInListDto(invoice))
                    // Calling .ToList() at the very end of the method,
                    // EF defers execution until the entire filter is
                    // built and applied to database query, minimizing
                    // bandwidth use. Much more efficient than fetching
                    // all rows and then filtering them.
                    .ToListAsync();

            return result;
        }

        public async Task SaveChangesAsync() =>
            await context.SaveChangesAsync();

        public void Delete(VendorInvoice invoice)
        {
            if (invoice is not null)
                context.Remove(invoice);
        }

        public async Task<IReadOnlyList<string>> GetVendorInvoiceNumbersAsync(long vendorId)
        {
            return await context.VendorInvoices
                .Where(invoice => invoice.Vendor.Id == vendorId)
                .Select(invoice => invoice.InvoiceNumber)
                    .Where(invoiceNumber => !string.IsNullOrWhiteSpace(invoiceNumber))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<VendorInvoiceToRead>> GetByParametersAsync(ResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
                return null;

            if (!resourceParameters.VendorId.HasValue && !resourceParameters.Status.HasValue)
                return await GetInvoicesAsync();

            var invoicesFromContext = GetInvoicesFromContextAsNoTracking();

            if (!resourceParameters.VendorId.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Vendor.Id == resourceParameters.VendorId.Value);

            if (!resourceParameters.Status.HasValue)
                invoicesFromContext = invoicesFromContext.Where(invoice => invoice.Status == resourceParameters.Status.Value);

            return await invoicesFromContext?.Select(invoice =>
                VendorInvoiceHelper.ConvertToReadDto(invoice))
                    .ToListAsync();
        }

        private async Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoicesAsync()
        {
            return await GetInvoicesFromContextAsNoTracking()?.Select(invoice =>
                VendorInvoiceHelper.ConvertToReadDto(invoice))
                    .ToListAsync();
        }

        private IQueryable<VendorInvoice> GetInvoicesFromContextAsNoTracking()
        {
            return context.VendorInvoices
                .Include(invoice => invoice.Vendor)
                    .ThenInclude(vendor => vendor.DefaultPaymentMethod)
                        .ThenInclude(defaultPaymentMethod => defaultPaymentMethod.PaymentMethod)
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
