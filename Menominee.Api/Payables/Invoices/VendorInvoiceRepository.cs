using Menominee.Api.Data;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.Invoices
{
    public class VendorInvoiceRepository : IVendorInvoiceRepository
    {
        private readonly ApplicationDbContext context;

        public VendorInvoiceRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(VendorInvoice invoice)
        {
            if (invoice is not null)
            {
                if (await Exists(invoice.Id))
                    throw new Exception("Invoice already exists");

                // Detach any existing tracked entity with the same key
                var existingEntity = context.VendorInvoices.FirstOrDefault(i => i.Id == invoice.Id);
                if (existingEntity != null)
                {
                    context.Entry(existingEntity).State = EntityState.Detached;
                }

                context.VendorInvoices.Attach(invoice);
            }
        }

        public async Task DeleteInvoiceAsync(long id)
        {
            var invoiceFromContext = await context.VendorInvoices.FindAsync(id);
            if (invoiceFromContext != null)
                context.Remove(invoiceFromContext);
        }

        public async Task<VendorInvoiceToRead> Get(long id)
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
            ? null
            : VendorInvoiceHelper.ConvertToReadDto(await invoiceFromContext);
        }

        public async Task<Vendor> GetVendor(long id)
        {
            return await context.Vendors.FirstOrDefaultAsync(vendor => vendor.Id == id);
        }

        public async Task<VendorInvoice> GetEntity(long id)
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

        public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetList(ResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
                throw new ArgumentException(nameof(resourceParameters));

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

        public async Task<bool> Exists(long id)
        {
            return await context.VendorInvoices.AnyAsync(invoice => invoice.Id == id);
        }

        public async Task SaveChanges() =>
            await context.SaveChangesAsync();


        public void Delete(VendorInvoice invoice)
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
                VendorInvoiceHelper.ConvertToReadDto(invoice))
                    .ToListAsync();
        }

        private async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoiceList()
        {
            return await GetInvoicesFromContextAsNoTracking()?.Select(invoice =>
                VendorInvoiceHelper.ConvertToReadInListDto(invoice))
                    .ToListAsync();
        }

        private async Task<IReadOnlyList<VendorInvoiceToRead>> GetInvoices()
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
