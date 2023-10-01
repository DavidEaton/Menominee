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

        public void Add(MaintenanceItem item)
        {
            if (item is not null)
                context.Attach(item);
        }

        public void Delete(MaintenanceItem item)
        {
            if (item is not null)
                context.Remove(item);
        }

        public async Task<MaintenanceItemToRead> GetAsync(long id)
        {
            var itemFromContext = await context.MaintenanceItems
                                               .Include(item => item.InventoryItem)
                                               .AsNoTracking()
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(item => item.Id == id);

            return MaintenanceItemHelper.ConvertToReadDto(itemFromContext);
        }

        public async Task<MaintenanceItem> GetEntityAsync(long id)
        {
            return await context.MaintenanceItems
                                .Include(item => item.InventoryItem)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IReadOnlyList<MaintenanceItemToReadInList>> GetListAsync()
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

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
