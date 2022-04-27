using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Manufacturers
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext context;

        public ManufacturerRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddManufacturerAsync(Manufacturer manufacturer)
        {
            if (manufacturer != null)
                await context.AddAsync(manufacturer);
        }

        public async Task DeleteManufacturerAsync(string code)
        {
            var mfrFromContext = await context.Manufacturers.FindAsync(code);
            if (mfrFromContext != null)
                context.Remove(mfrFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<ManufacturerToRead> GetManufacturerAsync(string code)
        {
            var mfrFromContext = await context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(mfr => mfr.Code == code);

            return ManufacturerToRead.ConvertToDto(mfrFromContext);
        }

        public async Task<Manufacturer> GetManufacturerEntityAsync(string code)
        {
            var mfrFromContext = await context.Manufacturers
                .FirstOrDefaultAsync(mfr => mfr.Code == code);

            return mfrFromContext;
        }

        public async Task<IReadOnlyList<ManufacturerToReadInList>> GetManufacturerListAsync()
        {
            IReadOnlyList<Manufacturer> mfrs = await context.Manufacturers
                .AsNoTracking()
                .ToListAsync();

            return mfrs.
                Select(mfr => ManufacturerToReadInList.ConvertToDto(mfr))
                .ToList();
        }

        public async Task<bool> ManufacturerExistsAsync(string code)
        {
            return await context.Manufacturers.AnyAsync(mfr => mfr.Code == code);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            // No code in this implementation
        }
    }
}
