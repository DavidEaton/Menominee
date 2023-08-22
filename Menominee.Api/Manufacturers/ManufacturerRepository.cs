using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Manufacturers
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

        public async Task<Manufacturer> GetManufacturerEntityAsync(long id)
        {
            return await context.Manufacturers
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

        public async Task<bool> ManufacturerExistsAsync(long id)
        {
            return await context.Manufacturers.AnyAsync(manufacturer => manufacturer.Id == id);
        }

        public async Task<List<string>> GetExistingPrefixList()
        {
            return await context.Manufacturers.Select(m => m.Prefix).ToListAsync();
        }

        public async Task<List<long>> GetExistingIdList()
        {
            return await context.Manufacturers
                .OrderBy(m => m.Id)
                .Select(m => m.Id)
                .ToListAsync();
        }
        // The value for a user created manufacturer.id is to start at 50,000
        public long DetermineManufacturerId(List<long> existingIds)
        {
            if (!existingIds.Any() || existingIds.Max() < 50000) // if there are no customer added manufacturers, start at 50,000
                return 50000;

            return existingIds.Max() + 1; // increment to the next value
        }
        public async Task ToggleIdentityInsert(bool enable)
        {
            // toggle identity insert
            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.Manufacturer {(enable ? "ON" : "OFF")};");
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operations)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                await operations();
                await context.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
