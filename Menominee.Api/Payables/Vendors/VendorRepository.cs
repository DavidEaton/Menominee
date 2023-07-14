﻿using Menominee.Api.Data;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Vendors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.Vendors
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ApplicationDbContext context;

        public VendorRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<Vendor> GetVendorEntityAsync(long id)
        {
            return await context.Vendors
                .Include(vendor => vendor.Phones)
                .Include(vendor => vendor.Emails)
                .Include(vendor => vendor.DefaultPaymentMethod.PaymentMethod)
                .FirstOrDefaultAsync(vendor => vendor.Id == id);
        }

        public async Task<IReadOnlyList<Vendor>> GetVendorEntitiesAsync(List<long> ids)
        {
            return await context.Vendors
                .Where(vendor => ids.Contains(vendor.Id))
                .ToListAsync();
        }

        public async Task AddVendorAsync(Vendor vendor)
        {
            if (vendor is not null)
                await context.AddAsync(vendor);
        }

        public async Task<VendorToRead> GetVendorAsync(long id)
        {
            var vendorFromContext = await context.Vendors
                .Include(vendor => vendor.Phones)
                .Include(vendor => vendor.Emails)
                //.Include(vendor => vendor.Contact)
                //    .ThenInclude(contact => contact.Phones)
                //.Include(vendor => vendor.Contact)
                //    .ThenInclude(contact => contact.Emails)
                .Include(vendor => vendor.DefaultPaymentMethod.PaymentMethod)
                .FirstOrDefaultAsync(vendor => vendor.Id == id);

            return vendorFromContext is not null
                ? VendorHelper.ConvertToReadDto(vendorFromContext)
                : null;
        }

        public async Task<IReadOnlyList<VendorToRead>> GetVendorsAsync()
        {
            IReadOnlyList<Vendor> vendorsFromContext = await context.Vendors
                .Include(vendor => vendor.DefaultPaymentMethod)
                    .ThenInclude(defaultPaymentMethod => defaultPaymentMethod.PaymentMethod)
                .AsNoTracking()
                .ToListAsync();

            return vendorsFromContext is not null
                ? vendorsFromContext.Select(vendor => VendorHelper.ConvertToReadDto(vendor)).ToList()
                : null;
        }

        public async Task<IReadOnlyList<VendorToRead>> GetVendorsListAsync()
        {
            IReadOnlyList<Vendor> vendors = await context.Vendors
                .AsNoTracking()
                .ToListAsync();

            return vendors.Select(vendor => new VendorToRead
            {
                Id = vendor.Id,
                VendorCode = vendor.VendorCode,
                VendorRole = vendor.VendorRole,
                Name = vendor.Name,
                IsActive = vendor.IsActive
            }).ToList();
        }

        public void DeleteVendor(Vendor vendor)
        {
            context.Remove(vendor);
        }

        public async Task<bool> VendorExistsAsync(long id)
        {
            return await context.Vendors.AnyAsync(vendor => vendor.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}