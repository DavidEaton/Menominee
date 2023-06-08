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

        public async Task DeleteManufacturerAsync(long id)
        {
            var manufacturerFromContext = await context.Manufacturers.FindAsync(id);

            if (manufacturerFromContext is not null)
                context.Remove(manufacturerFromContext);
        }

        public async Task<ManufacturerToRead> GetManufacturerAsync(string code)
        {
            return ManufacturerHelper.ConvertToReadDto(
                await context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(manufacturer => manufacturer.Code == code));
        }

        public async Task<ManufacturerToRead> GetManufacturerAsync(long id)
        {
            return ManufacturerHelper.ConvertToReadDto(
                await context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(manufacturer => manufacturer.Id == id));
        }

        public async Task<IReadOnlyList<Manufacturer>> GetManufacturerEntitiesAsync(List<long> ids)
        {
            return await context.Manufacturers
                .Where(manufacturer => ids.Contains(manufacturer.Id))
                .ToListAsync();
        }

        public async Task<Manufacturer> GetManufacturerEntityAsync(string code)
        {
            return await context.Manufacturers
                .FirstOrDefaultAsync(manufacturer => manufacturer.Code == code);
        }

        public async Task<Manufacturer> GetManufacturerEntityAsync(long id)
        {
            return await context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(manufacturer => manufacturer.Id == id);
        }

        public async Task<IReadOnlyList<ManufacturerToReadInList>> GetManufacturerListAsync()
        {
            IReadOnlyList<Manufacturer> manufacturers = await context.Manufacturers
                .AsNoTracking()
                .ToListAsync();

            return manufacturers.
                Select(manufacturer => ManufacturerHelper.ConvertToReadInListDto(manufacturer))
                .ToList();
        }

        public async Task<bool> ManufacturerExistsAsync(string code)
        {
            return await context.Manufacturers.AnyAsync(manufacturer => manufacturer.Code == code);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
