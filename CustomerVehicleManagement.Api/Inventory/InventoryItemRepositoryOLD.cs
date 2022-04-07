using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Inventory
{
    public class InventoryItemRepositoryOLD : IInventoryItemRepositoryOLD
    {
        private readonly ApplicationDbContext context;

        public InventoryItemRepositoryOLD(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddInventoryItemAsync(InventoryItem item)
        {
            if (item != null)
                await context.AddAsync(item);
        }

        public async Task DeleteInventoryItemAsync(long id)
        {
            var itemFromContext = await context.InventoryItems.FindAsync(id);
            if (itemFromContext != null)
                context.Remove(itemFromContext);
        }

        public void FixTrackingState()
        {
            context.FixState();
        }

        public async Task<InventoryItemToRead> GetInventoryItemAsync(long mfrId, string itemNumber)
        {
            var itemFromContext = await context.InventoryItems
                .FirstOrDefaultAsync(item => (item.ManufacturerId == mfrId && item.ItemNumber == itemNumber));

            return InventoryItemToRead.ConvertToDto(itemFromContext);
        }

        public async Task<InventoryItemToRead> GetInventoryItemAsync(long id)
        {
            var itemFromContext = await context.InventoryItems
                .FirstOrDefaultAsync(item => item.Id == id);

            return InventoryItemToRead.ConvertToDto(itemFromContext);
        }

        public async Task<InventoryItem> GetInventoryItemEntityAsync(long mfrId, string itemNumber)
        {
            var itemFromContext = await context.InventoryItems
                .FirstOrDefaultAsync(item => (item.ManufacturerId == mfrId && item.ItemNumber == itemNumber));

            return itemFromContext;
        }

        public async Task<InventoryItem> GetInventoryItemEntityAsync(long id)
        {
            var itemFromContext = await context.InventoryItems
                .FirstOrDefaultAsync(item => item.Id == id);

            return itemFromContext;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemListAsync()
        {
            IReadOnlyList<InventoryItem> items = await context.InventoryItems.ToListAsync();

            return items.
                Select(item => InventoryItemToReadInList.ConvertToDto(item))
                .ToList();
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetInventoryItemListAsync(long mfrId)
        {
            IReadOnlyList<InventoryItem> items = await context.InventoryItems.Where(item => item.ManufacturerId == mfrId).ToListAsync();

            return items.
                Select(item => InventoryItemToReadInList.ConvertToDto(item))
                .ToList();
        }

        public async Task<bool> InventoryItemExistsAsync(long mfrId, string itemNumber)
        {
            return await context.InventoryItems.AnyAsync(item => (item.ManufacturerId == mfrId && item.ItemNumber == itemNumber));
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
