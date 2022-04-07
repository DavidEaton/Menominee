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
    public class InventoryLaborRepository : IInventoryLaborRepository
    {
        private readonly ApplicationDbContext context;

        public InventoryLaborRepository(ApplicationDbContext context)
        {
            Guard.ForNull(context, "context");

            this.context = context;
        }

        public async Task AddLaborAsync(InventoryLabor labor)
        {
            Guard.ForNull(labor, "labor");

            if (await LaborExistsAsync(labor.Id))
                throw new Exception("Inventory Labor already exists");

            await context.AddAsync(labor);
        }

        public async Task DeleteLaborAsync(long id)
        {
            var labor = await context.InventoryLabor
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(labor => labor.Id == id);

            Guard.ForNull(labor, "labor");

            context.Remove(labor);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<IReadOnlyList<InventoryLaborToRead>> GetLaborsAsync()
        {
            var laborFromContext = await context.InventoryLabor.ToArrayAsync();

            return laborFromContext.Select(labor => InventoryLaborToRead.ConvertToDto(labor)).ToList();
        }

        public async Task<InventoryLaborToRead> GetLaborAsync(long id)
        {
            var laborFromContext = await context.InventoryLabor
                                                .FirstOrDefaultAsync(labor => labor.Id == id);

            return InventoryLaborToRead.ConvertToDto(laborFromContext);
        }

        public async Task<InventoryLabor> GetLaborEntityAsync(long id)
        {
            return await context.InventoryLabor
                                .FirstOrDefaultAsync(labor => labor.Id == id);
        }

        public async Task<IReadOnlyList<InventoryLaborToReadInList>> GetLaborListAsync()
        {
            var laborFromContext = await context.InventoryLabor.ToArrayAsync();

            return laborFromContext.Select(labor => InventoryLaborToReadInList.ConvertToDto(labor))
                                   .ToList();
        }

        public async Task<bool> LaborExistsAsync(long id)
        {
            return await context.InventoryLabor.AnyAsync(labor => labor.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<InventoryLabor> UpdateLaborAsync(InventoryLabor labor)
        {
            Guard.ForNull(labor, "labor");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(labor).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LaborExistsAsync(labor.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
    }
}
