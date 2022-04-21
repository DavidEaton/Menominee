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
    public class InventoryItemTireRepositoryXXX : IInventoryItemTireRepositoryXXX
    {
        private readonly ApplicationDbContext context;

        public InventoryItemTireRepositoryXXX(ApplicationDbContext context)
        {
            Guard.ForNull(context, "context");

            this.context = context;
        }

        public async Task AddTireAsync(InventoryItemTire tire)
        {
            Guard.ForNull(tire, "tire");

            if (await TireExistsAsync(tire.Id))
                throw new Exception("Inventory Tire already exists");

            await context.AddAsync(tire);
        }

        public async Task DeleteTireAsync(long id)
        {
            var tire = await context.InventoryItemTires
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(tire => tire.Id == id);

            Guard.ForNull(tire, "tire");

            context.Remove(tire);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<InventoryTireToRead> GetTireAsync(long id)
        {
            var tireFromContext = await context.InventoryItemTires
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(tire => tire.Id == id);

            Guard.ForNull(tireFromContext, "tireFromContext");

            return InventoryTireToRead.ConvertToDto(tireFromContext);
        }

        public async Task<InventoryItemTire> GetTireEntityAsync(long id)
        {
            return await context.InventoryItemTires
                                .FirstOrDefaultAsync(tire => tire.Id == id);
        }

        public async Task<IReadOnlyList<InventoryTireToReadInList>> GetTiresInListAsync()
        {
            var tiresFromContext = await context.InventoryItemTires
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return tiresFromContext.Select(tire => InventoryTireToReadInList.ConvertToDto(tire))
                                   .ToList();
        }

        public async Task<IReadOnlyList<InventoryTireToRead>> GetTiresAsync()
        {
            var tires = new List<InventoryTireToRead>();
            var tiresFromContext = await context.InventoryItemTires
                                                .AsNoTracking()
                                                .ToArrayAsync();

            foreach (var tire in tiresFromContext)
                tires.Add(InventoryTireToRead.ConvertToDto(tire));

            return tires;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> TireExistsAsync(long id)
        {
            return await context.InventoryItemTires.AnyAsync(tire => tire.Id == id);
        }

        public async Task<InventoryItemTire> UpdateTireAsync(InventoryItemTire tire)
        {
            Guard.ForNull(tire, "tire");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(tire).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TireExistsAsync(tire.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
    }
}
