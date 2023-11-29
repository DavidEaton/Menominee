using CSharpFunctionalExtensions;
using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Manufacturers
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext context;

        public ManufacturerRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public void Add(Manufacturer manufacturer)
        {
            if (manufacturer is not null)
                context.Attach(manufacturer);
        }

        public void Delete(Manufacturer manufacturer)
        {
            if (manufacturer is not null)
                context.Remove(manufacturer);
        }

        public async Task<ManufacturerToRead> GetAsync(long id)
        {
            return ManufacturerHelper.ConvertToReadDto(
                await context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(manufacturer => manufacturer.Id == id));
        }

        public async Task<Result<IReadOnlyList<Manufacturer>>> GetEntitiesAsync(List<long> ids = null)
        {
            try
            {
                IQueryable<Manufacturer> query = context.Manufacturers;

                if (ids is not null)
                    query = query.Where(manufacturer => ids.Contains(manufacturer.Id));


                var manufacturers = await query.ToListAsync();

                if (!manufacturers.Any())
                    return Result.Failure<IReadOnlyList<Manufacturer>>("No manufacturers found.");

                return Result.Success<IReadOnlyList<Manufacturer>>(manufacturers);
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return Result.Failure<IReadOnlyList<Manufacturer>>($"An error occurred while fetching manufacturers: {ex.Message}");
            }
        }

        public async Task<Manufacturer> GetEntityAsync(long id)
        {
            return await context.Manufacturers
                .FirstOrDefaultAsync(manufacturer => manufacturer.Id == id);
        }

        public async Task<IReadOnlyList<ManufacturerToReadInList>> GetListAsync()
        {
            IReadOnlyList<Manufacturer> manufacturers = await context.Manufacturers
                .AsNoTracking()
                .ToListAsync();

            return manufacturers.
                Select(manufacturer => ManufacturerHelper.ConvertToReadInListDto(manufacturer))
                .ToList();
        }

        public async Task<List<string>> GetExistingPrefixListAsync()
        {
            return await context.Manufacturers.Select(manufacturer => manufacturer.Prefix).ToListAsync();
        }

        public async Task<List<long>> GetExistingIdsAsync()
        {
            return await context.Manufacturers
                .OrderBy(m => m.Id)
                .Select(m => m.Id)
                .ToListAsync();
        }
        // The value for a user created manufacturer.id is to start at 50,000
        public long GetNextManufacturerId(List<long> existingIds)
        {
            if (!existingIds.Any() || existingIds.Max() < 50000) // if there are no customer added manufacturers, start at 50,000
                return 50000;

            return existingIds.Max() + 1; // increment to the next value
        }
        public async Task ToggleIdentityInsertAsync(bool enable)
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
