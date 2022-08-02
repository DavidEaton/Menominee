using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.PaymentMethods
{
    public class VendorInvoicePaymentMethodRepository : IVendorInvoicePaymentMethodRepository
    {
        private readonly ApplicationDbContext context;

        public VendorInvoicePaymentMethodRepository(ApplicationDbContext context)
        {
            Guard.ForNull(context, "context");
            this.context = context;
        }

        public async Task AddPaymentMethodAsync(VendorInvoicePaymentMethod payMethod)
        {
            Guard.ForNull(payMethod, "Payment Method");

            await context.AddAsync(payMethod);
        }

        public async Task DeletePaymentMethodAsync(long id)
        {
            var payMethodFromContext = await context.VendorInvoicePaymentMethods.FindAsync(id);

            Guard.ForNull(payMethodFromContext, "Payment Method");

            context.Remove(payMethodFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<VendorInvoicePaymentMethodToRead> GetPaymentMethodAsync(long id)
        {
            var payMethodFromContext = await context.VendorInvoicePaymentMethods
                                                        .Include(method => method.ReconcilingVendor)
                                                        .AsSplitQuery()
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(method => method.Id == id);
            Guard.ForNull(payMethodFromContext, "payMethodFromContext");

            return VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(payMethodFromContext);
        }

        public async Task<VendorInvoicePaymentMethod> GetPaymentMethodEntityAsync(long id)
        {
            var payMethodFromContext = await context.VendorInvoicePaymentMethods
                                                        .Include(method => method.ReconcilingVendor)
                                                        .AsSplitQuery()
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(method => method.Id == id);
            Guard.ForNull(payMethodFromContext, "payMethodFromContext");

            return payMethodFromContext;
        }

        public async Task<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>> GetPaymentMethodListAsync()
        {
            IReadOnlyList<VendorInvoicePaymentMethod> payMethods = await context.VendorInvoicePaymentMethods
                                                                                .Include(method => method.ReconcilingVendor)
                                                                                .AsSplitQuery()
                                                                                .AsNoTracking()
                                                                                .ToListAsync();

            return payMethods.Select(payMethod => VendorInvoicePaymentMethodHelper.ConvertEntityToReadInListDto(payMethod))
                             .ToList();
        }

        public async Task<bool> PaymentMethodExistsAsync(long id)
        {
            return await context.VendorInvoicePaymentMethods.AnyAsync(payMethod => payMethod.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdatePaymentMethod(VendorInvoicePaymentMethod payMethod)
        {
            //
        }
    }
}
