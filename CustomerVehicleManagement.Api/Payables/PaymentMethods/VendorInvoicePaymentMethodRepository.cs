﻿using CustomerVehicleManagement.Api.Data;
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

        public async Task<VendorInvoicePaymentMethodToRead> GetPaymentMethodAsync(long id)
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

        public async Task<IReadOnlyList<VendorInvoicePaymentMethodToRead>> GetPaymentMethodsAsync()
        {
            IReadOnlyList<VendorInvoicePaymentMethod> payMethods = await context.VendorInvoicePaymentMethods
                .Include(method => method.ReconcilingVendor)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            return payMethods.Select(payMethod => VendorInvoicePaymentMethodHelper.ConvertToReadDto(payMethod))
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

    }
}
