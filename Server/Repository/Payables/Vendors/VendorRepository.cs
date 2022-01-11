using MenomineePlayWASM.Shared;
using MenomineePlayWASM.Shared.Dtos.Payables.Vendors;
using MenomineePlayWASM.Shared.Entities.Payables.Vendors;
using MenomineePlayWASM.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
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
            var vendorFromContext = await context.Vendors
                                                 .FirstOrDefaultAsync(vendor => vendor.Id == id);

            return vendorFromContext;
        }

        public async Task CreateVendorAsync(Vendor vendor)
        {
            if (vendor != null)
                await context.AddAsync(vendor);
        }

        public async Task<VendorToRead> GetVendorAsync(long id)
        {
            var vendorFromContext = await context.Vendors
                //.Include(organization => organization.Phones)
                //.Include(organization => organization.Emails)
                //.Include(organization => organization.Contact)
                //    .ThenInclude(contact => contact.Phones)
                //.Include(organization => organization.Contact)
                //    .ThenInclude(contact => contact.Emails)
                .FirstOrDefaultAsync(vendor => vendor.Id == id);

            return new VendorToRead()
            {
                Id = vendorFromContext.Id,
                VendorCode = vendorFromContext.VendorCode,
                Name = vendorFromContext.Name,
                IsActive = vendorFromContext.IsActive
            };
        }

        public async Task<IReadOnlyList<VendorToRead>> GetVendorsAsync()
        {
            IReadOnlyList<Vendor> vendorsFromContext = await context.Vendors.ToListAsync();

            return vendorsFromContext
                .Select(vendor => new VendorToRead()
                {
                    Id = vendor.Id,
                    VendorCode = vendor.VendorCode,
                    Name = vendor.Name,
                    IsActive = vendor.IsActive
                }).ToList();
        }

        public async Task<IReadOnlyList<VendorToReadInList>> GetVendorsListAsync()
        {
            IReadOnlyList<Vendor> vendors = await context.Vendors.ToListAsync();

            return vendors.Select(vendor => new VendorToReadInList
            {
                Id = vendor.Id,
                VendorCode = vendor.VendorCode,
                Name = vendor.Name
            }).ToList();
        }

        public void UpdateVendorAsync(Vendor vendor)
        {
            // No code in this implementation.
        }

        public async Task DeleteVendorAsync(long id)
        {
            var vendorFromContext = await context.Vendors.FindAsync(id);
            context.Remove(vendorFromContext);
        }

        public async Task<bool> VendorExistsAsync(long id)
        {
            return await context.Vendors.AnyAsync(o => o.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

    }
}
