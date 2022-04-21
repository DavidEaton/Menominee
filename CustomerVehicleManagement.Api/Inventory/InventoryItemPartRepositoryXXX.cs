using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public class InventoryItemPartRepositoryXXX : IInventoryItemPartRepositoryXXX
    {
        private readonly ApplicationDbContext context;

        public InventoryItemPartRepositoryXXX(ApplicationDbContext context)
        {
            Guard.ForNull(context, "context");

            this.context = context;
        }

        public async Task AddPartAsync(InventoryItemPart part)
        {
            Guard.ForNull(part, "part");

            if (await PartExistsAsync(part.Id))
                throw new Exception("Inventory Part already exists");

            await context.AddAsync(part);
        }

        public async Task DeletePartAsync(long id)
        {
            var part = await context.InventoryItemParts
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(part => part.Id == id);

            Guard.ForNull(part, "part");

            context.Remove(part);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<InventoryPartToRead> GetPartAsync(long id)
        {
            var partFromContext = await context.InventoryItemParts
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(part => part.Id == id);

            Guard.ForNull(partFromContext, "partFromContext");

            return InventoryPartToRead.ConvertToDto(partFromContext);
        }

        public async Task<InventoryItemPart> GetPartEntityAsync(long id)
        {
            return await context.InventoryItemParts
                                .FirstOrDefaultAsync(part => part.Id == id);
        }

        public async Task<IReadOnlyList<InventoryPartToRead>> GetPartsAsync()
        {
            var parts = new List<InventoryPartToRead>();
            var partsFromContext = await context.InventoryItemParts
                                                .AsNoTracking()
                                                .ToArrayAsync();

            foreach (var part in partsFromContext)
                parts.Add(InventoryPartToRead.ConvertToDto(part));

            return parts;
        }

        public async Task<IReadOnlyList<InventoryPartToReadInList>> GetPartsInListAsync()
        {
            var partsFromContext = await context.InventoryItemParts
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return partsFromContext.Select(part => InventoryPartToReadInList.ConvertToDto(part))
                                   .ToList();
        }

        public async Task<bool> PartExistsAsync(long id)
        {
            return await context.InventoryItemParts.AnyAsync(part => part.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<InventoryItemPart> UpdatePartAsync(InventoryItemPart part)
        {
            Guard.ForNull(part, "part");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(part).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PartExistsAsync(part.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
    }
}
