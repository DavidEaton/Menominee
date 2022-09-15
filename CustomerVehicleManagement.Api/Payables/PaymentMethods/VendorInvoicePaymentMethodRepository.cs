using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
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
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddPaymentMethodAsync(VendorInvoicePaymentMethod payMethod)
        {
            if (payMethod is not null)
                await context.AddAsync(payMethod);
        }

        public void DeletePaymentMethod(VendorInvoicePaymentMethod payMethod)
        {
            context.Remove(payMethod);
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

            return payMethodFromContext is not null
                ? VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(payMethodFromContext)
                : null;
        }

        public async Task<VendorInvoicePaymentMethod> GetPaymentMethodEntityAsync(long id)
        {
            var payMethodFromContext = await context.VendorInvoicePaymentMethods
                .Include(method => method.ReconcilingVendor)
                .AsSplitQuery()
                .FirstOrDefaultAsync(method => method.Id == id);

            return payMethodFromContext is not null
                ? payMethodFromContext
                : null;
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

        public async Task<IList<string>> GetPaymentMethodNames()
        {
            IList<VendorInvoicePaymentMethod> payMethods = await context.VendorInvoicePaymentMethods
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            return (IList<string>)payMethods.Select(paymentMethod => new string(paymentMethod.Name));

        }

        public async Task<IReadOnlyList<VendorInvoicePaymentMethodToRead>> GetPaymentMethodsAsync()
        {
            IReadOnlyList<VendorInvoicePaymentMethod> payMethods = await context.VendorInvoicePaymentMethods
                .Include(method => method.ReconcilingVendor)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            return payMethods.Select(payMethod => VendorInvoicePaymentMethodHelper.ConvertEntityToReadDto(payMethod))
                             .ToList();
        }

        public async Task<bool> PaymentMethodExistsAsync(long id)
        {
            return await context.VendorInvoicePaymentMethods.AnyAsync(payMethod => payMethod.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        Task<IList<string>> IVendorInvoicePaymentMethodRepository.GetPaymentMethodNames()
        {
            throw new NotImplementedException();
        }
    }
}
