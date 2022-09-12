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
    public class MaintenanceItemRepository : IMaintenanceItemRepository
    {
        private readonly ApplicationDbContext context;

        public MaintenanceItemRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddItemAsync(MaintenanceItem item)
        {
            Guard.ForNull(item, "item");

            if (await ItemExistsAsync(item.Id))
                throw new Exception("Maintenance Item already exists");

            await context.AddAsync(item);
        }

        public void DeleteItem(MaintenanceItem item)
        {
            Guard.ForNull(item, "item");

            context.Remove(item);
            context.SaveChanges();
        }

        public async Task DeleteItemAsync(long id)
        {
            var item = await context.MaintenanceItems
                                    .FirstOrDefaultAsync(item => item.Id == id);

            Guard.ForNull(item, "item");

            context.Remove(item);
            context.SaveChanges();
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<MaintenanceItemToRead> GetItemAsync(long id)
        {
            var itemFromContext = await context.MaintenanceItems
                                               .Include(item => item.Item)
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(item => item.Id == id);

            Guard.ForNull(itemFromContext, "itemFromContext");

            return MaintenanceItemHelper.ConvertEntityToReadDto(itemFromContext);
        }

        public async Task<MaintenanceItem> GetItemEntityAsync(long id)
        {
            return await context.MaintenanceItems
                                .Include(item => item.Item)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IReadOnlyList<MaintenanceItemToReadInList>> GetItemsInListAsync()
        {
            var itemsFromContext = await context.MaintenanceItems
                                                .Include(item => item.Item)
                                                .AsSplitQuery()
                                                .OrderBy(item => item.DisplayOrder)
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => MaintenanceItemHelper.ConvertEntityToReadInListDto(item))
                                   .ToList();
        }

        public async Task<bool> ItemExistsAsync(long id)
        {
            return await context.MaintenanceItems.AnyAsync(item => item.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<MaintenanceItem> UpdateItemAsync(MaintenanceItem item)
        {
            Guard.ForNull(item, "item");

            // Tracking IS needed for commands for disconnected data collections
            context.Entry(item).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemExistsAsync(item.Id))
                    return null;// something that tells the controller to return NotFound();
                throw;
            }

            return null;
        }
    }
}
