using Menominee.Api.Data;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.PaymentMethods
{
    public class VendorInvoicePaymentMethodRepository : IVendorInvoicePaymentMethodRepository
    {
        private readonly ApplicationDbContext context;

        public VendorInvoicePaymentMethodRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Delete(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod is not null)
                context.Remove(payMethod);
        }

        public async Task<VendorInvoicePaymentMethodToRead> GetAsync(long id)
        {
            var payMethodFromContext = await context.VendorInvoicePaymentMethods
                .Include(method => method.ReconcilingVendor)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(method => method.Id == id);

            return payMethodFromContext is not null
                ? VendorInvoicePaymentMethodHelper.ConvertToReadDto(payMethodFromContext)
                : null;
        }

        public async Task<VendorInvoicePaymentMethod> GetEntityAsync(long id)
        {
            var payMethodFromContext = await context.VendorInvoicePaymentMethods
                .Include(method => method.ReconcilingVendor)
                .AsSplitQuery()
                .FirstOrDefaultAsync(method => method.Id == id);

            return payMethodFromContext is not null
                ? payMethodFromContext
                : null;
        }

        public async Task<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>> GetListAsync()
        {
            IReadOnlyList<VendorInvoicePaymentMethod> payMethods = await context.VendorInvoicePaymentMethods
                .Include(method => method.ReconcilingVendor)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            return payMethods.Select(payMethod => VendorInvoicePaymentMethodHelper.ConvertToReadInListDto(payMethod))
                             .ToList();
        }

        public async Task<IReadOnlyList<string>> GetPaymentMethodNamesAsync()
        {
            IList<VendorInvoicePaymentMethod> payMethods = await context.VendorInvoicePaymentMethods
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            var result = new List<string>();

            foreach (var method in payMethods)
                result.Add(method.Name);

            return result;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Add(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod is not null)
                context.Attach(payMethod);
        }
    }
}
