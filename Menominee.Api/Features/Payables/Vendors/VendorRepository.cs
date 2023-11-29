using CSharpFunctionalExtensions;
using Menominee.Api.Data;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Vendors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Payables.Vendors
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ApplicationDbContext context;

        public VendorRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<Vendor>> GetEntityAsync(long id)
        {
            var vendor = await context.Vendors
                .Include(v => v.Phones)
                .Include(v => v.Emails)
                .Include(v => v.DefaultPaymentMethod.PaymentMethod)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vendor is null)
                return Result.Failure<Vendor>($"No vendor found with Id = {id}");

            return Result.Success(vendor);
        }


        public async Task<IReadOnlyList<Vendor>> GetEntitiesAsync(List<long> ids)
        {
            return await context.Vendors
                .Where(vendor => ids.Contains(vendor.Id))
                .ToListAsync();
        }

        public void Add(Vendor vendor)
        {
            if (vendor is not null)
                context.Attach(vendor);
        }

        public async Task<VendorToRead> GetAsync(long id)
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

        public async Task<IReadOnlyList<VendorToRead>> GetAllAsync()
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

        public void Delete(Vendor vendor)
        {
            if (vendor is not null)
                context.Remove(vendor);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
