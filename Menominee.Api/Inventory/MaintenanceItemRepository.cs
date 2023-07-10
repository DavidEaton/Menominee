using Menominee.Api.Data;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.MaintenanceItems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Inventory
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
            if (await ItemExistsAsync(item.Id))
                throw new Exception("Maintenance Item already exists");

            if (item != null)
                context.Attach(item);
        }

        public void DeleteItem(MaintenanceItem item)
        {
            if (item is not null)
                context.Remove(item);
        }

        public async Task DeleteItemAsync(long id)
        {
            var item = await context.MaintenanceItems
                .FirstOrDefaultAsync(item => item.Id == id);

            if (item is not null)
                context.Remove(item);
        }

        public async Task<MaintenanceItemToRead> GetItemAsync(long id)
        {
            var itemFromContext = await context.MaintenanceItems
                                               .Include(item => item.InventoryItem)
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(item => item.Id == id);

            return MaintenanceItemHelper.ConvertToReadDto(itemFromContext);
        }

        public async Task<MaintenanceItem> GetItemEntityAsync(long id)
        {
            return await context.MaintenanceItems
                                .Include(item => item.InventoryItem)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IReadOnlyList<MaintenanceItemToReadInList>> GetItemsInListAsync()
        {
            var itemsFromContext = await context.MaintenanceItems
                                                .Include(item => item.InventoryItem)
                                                .AsSplitQuery()
                                                .OrderBy(item => item.DisplayOrder)
                                                .AsNoTracking()
                                                .ToArrayAsync();

            return itemsFromContext.Select(item => MaintenanceItemHelper.ConverToReadInListDto(item))
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
