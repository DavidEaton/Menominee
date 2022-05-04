using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly ApplicationDbContext context;

        public InventoryItemRepository(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddInventoryItemAsync(InventoryItem item)
        {
            if (item != null)
                await context.AddAsync(item);
        }

        public void DeleteInventoryItem(InventoryItem item)
        {
            context.Remove(item);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<InventoryItemToRead> GetInventoryItemAsync(long manufacturerId, string partNumber)
        {
            Guard.ForNullOrEmpty(partNumber, nameof(partNumber));
            
            var itemFromContext = await GetInventoryItemEntityAsync(manufacturerId, partNumber);
            return InventoryItemHelper.ConvertToDto(itemFromContext);
        }

        public async Task<InventoryItemToRead> GetInventoryItemAsync(long id)
        {
            var itemFromContext = await GetInventoryItemEntityAsync(id);
            return InventoryItemHelper.ConvertToDto(itemFromContext);
        }

        public async Task<InventoryItem> GetInventoryItemEntityAsync(long manufacturerId, string partNumber)
        {
            Guard.ForNullOrEmpty(partNumber, nameof(partNumber));

            var itemFromContext = await context.InventoryItems
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.ManufacturerId == manufacturerId && item.PartNumber == partNumber);

            return itemFromContext;
        }

        public async Task<InventoryItem> GetInventoryItemEntityAsync(long id)
        {
            var itemFromContext = await context.InventoryItems
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id);

            return itemFromContext;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemListAsync()
        {
            IReadOnlyList<InventoryItem> items = await context.InventoryItems
                .AsNoTracking()
                .ToListAsync();

            return items.
                Select(item => InventoryItemHelper.ConvertToReadInListDto(item))
                .ToList();
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemListAsync(long manufacturerId)
        {
            IReadOnlyList<InventoryItem> items = await context.InventoryItems.Where(item => item.ManufacturerId == manufacturerId)
                .AsNoTracking()
                .ToListAsync();

            return items.
                Select(item => InventoryItemHelper.ConvertToReadInListDto(item))
                .ToList();
        }

        public async Task<bool> InventoryItemExistsAsync(long manufacturerId, string partNumber)
        {
            return await context.InventoryItems.AnyAsync(item => item.ManufacturerId == manufacturerId && item.PartNumber == partNumber);
        }

        public async Task<bool> InventoryItemExistsAsync(long id)
        {
            return await context.InventoryItems.AnyAsync(item => item.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateInventoryItemAsync(InventoryItem item)
        {
            // No code in this implementation
        }
    }
}
